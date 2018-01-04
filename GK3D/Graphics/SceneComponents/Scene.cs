using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;

namespace GK3D.Graphics.SceneComponents
{
    public abstract class Scene
    {
        public SceneCollection Collection { get;  set; }
        public ShaderProgram ActiveShader { get; set; }
        public Camera ActiveCamera { get; set; }
        public Light ActiveLights { get; set; }

        public Scene()
        {
            Collection = new SceneCollection();
            Load();
        }
        protected abstract void Load();
        protected abstract void SetStart();
        protected abstract void Process(float deltaTime);

        protected int LoadImage(string filename)
        {
            try
            {
                Bitmap file = new Bitmap(filename);
                return LoadImage(file);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }
        protected int LoadImage(Bitmap image)
        {
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }
        protected void LoadMaterials(string filename)
        {
            foreach (var mat in Material.LoadFromFile(filename))
            {
                if (!Collection.Materials.ContainsKey(mat.Key))
                {
                    Collection.Materials.Add(mat.Key, mat.Value);
                }
            }

            foreach (Material mat in Collection.Materials.Values)
            {
                if (File.Exists(mat.AmbientMap) && !Collection.Textures.ContainsKey(mat.AmbientMap))
                {
                    Collection.Textures.Add(mat.AmbientMap, LoadImage(mat.AmbientMap));
                }

                if (File.Exists(mat.DiffuseMap) && !Collection.Textures.ContainsKey(mat.DiffuseMap))
                {
                    Collection.Textures.Add(mat.DiffuseMap, LoadImage(mat.DiffuseMap));
                }

                if (File.Exists(mat.SpecularMap) && !Collection.Textures.ContainsKey(mat.SpecularMap))
                {
                    Collection.Textures.Add(mat.SpecularMap, LoadImage(mat.SpecularMap));
                }

                if (File.Exists(mat.NormalMap) && !Collection.Textures.ContainsKey(mat.NormalMap))
                {
                    Collection.Textures.Add(mat.NormalMap, LoadImage(mat.NormalMap));
                }

                if (File.Exists(mat.OpacityMap) && !Collection.Textures.ContainsKey(mat.OpacityMap))
                {
                    Collection.Textures.Add(mat.OpacityMap, LoadImage(mat.OpacityMap));
                }
            }
        }
    }
}
