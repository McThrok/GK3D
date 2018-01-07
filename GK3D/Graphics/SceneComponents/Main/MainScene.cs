using GK3D.Graphics.Objects;
using GK3D.Graphics.SceneComponents.Base;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics.SceneComponents.Main
{
    public class MainScene : SceneLoader
    {
        public override SceneCollection Load()
        {
            var collection = new SceneCollection();

            collection.Lights.Add("mainLight", new Light(new Vector3(), new Vector3(0.9f, 0.80f, 0.8f)));
            collection.ActiveLights = collection.Lights.Values.First();

            // Load shaders from file
            collection.Shaders.Add("lit", new ShaderProgram("Graphics\\Resources\\Shaders\\vs_lit.glsl", "Graphics\\Resources\\Shaders\\fs_lit.glsl", true));
            collection.Shaders.Add("default", new ShaderProgram("Graphics\\Resources\\Shaders\\vs.glsl", "Graphics\\Resources\\Shaders\\fs.glsl", true));
            collection.ActiveShader = collection.Shaders.Values.First();

            collection.Textures.Add("opentksquare.png", LoadImage("Graphics\\Resources\\Textures\\opentksquare.png"));
            collection.Textures.Add("opentksquare2.png", LoadImage("Graphics\\Resources\\Textures\\opentksquare2.png"));
            LoadMaterials(collection,"Graphics\\Resources\\Materials\\opentk.mtl");


            // Create our objects
            TexturedCube tc1 = new TexturedCube();
            tc1.TextureID = collection.Textures[collection.Materials["opentk1"].DiffuseMap];
            tc1.CalculateNormals();
            tc1.Material = collection.Materials["opentk1"];
            collection.Objects.Add("cube1", tc1);

            TexturedCube tc2 = new TexturedCube();
            tc2.Position += new Vector3(1f, 1f, 1f);
            tc2.TextureID = collection.Textures[collection.Materials["opentk2"].DiffuseMap];
            tc2.CalculateNormals();
            tc2.Material = collection.Materials["opentk2"];
            collection.Objects.Add("cube2", tc2);

            collection.Textures.Add("earth.png", LoadImage("Graphics\\Resources\\Textures\\earth.png"));
            ObjVolume earth = ObjVolume.LoadFromFile("Graphics\\Resources\\Models\\earth.obj");
            earth.TextureID = collection.Textures["earth.png"];
            earth.Position += new Vector3(1f, 1f, -2f);
            earth.Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5);
            collection.Objects.Add("earth", earth);

            // Move camera away from origin
            Camera cam = new Camera();
            cam.Position += new Vector3(0f, 0f, 3f);
            collection.Cameras.Add("mainCamera", cam);
            collection.ActiveCamera = collection.Cameras.Values.First();

            return collection;
        }
    }
}
