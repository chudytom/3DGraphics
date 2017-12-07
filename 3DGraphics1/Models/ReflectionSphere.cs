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
    internal class ReflectionSphere : ModelBase
    {
        private TextureCube _skyboxTexture;
        public ReflectionSphere(Vector3 position)
        {
            _position = position;
        }
        public ReflectionSphere(Vector3 modelPosition, Effect effect) : base(effect)
        {
            _position = modelPosition;
            SetScale(5);
        }
        public void Initialize(ContentManager content)
        {
            _model = content.Load<Model>("Models/sphere");
            _skyboxTexture = content.Load<TextureCube>("Skyboxes/Islands");
            _effect = content.Load<Effect>("Shaders/Reflection");
        }

        protected override void PrepareEffect(Camera camera)
        {
            _effect.Parameters["SkyboxTexture"].SetValue(_skyboxTexture);

            base.PrepareEffect(camera);
        }

        protected override Matrix GetWorldMatrix()
        {
            //SetScale(0.3f);
            return base.GetWorldMatrix();
        }
    }
}
