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
    internal class Palm : ModelBase
    {
        public Palm(Vector3 modelPosition, Effect effect) : base(effect)
        {
            _position = modelPosition;
        }
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Palm1");
        }

        protected override void PrepareEffect(Camera camera)
        {
            _effect.Parameters["ModelTexture"].SetValue(_texture);
            base.PrepareEffect(camera);
        }

        protected override Matrix GetWorldMatrix()
        {
            SetScale(0.3f);
            return base.GetWorldMatrix();
            //Matrix worldMatrix = Matrix.CreateScale(0.3f) * Matrix.CreateRotationX(0) * Matrix.CreateTranslation(_position);
            //return worldMatrix;
        }
    }
}
