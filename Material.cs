using OpenTK.Graphics.OpenGL;

namespace KG_6
{
    internal class Material
    {
        public Material(int shader, int shininess, float smoothness, int[] textures)
        {
            Shader = shader;
            Shininess = shininess;
            Smoothness = smoothness;
            Textures = textures;
        }

        public int Shader { get; }

        public int Shininess { get; }

        public float Smoothness { get; }

        public int[] Textures { get; }


        public void Use()
        {
            Shader s = AssetsLibrary.Instance.GetShader(Shader);
            s.Use();

            for (int i = 0; i < Textures.Length; i++)
            {
                Texture t = AssetsLibrary.Instance.GetTexture(Textures[i]);

                TextureUnit tu = i switch
                {
                    1 => TextureUnit.Texture1,
                    _ => TextureUnit.Texture0,
                };

                string pn = i switch
                {
                    1 => "specular",
                    _ => "diffuse",
                };

                t.Use(tu);
                s.SetInt($"material.{pn}", i);
            }
        }
    }
}
