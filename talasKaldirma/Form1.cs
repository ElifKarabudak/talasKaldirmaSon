using System.Globalization;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using devDept;
using devDept.Eyeshot;
using devDept.Eyeshot.Control;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Milling;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using devDept.Graphics;

namespace talasKaldirma
{
    public partial class Form1 : Form
    {
        private Animation? _yRef;
        private Animation? _xRef;
        private Animation? _zRef;
        private const double MachineLocalWidth = 2100.0;
        private readonly Point3D machineWorkOrigin = new Point3D(-1005, -3800, 941);

        private readonly Point3D spindleNoseCad =
            new Point3D(1759.5, -546.2, 1231.49924);
        private double toolStickout = 50.0; 
        private double _lastX = double.NaN, _lastY = double.NaN, _lastZ = double.NaN;
        private int _redrawPending;
        private bool _loaded;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            if (_loaded) return;
            _loaded = true;
            try
            {
                string nc = Path.Combine(Application.StartupPath, "nc", "test3.NC");
                string tempNc = TnmTakimDestegi.NcHazirla(nc, out var takimIsimleri);
                try
                {
                    ReadGCode reader = new ReadGCode(tempNc);
                    await manufacture1.DoWorkAsync(reader);
                    reader.OpenTo(manufacture1);
                }
                finally
                {
                    if (File.Exists(tempNc)) File.Delete(tempNc);
                }

                List<Toolpath> toolpaths = manufacture1.Entities.OfType<Toolpath>().ToList();
                if (toolpaths.Count == 0) throw new InvalidOperationException("NC dosyasýnda takým yolu yok.");
                foreach (Toolpath tp in toolpaths)
                {
                    if (!takimIsimleri.ContainsKey(tp.Tool))
                        throw new InvalidOperationException("TNM öncesinde hareket var veya takým numarasý eþleþmedi.");
                    tp.Visible = false;
                }
                string dbYolu = Path.Combine(Application.StartupPath, "db", "CncDb.db");

                await GovdeOku();
                await XOku();
                await YOku();

                Block assembly = new Block("MachineAssembly");
                assembly.Entities.Add(new BlockReference(0, 0, 0, "Govde", 1, 1, 1, 0));
                _yRef = new Animation("y_block");
                assembly.Entities.Add(_yRef);
                manufacture1.Blocks.Add(assembly);

                BlockReference root = new BlockReference(0, 0, 0, "MachineAssembly", 1, 1, 1, 0);
                root.Rotate(-Math.PI / 2, Vector3D.AxisZ);
                root.Translate(0, MachineLocalWidth, 0);
                manufacture1.Entities.Add(root);

                manufacture1.Rendered.ShadowMode = shadowType.None;
                manufacture1.ActiveViewport.DisplayMode = displayType.Rendered;
                manufacture1.SimulationTool = TnmTakimDestegi.TakimBul(
                    dbYolu, takimIsimleri[toolpaths[0].Tool], toolpaths[0].Tool);
                manufacture1.SimulationSetup =
                    new Setup("Top", linearUnitsType.Millimeters, Plane.XY, null);
                Stock stock = new Stock(new LinearPath(0, 0, 2800, 2100), new Interval(0, 18));
                manufacture1.SimulationStock = stock.GetSimulationStock();

                UpdateMachineAt(0, 0, 30, requestRedraw: false);
                manufacture1.Entities.Regen();
                manufacture1.ActiveViewport.SetView(viewType.Left);
                // gridi gizle
                foreach (var grid in manufacture1.ActiveViewport.Grids)
                    grid.Visible = false;

                // orijini gizle
                foreach (var origin in manufacture1.ActiveViewport.OriginSymbols)
                    origin.Visible = false;

                manufacture1.ZoomFit();

                manufacture1.PositionChanged += (s, ev) => RequestMachineRedraw();
                manufacture1.BeforeMachineDraw += tip =>
                    UpdateMachineAt(tip.X, tip.Y, tip.Z, requestRedraw: false);
                manufacture1.Invalidate();

                foreach (Toolpath tp in toolpaths)
                {
                    // Önceki yol tamamen bitmiþtir: yeni TNM bölümünün takýmýný DB'den seç.
                    // SimulationStock tekrar oluþturulmaz; kaldýrýlmýþ talaþ korunur.
                    manufacture1.SimulationTool = TnmTakimDestegi.TakimBul(
                        dbYolu, takimIsimleri[tp.Tool], tp.Tool);
                    manufacture1.SimulationToolpath = tp;
                    await SimulasyonuCalistir();
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed && !Disposing)
                    MessageBox.Show(this, ex.ToString(), "Yükleme / simülasyon hatasý",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RequestMachineRedraw()
        {
            if (IsDisposed || Disposing || !IsHandleCreated) return;
            if (Interlocked.Exchange(ref _redrawPending, 1) != 0) return;
            try
            {
                BeginInvoke((Action)(() =>
                {
                    Interlocked.Exchange(ref _redrawPending, 0);
                    if (!IsDisposed && !Disposing && !manufacture1.IsDisposed)
                        manufacture1.Invalidate();
                }));
            }
            catch (InvalidOperationException)
            {
                Interlocked.Exchange(ref _redrawPending, 0);
            }
        }

        private void UpdateMachineAt(double worldX, double worldY, double worldZ,
            bool requestRedraw = true)
        {
            // Kök dönüþümün tersi: worldX = localY; worldY = 2100 - localX.
            double localX = MachineLocalWidth - worldY;
            double localY = worldX;
            double x = localX - (spindleNoseCad.X - machineWorkOrigin.X);
            double y = localY - (spindleNoseCad.Y - machineWorkOrigin.Y);
            double z = worldZ + toolStickout - (spindleNoseCad.Z - machineWorkOrigin.Z);

            if (_yRef != null && y != _lastY)
            {
                _yRef.SetPosition(0, y, 0);
                _lastY = y;
            }
            if (_xRef != null && x != _lastX)
            {
                _xRef.SetPosition(x, 0, 0);
                _lastX = x;
            }
            if (_zRef != null && z != _lastZ)
            {
                _zRef.SetPosition(0, 0, z);
                _lastZ = z;
            }
            if (requestRedraw) manufacture1.Invalidate();
        }

        private async Task SimulasyonuCalistir()
        {
            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            Manufacture.SimulationEndedEventHandler handler = (s, e) => tcs.TrySetResult();
            manufacture1.SimulationEnded += handler;
            try
            {
                manufacture1.Start();
                await tcs.Task;
            }
            finally
            {
                manufacture1.SimulationEnded -= handler;
            }
        }

        private async Task GovdeOku()
        {
            string file = Path.Combine(Application.StartupPath, "assets", "0_CNC-2128-GENEL-LIGHT.glb");
            Block body = new Block("Govde");
            body.Entities.AddRange(await GltfOku(file));
            manufacture1.Blocks.Add(body);
        }

        private async Task XOku()
        {
            string folder = Path.Combine(Application.StartupPath, "xAssets");
            Block z = new Block("z_block");
            z.Entities.AddRange(await GltfOku(Path.Combine(folder, "spindle.glb")));
            manufacture1.Blocks.Add(z);

            Block x = new Block("x_block");
            x.Entities.AddRange(await GltfOku(Path.Combine(folder, "x_araba.glb")));
            _zRef = new Animation("z_block");
            x.Entities.Add(_zRef);
            manufacture1.Blocks.Add(x);
        }

        private async Task YOku()
        {
            Block y = new Block("y_block");
            y.Entities.AddRange(await GltfOku(
                Path.Combine(Application.StartupPath, "assets", "kopru.glb")));
            _xRef = new Animation("x_block");
            y.Entities.Add(_xRef);
            manufacture1.Blocks.Add(y);
        }

        private async Task<List<Entity>> GltfOku(string dosyaYolu)
        {
            string okunacak = GltfIsimleriniDuzelt(dosyaYolu);

            ReadGLTF gltf = new ReadGLTF(okunacak);
            try
            {
                await manufacture1.DoWorkAsync(gltf);
                _ = gltf.Entities.Count;
                _ = gltf.Blocks.Count;
            }
            catch (EyeshotException ex)
            {
                throw new InvalidOperationException($"GLTF okunamadý: {dosyaYolu}", ex);
            }
            finally
            {
                if (okunacak != dosyaYolu && File.Exists(okunacak)) File.Delete(okunacak);
            }

            string onEk = Path.GetFileNameWithoutExtension(dosyaYolu)
                .Replace(" ", "_").Replace("-", "_") + "__";

            // 1. Bloklarý yeniden adlandýr 
            var adlar = new Dictionary<string, string>();
            int sayac = 0;
            foreach (Block b in gltf.Blocks)
            {
                string yeniAd = onEk + (string.IsNullOrEmpty(b.Name) ? $"isimsiz_{sayac++}" : b.Name);
                if (!string.IsNullOrEmpty(b.Name)) adlar[b.Name] = yeniAd;
                b.Name = yeniAd;
            }

            // 2. Referanslarý yeni adlara yönlendir
            void RefGuncelle(IEnumerable<Entity> liste)
            {
                foreach (Entity ent in liste)
                    if (ent is BlockReference br && br.BlockName != null &&
                        adlar.TryGetValue(br.BlockName, out string? yeni))
                        br.BlockName = yeni;
            }
            foreach (Block b in gltf.Blocks) RefGuncelle(b.Entities);
            RefGuncelle(gltf.Entities);

           
            var diffuseColors = gltf.Materials.ToDictionary(m => m.Name, m => m.Diffuse);
            void ShadedRenkleriDuzelt(IEnumerable<Entity> entities)
            {
                foreach (Entity entity in entities)
                {
                    if (entity is Mesh mesh)
                    {
                        mesh.ColorMethod = colorMethodType.byEntity;
                        if (mesh.MaterialName != null &&
                            diffuseColors.TryGetValue(mesh.MaterialName, out var color))
                            mesh.Color = color;
                        else if (mesh.Color.R == 0 && mesh.Color.G == 0 && mesh.Color.B == 0)
                            mesh.Color = System.Drawing.Color.LightGray;
                    }
                }
            }
            ShadedRenkleriDuzelt(gltf.Entities);
            foreach (Block block in gltf.Blocks) ShadedRenkleriDuzelt(block.Entities);

            // 4. Material ve bloklarý ekle
            foreach (Material m in gltf.Materials)
                if (!manufacture1.Materials.Contains(m.Name)) manufacture1.Materials.Add(m);

            foreach (Layer l in gltf.Layers)
                if (!manufacture1.Layers.Contains(l.Name)) manufacture1.Layers.Add(l);

            foreach (Block b in gltf.Blocks)
                manufacture1.Blocks.Add(b);

            // 5. Sadece kök entityleri döndür ve orijine taþý
            List<Entity> kokler = gltf.Entities.ToList();
            foreach (Entity ent in kokler)
            {
                ent.Scale(1000.0);

                ent.Translate(-machineWorkOrigin.X, -machineWorkOrigin.Y, -machineWorkOrigin.Z);
            }

            return kokler;
        }

        
        private static string GltfIsimleriniDuzelt(string yol)
        {
            bool glb = Path.GetExtension(yol).Equals(".glb", StringComparison.OrdinalIgnoreCase);
            byte[]? binChunk = null;
            string json;

            if (glb)
            {
                byte[] data = File.ReadAllBytes(yol);
                int jsonLen = BitConverter.ToInt32(data, 12);            // 1. chunk = JSON
                json = Encoding.UTF8.GetString(data, 20, jsonLen);
                int binStart = 20 + jsonLen;
                if (binStart < data.Length) binChunk = data[binStart..]; // BIN chunk (baþlýðýyla)
            }
            else json = File.ReadAllText(yol);

            JsonNode root = JsonNode.Parse(json)!;
            bool degisti = false;
            degisti |= IsimVer(root["scenes"] as JsonArray, "scene");
            degisti |= IsimVer(root["nodes"] as JsonArray, "node");
            degisti |= IsimVer(root["meshes"] as JsonArray, "mesh");
            degisti |= IsimVer(root["materials"] as JsonArray, "mat");
            if (!degisti) return yol;

            string yeni = Path.Combine(Path.GetDirectoryName(yol)!,
                Path.GetFileNameWithoutExtension(yol) + "__" + Guid.NewGuid().ToString("N") + Path.GetExtension(yol));

            if (!glb)
            {
                File.WriteAllText(yeni, root.ToJsonString());
                return yeni;
            }

            byte[] j = Encoding.UTF8.GetBytes(root.ToJsonString());
            int pad = (4 - j.Length % 4) % 4;
            int jsonChunkLen = j.Length + pad;
            int toplam = 12 + 8 + jsonChunkLen + (binChunk?.Length ?? 0);

            using (var bw = new BinaryWriter(File.Create(yeni)))
            {
                // GLB baþlýðý
                bw.Write(0x46546C67u);          
                bw.Write(2u);                  
                bw.Write((uint)toplam);        
                // JSON chunk
                bw.Write((uint)jsonChunkLen);
                bw.Write(0x4E4F534Au);          
                bw.Write(j);
                for (int i = 0; i < pad; i++) bw.Write((byte)0x20);
                
                if (binChunk != null) bw.Write(binChunk);
            }
            return yeni;
        }

        private static bool IsimVer(JsonArray? dizi, string onEk)
        {
            if (dizi == null) return false;
            var kullanilan = new HashSet<string>();
            bool degisti = false;
            for (int i = 0; i < dizi.Count; i++)
            {
                if (dizi[i] is not JsonObject o) continue;
                string? ad = o["name"]?.GetValue<string>();
                if (string.IsNullOrWhiteSpace(ad) || !kullanilan.Add(ad))
                {
                    string yeniAd = $"{onEk}_{i}";
                    while (!kullanilan.Add(yeniAd)) yeniAd += "_";
                    o["name"] = yeniAd;
                    degisti = true;
                }
            }
            return degisti;
        }

    }
}


namespace talasKaldirma
{
    internal static class TnmTakimDestegi
    {
        internal const int CncNo = 1;

        
        internal static string NcHazirla(string path, out Dictionary<int, string> isimler)
        {
            isimler = new Dictionary<int, string>();
            var lines = new List<string>();
            bool firstRapid = false;
            int lineNo = 0;
            foreach (string line in File.ReadAllLines(path))
            {
                lineNo++;
                Match match = Regex.Match(line,
                    @"^\s*(?:N\d+\s*)?TNM\s*=\s*\((?<name>.+)\)\s*(?:;.*)?$",
                    RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    string name = match.Groups["name"].Value.Trim();
                    if (name.Length == 0) throw new InvalidDataException($"Boþ TNM: satýr {lineNo}.");
                    int id = isimler.Count + 1;
                    isimler.Add(id, name);
                    lines.Add($"T{id} M06");
                    continue;
                }
                string clean = Regex.Replace(line, @"\([^)]*\)", "").Split(';')[0];
                if (Regex.IsMatch(clean, @"\bTNM\s*=", RegexOptions.IgnoreCase))
                    throw new InvalidDataException($"TNM biçimi geçersiz: satýr {lineNo}: {line}");
                if (Regex.IsMatch(clean, @"\bT\d+\b|\bM0?6\b", RegexOptions.IgnoreCase))
                    throw new InvalidDataException("TNM ile sayýsal T/M06 karýþýk kullanýlmýþ. Bu sürüm TNM ile takým seçer.");

                if (Regex.IsMatch(clean, @"^\s*(?:N\d+\s*)?[A-Za-z]\w*\s*=")) continue;
                bool rapidXY = Regex.IsMatch(clean, @"\bG0?0\b", RegexOptions.IgnoreCase) &&
                               Regex.IsMatch(clean, @"\b[XY][-+]?\d", RegexOptions.IgnoreCase);
                if (!firstRapid && rapidXY)
                {
                    lines.Add("G00 Z30.0");
                    firstRapid = true;
                }
                lines.Add(line);
            }
            if (isimler.Count == 0) throw new InvalidDataException("NC dosyasýnda TNM=(TAKIM ADI) bulunamadý.");
            string temp = Path.Combine(Path.GetTempPath(), $"Eyeshot_TNM_{Guid.NewGuid():N}.NC");
            File.WriteAllLines(temp, lines);
            return temp;
        }

        internal static EndMill TakimBul(string dbPath, string name, int simulationNumber)
        {
            if (!File.Exists(dbPath)) throw new FileNotFoundException("CncDb veritabaný bulunamadý.", dbPath);
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Mode = SqliteOpenMode.ReadOnly
            };
            using var connection = new SqliteConnection(builder.ToString());
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Cap, KoseYaricapi, KesmeYuksekligi, Uzunluk,
                       Tip, KonikAcisi, MatkapKonikAcisi
                FROM TakimlarTablosu
                WHERE Ad = $ad COLLATE NOCASE AND CncNo = $cnc
                LIMIT 2";
            command.Parameters.AddWithValue("$ad", name);
            command.Parameters.AddWithValue("$cnc", CncNo);
            using var reader = command.ExecuteReader();
            if (!reader.Read())
                throw new InvalidOperationException($"Takým bulunamadý: '{name}' (CNC {CncNo}).");
            // Reader'ý ikinci kayda ilerletmeden önce deðerleri al.
            var values = new object[7];
            reader.GetValues(values);
            if (reader.Read())
                throw new InvalidOperationException($"'{name}' için birden fazla kayýt var (CNC {CncNo}). Takým adlarý tekil olmalý.");
            return EndMillOlustur(values, name, simulationNumber);
        }

        internal static EndMill EndMillOlustur(object[] row, string name, int number)
        {
            double d = Sayi(row[0], "Cap", name);
            double r = Sayi(row[1], "KoseYaricapi", name, 0);
            double length = Sayi(row[3], "Uzunluk", name);
            
            // endmill methoduna göre uzunluk toplam boy kabul ediliyort  kalan bölüm þaft boyu ama excellerde öyle bi ayrým yok
            //double h = Sayi(row[2], "KesmeYuksekligi", name, length);

            string kesmeYuksekligi = Convert.ToString(row[2])?.Trim() ?? "";

            double h = string.IsNullOrEmpty(kesmeYuksekligi) || kesmeYuksekligi == "-"
                ? length
                : Sayi(row[2], "KesmeYuksekligi", name);


            double shaft = length - h;
            string type = Convert.ToString(row[4], CultureInfo.InvariantCulture)?.Trim() ?? "";
            double taperDegrees = Sayi(row[5], "KonikAcisi", name, 0);
            double tipDegrees = Sayi(row[6], "MatkapKonikAcisi", name, 0);
            if (d <= 0 || length <= 0 || h <= 0 || shaft < 0)
                throw new InvalidOperationException($"'{name}': çap/boy pozitif olmalý ve KesmeYuksekligi, Uzunluk'tan büyük olmamalý.");
            if (type.Equals("Oval", StringComparison.OrdinalIgnoreCase)) r = d / 2;

            if (type.Equals("Matkap", StringComparison.OrdinalIgnoreCase) && tipDegrees == 0)
                tipDegrees = 118;
            if (r < 0 || r > d / 2 || tipDegrees < 0 || tipDegrees > 180 || taperDegrees < 0 || taperDegrees >= 90)
                throw new InvalidOperationException($"'{name}': yarýçap veya açý aralýðý geçersiz.");
           
            double radians = Math.PI / 180;
            double alpha = tipDegrees > 0 ? (90 - tipDegrees / 2) * radians : 0;
            return new EndMill(d, alpha, r, taperDegrees * radians, h, shaft, number);
        }

        private static double Sayi(object value, string column, string name, double? emptyDefault = null)
        {
            string text = value == DBNull.Value ? "" : Convert.ToString(value, CultureInfo.InvariantCulture)?.Trim() ?? "";
            if (text.Length == 0 || text == "-")
                return 0;
            if (!double.TryParse(text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out double result) ||
                double.IsNaN(result) || double.IsInfinity(result))
                throw new InvalidOperationException($"'{name}': {column} sayýsal deðil: '{text}'.");
            return result;
        }
    }
}
