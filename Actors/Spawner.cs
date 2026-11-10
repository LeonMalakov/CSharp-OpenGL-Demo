using System;
using OpenTK.Mathematics;

namespace KG_6.Actors
{
    internal class Spawner : Actor
    {
        public float SpawnInterval { get; set; } = 6;

        public int TargetCount { get; set; } = 6;

        private float _timer;
        private Random _rnd = new Random();

        public Spawner()
        {
            for (int i = 0; i < TargetCount / 2; i++)
                Spawn();
        }

        protected override void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            if(_timer >= 10)
            {
                _timer = 0;
                Spawn();
            }
        }

        private void Spawn()
        {
            if (Vehicle.AlivesCount >= TargetCount) 
                return;

            Vehicle veh = new Vehicle();

            Vector3[] path = new Vector3[_rnd.Next(4, 20)];
            for(int i = 0; i < path.Length; i++)
            {
                path[i] = new Vector3(_rnd.Next(-100, 100),0,_rnd.Next(-100, 100));
            }

            veh.Path = path;

            veh.Transform.Position = new Vector3(200 * _rnd.Next(-1, 2), 0, 200 * _rnd.Next(-1, 2));

            Game.SpawnActor(veh);
        }
    }
}
