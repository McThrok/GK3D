using GK3D.Graphics.Objects;
using GK3D.Graphics.SceneComponents.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GK3D.Graphics.SceneComponents.Test
{
    public class TestSceneLoader : SceneLoader
    {
        public TestSceneLoader() : base() { }

        public override SceneCollection Load()
        {
            var collection = new SceneCollection();

            collection.Lights.Add("mainLight", new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f));
            Light spotLight2 = new Light(new Vector3(0, 3f, 0), new Vector3(1f, 1f, 1f), 1, 1);
            spotLight2.Type = LightType.Spot;
            spotLight2.Rotation = new Vector3(0, 1.0f, 0).Normalized();
            spotLight2.ConeAngle = 10f;
            collection.Lights.Add("spotLight2", spotLight2);

            collection.ActiveLights = collection.Lights.Values.FirstOrDefault();

            // Load shaders from file
            collection.Shaders.Add("lit_mat", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit_mat.glsl", "Graphics\\Resources\\Shaders\\fs_lit_mat.glsl", true));
            collection.ActiveShader = collection.Shaders.Values.FirstOrDefault();

            Capsule2D capsule = new Capsule2D(1, new Vector3(1, 1, 0), 1);
            capsule.Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5);
            capsule.Position = new Vector3(0, 0, -3f);
            capsule.CalculateNormals();
            collection.Objects.Add("cap", capsule);


            Cube cubex = new ColoredCube(new Vector3(1, 1, 0))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(2f, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            cubex.CalculateNormals();
            collection.Objects.Add("x", cubex);


            Cube center = new ColoredCube(new Vector3(0, 0, 1))
            {
                Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0),
                Scale = new Vector3(3f, 3f, 3f)
            };
            center.CalculateNormals();
            // Collection.Objects.Add("center", center);



            Cube car = new ColoredCube(new Vector3(1, 1, 1))
            {
                Material = new Material(new Vector3(0.1f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(2f, 2f, 2f),
                Rotation = new Vector3(0, 0, 0)
            };
            car.CalculateNormals();
            //Collection.Objects.Add("car", car);

            //ComplexObject co = new ComplexObject();
            //co.Objects.Add("car", Collection.Objects["car"]);
            //co.Objects.Add("center", Collection.Objects["center"]);
            //co.Rotate(new Vector3(0, (float)Math.PI / 16, 0));
            //Collection.ComplexObjects.Add("co", co);


            //Move camera away from origin
            Camera cam = new Camera();
            cam.Position += new Vector3(0, 0f, 7f);
            cam.Rotation = new Vector3(0f, (float)Math.PI, 0f);
            collection.Cameras.Add("mainCamera", cam);


            Camera secondCamera = new Camera();
            secondCamera.Position = new Vector3(0f, 8f, 0);
            secondCamera.Rotation = new Vector3(-(float)Math.PI / 2 + 0.1f, (float)Math.PI, 0f);
            collection.Cameras.Add("secondCamera", secondCamera);


            collection.ActiveCamera = collection.Cameras.Values.FirstOrDefault();

            return collection;
        }
    }
}
