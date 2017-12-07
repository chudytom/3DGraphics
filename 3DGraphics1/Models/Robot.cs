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
    internal class Robot : ModelBase
    {
        float angle;
        //private Texture2D _texture;

        //List<DirLight> light;

        public Robot(Effect effect) : base(effect) { }

        public void Initialize(ContentManager contentManager)
        {
            _model = contentManager.Load<Model>("Models/robot");
        }

        public void Update(GameTime gameTime)
        {
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        //public void SetTexture(Texture2D texture)
        //{
        //    _texture = texture;
        //}

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
            const float circleRadius = 8;
            const float heightOffGround = 3;

            Matrix translationMatrix = Matrix.CreateTranslation(0, heightOffGround, circleRadius);
            Matrix rotationMatrix = Matrix.CreateRotationY(angle);
            Matrix combinedMatrix = Matrix.CreateRotationX(-MathHelper.PiOver2)* translationMatrix * rotationMatrix;
            return combinedMatrix;
        }
    }
}
