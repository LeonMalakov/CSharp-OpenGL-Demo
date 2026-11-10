namespace KG_6.Actors
{
    internal class DestroyedVehicle : Actor
    {
        private float _time;

        public DestroyedVehicle() :  base(AssetsLibrary.Instance.GetMeshByName("Vehicle_Destroyed"), AssetsLibrary.Instance.GetMaterialByName("Vehicle"))
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            _time += deltaTime;

            if (_time > 25)
                Game.DestroyActor(this);
        }
    }
}
