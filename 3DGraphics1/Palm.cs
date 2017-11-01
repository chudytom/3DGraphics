using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    public class Palm
    {
        private Model model;
        float angle;
        Vector3 modelPosition;
        public Palm(Vector3 modelPosition)
        {
            this.modelPosition = modelPosition;
        }
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Palm1");

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
                    //effect.DiffuseColor = new Vector3(0, 1, 0);
                    effect.EnableDefaultLighting();
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
            Matrix combinedMatrix = Matrix.CreateScale(0.3f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateTranslation(modelPosition);
            return combinedMatrix;
        }
    }
}
