﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK3D.Graphics
{
    public class AttributeInfo
    {
        public string name = "";
        public int address = -1;
        public int size = -1;
        public ActiveAttribType type;
    }
    public class UniformInfo
    {
        public string name = "";
        public int address = -1;
        public int size = -1;
        public ActiveUniformType type;
    }

    public class ShaderProgram
    {
        public int ProgramID = -1;
        public int VShaderID = -1;
        public int FShaderID = -1;
        public int AttributeCount = 0;
        public int UniformCount = 0;

        public Dictionary<string, AttributeInfo> Attributes = new Dictionary<string, AttributeInfo>();
        public Dictionary<string, UniformInfo> Uniforms = new Dictionary<string, UniformInfo>();
        public Dictionary<string, uint> Buffers = new Dictionary<string, uint>();

        public ShaderProgram()
        {
            ProgramID = GL.CreateProgram();
        }
        public ShaderProgram(string vshader, string fshader, bool fromFile = false)
        {
            ProgramID = GL.CreateProgram();

            if (fromFile)
            {
                LoadShaderFromFile(vshader, ShaderType.VertexShader);
                LoadShaderFromFile(fshader, ShaderType.FragmentShader);
            }
            else
            {
                LoadShaderFromString(vshader, ShaderType.VertexShader);
                LoadShaderFromString(fshader, ShaderType.FragmentShader);
            }

            Link();
            GenBuffers();
        }
        private void loadShader(string code, ShaderType type, out int address)
        {
            address = GL.CreateShader(type);
            GL.ShaderSource(address, code);
            GL.CompileShader(address);
            GL.AttachShader(ProgramID, address);
            Console.WriteLine(GL.GetShaderInfoLog(address));
        }
        public void LoadShaderFromString(string code, ShaderType type)
        {
            if (type == ShaderType.VertexShader)
                loadShader(code, type, out VShaderID);
            else if (type == ShaderType.FragmentShader)
                loadShader(code, type, out FShaderID);
        }
        public void LoadShaderFromFile(string filename, ShaderType type)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                if (type == ShaderType.VertexShader)
                    loadShader(sr.ReadToEnd(), type, out VShaderID);
                else if (type == ShaderType.FragmentShader)
                    loadShader(sr.ReadToEnd(), type, out FShaderID);
            }
        }
        public void Link()
        {
            GL.LinkProgram(ProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(ProgramID));
            GL.GetProgram(ProgramID, ProgramParameter.ActiveAttributes, out AttributeCount);
            GL.GetProgram(ProgramID, ProgramParameter.ActiveUniforms, out UniformCount);

            for (int i = 0; i < AttributeCount; i++)
            {
                var info = new AttributeInfo();
                StringBuilder name = new StringBuilder();
                GL.GetActiveAttrib(ProgramID, i, 256, out int length, out info.size, out info.type, name);
                info.name = name.ToString();
                info.address = GL.GetAttribLocation(ProgramID, info.name);
                Attributes.Add(name.ToString(), info);
            }

            for (int i = 0; i < UniformCount; i++)
            {
                var info = new UniformInfo();
                StringBuilder name = new StringBuilder();
                GL.GetActiveUniform(ProgramID, i, 256, out int length, out info.size, out info.type, name);
                info.name = name.ToString();
                Uniforms.Add(name.ToString(), info);
                info.address = GL.GetUniformLocation(ProgramID, info.name);
            }
        }
        public void GenBuffers()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                uint buffer = 0;
                GL.GenBuffers(1, out buffer);
                Buffers.Add(Attributes.Values.ElementAt(i).name, buffer);
            }

            for (int i = 0; i < Uniforms.Count; i++)
            {
                uint buffer = 0;
                GL.GenBuffers(1, out buffer);
                Buffers.Add(Uniforms.Values.ElementAt(i).name, buffer);
            }
        }
        public void EnableVertexAttribArrays()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                GL.EnableVertexAttribArray(Attributes.Values.ElementAt(i).address);
            }
        }
        public void DisableVertexAttribArrays()
        {
            for (int i = 0; i < Attributes.Count; i++)
            {
                GL.DisableVertexAttribArray(Attributes.Values.ElementAt(i).address);
            }
        }
        public int GetAttribute(string name)
        {
            if (Attributes.ContainsKey(name))
                return Attributes[name].address;
            else
                return -1;
        }
        public int GetUniform(string name)
        {
            if (Uniforms.ContainsKey(name))
                return Uniforms[name].address;
            else
                return -1;
        }
        public uint GetBuffer(string name)
        {
            if (Buffers.ContainsKey(name))
                return Buffers[name];
            else
                return 0;
        }

    }
}
