using OpenTK.Mathematics;
using System.Collections.Generic;
using System.Drawing;

namespace KG_6
{
    internal class Scene
    {
        private List<Actor> _actors = new List<Actor>();

        private Stack<Actor> _actorsToAdd = new Stack<Actor>();
        private Stack<Actor> _actorsToDestroy = new Stack<Actor>();

        public IReadOnlyList<Actor> Actors => _actors;


        #region Lights
        public Color AmbientLight { get; set; } = Color.FromArgb(255, 87, 77, 120);

        public Color SunLight { get; set; } = Color.FromArgb(255, 241, 255, 163);

        public Vector3 SunDirection { get; set; } = new Vector3(1, -1, -1).Normalized();
        #endregion

        public void AddActor(Actor actor)
        {
            _actorsToAdd.Push(actor);       
        }

        public void DestroyActor(Actor actor)
        {
            _actorsToDestroy.Push(actor);         
        }

        public void Update(float deltaTime)
        {
            foreach (Actor actor  in _actors)
                actor.Update(deltaTime);

            while(_actorsToDestroy.Count > 0)
            {
                Actor a = _actorsToDestroy.Pop();
                a.Destroy();
                _actors.Remove(a);
            }

            while (_actorsToAdd.Count > 0)
            {
                Actor a = _actorsToAdd.Pop();
                _actors.Add(a);
            }
        }
    }
}
