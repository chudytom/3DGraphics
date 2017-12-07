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
    internal class Ocean : ModelBase
    {

        public Ocean(Effect effect) : base(effect) { }

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Models/Ocean");
        }

        protected override void PrepareEffect(Camera camera)
        {
            _effect.Parameters["ModelTexture"].SetValue(_texture);
            base.PrepareEffect(camera);
        }

        protected override Matrix GetWorldMatrix()
        {
            return base.GetWorldMatrix();
        }
    }
}
