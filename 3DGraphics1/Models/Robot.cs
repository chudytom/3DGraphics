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
        //List<DirLight> light;

        public Robot(Effect effect) : base(effect) { }

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("robot");
        }

        public void Update(GameTime gameTime)
        {
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override Matrix GetWorldMatrix()
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
