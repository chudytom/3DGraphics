using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    public struct VertexPositionNormalTexture : IVertexType
    {
        public Vector3 Position;

        public Vector3 Normal;

        public Vector2 TextureCoordinate;

        public VertexPositionNormalTexture(Vector3 position, Vector3 normal, Vector2 textureCoordinate)
        {
            Position = position;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
        );

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    }

    public class PrimitiveOcean
    {
        private readonly Camera _camera;
        private readonly List<ushort> _indices;
        private readonly VertexBuffer _vertexBuffer;
        private readonly IndexBuffer _indexBuffer;
        private Effect _oceanEffect;
        private readonly List<VertexPositionNormalTexture> _vertices;
        private Texture2D[] OceanNormalMaps;
        GraphicsDevice _graphicsDevice;
        private Vector3 position = new Vector3(0, 0, 0);

        public PrimitiveOcean(GraphicsDevice graphicsDevice, Camera camera, ContentManager contentManager)
        {
            _graphicsDevice = graphicsDevice;
            _camera = camera;
            var graphicsDevice1 = graphicsDevice;
            _vertices = new List<VertexPositionNormalTexture>();
            _indices = new List<ushort>();

            _vertices.Add(new VertexPositionNormalTexture(new Vector3(2000, 30, -2000), Vector3.Up, new Vector2(1, 0)));
            _vertices.Add(new VertexPositionNormalTexture(new Vector3(-2000, 30, 2000), Vector3.Up, new Vector2(0, 1)));
            _vertices.Add(new VertexPositionNormalTexture(new Vector3(-2000, 30, -2000), Vector3.Up, new Vector2(0, 0)));

            _vertices.Add(new VertexPositionNormalTexture(new Vector3(2000, 30, -2000), Vector3.Up, new Vector2(1, 0)));
            _vertices.Add(new VertexPositionNormalTexture(new Vector3(2000, 30, 2000), Vector3.Up, new Vector2(1, 1)));
            _vertices.Add(new VertexPositionNormalTexture(new Vector3(-2000, 30, 2000), Vector3.Up, new Vector2(0, 1)));

            _indices.Add(0);
            _indices.Add(1);
            _indices.Add(2);
            _indices.Add(1);
            _indices.Add(3);
            _indices.Add(2);
            _vertexBuffer = new VertexBuffer(graphicsDevice1, typeof(VertexPositionNormalTexture), _vertices.Count, BufferUsage.None);
            _vertexBuffer.SetData(_vertices.ToArray());
            _indexBuffer = new IndexBuffer(graphicsDevice1, typeof(ushort), _indices.Count, BufferUsage.None);
            _indexBuffer.SetData(_indices.ToArray());
            _oceanEffect = contentManager.Load<Effect>("Shaders/OceanShader");

            OceanNormalMaps = new Texture2D[4];
            for (var i = 0; i < 4; i++)
                using (var stream = TitleContainer.OpenStream("Content/Images/sea" + (i + 1) + ".jpg"))
                {
                    OceanNormalMaps[i] = Texture2D.FromStream(graphicsDevice1, stream);
                }
            //OceanNormalMaps[i] = contentManager.Load<Texture2D>("Images/sea" + (i + 1));
        }

        public void Draw(GameTime gameTime, TextureCube skyTexture)
        {
            var graphicsDevice = _graphicsDevice;
            graphicsDevice.RasterizerState = new RasterizerState { FillMode = FillMode.Solid };
            graphicsDevice.SetVertexBuffer(_vertexBuffer);
            graphicsDevice.Indices = _indexBuffer;
  
            _oceanEffect.Parameters["World"].SetValue(GetWorldMatrix());
            _oceanEffect.Parameters["View"].SetValue(_camera.ViewMatrix);
            _oceanEffect.Parameters["Projection"].SetValue(_camera.ProjectionMatrix);
            _oceanEffect.Parameters["EyePos"].SetValue(_camera.Position);
            // set the sky texture
            //_oceanEffect.Parameters["cubeTex"].SetValue(skyTexture);

            // choose and set the ocean textures
            var oceanTexIndex = (int)gameTime.TotalGameTime.TotalSeconds % 4;
            _oceanEffect.Parameters["normalTex"].SetValue(OceanNormalMaps[0]);
            _oceanEffect.Parameters["normalTex2"].SetValue(OceanNormalMaps[3]);
            _oceanEffect.Parameters["textureLerp"].SetValue(0.5f);
            _oceanEffect.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds * 0.02f);

            _oceanEffect.CurrentTechnique = _oceanEffect.Techniques["Ocean"];
            _oceanEffect.CurrentTechnique.Passes[0].Apply();
            //foreach (var pass in _oceanEffect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
                //var primitiveCount = _indices.Count / 3;
                graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices.ToArray(), 0, 2);
            //}
        }

        protected Matrix GetWorldMatrix()
        {
            Matrix combinedMatrix = Matrix.CreateScale(1.0f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateTranslation(position);
            return combinedMatrix;
        }
    }
}
