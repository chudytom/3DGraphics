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

        Vector3 position = new Vector3(0, 20, 20);
        float cameraSpeed = 3;

        float angleAroundZ;
        float angleAroundX;

        public Camera(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public Camera(GraphicsDevice graphicsDevice, float cameraSpeed) : this(graphicsDevice)
        {
            this.cameraSpeed = cameraSpeed;
        }
        public Matrix ViewMatrix
        {
            get
            {
                var lookAtVector = new Vector3(0, -1, -0.5f);
                var rotationMatrix = GetRotationMatrix();

                lookAtVector = Vector3.Transform(lookAtVector, rotationMatrix);
                lookAtVector += position;

                var upVector = Vector3.UnitZ;

                return Matrix.CreateLookAt(
                position, lookAtVector, upVector);
            }
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

            var pressedKeys = state.GetPressedKeys();
            if(pressedKeys.Length>0)
            {

                if (state.IsKeyDown(Keys.Q))
                    angleAroundZ += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (state.IsKeyDown(Keys.E))
                    angleAroundZ -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (state.IsKeyDown(Keys.F))
                    angleAroundX += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (state.IsKeyDown(Keys.R))
                    angleAroundX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    var forwardVector = new Vector3(0, 0, 0);
                    Matrix rotationMatrix = GetRotationMatrix();
                    if (state.IsKeyDown(Keys.Up))
                        forwardVector = new Vector3(0, -1, 0);
                    else if (state.IsKeyDown(Keys.Down))
                        forwardVector = new Vector3(0, 1, 0);
                    else if (state.IsKeyDown(Keys.Left))
                        forwardVector = new Vector3(1, 0, 0);
                    else if (state.IsKeyDown(Keys.Right))
                        forwardVector = new Vector3(-1, 0, 0);
                    else if (state.IsKeyDown(Keys.W))
                        forwardVector = new Vector3(0, 0, 1);
                    else if (state.IsKeyDown(Keys.S))
                        forwardVector = new Vector3(0, 0, -1);
                    forwardVector = Vector3.Transform(forwardVector, rotationMatrix);

                    this.position += forwardVector * cameraSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            // We'll be doing some input-based movement here
        }

        private Matrix GetRotationMatrix()
        {
            var rotationMatrixZ = Matrix.CreateRotationZ(angleAroundZ);
            var rotationMatrixX = Matrix.CreateRotationX(angleAroundX);
            return Matrix.CreateFromYawPitchRoll(0, angleAroundX, angleAroundZ);
        }

        public void SetCameraSpeed(float cameraSpeed)
        {
            this.cameraSpeed = cameraSpeed;
        }
    }
}
