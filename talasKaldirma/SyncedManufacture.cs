using System;
using devDept.Eyeshot;
using devDept.Eyeshot.Control;
using devDept.Geometry;

namespace talasKaldirma
{
    public class SyncedManufacture : Manufacture
    {
        public event Action<Point3D>? BeforeMachineDraw;

        protected override void DrawViewport(DrawSceneParams data)
        {

            var toolRef = ToolBlockRef;
            if (toolRef != null && BeforeMachineDraw != null)
            {
                var tip = new Point3D(0, 0, 0);
                tip.TransformBy(toolRef.Transformation);
                BeforeMachineDraw(tip);
            }
            base.DrawViewport(data);
        }
    }
}