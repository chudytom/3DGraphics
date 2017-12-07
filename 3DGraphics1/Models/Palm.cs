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
        public void Initialize(ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            _model = contentManager.Load<Model>("Palm1");

            using (var stream = TitleContainer.OpenStream("Content/Images/palmTexture1.jpg"))
            {
                _texture = Texture2D.FromStream(graphics.GraphicsDevice, stream);
            }
        }

        protected override void PrepareEffect(Camera camera)
        {
            base.PrepareEffect(camera);
            _effect.Parameters["ModelTexture"].SetValue(_texture);
            _effect.Parameters["AmbientColor"].SetValue(Color.Green.ToVector4());
            _effect.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
            _effect.Parameters["AmbientIntensity"].SetValue(0.1f);
            _effect.Parameters["DiffuseIntensity"].SetValue(15.0f);
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
