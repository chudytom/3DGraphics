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
        private float _time = 0.0f;
        private float _oceanMoveSpeed = 0.05f;
        private List<Texture2D> _oceanTextures = new List<Texture2D>();
        public Ocean(Effect effect) : base(effect) { }

        public void Initialize(ContentManager contentManager, GraphicsDeviceManager graphics)
        {
            _model = contentManager.Load<Model>("Models/Ocean");

            for (int i = 0; i < 4; i++)
            {
                using (var stream = TitleContainer.OpenStream($"Content/Images/sea{i + 1}.jpg"))
                {
                    _oceanTextures.Add(Texture2D.FromStream(graphics.GraphicsDevice, stream));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void PrepareEffect(Camera camera)
        {
            base.PrepareEffect(camera);
            _effect.Parameters["ModelTexture1"].SetValue(_oceanTextures[0]);
            _effect.Parameters["ModelTexture2"].SetValue(_oceanTextures[2]);
            _effect.Parameters["TextureLerp"].SetValue(0.5f);
            _effect.Parameters["Time"].SetValue(_time * _oceanMoveSpeed);
        }

        protected override Matrix GetWorldMatrix()
        {
            return base.GetWorldMatrix();
        }
    }
}
