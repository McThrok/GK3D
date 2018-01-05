using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using GK3D.Graphics.SceneComponents;

namespace GK3D.Graphics
{
    public class FrameManager
    {
        private const int MaxLight = 30; //taken from shader

        public Scene Scene { get; set; }
        private Matrix4 _viewCameraMatrix;
        private int _iboElements;

        public FrameManager()
        {
            GL.GenBuffers(1, out _iboElements);
        }

        public void RenderFrame()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.UseProgram(Scene.ActiveShader.ProgramID);
            Scene.ActiveShader.EnableVertexAttribArrays();

            int indiceat = 0;
            var lights = Scene.Collection.Lights.Values.ToList();

            foreach (var v in Scene.Collection.Objects.Values)
            {
                GL.BindTexture(TextureTarget.Texture2D, v.TextureID);

                GL.UniformMatrix4(Scene.ActiveShader.GetUniform("modelviewproj"), false, ref v.ModelViewProjectionMatrix);

                if (Scene.ActiveShader.GetUniform("maintexture") != -1)
                {
                    GL.Uniform1(Scene.ActiveShader.GetUniform("maintexture"), 0);
                }

                if (Scene.ActiveShader.GetUniform("viewPos") != -1)
                {
                    GL.Uniform3(Scene.ActiveShader.GetUniform("viewPos"), ref Scene.ActiveCamera.Position);
                }

                if (Scene.ActiveShader.GetUniform("view") != -1)
                {
                    GL.UniformMatrix4(Scene.ActiveShader.GetUniform("view"), false, ref _viewCameraMatrix);
                }

                if (Scene.ActiveShader.GetUniform("model") != -1)
                {
                    GL.UniformMatrix4(Scene.ActiveShader.GetUniform("model"), false, ref v.ModelMatrix);
                }

                if (Scene.ActiveShader.GetUniform("material_ambient") != -1)
                {
                    GL.Uniform3(Scene.ActiveShader.GetUniform("material_ambient"), ref v.Material.AmbientColor);
                }

                if (Scene.ActiveShader.GetUniform("material_diffuse") != -1)
                {
                    GL.Uniform3(Scene.ActiveShader.GetUniform("material_diffuse"), ref v.Material.DiffuseColor);
                }

                if (Scene.ActiveShader.GetUniform("material_specular") != -1)
                {
                    GL.Uniform3(Scene.ActiveShader.GetUniform("material_specular"), ref v.Material.SpecularColor);
                }

                if (Scene.ActiveShader.GetUniform("material_specExponent") != -1)
                {
                    GL.Uniform1(Scene.ActiveShader.GetUniform("material_specExponent"), v.Material.SpecularExponent);
                }

                if (Scene.ActiveShader.GetUniform("light_position") != -1)
                {
                    GL.Uniform3(Scene.ActiveShader.GetUniform("light_position"), ref Scene.ActiveLights.Position);
                }

                if (Scene.ActiveShader.GetUniform("light_color") != -1)
                {
                    GL.Uniform3(Scene.ActiveShader.GetUniform("light_color"), ref Scene.ActiveLights.Color);
                }

                if (Scene.ActiveShader.GetUniform("light_diffuseIntensity") != -1)
                {
                    GL.Uniform1(Scene.ActiveShader.GetUniform("light_diffuseIntensity"), Scene.ActiveLights.DiffuseIntensity);
                }

                if (Scene.ActiveShader.GetUniform("light_ambientIntensity") != -1)
                {
                    GL.Uniform1(Scene.ActiveShader.GetUniform("light_ambientIntensity"), Scene.ActiveLights.AmbientIntensity);
                }

                for (int i = 0; i < Math.Min(lights.Count, MaxLight); i++)
                {
                    if (Scene.ActiveShader.GetUniform("lights[" + i + "].position") != -1)
                    {
                        GL.Uniform3(Scene.ActiveShader.GetUniform("lights[" + i + "].position"), ref lights[i].Position);
                    }

                    if (Scene.ActiveShader.GetUniform("lights[" + i + "].color") != -1)
                    {
                        GL.Uniform3(Scene.ActiveShader.GetUniform("lights[" + i + "].color"), ref lights[i].Color);
                    }

                    if (Scene.ActiveShader.GetUniform("lights[" + i + "].diffuseIntensity") != -1)
                    {
                        GL.Uniform1(Scene.ActiveShader.GetUniform("lights[" + i + "].diffuseIntensity"), lights[i].DiffuseIntensity);
                    }

                    if (Scene.ActiveShader.GetUniform("lights[" + i + "].ambientIntensity") != -1)
                    {
                        GL.Uniform1(Scene.ActiveShader.GetUniform("lights[" + i + "].ambientIntensity"), lights[i].AmbientIntensity);
                    }
                    if (Scene.ActiveShader.GetUniform("lights[" + i + "].direction") != -1)
                    {
                        GL.Uniform3(Scene.ActiveShader.GetUniform("lights[" + i + "].direction"), ref lights[i].Direction);
                    }

                    if (Scene.ActiveShader.GetUniform("lights[" + i + "].type") != -1)
                    {
                        GL.Uniform1(Scene.ActiveShader.GetUniform("lights[" + i + "].type"), (int)lights[i].Type);
                    }

                    if (Scene.ActiveShader.GetUniform("lights[" + i + "].coneAngle") != -1)
                    {
                        GL.Uniform1(Scene.ActiveShader.GetUniform("lights[" + i + "].coneAngle"), lights[i].ConeAngle);
                    }
                }

                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.IndiceCount;
            }

            Scene.ActiveShader.DisableVertexAttribArrays();
            GL.Flush();
        }
        public void UpdateFrame(float aspect)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            // Assemble vertex and indice data for all volumes
            int vertcount = 0;
            foreach (var v in Scene.Collection.Objects.Values)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                texcoords.AddRange(v.GetTextureCoords());
                normals.AddRange(v.GetNormals().ToList());
                vertcount += v.VertCount;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, Scene.ActiveShader.GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(verts.Count * Vector3.SizeInBytes), verts.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Scene.ActiveShader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (Scene.ActiveShader.GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, Scene.ActiveShader.GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colors.Count * Vector3.SizeInBytes), colors.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(Scene.ActiveShader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Buffer texture coordinates if shader supports it
            if (Scene.ActiveShader.GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, Scene.ActiveShader.GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoords.Count * Vector2.SizeInBytes), texcoords.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(Scene.ActiveShader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (Scene.ActiveShader.GetAttribute("vNormal") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, Scene.ActiveShader.GetBuffer("vNormal"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normals.Count * Vector3.SizeInBytes), normals.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(Scene.ActiveShader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Update object positions

            // Update model view matrices
            foreach (var v in Scene.Collection.Objects.Values)
            {
                v.CalculateModelMatrix();
                v.ViewProjectionMatrix = Scene.ActiveCamera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, aspect, 0.1f, 40.0f);
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }

            GL.UseProgram(Scene.ActiveShader.ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboElements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(inds.Count * sizeof(int)), inds.ToArray(), BufferUsageHint.StaticDraw);


            _viewCameraMatrix = Scene.ActiveCamera.GetViewMatrix();


        }
    }
}
