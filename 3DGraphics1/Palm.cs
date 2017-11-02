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
        Vector3 modelPosition;
        public Color Color { get; set; }
        public Palm(Vector3 modelPosition)
        {
            this.modelPosition = modelPosition;
        }
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Palm1");

        }
        public void Draw(Camera camera)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.DiffuseColor = new Vector3(Color.R, Color.G, Color.B);
                    effect.PreferPerPixelLighting = true;
                    //effect.EnableDefaultLighting();
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight1.Enabled = true;
                    effect.DirectionalLight2.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0.0f, 0);
                    effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);
                    //effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0);
                    effect.DirectionalLight1.DiffuseColor = new Vector3(0, 0.5f, 0);
                    effect.DirectionalLight1.Direction = new Vector3(-1, 0, 0);
                    //effect.DirectionalLight1.SpecularColor = new Vector3(1, 0, 0);
                    effect.DirectionalLight2.DiffuseColor = new Vector3(0.02f, 0.02f, 1);
                    effect.DirectionalLight2.Direction = new Vector3(0, 0, -1);
                    //effect.DirectionalLight2.SpecularColor = new Vector3(0, 0, 1);
                    //effect.EnableDefaultLighting();
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
