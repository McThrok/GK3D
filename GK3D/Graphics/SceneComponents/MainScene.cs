using GK3D.Graphics.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents
{
    public class MainScene : Scene
    {
        public MainScene() : base() { }

        protected override void Load()
        {
            Collection.Lights.Add("mainLight", new Light(new Vector3(), new Vector3(0.9f, 0.80f, 0.8f)));
            ActiveLights = Collection.Lights.Values.First();

            // Load shaders from file
            Collection.Shaders.Add("lit", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit.glsl", "Graphics\\Resources\\Shaders\\fs_lit.glsl", true));
            Collection.Shaders.Add("default", new ShaderProgram("Graphics\\Resources\\Shaders\\vs.glsl", "Graphics\\Resources\\Shaders\\fs.glsl", true));
            ActiveShader = Collection.Shaders.Values.First();

            Collection.Textures.Add("opentksquare.png", LoadImage("Graphics\\Resources\\Textures\\opentksquare.png"));
            Collection.Textures.Add("opentksquare2.png", LoadImage("Graphics\\Resources\\Textures\\opentksquare2.png"));
            LoadMaterials("Graphics\\Resources\\Materials\\opentk.mtl");


            // Create our objects
            TexturedCube tc1 = new TexturedCube();
            tc1.TextureID = Collection.Textures[Collection.Materials["opentk1"].DiffuseMap];
            tc1.CalculateNormals();
            tc1.Material = Collection.Materials["opentk1"];
            Collection.Objects.Add("cube1", tc1);

            TexturedCube tc2 = new TexturedCube();
            tc2.Position += new Vector3(1f, 1f, 1f);
            tc2.TextureID = Collection.Textures[Collection.Materials["opentk2"].DiffuseMap];
            tc2.CalculateNormals();
            tc2.Material = Collection.Materials["opentk2"];
            Collection.Objects.Add("cube2", tc2);

            Collection.Textures.Add("earth.png", LoadImage("Graphics\\Resources\\Textures\\earth.png"));
            ObjVolume earth = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\earth.obj");
            earth.TextureID = Collection.Textures["earth.png"];
            earth.Position += new Vector3(1f, 1f, -2f);
            earth.Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5);
            Collection.Objects.Add("earth", earth);

            // Move camera away from origin
            Camera cam = new Camera();
            cam.Position += new Vector3(0f, 0f, 3f);
            Collection.Cameras.Add("mainCamera", cam);
            ActiveCamera = Collection.Cameras.Values.First();
        }

    }
}
