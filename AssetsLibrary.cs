using System;
using System.Collections.Generic;
using System.IO;

namespace KG_6
{
    internal class AssetsLibrary : IDisposable
    {
        private static readonly string ResourcesPath = Path.Combine(AppContext.BaseDirectory, "Resources");
        private const string MeshesPath = "Meshes";
        private const string ShadersPath = "Shaders";
        private const string TexturesPath = "Textures";
        private const string MaterialsPath = "Materials";

        private List<(string, Mesh)> _meshes = new List<(string, Mesh)>();

        private List<(string, Shader)> _shaders = new List<(string, Shader)>();

        private List<(string, Texture)> _textures = new List<(string, Texture)>();

        private List<(string, Material)> _materials = new List<(string, Material)>();

        private bool _disposedValue;


        private static AssetsLibrary _instance;

        public AssetsLibrary()
        {
            _instance = this;

            LoadMaterials();
        }

        public static AssetsLibrary Instance
        {
            get
            {
                if (_instance == null)
                    throw new Exception("Assets library not initialized.");

                return _instance;
            }
        }

        public int GetMeshByName(string name)
        {
            int ind = _meshes.FindIndex(c => c.Item1 == name);

            if (ind == -1)
            {
                _meshes.Add((name, ObjLoader.Load(Path.Combine(ResourcesPath, MeshesPath, name + ".obj"))));
                ind = _meshes.Count - 1;
            }

            return ind;
        }

        public Mesh GetMesh(int index)
        {
            return _meshes[index].Item2;
        }

        public int GetShaderByName(string name)
        {
            int ind = _shaders.FindIndex(c => c.Item1 == name);

            if (ind == -1)
            {
                string path = Path.Combine(ResourcesPath, ShadersPath);
                _shaders.Add((name, new Shader(Path.Combine(path, name + ".vert"), Path.Combine(path, name + ".frag"))));
                ind = _shaders.Count - 1;
            }

            return ind;
        }

        public Shader GetShader(int index)
        {
            return _shaders[index].Item2;
        }


        public int GetTextureByName(string name)
        {
            int ind = _textures.FindIndex(c => c.Item1 == name);

            if (ind == -1)
            {
                _textures.Add((name, new Texture(Path.Combine(ResourcesPath, TexturesPath, name + ".png"))));
                ind = _textures.Count - 1;
            }

            return ind;
        }

        public Texture GetTexture(int index)
        {
            return _textures[index].Item2;
        }



        public bool AddMaterial(string name, Material material)
        {
            int ind = _materials.FindIndex(c => c.Item1 == name);

            if (ind != -1) return false;

            _materials.Add((name, material));

            return true;
        }

        public int GetMaterialByName(string name)
        {
            int ind = _materials.FindIndex(c => c.Item1 == name);

            return ind;
        }

        public Material GetMaterial(int index)
        {
            return _materials[index].Item2;
        }


        private void LoadMaterials()
        {
            foreach((string, Material) mat in MaterialsLoader.LoadMaterials(Path.Combine(ResourcesPath, MaterialsPath)))
            {
                System.Console.WriteLine($"Loaded material: {mat.Item1}.");
                _materials.Add(mat);
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _instance = null;

                    foreach ((string, Shader) s in _shaders)
                        s.Item2.Dispose();

                    foreach ((string, Texture) s in _textures)
                        s.Item2.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AssetsLibrary()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
