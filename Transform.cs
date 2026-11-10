using OpenTK.Mathematics;

namespace KG_6
{
    internal class Transform
    {
        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }

        public Vector3 Position { get; set; }


        private Quaternion _rotation = Quaternion.Identity;

        public Quaternion Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value.Normalized();
            }
        }

        public Vector3 Scale { get; set; }



        public Vector3 Forward => Rotation * Vector3.UnitZ;


        public Matrix4 CreateTransformMatrix()
        {
            return Matrix4.CreateScale(Scale) * Matrix4.CreateFromQuaternion(Rotation) * Matrix4.CreateTranslation(Position);
        }

    }
}
