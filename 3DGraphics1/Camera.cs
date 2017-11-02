using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirstProject
{
    public class Camera
    {
        // We need this to calculate the aspectRatio
        // in the ProjectionMatrix property.
        GraphicsDevice graphicsDevice;

        Vector3 cameraPosition = new Vector3(0, -10, 20);
        Vector3 frontVector = new Vector3(0, -0.5f, 1);
        Vector3 upVector = new Vector3(0, 1, 0.5f);
        float cameraSpeed = 2.0f;

        Vector3 lookAtVector;

        //public Matrix View { get; private set; }

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            var upVector = Vector3.UnitZ;
            viewMatrix = Matrix.CreateLookAt(
            cameraPosition, lookAtVector, upVector);
        }

        public Camera(GraphicsDevice graphicsDevice, float cameraSpeed) : this(graphicsDevice)
        {
            this.cameraSpeed = cameraSpeed;
        }
        public Matrix ViewMa { get; private set; }
        private Matrix viewMatrix;
        public Matrix ViewMatrix
        {
            get => viewMatrix;
            private set { viewMatrix = value; }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                float nearClipPlane = 1;
                float farClipPlane = 200;
                float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);
            }
        }


        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            float speed = (float)(cameraSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            var pressedKeys = state.GetPressedKeys();
            if (pressedKeys.Length > 0)
            {
                //Linear moves
                if (state.IsKeyDown(Keys.Up))
                    cameraPosition -= frontVector * cameraSpeed / 20;
                if (state.IsKeyDown(Keys.Down))
                    cameraPosition += frontVector * cameraSpeed/20;
                if (state.IsKeyDown(Keys.Left))
                {
                    var moveDirection = Vector3.Cross(frontVector, upVector);
                    moveDirection.Normalize();
                    cameraPosition += moveDirection * speed;
                }
                if (state.IsKeyDown(Keys.Right))
                {
                    var moveDirection = Vector3.Cross(upVector, frontVector);
                    moveDirection.Normalize();
                    cameraPosition += moveDirection * speed;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    cameraPosition += upVector * speed;
                }
                if (state.IsKeyDown(Keys.S))
                {
                    cameraPosition -= upVector * speed;
                }
                //Rotations
                if (state.IsKeyDown(Keys.Q))
                {
                    frontVector = Vector3.Transform(frontVector, Matrix.CreateFromAxisAngle(upVector, MathHelper.ToRadians(cameraSpeed)));
                    //frontVector.Normalize();
                }
                if (state.IsKeyDown(Keys.E))
                {
                    frontVector = Vector3.Transform(frontVector, Matrix.CreateFromAxisAngle(upVector, MathHelper.ToRadians(-cameraSpeed)));
                }
                if (state.IsKeyDown(Keys.R))
                {
                    var rotationAxis = Vector3.Cross(frontVector, upVector);
                    upVector = Vector3.Transform(upVector, Matrix.CreateFromAxisAngle(rotationAxis, MathHelper.ToRadians(-cameraSpeed)));
                    frontVector = Vector3.Transform(frontVector, Matrix.CreateFromAxisAngle(rotationAxis, MathHelper.ToRadians(-cameraSpeed)));
                }
                if (state.IsKeyDown(Keys.F))
                {
                    var rotationAxis = Vector3.Cross(frontVector, upVector);
                    upVector = Vector3.Transform(upVector, Matrix.CreateFromAxisAngle(rotationAxis, MathHelper.ToRadians(cameraSpeed)));
                    frontVector = Vector3.Transform(frontVector, Matrix.CreateFromAxisAngle(rotationAxis, MathHelper.ToRadians(cameraSpeed)));
                }

            }
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition - frontVector, upVector);

        }

        public void SetCameraSpeed(float cameraSpeed)
        {
            this.cameraSpeed = cameraSpeed;
        }
    }
}
