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
        //List<DirLight> light;
        Effect _effect;
        public Robot(Effect effect)
        {
            //this.light = light;
            _effect = effect;
        }
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
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    //effect.FogEnabled = true;
                    //effect.FogColor = Color.White.ToVector3(); // For best results, ake this color whatever your background is.
                    //effect.FogStart = 0.75f;
                    //effect.FogEnd = 100.25f;
                    //effect.EnableDefaultLighting();
                    //effect.LightingEnabled = true;
                    //effect.DirectionalLight0.Enabled = true;
                    //effect.DirectionalLight1.Enabled = true;
                    //effect.DirectionalLight2.Enabled = true;
                    //if (light.Count > 0)
                    //{
                    //    effect.DirectionalLight0.DiffuseColor = light[0].DiffuseColor;
                    //    effect.DirectionalLight0.Direction = light[0].Direction;
                    //    effect.DirectionalLight0.SpecularColor = light[0].SpecularColor;
                    //}
                    //if (light.Count > 1)
                    //{
                    //    effect.DirectionalLight1.DiffuseColor = light[1].DiffuseColor;
                    //    effect.DirectionalLight1.Direction = light[1].Direction;
                    //    effect.DirectionalLight1.SpecularColor = light[1].SpecularColor;
                    //}
                    //if (light.Count > 2)
                    //{
                    //    effect.DirectionalLight2.DiffuseColor = light[2].DiffuseColor;
                    //    effect.DirectionalLight2.Direction = light[2].Direction;
                    //    effect.DirectionalLight2.SpecularColor = light[2].SpecularColor;
                    //}
                    //effect.PreferPerPixelLighting = true;
                    //effect.World = GetWorldMatrix();

                    //effect.View = camera.ViewMatrix;
                    //effect.Projection = camera.ProjectionMatrix;
                    part.Effect = _effect;
                    _effect.Parameters["World"].SetValue(GetWorldMatrix());
                    _effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    _effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(GetWorldMatrix()));
                    _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                    _effect.Parameters["AmbientColor"].SetValue(Color.Green.ToVector4());
                    _effect.Parameters["AmbientIntensity"].SetValue(0.5f);
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
