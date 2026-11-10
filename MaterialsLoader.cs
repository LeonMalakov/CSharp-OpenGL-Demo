using System.Collections.Generic;
using System.IO;

namespace KG_6
{
    internal static class MaterialsLoader
    {
        public static IEnumerable<(string, Material)> LoadMaterials(string materialsPath)
        {      
            foreach (string path in Directory.GetFiles(materialsPath))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    int shader = AssetsLibrary.Instance.GetShaderByName(sr.ReadLine());

                    int shininess = int.Parse(sr.ReadLine());

                    float smoothness = Utilities.FloatParse(sr.ReadLine());

                    List<int> textures = new List<int>();
                    while (!sr.EndOfStream)
                    {
                        textures.Add(AssetsLibrary.Instance.GetTextureByName(sr.ReadLine()));
                    }

                    Material mat = new Material(shader, shininess, smoothness, textures.ToArray());
                    yield return (Path.GetFileNameWithoutExtension(path), mat);
                }
            }
        }

    }
}
