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
        List<DirLight> light = new List<DirLight>();
        Effect _effect;
        public Palm(Vector3 modelPosition, Effect effect)
        {
            this.modelPosition = modelPosition;
            _effect = effect;
            //this.light = light;
        }
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Palm1");

        }
        public void Draw(Camera camera)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    //effect.DiffuseColor = new Vector3(Color.R, Color.G, Color.B);
                    //effect.PreferPerPixelLighting = true;
                    ////effect.EnableDefaultLighting();
                    //effect.LightingEnabled = true;
                    //effect.DirectionalLight0.Enabled = true;
                    //effect.DirectionalLight1.Enabled = true;
                    //effect.DirectionalLight2.Enabled = true;
                    //if(light.Count>0)
                    //{
                    //    effect.DirectionalLight0.DiffuseColor = light[0].DiffuseColor;
                    //    effect.DirectionalLight0.Direction = light[0].Direction;
                    //    effect.DirectionalLight0.SpecularColor = light[0].SpecularColor;
                    //}
                    //if(light.Count>1)
                    //{
                    //    effect.DirectionalLight1.DiffuseColor = light[1].DiffuseColor;
                    //    effect.DirectionalLight1.Direction = light[1].Direction;
                    //    effect.DirectionalLight1.SpecularColor = light[1].SpecularColor;
                    //}
                    //if(light.Count>2)
                    //{
                    //    effect.DirectionalLight2.DiffuseColor = light[2].DiffuseColor;
                    //    effect.DirectionalLight2.Direction = light[2].Direction;
                    //    effect.DirectionalLight2.SpecularColor = light[2].SpecularColor;
                    //}
                    ////effect.EnableDefaultLighting();
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
            Matrix combinedMatrix = Matrix.CreateScale(0.3f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateTranslation(modelPosition);
            return combinedMatrix;
        }
    }
}
