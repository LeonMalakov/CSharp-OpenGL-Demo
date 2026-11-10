namespace KG_6.Actors
{
    internal class Pointer : Actor
    {
        public Pointer() : base(AssetsLibrary.Instance.GetMeshByName("Sphere"), AssetsLibrary.Instance.GetMaterialByName("Pointer"))
        {
        }
    }
}
