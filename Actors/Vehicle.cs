using OpenTK.Mathematics;

namespace KG_6.Actors
{
    internal class Vehicle : Actor
    {
        public static int AlivesCount;

        public Vector3[] Path { get; set; }

        public float Speed { get; set; } = 10f;
        public float AngularSpeed { get; set; } = 1f;

        private int _currentWaypoint;

        public Vehicle() : base(AssetsLibrary.Instance.GetMeshByName("Vehicle"), AssetsLibrary.Instance.GetMaterialByName("Vehicle"))
        {
            AlivesCount++;
        }

        public void Blow()
        {
            // Create destroyed.
            AlivesCount--;

            Actor actor = new DestroyedVehicle();
            actor.Transform.Position = Transform.Position;
            actor.Transform.Rotation = Transform.Rotation;
            Game.SpawnActor(actor);


            Game.DestroyActor(this);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Path.Length < 4) return;

            if (Vector3.Distance(Path[_currentWaypoint], Transform.Position) <= 3)
            {
                _currentWaypoint++;
                if (_currentWaypoint >= Path.Length)
                    _currentWaypoint = 0;
            }

            Vector3 targetDir = (Path[_currentWaypoint] - Transform.Position);
            targetDir.NormalizeFast();


            float angle = Vector3.CalculateAngle(Vector3.UnitZ, targetDir);
            if (targetDir.X < 0) angle = -angle;

            Quaternion targetRot = Quaternion.FromEulerAngles(0, angle, 0);

            Transform.Rotation = Quaternion.Slerp(Transform.Rotation, targetRot, deltaTime * AngularSpeed);

            Transform.Position += Transform.Forward * Speed * deltaTime;
        }
    }
}
