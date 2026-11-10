using OpenTK.Mathematics;
using System;

namespace KG_6.Actors
{
    internal class Rocket : Actor
    {
        public float Speed { get; set; } = 20;

        public float AngularSpeed { get; set; } = 4f;

        public float DestroyHeight { get; set; } = 1.5f;

        public Vector3 Target { get; set; }

        private float _time;


        public Rocket() : base(AssetsLibrary.Instance.GetMeshByName("Rocket"), AssetsLibrary.Instance.GetMaterialByName("Rocket"))
        {
        }

        protected override void OnUpdate(float deltaTime)
        {
            Vector3 targetDir = (Target - Transform.Position);
            targetDir.NormalizeFast();

            Vector3 side = Vector3.Cross(targetDir, Vector3.UnitY);
            side.NormalizeFast();

            Vector3 up = Vector3.Cross(targetDir, side);
            up.NormalizeFast();

            Matrix3 rotmat = new Matrix3(side, up, targetDir);
            rotmat.Transpose();

            Quaternion targetRot = Quaternion.FromMatrix(rotmat);


            float sqrtTime = MathF.Sqrt(_time + 1);
            float sqrtSqrtTime = MathF.Sqrt(sqrtTime);

            Transform.Rotation = Quaternion.Slerp(Transform.Rotation, targetRot, deltaTime * AngularSpeed / sqrtSqrtTime);


            Transform.Position += Transform.Forward * Speed * deltaTime * sqrtTime;

            // Disp.
            _time += deltaTime * 1.2f * sqrtSqrtTime;
            Vector3 pos = new Vector3(MathF.Cos(_time), MathF.Sin(_time), 0) * 5 / sqrtSqrtTime;
            pos = Transform.Rotation * pos;
            Transform.Position += pos * deltaTime;

            if (Transform.Position.Y <= DestroyHeight)
            {
                foreach(Vehicle veh in Game.GetActors<Vehicle>())
                {
                    if(Vector3.Distance(veh.Transform.Position, Transform.Position) <= 8)
                    {
                        veh.Blow();
                    }
                }

                Actor blow = new Blow();
                blow.Transform.Position = Transform.Position;
                Game.SpawnActor(blow);

                Game.DestroyActor(this);
            }
        }
    }
}
