using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    internal abstract class ModelBase
    {
        protected Model model;
        protected Effect _effect;

        protected ModelBase(Effect effect)
        {
            //this.light = light;
            _effect = effect;
        }

        protected virtual void PrepareEffect(Camera camera)
        {
            _effect.Parameters["World"].SetValue(GetWorldMatrix());
            _effect.Parameters["View"].SetValue(camera.ViewMatrix);
            _effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(GetWorldMatrix()));
            _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            _effect.Parameters["AmbientColor"].SetValue(Color.Green.ToVector4());
            _effect.Parameters["AmbientIntensity"].SetValue(0.5f);
        }

        public virtual void Draw(Camera camera)
        {
            PrepareEffect(camera);
            foreach (var mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                }
                mesh.Draw();
            }
        }

        protected abstract Matrix GetWorldMatrix();
    }
}
