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
    public class Torch
    {
        private Model model;
        Vector3 modelPosition;
        public Color Color { get; set; }
        public Torch(Vector3 modelPosition)
        {
            this.modelPosition = modelPosition;
        }
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Row_Boat");
        }
        public void Draw(Camera camera)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.DiffuseColor = new Vector3(Color.R, Color.G, Color.B);
                    effect.TextureEnabled = true;
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
