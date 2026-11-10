namespace KG_6.Actors
{
    internal class Blow : ParticleSystem
    {
        public Blow() : base(AssetsLibrary.Instance.GetMaterialByName("Blow"))
        {
            LifeTime = 0.8f;
            Frequency = 0.0005f;
            MaxCount = 5;
            ParticleScale = new OpenTK.Mathematics.Vector3(7, 7, 7);
            Speed = 1f;
            SpawnPoint = new OpenTK.Mathematics.Vector3(0, -0.5f, 0);
        }
    }
}
