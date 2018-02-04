using GK3D.Graphics.Common;
using GK3D.Graphics.SceneComponents.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.Objects
{
    public class ComplexObject : GameObject
    {
        public List<Light> Lights { get; set; } = new List<Light>();
        public List<Primitive> Primitives { get; set; } = new List<Primitive>();
        public List<Camera> Cameras { get; set; } = new List<Camera>();
        public List<ComplexObject> ComplexObjects { get; set; } = new List<ComplexObject>();


        public List<CollectionItem<Camera>> GetCamerasWiThGlobalModelMatrices()
        {
            var matrix = CalculateModelMatrix();
            var cameras = new List<CollectionItem<Camera>>();

            foreach (var obj in ComplexObjects)
                cameras.AddRange(obj.GetCamerasWiThGlobalModelMatrices());

            cameras.AddRange(Cameras.Select(x => new CollectionItem<Camera>()
            {
                Object = x,
                GlobalModelMatrix = x.CalculateModelMatrix(),
            }));

            foreach (var camera in cameras)
                camera.GlobalModelMatrix *= matrix;

            return cameras;
        }

        public List<CollectionItem<Light>> GetLightsWiThGlobalModelMatrices()
        {
            var matrix = CalculateModelMatrix();
            var lights = new List<CollectionItem<Light>>();

            foreach (var obj in ComplexObjects)
                lights.AddRange(obj.GetLightsWiThGlobalModelMatrices());

            lights.AddRange(Lights.Select(x => new CollectionItem<Light>()
            {
                Object = x,
                GlobalModelMatrix = x.CalculateModelMatrix(),
            }));

            foreach (var light in lights)
                light.GlobalModelMatrix *= matrix;

            return lights;
        }

        public List<CollectionItem<Primitive>> GetPrimitivesWiThGlobalModelMatrices()
        {
            var matrix = CalculateModelMatrix();
            var primitives = new List<CollectionItem<Primitive>>();

            foreach (var obj in ComplexObjects)
                primitives.AddRange(obj.GetPrimitivesWiThGlobalModelMatrices());

            primitives.AddRange(Primitives.Select(x => new CollectionItem<Primitive>()
            {
                Object = x,
                GlobalModelMatrix = x.CalculateModelMatrix(),
            }));

            foreach (var primitive in primitives)
                primitive.GlobalModelMatrix *= matrix;

            return primitives;
        }


    }
}
