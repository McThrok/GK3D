﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using OpenTK;
using GK3D.Graphics.SceneComponents;
using GK3D.Graphics.SceneComponents.Base;
using GK3D.Graphics.Objects;
using GK3D.Graphics.Common;

namespace GK3D.Graphics
{
    public class FrameManager
    {
        private const int MaxLight = 30; //taken from shader

        public SceneCollection Collection { get; set; }
        private int _iboElements;

        public FrameManager()
        {
            _iboElements = GL.GenBuffer();
            GL.Enable(EnableCap.CullFace);
        }

        public void RenderFrame(float aspect)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);


            Collection.ActiveCamera.GlobalModelMatrix = Collection.SceneObjects.GetCamerasWiThGlobalModelMatrices().First(x => x.Object.Name == Collection.ActiveCamera.Object.Name).GlobalModelMatrix;

            var view = Collection.ActiveCamera.Object.GetViewMatrix(Collection.ActiveCamera.GlobalModelMatrix);
            var projection = MatrixHelper.CreatePerspectiveFieldOfView(1.3f, aspect, 0.1f, 80.0f);

            foreach (var primitive in Collection.SceneObjects.GetPrimitivesWiThGlobalModelMatrices())
            {
                var shader = Collection.Shaders[Collection.ActiveShader];
                GL.UseProgram(shader.ProgramID);
                shader.EnableVertexAttribArrays();

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _iboElements);
                var inds = primitive.Object.GetIndices().ToArray();
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(inds.Length * sizeof(int)), inds, BufferUsageHint.StaticDraw);


                GL.UniformMatrix4(shader.GetUniform("projection"), false, ref projection);

                GL.BindTexture(TextureTarget.Texture2D, primitive.Object.TextureID);
                if (shader.GetUniform("maintexture") != -1)
                {
                    GL.Uniform1(shader.GetUniform("maintexture"), 0);
                }

                if (shader.GetUniform("model") != -1)
                {
                    var modelMatrix = primitive.GlobalModelMatrix;
                    GL.UniformMatrix4(shader.GetUniform("model"), false, ref modelMatrix);
                }

                LoadPrimitiveData(shader, primitive.Object);
                LoadCamera(shader, view, Vector3.Zero.ApplyOnPoint(Collection.ActiveCamera.GlobalModelMatrix));
                LoadLights(shader, Collection.SceneObjects.GetLightsWiThGlobalModelMatrices());

                GL.DrawElements(BeginMode.Triangles, primitive.Object.IndiceCount, DrawElementsType.UnsignedInt, 0);
                shader.DisableVertexAttribArrays();
            }

            GL.Flush();
        }
        private void LoadPrimitiveData(ShaderProgram shader, Primitive primitive)
        {
            LoadPrimitiveArrayData(shader, primitive);
             LoadMaterial(shader, primitive.Material);
        }
        private void LoadPrimitiveArrayData(ShaderProgram shader, Primitive primitive)
        {
            var verts = primitive.GetVerts().ToArray();
            var colors = primitive.GetColorData().ToArray();
            var texcoords = primitive.GetTextureCoords().ToArray();
            var normals = primitive.GetNormals().ToArray();

            GL.BindBuffer(BufferTarget.ArrayBuffer, shader.GetBuffer("vPosition"));
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * Vector3.SizeInBytes), verts, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(shader.GetAttribute("vPosition"), 3, VertexAttribPointerType.Float, false, 0, 0);

            if (shader.GetAttribute("vColor") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shader.GetBuffer("vColor"));
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length * Vector3.SizeInBytes), colors, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shader.GetAttribute("vColor"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (shader.GetAttribute("texcoord") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shader.GetBuffer("texcoord"));
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texcoords.Length * Vector2.SizeInBytes), texcoords, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shader.GetAttribute("texcoord"), 2, VertexAttribPointerType.Float, true, 0, 0);
            }

            if (shader.GetAttribute("vNormal") != -1)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, shader.GetBuffer("vNormal"));
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normals.Length * Vector3.SizeInBytes), normals, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(shader.GetAttribute("vNormal"), 3, VertexAttribPointerType.Float, true, 0, 0);
            }
        }
        private void LoadMaterial(ShaderProgram shader, Material material)
        {
            if (shader.GetUniform("material_ambient") != -1)
            {
                GL.Uniform3(shader.GetUniform("material_ambient"), ref material.AmbientColor);
            }

            if (shader.GetUniform("material_diffuse") != -1)
            {
                GL.Uniform3(shader.GetUniform("material_diffuse"), ref material.DiffuseColor);
            }
            if (shader.GetUniform("material_ambient") != -1)
            {
                GL.Uniform3(shader.GetUniform("material_ambient"), ref material.AmbientColor);
            }

            if (shader.GetUniform("material_diffuse") != -1)
            {
                GL.Uniform3(shader.GetUniform("material_diffuse"), ref material.DiffuseColor);
            }

            if (shader.GetUniform("material_specular") != -1)
            {
                GL.Uniform3(shader.GetUniform("material_specular"), ref material.SpecularColor);
            }

            if (shader.GetUniform("material_specExponent") != -1)
            {
                GL.Uniform1(shader.GetUniform("material_specExponent"), material.SpecularExponent);
            }
        }
        private void LoadCamera(ShaderProgram shader, Matrix4 view, Vector3 cameraPosition)
        {
            if (shader.GetUniform("viewPos") != -1)
            {
                GL.Uniform3(shader.GetUniform("viewPos"), ref cameraPosition);
            }

            if (shader.GetUniform("view") != -1)
            {
                GL.UniformMatrix4(shader.GetUniform("view"), false, ref view);
            }
        }

        private void LoadLights(ShaderProgram shader, List<CollectionItem<Light>> lights)
        {
            for (int i = 0; i < Math.Min(lights.Count, MaxLight); i++)
            {
                if (shader.GetUniform("lights[" + i + "].position") != -1)
                {
                    var position = (Vector3.Zero).ApplyOnPoint(lights[i].GlobalModelMatrix);
                    GL.Uniform3(shader.GetUniform("lights[" + i + "].position"), position);
                }
                if (shader.GetUniform("lights[" + i + "].direction") != -1)
                {
                    var direction = (-Vector3.UnitZ).ApplyOnVector(lights[i].GlobalModelMatrix);
                    GL.Uniform3(shader.GetUniform("lights[" + i + "].direction"), direction);
                }
                if (shader.GetUniform("lights[" + i + "].color") != -1)
                {
                    GL.Uniform3(shader.GetUniform("lights[" + i + "].color"), lights[i].Object.Color);
                }
                if (shader.GetUniform("lights[" + i + "].ambientIntensity") != -1)
                {
                    GL.Uniform1(shader.GetUniform("lights[" + i + "].ambientIntensity"), lights[i].Object.AmbientIntensity);
                }
                if (shader.GetUniform("lights[" + i + "].diffuseIntensity") != -1)
                {
                    GL.Uniform1(shader.GetUniform("lights[" + i + "].diffuseIntensity"), lights[i].Object.DiffuseIntensity);
                }
                if (shader.GetUniform("lights[" + i + "].type") != -1)
                {
                    GL.Uniform1(shader.GetUniform("lights[" + i + "].type"), (int)lights[i].Object.Type);
                }
                if (shader.GetUniform("lights[" + i + "].coneAngle") != -1)
                {
                    GL.Uniform1(shader.GetUniform("lights[" + i + "].coneAngle"),lights[i].Object.ConeAngle);
                }
            }
        }
    }
}
