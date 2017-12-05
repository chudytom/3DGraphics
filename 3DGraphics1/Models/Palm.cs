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
        Vector3 modelPosition;
        public Color Color { get; set; }
        List<DirLight> light = new List<DirLight>();
        public Palm(Vector3 modelPosition, Effect effect) : base(effect)
        {
            this.modelPosition = modelPosition;
        }
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("Palm1");
        }

        protected override Matrix GetWorldMatrix()
        {
            Matrix combinedMatrix = Matrix.CreateScale(0.3f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateTranslation(modelPosition);
            return combinedMatrix;
        }
    }
}
