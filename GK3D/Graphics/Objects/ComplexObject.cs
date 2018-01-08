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
        public Dictionary<string, Light> Lights { get; set; } = new Dictionary<string, Light>();
        public Dictionary<string, Primitive> Primitives { get; set; } = new Dictionary<string, Primitive>();
        public Dictionary<string, Camera> Cameras { get; set; } = new Dictionary<string, Camera>();
        public Dictionary<string, ComplexObject> ComplexObjects { get; set; } = new Dictionary<string, ComplexObject>();

        public List<Light> GetAllLights()
        {
            var lights = new List<Light>(Lights.Values);
            foreach (var obj in ComplexObjects.Values)
                lights.AddRange(obj.GetAllLights());

            return lights;
        }
        public List<Camera> GetAllCameras()
        {
            var cameras = new List<Camera>(Cameras.Values);
            foreach (var obj in ComplexObjects.Values)
                cameras.AddRange(obj.GetAllCameras());

            return cameras;
        }
        public List<Primitive> GetAllPrimitiveObjects()
        {
            var objs = new List<Primitive>(Primitives.Values);
            foreach (var obj in ComplexObjects.Values)
                objs.AddRange(obj.GetAllPrimitiveObjects());

            return objs;
        }

        public IEnumerable<KeyValuePair<Primitive, Matrix4>> GetPrimitivesWiThGlobalModelMatrices()
        {
            var matrix = CalculateModelMatrix();
            var tuples = new List<KeyValuePair<Primitive, Matrix4>>();
            foreach (var obj in ComplexObjects.Values)
                tuples.AddRange(obj.GetPrimitivesWiThGlobalModelMatrices());
            tuples.AddRange(Primitives.Values.Select(x => new KeyValuePair<Primitive, Matrix4>(x, x.CalculateModelMatrix())));

            var updatedTuples = tuples.Select(x => new KeyValuePair<Primitive, Matrix4>(x.Key, matrix * x.Value));
            return updatedTuples;
        }
    }
}
