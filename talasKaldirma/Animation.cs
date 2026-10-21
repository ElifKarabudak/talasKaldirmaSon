using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using devDept.Geometry;

/// <summary>
/// Hareketli blok: geometriye dokunmadan, çizimde ModelView'a öteleme ekler.
/// SetPosition çok ucuz; Regen gerekmez.
/// </summary>
public class Animation : BlockReference
{
    private double _x, _y, _z;

    public Animation(string blockName) : base(0, 0, 0, blockName, 1, 1, 1, 0) { }

    public void SetPosition(double x, double y, double z)
    {
        _x = x; _y = y; _z = z;
    }

    public override void MoveTo(DrawParams data)
    {
        base.MoveTo(data);
        data.RenderContext.MultMatrixModelView(Transformation.CreateTranslation(_x, _y, _z));
    }

    public override bool IsInFrustum(FrustumParams data, Point3D center, double radius) => true;
}