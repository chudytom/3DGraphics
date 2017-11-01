using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    public class Sphere
    {
        VertexPositionColor[] vertices; //later, I will provide another example with VertexPositionNormalTexture
        VertexBuffer vbuffer;
        short[] indices; //my laptop can only afford Reach, no HiDef :(
        IndexBuffer ibuffer;
        float radius;
        int nvertices, nindices;
        BasicEffect effect;
        GraphicsDevice graphicd;
        int latitudes = 15; //how many vertices in a circle
        int longitudes = 15; //how many circles
        Color sphereColor;
        Texture2D texture;
        public Sphere(GraphicsDevice graphics, float radius, int latitudes, int longitudes, Color color)
        {
            graphicd = graphics;
            this.radius = radius;
            this.latitudes = latitudes;
            this.longitudes = longitudes;
            this.sphereColor = color;
            effect = new BasicEffect(graphicd);
            nvertices = latitudes * longitudes; // 90 vertices in a circle, 90 circles in a sphere
            nindices = latitudes * longitudes * 6;
            vbuffer = new VertexBuffer(graphics, typeof(VertexPositionNormalTexture), nvertices, BufferUsage.WriteOnly);
            ibuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, nindices, BufferUsage.WriteOnly);
            Createspherevertices();
            Createindices();
            vbuffer.SetData<VertexPositionColor>(vertices);
            ibuffer.SetData<short>(indices);
            effect.VertexColorEnabled = true;
            effect.SpecularPower = 5;
            //effect.vie
            effect.PreferPerPixelLighting = true;
            //effect.EnableDefaultLighting();
        }
        void Createspherevertices()
        {
            vertices = new VertexPositionColor[nvertices];
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 rad = new Vector3((float)Math.Abs(radius), 0, 0);
            for (int x = 0; x < longitudes; x++) //number of veritces in a circle, difference between each is 360/longitudes degrees
            {
                float difx = 360.0f / longitudes;
                for (int y = 0; y < latitudes/1; y++) // number of circles, difference between each is 360/latitudes degrees
                {
                    float dify = 360.0f / latitudes;
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); 
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx)); 
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation

                    vertices[x * latitudes + y] = new VertexPositionColor(point, sphereColor);
                }
            }
        }
        void Createindices()
        {
            indices = new short[nindices];
            int i = 0;
            for (int x = 0; x < longitudes; x++)
            {
                for (int y = 0; y < latitudes/1; y++)
                {
                    int s1 = x == longitudes - 1 ? 0 : x + 1;
                    int s2 = y == latitudes - 1 ? 5 : y + 1;
                    short upperLeft = (short)(x * latitudes + y);
                    short upperRight = (short)(s1 * latitudes + y);
                    short lowerLeft = (short)(x * latitudes + s2);
                    short lowerRight = (short)(s1 * latitudes + s2);
                    indices[i++] = lowerRight;
                    indices[i++] = upperRight;
                    indices[i++] = upperLeft;
                    indices[i++] = lowerLeft;
                    indices[i++] = lowerRight;
                    indices[i++] = upperLeft;
                }
            }
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(Camera cam) // the camera class contains the View and Projection Matrices
        {
            effect.View = cam.ViewMatrix;
            effect.DiffuseColor = new Vector3(1, 0, 1);
            effect.Projection = cam.ProjectionMatrix;
            effect.World = Matrix.CreateRotationX(MathHelper.PiOver2);
            //effect.Texture = texture;
            graphicd.RasterizerState = new RasterizerState() { FillMode = FillMode.Solid }; // Wireframe as in the picture
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicd.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, nvertices, indices, 0, indices.Length/3);
                //graphicd.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineStrip, vertices, 0, nvertices -1);
            }
        }

    }
}
