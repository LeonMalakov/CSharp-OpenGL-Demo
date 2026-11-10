using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;

namespace KG_6
{
    internal class Texture : IDisposable
    {
        private int _handle;
        private bool _disposedValue;

        public Texture(string path)
        {
            _handle = GL.GenTexture();

            Use();

            Image<Rgba32> image = Image.Load<Rgba32>(path);

            // Vertical flip image.
            image.Mutate(x => x.Flip(FlipMode.Vertical));


            // Image to byte list.
            List<byte> pixels = new List<byte>();

            for (int i = 0; i < image.Width; i++)
                for (int j = 0; j < image.Height; j++)
                {
                    Rgba32 rgba = image[j, i];

                    pixels.Add(rgba.R);
                    pixels.Add(rgba.G);
                    pixels.Add(rgba.B);
                    pixels.Add(rgba.A);
                }

            // Load bytes to texture.
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                image.Width, image.Height, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, pixels.ToArray());

            // Generate mipmaps
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            // Unuse.
            GL.BindTexture(TextureTarget.Texture2D, 0);

            // Cleanup.
            image.Dispose();
        }

        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, _handle);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                GL.DeleteTexture(_handle);

                _disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Texture()
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
