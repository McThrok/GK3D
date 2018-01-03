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
        private int _iboElements;
        Matrix4 view;

        public FrameManager()
        {
            GL.GenBuffers(1, out _iboElements);
        }

        public void RenderFrame(Scene scene)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            GL.UseProgram(scene.ActiveShader.ProgramID);

            scene.ActiveShader.EnableVertexAttribArrays();

            int indiceat = 0;

            foreach (var v in scene.Collection.Objects.Values)
            {
                GL.BindTexture(TextureTarget.Texture2D, v.TextureID);

                GL.UniformMatrix4(scene.ActiveShader.GetUniform("modelview"), false, ref v.ModelViewProjectionMatrix);

                if (scene.ActiveShader.GetUniform("maintexture") != -1)
                {
                    GL.Uniform1(scene.ActiveShader.GetUniform("maintexture"), 0);
                }

                if (scene.ActiveShader.GetUniform("view") != -1)
                {
                    GL.UniformMatrix4(scene.ActiveShader.GetUniform("view"), false, ref view);
                }

                if (scene.ActiveShader.GetUniform("model") != -1)
                {
                    GL.UniformMatrix4(scene.ActiveShader.GetUniform("model"), false, ref v.ModelMatrix);
                }

                if (scene.ActiveShader.GetUniform("material_ambient") != -1)
                {
                    GL.Uniform3(scene.ActiveShader.GetUniform("material_ambient"), ref v.Material.AmbientColor);
                }

                if (scene.ActiveShader.GetUniform("material_diffuse") != -1)
                {
                    GL.Uniform3(scene.ActiveShader.GetUniform("material_diffuse"), ref v.Material.DiffuseColor);
                }

                if (scene.ActiveShader.GetUniform("material_specular") != -1)
                {
                    GL.Uniform3(scene.ActiveShader.GetUniform("material_specular"), ref v.Material.SpecularColor);
                }

                if (scene.ActiveShader.GetUniform("material_specExponent") != -1)
                {
                    GL.Uniform1(scene.ActiveShader.GetUniform("material_specExponent"), v.Material.SpecularExponent);
                }

                if (scene.ActiveShader.GetUniform("light_position") != -1)
                {
                    GL.Uniform3(scene.ActiveShader.GetUniform("light_position"), ref scene.ActiveLights.Position);
                }

                if (scene.ActiveShader.GetUniform("light_color") != -1)
                {
                    GL.Uniform3(scene.ActiveShader.GetUniform("light_color"), ref scene.ActiveLights.Color);
                }

                if (scene.ActiveShader.GetUniform("light_diffuseIntensity") != -1)
                {
                    GL.Uniform1(scene.ActiveShader.GetUniform("light_diffuseIntensity"), scene.ActiveLights.DiffuseIntensity);
                }

                if (scene.ActiveShader.GetUniform("light_ambientIntensity") != -1)
                {
                    GL.Uniform1(scene.ActiveShader.GetUniform("light_ambientIntensity"), scene.ActiveLights.AmbientIntensity);
                }

                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.IndiceCount;
            }

            scene.ActiveShader.DisableVertexAttribArrays();

            GL.Flush();

        }
        public void UpdateFrame(Scene scene, float aspect)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> inds = new List<int>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texcoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();

            // Assemble vertex and indice data for all volumes
            int vertcount = 0;
            foreach (var v in scene.Collection.Objects.Values)
            {
                verts.AddRange(v.GetVerts().ToList());
                inds.AddRange(v.GetIndices(vertcount).ToList());
                colors.AddRange(v.GetColorData().ToList());
                texcoords.AddRange(v.GetTextureCoords());
                normals.AddRange(v.GetNormals().ToList());
                vertcount += v.VertCount;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, scene.ActiveShader.GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(verts.Count * Vector3.SizeInBytes), verts.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(scene.ActiveShader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (scene.ActiveShader.GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, scene.ActiveShader.GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colors.Count * Vector3.SizeInBytes), colors.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(scene.ActiveShader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Buffer texture coordinates if shader supports it
            if (scene.ActiveShader.GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, scene.ActiveShader.GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoords.Count * Vector2.SizeInBytes), texcoords.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(scene.ActiveShader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (scene.ActiveShader.GetAttribute("vNormal") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, scene.ActiveShader.GetBuffer("vNormal"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normals.Count * Vector3.SizeInBytes), normals.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(scene.ActiveShader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Update object positions

            // Update model view matrices
            foreach (var v in scene.Collection.Objects.Values)
            {
                v.CalculateModelMatrix();
                v.ViewProjectionMatrix = scene.ActiveCamera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, aspect, 0.1f, 40.0f);
                v.ModelViewProjectionMatrix = v.ModelMatrix * v.ViewProjectionMatrix;
            }

            GL.UseProgram(scene.ActiveShader.ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboElements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(inds.Count * sizeof(int)), inds.ToArray(), BufferUsageHint.StaticDraw);


            view = scene.ActiveCamera.GetViewMatrix();
           

        }
    }
}
