using System;
using System.Drawing;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace KG_6
{
    internal class Shader : IDisposable
    {
        private int _handle;
        private bool _disposedValue;

        public Shader(string vertPath, string fragPath)
        {
            string vertSource;
            string fragSource;
            int vert;
            int frag;
            string errorLog;

            // Read sources.
            using (StreamReader sr = new StreamReader(vertPath, Encoding.UTF8))
            {
                vertSource = sr.ReadToEnd();
            }

            using (StreamReader sr = new StreamReader(fragPath, Encoding.UTF8))
            {
                fragSource = sr.ReadToEnd();
            }

            // Vert shader compilation.
            vert = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vert, vertSource);

            GL.CompileShader(vert);
            errorLog = GL.GetShaderInfoLog(vert);
            if (errorLog != string.Empty)
                throw new Exception(errorLog);


            // Frag shader compilation.
            frag = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(frag, fragSource);

            GL.CompileShader(frag);
            errorLog = GL.GetShaderInfoLog(frag);
            if (errorLog != string.Empty)
                throw new Exception(errorLog);


            // Shader program creation.
            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vert);
            GL.AttachShader(_handle, frag);
            GL.LinkProgram(_handle);


            // Cleanup.
            GL.DetachShader(_handle, vert);
            GL.DetachShader(_handle, frag);
            GL.DeleteShader(vert);
            GL.DeleteShader(frag);
        }
    
        public void Use()
        {
            GL.UseProgram(_handle);
        }

        public void SetInt(string name, int value)
        {
            int location = GL.GetUniformLocation( _handle, name);

            GL.Uniform1(location, value);
        }

        public void SetFloat(string name, float value)
        {
            int location = GL.GetUniformLocation(_handle, name);

            GL.Uniform1(location, value);
        }

        public void SetMatrix4(string name, ref Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(_handle, name);

            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void SetMatrix3(string name, ref Matrix3 matrix)
        {
            int location = GL.GetUniformLocation(_handle, name);

            GL.UniformMatrix3(location, false, ref matrix);
        }

        public void SetVector3(string name, Vector3 vector)
        {
            int location = GL.GetUniformLocation(_handle, name);

            GL.Uniform3(location, vector.X, vector.Y, vector.Z);
        }

        public int GetAttribLocation(string name)
        {
            return GL.GetAttribLocation(_handle, name);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                GL.DeleteProgram(_handle);

                _disposedValue = true;
            }
        }

        ~Shader()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
