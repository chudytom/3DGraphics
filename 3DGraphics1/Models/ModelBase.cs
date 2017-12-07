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
        protected Model _model;
        protected Effect _effect;
        protected Vector3 _position = new Vector3();
        protected float _scale = 1.0f;
        protected Texture2D _texture;

        protected ModelBase() { }

        protected ModelBase(Effect effect)
        {
            //this.light = light;
            _effect = effect;
        }

        protected virtual void PrepareEffect(Camera camera)
        {
            _effect.Parameters["World"].SetValue(GetWorldMatrix());
            //_effect.Parameters["View"].SetValue(new Matrix(new Vector4(), new Vector4(), new Vector4(), new Vector4()));
            _effect.Parameters["View"].SetValue(camera.ViewMatrix);
            _effect.Parameters["CameraPosition"].SetValue(camera.Position);
            _effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(GetWorldMatrix()));
            _effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
        }

        public virtual void Draw(Camera camera)
        {
            PrepareEffect(camera);
            foreach (var mesh in _model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = _effect;
                }
                mesh.Draw();
            }
        }

        protected virtual Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(_scale) * Matrix.CreateTranslation(_position);
        }

        public void SetPosition(Vector3 position)
        {
            _position = position;
        }

        public void SetScale(float scale)
        {
            _scale = scale;
        }


        public void SetTexture(Texture2D texture)
        {
            _texture = texture;
        }
    }
}
