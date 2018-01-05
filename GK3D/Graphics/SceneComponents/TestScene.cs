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

        protected override void Load()
        {
            Collection = new SceneCollection();
            Collection.Lights.Add("mainLight", new Light(new Vector3(), new Vector3(0.8f, 0.8f, 0.8f)));
            ActiveLights = Collection.Lights.Values.First();

            // Load shaders from file
            Collection.Shaders.Add("lit_mat", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit_mat.glsl", "Graphics\\Resources\\Shaders\\fs_lit_mat.glsl", true));
            //Collection.Shaders.Add("default", new ShaderProgram("Graphics\\Resources\\Shaders\\vs.glsl", "Graphics\\Resources\\Shaders\\fs.glsl", true));
            // Collection.Shaders.Add("lit_color", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit_color.glsl", "Graphics\\Resources\\Shaders\\fs_lit_color.glsl", true));
            //  Collection.Shaders.Add("lit", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit.glsl", "Graphics\\Resources\\Shaders\\fs_lit.glsl", true));
            ActiveShader = Collection.Shaders.Values.First();


            Collection.Textures.Add("opentksquare.png", LoadImage("Graphics\\Resources\\Textures\\opentksquare.png"));
            Collection.Textures.Add("opentksquare2.png", LoadImage("Graphics\\Resources\\Textures\\opentksquare2.png"));
            LoadMaterials("Graphics\\Resources\\Materials\\opentk.mtl");

            // Create our objects
            Cube cube = new Cube();
            cube.Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5);
            cube.TextureID = Collection.Textures[Collection.Materials["opentk1"].DiffuseMap];
            cube.Position = new Vector3(2f, 0, 0);
            cube.Rotation = new Vector3(0, (float)Math.PI / 2, 0);
            cube.CalculateNormals();
            Collection.Objects.Add("cube", cube);

            Cube cube1 = new Cube();
            cube1.Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5);
            cube1.TextureID = Collection.Textures[Collection.Materials["opentk1"].DiffuseMap];
            cube1.Position = new Vector3(0, 0, 0);
            cube1.Rotation = new Vector3(0, (float)Math.PI / 2, 0);
            cube1.CalculateNormals();
            Collection.Objects.Add("cube1", cube1);

            // Move camera away from origin
            Camera cam = new Camera();
            cam.Position += new Vector3(0f, 0f, 3f);
            Collection.Cameras.Add("mainCamera", cam);
            ActiveCamera = Collection.Cameras.Values.First();
        }

        protected override void SetStart()
        {
        }
        protected override void Process(float deltaTime)
        {

        }
       
    }
}
