using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace KG_6
{
    internal class Game : GameWindow
    {
        private static Game _instance;


        private Camera _camera;
        private CameraController _cameraController;

        private AssetsLibrary _assets;

        private Batcher _batcher;

        private Scene _scene;

        public static Vector3 CameraPosition => _instance._camera.Position;

        public static void SpawnActor(Actor actor)
        {
            _instance._scene.AddActor(actor);
        }

        public static void DestroyActor(Actor actor)
        {
            _instance._scene.DestroyActor(actor);
        }

        public static IEnumerable<Actor> GetActors<T>()
        {
            Type type = typeof(T);
            return _instance._scene.Actors.Where(a => a.GetType() == type);
        }

        private Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            if (_instance != null)
                throw new Exception("You can create only one Game instance.");

            _instance = this;
        }

        public static Game Create()
        {

            GameWindowSettings gs = GameWindowSettings.Default;
            // gs.IsMultiThreaded = true;
            gs.RenderFrequency = 60;
            gs.UpdateFrequency = 60;

            NativeWindowSettings ns = NativeWindowSettings.Default;
            ns.Title = "";
         //   ns.Size = new Vector2i(350, 250);
            ns.Size = new Vector2i(1280, 1024);
         //   ns.WindowState = OpenTK.Windowing.Common.WindowState.Fullscreen;

            return new Game(gs, ns);
        }

        protected override void OnLoad()
        {
            _assets = new AssetsLibrary();
            _batcher = new Batcher();
            _scene = new Scene();


            GL.ClearColor(Color.AliceBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc( BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            // Camera setup.
            _camera = new Camera();
            _cameraController = new CameraController();
            _cameraController.Init(_camera, this);

            // Mouse lock.
            CursorGrabbed = true;


            // Scene objects.    
            Actor ground = new Actor(AssetsLibrary.Instance.GetMeshByName("Ground"), AssetsLibrary.Instance.GetMaterialByName("Sand"));
            ground.Transform.Position = new Vector3(0, 0, 0);
            _scene.AddActor(ground);

            Actor road = new Actor(AssetsLibrary.Instance.GetMeshByName("Road"), AssetsLibrary.Instance.GetMaterialByName("Road"));
            road.Transform.Position = new Vector3(0, 0.1f, 0);
            _scene.AddActor(road);

            _scene.AddActor(new Actors.Spawner());

            base.OnLoad();
        }

        protected override void OnUnload()
        {
            _batcher.Dispose();
            _assets.Dispose();

            base.OnUnload();
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = _camera.CreateViewMatrix();
            Matrix4 projection = _camera.ProjectionMatrix;

            foreach (Actor actor in _scene.Actors)
            {
                if (actor.Renderer.Mesh == -1 || actor.Renderer.Material == -1) 
                    continue;

                Batch batch = _batcher.GetBatch(actor.Renderer.Mesh, actor.Renderer.Material);

                ParticleSystem particleSystem = actor as ParticleSystem;

                if (particleSystem != null)
                {
                    batch.DrawInstanced(particleSystem.CreateParticlesTransformMatrix(), view, projection, _scene, particleSystem.Count, particleSystem.Offsets);
                }
                else
                {
                    batch.Draw(actor.Transform.CreateTransformMatrix(), view, projection, _scene);
                }
            }


            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            float deltaTime = (float)args.Time;

            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                Close();
            }


            _cameraController.OnUpdate(this, args);

            // Update scene's actors.
            _scene.Update(deltaTime);

            base.OnUpdateFrame(args);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            _cameraController.ResetPerspective(this);
            base.OnResize(e);
        }

    }
}
