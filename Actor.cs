namespace KG_6
{
    internal class Actor
    {
        public bool IsAlive { get; private set; } = true;

        public Transform Transform { get; } = new Transform();

        public Renderer Renderer { get; } = new Renderer();


        public Actor()
        {
            OnInit();
        }

        public Actor(int mesh, int material)
        {
            Renderer.Mesh = mesh;
            Renderer.Material = material;

            OnInit();
        }

        public void Update(float deltaTime)
        {
            if (!IsAlive) return;

            OnUpdate(deltaTime);
        }

        public void Destroy()
        {
            if (!IsAlive) return;

            IsAlive = false;
            OnDestroy();
        }


        protected virtual void OnInit() { }
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnDestroy() { }
    }
}
