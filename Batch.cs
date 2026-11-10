using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;

namespace KG_6
{
    internal class Batch : IDisposable
    {
        private int _batchObject;
        private int _vertexBuffer;

        private int _material;
        private int _shader;
        private int _length;

        private bool _disposedValue;

        public Batch(int mesh, int material)
        {
            float[] array = AssetsLibrary.Instance.GetMesh(mesh).Array;

            _material = material;
            _shader = AssetsLibrary.Instance.GetMaterial(material).Shader;

            Shader shader = AssetsLibrary.Instance.GetShader(_shader);

            _length = array.Length;

            // Create & Bind.
            _batchObject = GL.GenVertexArray();
            Bind();


            // Create vertex array buffer.
            _vertexBuffer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, array.Length * sizeof(float), array, BufferUsageHint.StaticDraw);

            // Create indices array buffer.
        //    _indicesBuffer = GL.GenBuffer();
        //    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indicesBuffer);
        //    GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            // Set vertex arributes pointers.
            int posLocation = shader.GetAttribLocation("Position");
            GL.VertexAttribPointer(posLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(posLocation);

            // Set texture pointers.
            int texCoordLocation = shader.GetAttribLocation("TexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            // Set normal pointers.
            int normalLocation = shader.GetAttribLocation("Normal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 5 * sizeof(float));

            // Unbind.
            Unbind();
        }


        public void Draw(Matrix4 model, Matrix4 view, Matrix4 projection, Scene scene)
        {
            Material material = AssetsLibrary.Instance.GetMaterial(_material);
            Shader shader = AssetsLibrary.Instance.GetShader(_shader);

            material.Use();

            // VERTEX.
            // Transfromation.
            shader.SetMatrix4("model", ref model);
            shader.SetMatrix4("view", ref view);
            shader.SetMatrix4("projection", ref projection);

            Matrix4 nm = (model * view).Inverted();
            nm.Transpose();
            Matrix3 normalMatrix = new Matrix3(nm);
            shader.SetMatrix3("normalMatrix", ref normalMatrix);



            // FRAGMENT.

            // Material.
            shader.SetFloat("material.shininess", material.Shininess);
            shader.SetFloat("material.smoothness", material.Smoothness);

            // LIGHTS.
            // Ambient.
            shader.SetVector3("ambient.color", new Vector3(scene.AmbientLight.R, scene.AmbientLight.G, scene.AmbientLight.B) / 255);

            // Sun.
            shader.SetVector3("lights[0].color", new Vector3(scene.SunLight.R, scene.SunLight.G, scene.SunLight.B) / 255);
            Vector3 sunPos = (new Vector4(scene.SunDirection, 0f) * view).Xyz;
            shader.SetVector3("lights[0].direction", sunPos);


            // Draw.
            Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, _length);
            Unbind();
        }

        public void DrawInstanced(Matrix4 model, Matrix4 view, Matrix4 projection, Scene scene, int instancesCount, Vector3[] offsets)
        {
            Material material = AssetsLibrary.Instance.GetMaterial(_material);
            Shader shader = AssetsLibrary.Instance.GetShader(_shader);

            material.Use();

            // VERTEX.
            // Transfromation.
            shader.SetMatrix4("model", ref model);
            shader.SetMatrix4("view", ref view);
            shader.SetMatrix4("projection", ref projection);

            Matrix4 nm = (model * view).Inverted();
            nm.Transpose();
            Matrix3 normalMatrix = new Matrix3(nm);
            shader.SetMatrix3("normalMatrix", ref normalMatrix);

            for(int i = 0; i < instancesCount; i++)
            {
                shader.SetVector3($"offsets[{i}]", offsets[i]);
            }


            // FRAGMENT.

            // Material.
            shader.SetFloat("material.shininess", material.Shininess);
            shader.SetFloat("material.smoothness", material.Smoothness);

            // LIGHTS.
            // Ambient.
            shader.SetVector3("ambient.color", new Vector3(scene.AmbientLight.R, scene.AmbientLight.G, scene.AmbientLight.B) / 255);

            // Sun.
            shader.SetVector3("lights[0].color", new Vector3(scene.SunLight.R, scene.SunLight.G, scene.SunLight.B) / 255);

            Vector3 sunPos = (new Vector4(scene.SunDirection, 0f) * view).Xyz;
            shader.SetVector3("lights[0].direction", sunPos);


            // Draw.
            Bind();
            GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, _length, instancesCount);
            Unbind();
        }

        private void Bind()
        {
            GL.BindVertexArray(_batchObject);
        }

        private static void Unbind()
        {
            GL.BindVertexArray(0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                // МБ НАДО БИНДИТЬ СНАЧАЛА БАТЧ?

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
              //  GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
                GL.DeleteBuffer(_vertexBuffer);
             //   GL.DeleteBuffer(_indicesBuffer);

                _disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Batch()
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
