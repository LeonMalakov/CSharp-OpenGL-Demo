using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace KG_6
{
    class Camera
    {
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

        public Vector3 Forward => Rotation * Vector3.UnitZ;

        public Vector3 Up => Rotation * Vector3.UnitY;

        public Vector3 Right => Rotation * Vector3.UnitX;

        public Matrix4 ProjectionMatrix { get; private set; }

        public void SetPerspective(float fov, float aspect, float near, float far)
        {
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), aspect, near, far);
        }

        public Matrix4 CreateViewMatrix()
        {
             return  Matrix4.CreateTranslation(-Position) * Matrix4.CreateFromQuaternion(Rotation);
          // return Matrix4.CreateTranslation(-Position);
         //     return Matrix4.LookAt(Position, Position + Forward, Vector3.UnitY);
        }

    }
}
