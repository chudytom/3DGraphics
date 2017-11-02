using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace FirstProject
{
    public class Robot
    {
        private Model model;
        float angle;
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("robot");
        }

        public void Update(GameTime gameTime)
        {
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Camera camera)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight2.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0.0f, 0);
                    effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);
                    effect.DirectionalLight0.SpecularColor = new Vector3(0, 0, 1);
                    effect.DirectionalLight1.DiffuseColor = new Vector3(0, 0.5f, 0);
                    effect.DirectionalLight1.Direction = new Vector3(-1, 0, 0);
                    effect.DirectionalLight1.SpecularColor = new Vector3(0, 0, 1);
                    effect.DirectionalLight2.DiffuseColor = new Vector3(0.02f, 0.02f, 1);
                    effect.DirectionalLight2.Direction = new Vector3(0, 0, -1);
                    effect.DirectionalLight2.SpecularColor = new Vector3(0, 0, 1);
                    effect.PreferPerPixelLighting = true;
                    effect.World = GetWorldMatrix();

                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        private Matrix GetWorldMatrix()
        {
            const float circleRadius = 8;
            const float heightOffGround = 3;

            Matrix translationMatrix = Matrix.CreateTranslation(circleRadius, 0, heightOffGround);
            Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
            Matrix combinedMatrix = translationMatrix * rotationMatrix;
            return combinedMatrix;
        }
    }
}
