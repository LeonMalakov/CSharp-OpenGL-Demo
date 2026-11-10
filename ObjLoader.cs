using OpenTK.Mathematics;
using System.Collections.Generic;
using System.IO;

namespace KG_6
{
    internal static class ObjLoader
    {

        public static Mesh Load(string path)
        {
            string data;
            using (StreamReader sr = new StreamReader(path))
            {
                data = sr.ReadToEnd();
            }


            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int[,]> indices = new List<int[,]>();

            foreach (string line in data.Split('\n'))
            {
                if (line.StartsWith("v "))
                    ParseVector3Line(line.Substring(2), vertices);
                else if (line.StartsWith("vt "))
                    ParseVector2Line(line.Substring(3), uv);
                else if (line.StartsWith("vn "))
                    ParseVector3Line(line.Substring(3), normals);
                else if (line.StartsWith("f "))
                    ParseFLine(line.Substring(2), indices);
            }


            List<float> targetArray = new List<float>();
            // create target array contains vertices, uv, (normals)
            foreach (int[,] ind in indices)
            {
                for (int i = 0; i < 3; i++)
                {
                    // Vert position.
                    targetArray.Add(vertices[ind[i, 0]].X);
                    targetArray.Add(vertices[ind[i, 0]].Y);
                    targetArray.Add(vertices[ind[i, 0]].Z);

                    // UV.
                    targetArray.Add(uv[ind[i, 1]].X);
                    targetArray.Add(uv[ind[i, 1]].Y);

                    // Vert Normal.
                    targetArray.Add(normals[ind[i, 2]].X);
                    targetArray.Add(normals[ind[i, 2]].Y);
                    targetArray.Add(normals[ind[i, 2]].Z);
                }
            }


            return new Mesh(targetArray.ToArray());
        }

        private static void ParseVector3Line(string line, List<Vector3> list)
        {
            string[] values = line.Split(' ');

            Vector3 v = new Vector3(
                Utilities.FloatParse(values[0]), 
                Utilities.FloatParse(values[1]), 
                Utilities.FloatParse(values[2]));

            list.Add(v);
        }

        private static void ParseVector2Line(string line, List<Vector2> list)
        {
            string[] values = line.Split(' ');

            Vector2 v = new Vector2(
                Utilities.FloatParse(values[0]),
                Utilities.FloatParse(values[1]));

            list.Add(v);
        }


        private static void ParseFLine(string line, List<int[,]> list)
        {
            string[] values = line.Split(' ');

            int[,] elem = new int[3,3];

            for (int i = 0; i < 3; i++)
            {
                string[] vals = values[i].Split('/');
                elem[i, 0] = int.Parse(vals[0]) - 1;
                elem[i, 1] = int.Parse(vals[1]) - 1;
                elem[i, 2] = int.Parse(vals[2]) - 1;
            }

            list.Add(elem);
        }

    }
}
