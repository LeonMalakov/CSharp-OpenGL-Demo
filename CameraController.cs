using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;

namespace KG_6
{
    internal class CameraController
    {
        private const float Speed = 15;
        private const float MouseSensitivity = 0.0017f;
        private const float AngularSpeed = 0.1f;

        private readonly Vector3[] Path = new Vector3[] {
            new Vector3(0, 90, -50),
            new Vector3(80, 100, 60),
            new Vector3(-80, 90, 80),
            new Vector3(-50, 90, 0)
        };



        private Camera _camera;

        private Vector3 _cameraRot = new Vector3(MathHelper.DegreesToRadians(25), 0, 0);
        private Vector2 _lastMousePos;
        private float _zoom = 1;
        private int _currentWaypoint;
        private Vector3 _direction = Vector3.UnitZ;

        private Actors.Rocket _rocket;
        private Actors.Pointer _pointer;

        public void Init(Camera camera, Game g)
        {
            _camera = camera;

            ResetPerspective(g);
            _camera.Position = new Vector3(0, 90, -50);
            _camera.Rotation = Quaternion.FromEulerAngles(_cameraRot);

            _lastMousePos = g.MousePosition;

            _pointer = new Actors.Pointer();
            Game.SpawnActor(_pointer);
        }

        public void ResetPerspective(Game g)
        {
            _camera.SetPerspective(45 * _zoom, g.Size.X / (float)g.Size.Y, 0.1f, 1000f);
        }

        public void OnUpdate(Game g, FrameEventArgs args)
        {
            // Look around.
            Vector2 delta = g.MousePosition - _lastMousePos;
            _lastMousePos = g.MousePosition;

            _cameraRot.X += delta.Y * MouseSensitivity * _zoom;
            _cameraRot.Y += delta.X * MouseSensitivity * _zoom;

            if (_cameraRot.X > MathHelper.DegreesToRadians(90))
                _cameraRot.X = MathHelper.DegreesToRadians(90);
            else if (_cameraRot.X < MathHelper.DegreesToRadians(15))
                _cameraRot.X = MathHelper.DegreesToRadians(15);

            _camera.Rotation = Quaternion.FromEulerAngles(_cameraRot);
            //  _camera.Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, _cameraRot.X) * Quaternion.FromAxisAngle(Vector3.UnitY, _cameraRot.Y);


            Vector3 forw = new Vector3(
                MathF.Cos(_cameraRot.X) * MathF.Sin(_cameraRot.Y),
                -MathF.Sin(_cameraRot.X),
                -MathF.Cos(_cameraRot.Y) * (MathF.Cos(_cameraRot.X)));


            // Movement.
            KeyboardState ks = g.KeyboardState;


            if (Vector3.Distance(Path[_currentWaypoint], _camera.Position) <= 10)
            {
                _currentWaypoint++;
                if (_currentWaypoint >= Path.Length)
                    _currentWaypoint = 0;
            }

            Vector3 targetDir = (Path[_currentWaypoint] - _camera.Position);
            targetDir.NormalizeFast();

            _direction = Vector3.Lerp(_direction, targetDir, (float)args.Time * AngularSpeed);

            _camera.Position += _direction * Speed * (float)args.Time;

            /*
            Vector3 direction = Vector3.Zero;

            direction.Z = ks.IsKeyDown(Keys.W) ? -1 : (ks.IsKeyDown(Keys.S) ? 1 : 0);
            direction.X = ks.IsKeyDown(Keys.D) ? 1 : (ks.IsKeyDown(Keys.A) ? -1 : 0);
            direction.Y = ks.IsKeyDown(Keys.Space) ? 1 : (ks.IsKeyDown(Keys.LeftControl) ? -1 : 0);

            _camera.Position += _camera.Rotation.Inverted() * direction * Speed * (float)args.Time;
            */

            // Zoom.
            if (ks.IsKeyDown(Keys.E))
            {
                _zoom -= (float)args.Time * 0.4f;

                if (_zoom < 0.2f)
                    _zoom = 0.2f;

                ResetPerspective(g);
            }
            else if (ks.IsKeyDown(Keys.Q))
            {
                _zoom += (float)args.Time * 0.4f;

                if (_zoom > 1f)
                    _zoom = 1f;

                ResetPerspective(g);
            }


            // Launch rocket.
            if (ks.IsKeyDown(Keys.Space) && (_rocket == null || !_rocket.IsAlive))
            {
                _rocket = new Actors.Rocket();              

                Vector3 side = Vector3.Cross(forw, Vector3.UnitY);
                side.NormalizeFast();

                Vector3 up = Vector3.Cross(forw, side);
                up.NormalizeFast();

                Matrix3 rotmat = new Matrix3(side, up, forw);
                rotmat.Transpose();

                _rocket.Transform.Position = _camera.Position + side * 3 + up * 1;

                _rocket.Transform.Rotation = Quaternion.FromMatrix(rotmat);
               // _rocket.Transform.Rotation = Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(-90), 0,0);

                _rocket.Target = _rocket.Transform.Forward * 100;
                Game.SpawnActor(_rocket);
            }


            // Target.
            float xRot = _cameraRot.X;

            if (xRot > 0)
            {
                float angle = (MathF.PI / 2) - xRot;
                float dist = _camera.Position.Y / MathF.Cos(angle);

                Vector3 target = _camera.Position + forw.Normalized() * dist;
                //      System.Console.WriteLine($"{_camera.Position.Y}   {_cameraRot}    {forw}");

                if (_rocket != null && _rocket.IsAlive)
                {
                    _rocket.Target = target;
                }
                _pointer.Transform.Position = target;
            }
        }
    }
}
