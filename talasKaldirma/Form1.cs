using devDept.Eyeshot;
using devDept.Eyeshot.Control;
using devDept.Eyeshot.Entities;
using devDept.Eyeshot.Milling;
using devDept.Eyeshot.Translators;
using devDept.Geometry;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;


namespace talasKaldirma
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {

            manufacture1.Grid.Visible = false;
            manufacture1.OriginSymbol.Visible = false;

            //g code oku
            string path = Path.Combine(Application.StartupPath, "nc", "test3.NC");

            // Baþlangýç hareketini otomatik düzelt
            string duzenlenmisPath = BaslangicHareketiniDuzenle(path);

            ReadGCode reader = new ReadGCode(duzenlenmisPath);

            await manufacture1.DoWorkAsync(reader);
            reader.OpenTo(manufacture1);

            //toolpath hesapla
            //foreach (Entity entity in manufacture1.Entities)
            //{
            //    if (entity is Toolpath toolpath)
            //    {
            //        manufacture1.SimulationToolpath = toolpath;
            //        break;
            //    }
            //}

            List<Toolpath> toolpathler = manufacture1.Entities.OfType<Toolpath>().ToList();

            foreach (Toolpath tp in toolpathler)//toolpathi gizle
            {
                tp.Visible = false;
            }

            manufacture1.Invalidate();

            //endmiill oluþtur
            EndMill tool = new EndMill(32.0, 0.0, 32.0, 0.0, 30.0, 50.0);

            manufacture1.SimulationTool = tool;

            //
            manufacture1.SimulationSetup = new Setup("Top", linearUnitsType.Millimeters, Plane.XY, null);


            //stock oluþtur
            Stock stock = new Stock(new LinearPath(0, 0, 2800, 2100), new Interval(0, 18));

            //Stock stock = new Stock(new LinearPath(-100, -50, 100 - (-100), 50 - (-50)), new Interval(-30, 0));


           

            manufacture1.SimulationStock = stock.GetSimulationStock();


            manufacture1.ActiveViewport.SetView(viewType.Trimetric);
            manufacture1.ZoomFit();
            manufacture1.Invalidate();

            manufacture1.PositionChanged += (s, e) =>
            {
                var p = e.CutterLocation;
                System.Diagnostics.Debug.WriteLine(
                    $"motion={p.MotionIndex}  X={p.X:F2}  Y={p.Y:F2}  Z={p.Z:F2}");
            };


            foreach (Toolpath tp in toolpathler)
            {
                manufacture1.SimulationToolpath = tp;
                await SimulasyonuCalistir();
            }
        }

        private Task SimulasyonuCalistir()
        {
            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            Manufacture.SimulationEndedEventHandler? handler = null;
            handler = (s, e) =>
            {
                manufacture1.SimulationEnded -= handler;
                tcs.TrySetResult();
            };
            manufacture1.SimulationEnded += handler;

            manufacture1.Start();
            return tcs.Task;
        }

        //baslangýcta talaþ kaldýrma yapmamalý
        private string BaslangicHareketiniDuzenle(string dosyaYolu)
        {
            string[] satirlar = File.ReadAllLines(dosyaYolu);

            List<string> yeniSatirlar = new();

            bool ilkHareketBulundu = false;

            foreach (string satir in satirlar)
            {
                string temizSatir = satir.Trim();

                // Ýlk G00 XY hareketini bul
                bool xyHareketi =
                    System.Text.RegularExpressions.Regex.IsMatch(
                        temizSatir,
                        @"\bG0?0\b",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                    &&
                    System.Text.RegularExpressions.Regex.IsMatch(
                        temizSatir,
                        @"\b[XY][-+]?\d",
                        System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                if (!ilkHareketBulundu && xyHareketi)
                {
                    // Önce takýmý güvenli yüksekliðe kaldýr
                    yeniSatirlar.Add("G00 Z30.0");

                    ilkHareketBulundu = true;
                }

                yeniSatirlar.Add(satir);
            }

            // Orijinal dosyaya dokunmadan geçici NC oluþtur
            string yeniDosya = Path.Combine(
                Path.GetTempPath(),
                $"Eyeshot_{Guid.NewGuid():N}.NC"
            );

            File.WriteAllLines(yeniDosya, yeniSatirlar);

            return yeniDosya;
        }

 

    }
}

