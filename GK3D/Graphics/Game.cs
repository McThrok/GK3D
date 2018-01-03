﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.IO;
using System.Drawing;
using OpenTK.Input;
using System.Drawing.Imaging;
using GK3D.Graphics.Objects;

namespace GK3D.Graphics
{

    class Game : GameWindow
    {
        Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        string activeShader = "default";

        Dictionary<string, int> textures = new Dictionary<string, int>();
        Vector2[] texcoorddata;

        Dictionary<string, Material> materials = new Dictionary<string, Material>();

        Light activeLight = new Light(new Vector3(), new Vector3(0.9f, 0.80f, 0.8f));
        private int ibo_elements;
        private Vector3[] vertdata;
        private Vector3[] coldata;
        Vector3[] normdata;
        Matrix4 view = Matrix4.Identity;

        private int[] indicedata;
        private List<Volume> objects = new List<Volume>();
        private Camera cam = new Camera();
        Vector2 lastMousePos = new Vector2();

        float time = 0.0f;
        public Game() : base(512, 512, new GraphicsMode(32, 24, 0, 4))
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitProgram();

            Title = "Hello OpenTK!";
            GL.ClearColor(Color.CornflowerBlue);
            GL.PointSize(5f);
        }
        void InitProgram()
        {
            lastMousePos = new Vector2(Mouse.X, Mouse.Y);

            GL.GenBuffers(1, out ibo_elements);

            // Load shaders from file
            shaders.Add("default", new ShaderProgram("Graphics\\Shaders\\vs.glsl", "Graphics\\Shaders\\fs.glsl", true));
            //shaders.Add("textured", new ShaderProgram("vs_tex.glsl", "fs_tex.glsl", true));
           // shaders.Add("normal", new ShaderProgram("vs_norm.glsl", "fs_norm.glsl", true));
            shaders.Add("lit", new ShaderProgram("Graphics\\Shaders\\vs_lit.glsl", "Graphics\\Shaders\\fs_lit.glsl", true));

            activeShader = "lit";
            loadMaterials("Graphics\\Materials\\opentk.mtl");

            // Create our objects
            textures.Add("opentksquare.png", loadImage("Graphics\\Textures\\opentksquare.png"));
            TexturedCube tc = new TexturedCube();
            tc.TextureID = textures[materials["opentk1"].DiffuseMap];
            tc.CalculateNormals();
            tc.Material = materials["opentk1"];
            objects.Add(tc);

            textures.Add("opentksquare2.png", loadImage("Graphics\\Textures\\opentksquare2.png"));
            TexturedCube tc2 = new TexturedCube();
            tc2.Position += new Vector3(1f, 1f, 1f);
            tc2.TextureID = textures[materials["opentk2"].DiffuseMap];
            tc2.CalculateNormals();
            tc2.Material = materials["opentk2"];
            objects.Add(tc2);

            // Move camera away from origin
            cam.Position += new Vector3(0f, 0f, 3f);

            textures.Add("earth.png", loadImage("Graphics\\Textures\\earth.png"));
            ObjVolume earth = ObjVolume.LoadFromFile("Graphics\\Models\\earth.obj");
            earth.TextureID = textures["earth.png"];
            earth.Position += new Vector3(1f, 1f, -2f);
            earth.Material = new Material(new Vector3(0.15f), new Vector3(1), new Vector3(0.2f), 5);
            objects.Add(earth);

            //ObjVolume car = ObjVolume.LoadFromFile("Low-Poly-Racing-Car.obj");
            //car.Scale = new Vector3(0.001f);
            //car.Position += new Vector3(1f, 1f, -2f);
            //objects.Add(car);


        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Viewport(0, 0, Width, Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.UseProgram(shaders[activeShader].ProgramID);
            shaders[activeShader].EnableVertexAttribArrays();

            int indiceat = 0;

            foreach (Volume v in objects)
            {
                GL.BindTexture(TextureTarget.Texture2D, v.TextureID);

                GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref v.ModelViewProjectionMatrix);

                if (shaders[activeShader].GetUniform("maintexture") != -1)
                {
                    GL.Uniform1(shaders[activeShader].GetUniform("maintexture"), 0);
                }

                if (shaders[activeShader].GetUniform("view") != -1)
                {
                    GL.UniformMatrix4(shaders[activeShader].GetUniform("view"), false, ref view);
                }

                if (shaders[activeShader].GetUniform("model") != -1)
                {
                    GL.UniformMatrix4(shaders[activeShader].GetUniform("model"), false, ref v.ModelMatrix);
                }

                if (shaders[activeShader].GetUniform("material_ambient") != -1)
                {
                    GL.Uniform3(shaders[activeShader].GetUniform("material_ambient"), ref v.Material.AmbientColor);
                }

                if (shaders[activeShader].GetUniform("material_diffuse") != -1)
                {
                    GL.Uniform3(shaders[activeShader].GetUniform("material_diffuse"), ref v.Material.DiffuseColor);
                }

                if (shaders[activeShader].GetUniform("material_specular") != -1)
                {
                    GL.Uniform3(shaders[activeShader].GetUniform("material_specular"), ref v.Material.SpecularColor);
                }

                if (shaders[activeShader].GetUniform("material_specExponent") != -1)
                {
                    GL.Uniform1(shaders[activeShader].GetUniform("material_specExponent"), v.Material.SpecularExponent);
                }

                if (shaders[activeShader].GetUniform("light_position") != -1)
                {
                    GL.Uniform3(shaders[activeShader].GetUniform("light_position"), ref activeLight.Position);
                }

                if (shaders[activeShader].GetUniform("light_color") != -1)
                {
                    GL.Uniform3(shaders[activeShader].GetUniform("light_color"), ref activeLight.Color);
                }

                if (shaders[activeShader].GetUniform("light_diffuseIntensity") != -1)
                {
                    GL.Uniform1(shaders[activeShader].GetUniform("light_diffuseIntensity"), activeLight.DiffuseIntensity);
                }

                if (shaders[activeShader].GetUniform("light_ambientIntensity") != -1)
                {
                    GL.Uniform1(shaders[activeShader].GetUniform("light_ambientIntensity"), activeLight.AmbientIntensity);
                }

                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.IndiceCount;
            }

            shaders[activeShader].DisableVertexAttribArrays();

            GL.Flush();
            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            // Assemble vertex and indice data for all volumes
            int vertcount = 0;
            foreach (Volume v in objects)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                texcoords.AddRange(v.GetTextureCoords());
                normals.AddRange(v.GetNormals().ToList());
                vertcount += v.VertCount;
            }

            vertdata = verts.ToArray();
            indicedata = inds.ToArray();
            coldata = colors.ToArray();
            texcoorddata = texcoords.ToArray();
            normdata = normals.ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(vertdata.Length * Vector3.SizeInBytes), vertdata, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (shaders[activeShader].GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(coldata.Length * Vector3.SizeInBytes), coldata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Buffer texture coordinates if shader supports it
            if (shaders[activeShader].GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoorddata.Length * Vector2.SizeInBytes), texcoorddata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (shaders[activeShader].GetAttribute("vNormal") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shaders[activeShader].GetBuffer("vNormal"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normdata.Length * Vector3.SizeInBytes), normdata, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shaders[activeShader].GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Update object positions
            time += (float)e.Time;

            //objects[0].Position = new Vector3(0.3f, -0.5f + (float)Math.Sin(time), -3.0f);
            //objects[0].Rotation = new Vector3(0.55f * time, 0.25f * time, 0);
            //objects[0].Scale = new Vector3(0.5f, 0.5f, 0.5f);

            //objects[1].Position = new Vector3(-1f, 0.5f + (float)Math.Cos(time), -2.0f);
            //objects[1].Rotation = new Vector3(-0.25f * time, -0.35f * time, 0);
            //objects[1].Scale = new Vector3(0.7f, 0.7f, 0.7f);

            // Update model view matrices
            foreach (Volume v in objects)
            {
                v.CalculateModelMatrix();
                v.ViewProjectionMatrix = cam.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, ClientSize.Width / (float)ClientSize.Height, 0.1f, 40.0f);
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }

            GL.UseProgram(shaders[activeShader].ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ibo_elements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indicedata.Length * sizeof(int)), indicedata, BufferUsageHint.StaticDraw);


            // Reset mouse position
            if (Focused)
            {
                Vector2 delta = lastMousePos - new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
                lastMousePos += delta;

                cam.AddRotation(delta.X, delta.Y);
                ResetCursor();
            }

            view = cam.GetViewMatrix();

        }

        private int loadImage(string filename)
        {
            try
            {
                Bitmap file = new System.Drawing.Bitmap(filename);
                return loadImage(file);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }
        private int loadImage(Bitmap image)
        {
            int texID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texID);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return texID;
        }
        private void loadMaterials(String filename)
        {
            foreach (var mat in Material.LoadFromFile(filename))
            {
                if (!materials.ContainsKey(mat.Key))
                {
                    materials.Add(mat.Key, mat.Value);
                }
            }

            // Load textures
            foreach (Material mat in materials.Values)
            {
                if (File.Exists(mat.AmbientMap) && !textures.ContainsKey(mat.AmbientMap))
                {
                    textures.Add(mat.AmbientMap, loadImage(mat.AmbientMap));
                }

                if (File.Exists(mat.DiffuseMap) && !textures.ContainsKey(mat.DiffuseMap))
                {
                    textures.Add(mat.DiffuseMap, loadImage(mat.DiffuseMap));
                }

                if (File.Exists(mat.SpecularMap) && !textures.ContainsKey(mat.SpecularMap))
                {
                    textures.Add(mat.SpecularMap, loadImage(mat.SpecularMap));
                }

                if (File.Exists(mat.NormalMap) && !textures.ContainsKey(mat.NormalMap))
                {
                    textures.Add(mat.NormalMap, loadImage(mat.NormalMap));
                }

                if (File.Exists(mat.OpacityMap) && !textures.ContainsKey(mat.OpacityMap))
                {
                    textures.Add(mat.OpacityMap, loadImage(mat.OpacityMap));
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            switch (e.KeyChar)
            {
                case 'w':
                    cam.Move(0f, 0.1f, 0f);
                    break;
                case 'a':
                    cam.Move(-0.1f, 0f, 0f);
                    break;
                case 's':
                    cam.Move(0f, -0.1f, 0f);
                    break;
                case 'd':
                    cam.Move(0.1f, 0f, 0f);
                    break;
                case 'q':
                    cam.Move(0f, 0f, 0.1f);
                    break;
                case 'e':
                    cam.Move(0f, 0f, -0.1f);
                    break;
            }
        }
        void ResetCursor()
        {
            OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
            lastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }
        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (Focused)
            {
                ResetCursor();
            }
        }
    }
}
