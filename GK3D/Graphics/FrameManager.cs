using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using GK3D.Graphics.SceneComponents;
using GK3D.Graphics.SceneComponents.Base;

namespace GK3D.Graphics
{
    public class FrameManager
    {
        private const int MaxLight = 30; //taken from shader

        public SceneCollection Collection { get; set; }
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
            GL.UseProgram(Collection.ActiveShader.ProgramID);
            Collection.ActiveShader.EnableVertexAttribArrays();

            int indiceat = 0;
            var lights = Collection.SceneObjects.Lights.Values.ToList();

            foreach (var v in Collection.SceneObjects.GetPrimitivesWiThGlobalModelMatrices())
            {
                GL.BindTexture(TextureTarget.Texture2D, v.Key.TextureID);

                GL.UniformMatrix4(Collection.ActiveShader.GetUniform("modelviewproj"), false, ref v.Key.ModelViewProjectionMatrix);

                if (Collection.ActiveShader.GetUniform("maintexture") != -1)
                {
                    GL.Uniform1(Collection.ActiveShader.GetUniform("maintexture"), 0);
                }

                if (Collection.ActiveShader.GetUniform("viewPos") != -1)
                {
                    GL.Uniform3(Collection.ActiveShader.GetUniform("viewPos"), ref Collection.ActiveCamera.Position);
                }

                if (Collection.ActiveShader.GetUniform("view") != -1)
                {
                    GL.UniformMatrix4(Collection.ActiveShader.GetUniform("view"), false, ref _viewCameraMatrix);
                }

                if (Collection.ActiveShader.GetUniform("model") != -1)
                {
                    GL.UniformMatrix4(Collection.ActiveShader.GetUniform("model"), false, ref v.Key.ModelMatrix);
                }

                if (Collection.ActiveShader.GetUniform("material_ambient") != -1)
                {
                    GL.Uniform3(Collection.ActiveShader.GetUniform("material_ambient"), ref v.Key.Material.AmbientColor);
                }

                if (Collection.ActiveShader.GetUniform("material_diffuse") != -1)
                {
                    GL.Uniform3(Collection.ActiveShader.GetUniform("material_diffuse"), ref v.Key.Material.DiffuseColor);
                }

                if (Collection.ActiveShader.GetUniform("material_specular") != -1)
                {
                    GL.Uniform3(Collection.ActiveShader.GetUniform("material_specular"), ref v.Key.Material.SpecularColor);
                }

                if (Collection.ActiveShader.GetUniform("material_specExponent") != -1)
                {
                    GL.Uniform1(Collection.ActiveShader.GetUniform("material_specExponent"), v.Key.Material.SpecularExponent);
                }

                for (int i = 0; i < Math.Min(lights.Count, MaxLight); i++)
                {
                    if (Collection.ActiveShader.GetUniform("lights[" + i + "].position") != -1)
                    {
                        GL.Uniform3(Collection.ActiveShader.GetUniform("lights[" + i + "].position"), lights[i].Position);
                    }

                    if (Collection.ActiveShader.GetUniform("lights[" + i + "].color") != -1)
                    {
                        GL.Uniform3(Collection.ActiveShader.GetUniform("lights[" + i + "].color"), lights[i].Color);
                    }

                    if (Collection.ActiveShader.GetUniform("lights[" + i + "].diffuseIntensity") != -1)
                    {
                        GL.Uniform1(Collection.ActiveShader.GetUniform("lights[" + i + "].diffuseIntensity"), lights[i].DiffuseIntensity);
                    }

                    if (Collection.ActiveShader.GetUniform("lights[" + i + "].ambientIntensity") != -1)
                    {
                        GL.Uniform1(Collection.ActiveShader.GetUniform("lights[" + i + "].ambientIntensity"), lights[i].AmbientIntensity);
                    }
                    if (Collection.ActiveShader.GetUniform("lights[" + i + "].direction") != -1)
                    {
                        GL.Uniform3(Collection.ActiveShader.GetUniform("lights[" + i + "].direction"), lights[i].Rotation);
                    }

                    if (Collection.ActiveShader.GetUniform("lights[" + i + "].type") != -1)
                    {
                        GL.Uniform1(Collection.ActiveShader.GetUniform("lights[" + i + "].type"), (int)lights[i].Type);
                    }

                    if (Collection.ActiveShader.GetUniform("lights[" + i + "].coneAngle") != -1)
                    {
                        GL.Uniform1(Collection.ActiveShader.GetUniform("lights[" + i + "].coneAngle"), lights[i].ConeAngle);
                    }
                }

                GL.DrawElements(BeginMode.Triangles, v.Key.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.Key.IndiceCount;
            }

            Collection.ActiveShader.DisableVertexAttribArrays();
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
            foreach (var v in Collection.SceneObjects.GetPrimitivesWiThGlobalModelMatrices())
            {
                verts.AddRange(v.Key.GetVerts().ToList());
                inds.AddRange(v.Key.GetIndices(vertcount).ToList());
                colors.AddRange(v.Key.GetColorData().ToList());
                texcoords.AddRange(v.Key.GetTextureCoords());
                normals.AddRange(v.Key.GetNormals().ToList());
                vertcount += v.Key.VertCount;
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, Collection.ActiveShader.GetBuffer("vPosition"));

            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(verts.Count * Vector3.SizeInBytes), verts.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(Collection.ActiveShader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            // Buffer vertex color if shader supports it
            if (Collection.ActiveShader.GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, Collection.ActiveShader.GetBuffer("vColor"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(colors.Count * Vector3.SizeInBytes), colors.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(Collection.ActiveShader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }


            // Buffer texture coordinates if shader supports it
            if (Collection.ActiveShader.GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, Collection.ActiveShader.GetBuffer("texcoord"));
                GL.BufferData<Vector2>(BufferTarget.ArrayBuffer, (IntPtr)(texcoords.Count * Vector2.SizeInBytes), texcoords.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(Collection.ActiveShader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (Collection.ActiveShader.GetAttribute("vNormal") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, Collection.ActiveShader.GetBuffer("vNormal"));
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, (IntPtr)(normals.Count * Vector3.SizeInBytes), normals.ToArray(), BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(Collection.ActiveShader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            // Update object positions

            // Update model view matrices
            foreach (var v in Collection.SceneObjects.GetPrimitivesWiThGlobalModelMatrices())
            {
                v.Key.CalculateModelMatrix();
                v.Key.ModelMatrix = v.Value;
                v.Key.ViewProjectionMatrix = Collection.ActiveCamera.GetViewMatrix() * Matrix4.CreatePerspectiveFieldOfView(1.3f, aspect, 0.1f, 40.0f);
                v.Key.ModelViewProjectionMatrix = v.Key.ModelMatrix * v.Key.ViewProjectionMatrix;
            }

            GL.UseProgram(Collection.ActiveShader.ProgramID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Buffer index data
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboElements);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(inds.Count * sizeof(int)), inds.ToArray(), BufferUsageHint.StaticDraw);


            _viewCameraMatrix = Collection.ActiveCamera.GetViewMatrix();


        }
    }
}
