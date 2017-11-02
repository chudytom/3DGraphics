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
    class Sphere2
    {
        private int degree;



        private Vector3 getNormalVector(Vector3 point)

        {

            var normal = new Vector3(point.X, point.Y, point.Z);

            normal.Normalize();

            return normal;

        }



        public Sphere2(ContentManager ctx, float curvyness, int degree, float radius, GraphicsDevice dev, Vector3 position, float xRotation, float yRotation, float zRotation)

        {



            this.degree = degree;

            var fragmentVertices = new List<Vector3>(90 / degree + 1);

            var v1 = new Vector3(radius, 0, 0);



            //this.vertices = new VertexNormalVector[(360 / degree + 1) * (90 / degree + 1) * 6];

            //generate fragment

            for (var i = 0; i <= 90 / degree; i++)

            {

                fragmentVertices.Add(Vector3.Multiply(Vector3.Transform(v1, Matrix.CreateRotationZ(MathHelper.ToRadians(degree * i))), 1 - curvyness * (float)Math.Sin(MathHelper.ToRadians(i * degree))));

            }



            for (var i = 0; i < 360 / degree; i++)

            {

                var f1 = fragmentVertices.Select(e => Vector3.Transform(e, Matrix.CreateRotationY(MathHelper.ToRadians(i * degree)))).ToList();

                var f2 = fragmentVertices.Select(e => Vector3.Transform(e, Matrix.CreateRotationY(MathHelper.ToRadians((i + 1) * degree)))).ToList();



                for (var j = 0; j < 90 / degree; j++)

                {

                    var currentVertexNumber = i * 90 / degree * 6 + j * 6;

                    var normal = this.getNormalVector(f1[0 + j]);

                    //this.vertices[currentVertexNumber] = new VertexNormalVector(f1[0 + j], normal, Color.SandyBrown);

                    normal = this.getNormalVector(f2[0 + j]);

                    //this.vertices[currentVertexNumber + 2] = new VertexNormalVector(f2[0 + j], normal, Color.SandyBrown);

                    normal = this.getNormalVector(f1[1 + j]);

                    //this.vertices[currentVertexNumber + 1] = new VertexNormalVector(f1[1 + j], normal, Color.SandyBrown);



                    normal = this.getNormalVector(f1[1 + j]);

                    //this.vertices[currentVertexNumber + 4] = new VertexNormalVector(f1[1 + j], normal, Color.SandyBrown);

                    normal = this.getNormalVector(f2[0 + j]);

                    //this.vertices[currentVertexNumber + 3] = new VertexNormalVector(f2[0 + j], normal, Color.SandyBrown);

                    normal = this.getNormalVector(f2[1 + j]);

                    //this.vertices[currentVertexNumber + 5] = new VertexNormalVector(f2[1 + j], normal, Color.SandyBrown);

                }
            }

        }
    }
}
