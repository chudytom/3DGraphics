using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    internal class Sphere : ModelBase
    {
        VertexNormalVector[] vertices; //later, I will provide another example with VertexPositionNormalTexture
        VertexBuffer vbuffer;
        short[] indices; //my laptop can only afford Reach, no HiDef :(
        IndexBuffer ibuffer;
        float radius;
        int nvertices, nindices;
        //BasicEffect effect;
        GraphicsDevice graphicd;
        int latitudes = 15; //how many vertices in a circle
        int longitudes = 15; //how many circles
        Vector3 spherePosition;
        Color sphereColor;
        Texture2D texture;
        public Sphere(GraphicsDevice graphics, float radius, int latitudes, int longitudes, Color color, Effect effect) : base(effect)
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
            CalculateNormals();
            vbuffer.SetData<VertexNormalVector>(vertices);
            ibuffer.SetData<short>(indices);
            //effect.VertexColorEnabled = false;
            //effect.PreferPerPixelLighting = true;
            //effect.SpecularPower = 5;
            spherePosition = new Vector3(0, 0, 0);
            //effect.vie
            //effect.PreferPerPixelLighting = false;
            //effect.EnableDefaultLighting();
        }

        void Createspherevertices()
        {
            vertices = new VertexNormalVector[nvertices];
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 rad = new Vector3((float)Math.Abs(radius), 0, 0);
            for (int x = 0; x < longitudes; x++) //number of veritces in a circle, difference between each is 360/longitudes degrees
            {
                float difx = 360.0f / longitudes;

                for (int y = 0; y < latitudes/2; y++) // number of circles, difference between each is 360/latitudes degrees
                {
                    float dify = 360.0f / latitudes;
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify));
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx));
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation

                    int vertexNumber = x * latitudes + y;
                    vertices[vertexNumber] = new VertexNormalVector();
                    vertices[vertexNumber].Position = point;
                    vertices[vertexNumber].Color = sphereColor;
                }
            }
        }
        void Createindices()
        {
            indices = new short[nindices];
            int i = 0;
            for (int x = 0; x < longitudes; x++)
            {

                for (int y = 0; y < latitudes/2; y++)
                {
                    int s1 = x == longitudes - 1 ? 0 : x + 1;
                    int s2 = y == latitudes - 1 ? 0 : y + 1;
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

        private void CalculateNormals()
        {
            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public override void Draw(Camera camera) // the camera class contains the View and Projection Matrices
        {
            //graphicd.RasterizerState = new RasterizerState() { FillMode = FillMode.Solid }; // Wireframe as in the picture
            PrepareEffect(camera);
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicd.DrawUserIndexedPrimitives<VertexNormalVector>(PrimitiveType.TriangleList, vertices, 0, nvertices, indices, 0, indices.Length / 3, vertices[0].VertexDeclaration);
            }
        }

        protected override Matrix GetWorldMatrix()
        {
            Matrix combinedMatrix = Matrix.CreateScale(1.0f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateTranslation(spherePosition);
            return combinedMatrix;
        }

    }
}
