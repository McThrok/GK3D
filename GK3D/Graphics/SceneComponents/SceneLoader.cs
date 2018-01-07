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
    public abstract class SceneLoader
    {
        public abstract SceneCollection Load();

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
        protected void LoadMaterials(SceneCollection collection, string filename)
        {
            foreach (var mat in Material.LoadFromFile(filename))
            {
                if (!collection.Materials.ContainsKey(mat.Key))
                {
                    collection.Materials.Add(mat.Key, mat.Value);
                }
            }

            foreach (Material mat in collection.Materials.Values)
            {
                if (File.Exists(mat.AmbientMap) && !collection.Textures.ContainsKey(mat.AmbientMap))
                {
                    collection.Textures.Add(mat.AmbientMap, LoadImage(mat.AmbientMap));
                }

                if (File.Exists(mat.DiffuseMap) && !collection.Textures.ContainsKey(mat.DiffuseMap))
                {
                    collection.Textures.Add(mat.DiffuseMap, LoadImage(mat.DiffuseMap));
                }

                if (File.Exists(mat.SpecularMap) && !collection.Textures.ContainsKey(mat.SpecularMap))
                {
                    collection.Textures.Add(mat.SpecularMap, LoadImage(mat.SpecularMap));
                }

                if (File.Exists(mat.NormalMap) && !collection.Textures.ContainsKey(mat.NormalMap))
                {
                    collection.Textures.Add(mat.NormalMap, LoadImage(mat.NormalMap));
                }

                if (File.Exists(mat.OpacityMap) && !collection.Textures.ContainsKey(mat.OpacityMap))
                {
                    collection.Textures.Add(mat.OpacityMap, LoadImage(mat.OpacityMap));
                }
            }
        }
    }
}
