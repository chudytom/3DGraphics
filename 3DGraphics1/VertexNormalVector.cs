using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    public struct VertexNormalVector : IVertexType
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;
        public VertexNormalVector(Vector3 position, Vector3 normal, Color color)
        {
            Position = position;
            Normal = normal;
            Color = color;
        }



        public static int SizeInBytes = (3 + 3 + 1) * 4; // 3 floats for Position + 3 floats for Normal + 4 bytes for Color

        public static VertexElement[] VertexElements = new[]
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
        };

        public VertexDeclaration VertexDeclaration => new VertexDeclaration(VertexElements);
    }
}
