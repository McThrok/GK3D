using GK3D.Graphics.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GK3D.Graphics.SceneComponents
{
    public class TestScene : Scene
    {
        public TestScene() : base() { }

        public override void Load()
        {
            Collection = new SceneCollection();
            Collection.Lights.Add("mainLight", new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f), 0.3f));
            Light spotLight2 = new Light(new Vector3(0, 3f, 0), new Vector3(1f, 1f, 1f));
            spotLight2.Type = LightType.Spot;
            spotLight2.Rotation = new Vector3(0, 1.0f, 0).Normalized();
            spotLight2.ConeAngle = 10f;
            Collection.Lights.Add("spotLight2", spotLight2);

            ActiveLights = Collection.Lights.Values.FirstOrDefault();

            // Load shaders from file
            Collection.Shaders.Add("lit_mat", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit_mat.glsl", "Graphics\\Resources\\Shaders\\fs_lit_mat.glsl", true));
            ActiveShader = Collection.Shaders.Values.FirstOrDefault();

            // Create our objects
            Cube cubex = new Cube
            {
                Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(2f, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            cubex.CalculateNormals();
            Collection.Objects.Add("x", cubex);

            Cube cubey = new Cube
            {
                Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 5f, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            cubey.CalculateNormals();
            Collection.Objects.Add("y", cubey);

            Cube cubez = new Cube
            {
                Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 0, 3.5f),
                Rotation = new Vector3(0, 0, 0)
            };
            cubez.CalculateNormals();
            Collection.Objects.Add("z", cubez);

            Cube center = new Cube
            {
                Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5),
                Position = new Vector3(0, 0, 0),
                Rotation = new Vector3(0, 0, 0)
            };
            center.CalculateNormals();
            center.Scale = new Vector3(2f, 2f, 2f);
            Collection.Objects.Add("center", center);

            ComplexObject co = new ComplexObject();
            //co.Objects.Add("car", cubex);
            //co.Move(Vector3.One);



            //Move camera away from origin
            Camera cam = new Camera();
            cam.Position += new Vector3(5f, 0f, 0);
            cam.Rotation = new Vector3(0f, (float)Math.PI / 2, 0f);
            Collection.Cameras.Add("mainCamera", cam);


            Camera secondCamera = new Camera();
            secondCamera.Position = new Vector3(0f, 0f, -7f);
            secondCamera.Rotation = new Vector3(0, 0.1f, 0f);
            Collection.Cameras.Add("secondCamera", secondCamera);


            ActiveCamera = Collection.Cameras.Values.FirstOrDefault();
        }

        public override void SetStart()
        {
        }
        public override void Process(float deltaTime)
        {
            // Collection.Objects["car"].Position += new Vector3(2*deltaTime, 0, 0);
        }

    }
}
