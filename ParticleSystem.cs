using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KG_6
{
    internal class ParticleSystem : Actor
    {
        public float LifeTime { get; set; } = 2f;

        public int MaxCount { get; set; } = 25;

        public float Speed { get; set; } = 1.5f;

        public float Frequency { get; set; } = 0.1f;

        public bool OneShot { get; set; } = true;

        public Vector3 ParticleScale { get; set; } = new Vector3(4,4,4);

        public Vector3 Direction { get; set; } = Vector3.UnitY;

        public Vector3 SpawnPoint { get; set; } = Vector3.Zero;

        private class Particle
        {
            public float BornTime;
            public Vector3 Offset;

            public Particle(float bornTime, Vector3 offset)
            {
                BornTime = bornTime;
                Offset = offset;
            }
        }

        private Queue<Particle> _particles = new Queue<Particle>();

        private float _time;

        private float _lastSpawnTime;

        private int _particlesWasSpawned;

        private Random _rnd;

        public int Count => _particles.Count;

        public Vector3[] Offsets => _particles.Select(p => p.Offset).ToArray();


        public ParticleSystem(int material) : base(AssetsLibrary.Instance.GetMeshByName("Quad"), material)
        {
            _rnd = new Random();
        }

        protected override void OnUpdate(float deltaTime)
        {
            _time += deltaTime;
        
            if(_particles.Count > 0 && _particles.Peek().BornTime + LifeTime < _time)
            {
                RemoveParticle();
            }

            if(_particles.Count < MaxCount && _lastSpawnTime + Frequency < _time)
            {
                if (!OneShot || (OneShot && _particlesWasSpawned < MaxCount))
                {
                    SpawnParticle();
                }
                else if (_particles.Count == 0)
                {
                    Game.DestroyActor(this);
                }
            }

            foreach(Particle particle in _particles)
            {
                particle.Offset += Direction * Speed * deltaTime;        
            }
        }

        public Matrix4 CreateParticlesTransformMatrix()
        {
            Vector3 targetDir = (Game.CameraPosition - Transform.Position);
            targetDir.NormalizeFast();

            Vector3 side = Vector3.Cross(targetDir, Vector3.UnitY);
            side.NormalizeFast();

            Vector3 up = Vector3.Cross(targetDir, side);
            up.NormalizeFast();

            Matrix3 rotmat = new Matrix3(side, up, targetDir);
            rotmat.Transpose();

            Quaternion targetRot = Quaternion.FromMatrix(rotmat) * Quaternion.FromEulerAngles(MathF.PI * 1, 0, 0);

            return Matrix4.CreateScale(ParticleScale) * Matrix4.CreateFromQuaternion(targetRot) * Matrix4.CreateTranslation(Transform.Position);
        }

        private void RemoveParticle()
        {
            _particles.Dequeue();
        }

        private void SpawnParticle()
        {
            _lastSpawnTime = _time;
            _particlesWasSpawned++;
            _particles.Enqueue(new Particle(_time, SpawnPoint + new Vector3(_rnd.Next(-1000, 1000), -0.5f * ParticleScale.Y, _rnd.Next(-1000, 1000)) / 2000f));
        }

    }
}
