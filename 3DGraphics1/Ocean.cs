using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    public class Ocean
    {
        VertexPositionTexture[] oceanVerts;
        float oceanSize = 20;
        Texture2D texture;

        public Ocean(float size)
        {
            this.oceanSize = size;
            InitializeVerts();
        }

        private void InitializeVerts()
        {
            oceanVerts = new VertexPositionTexture[6];
            oceanVerts[0].Position = new Vector3(-oceanSize, -oceanSize, 0);
            oceanVerts[1].Position = new Vector3(-oceanSize, oceanSize, 0);
            oceanVerts[2].Position = new Vector3(oceanSize, -oceanSize, 0);
            oceanVerts[3].Position = oceanVerts[1].Position;
            oceanVerts[4].Position = new Vector3(oceanSize, oceanSize, 0);
            oceanVerts[5].Position = oceanVerts[2].Position;

            float repetitions = 20.0f;
            oceanVerts[0].TextureCoordinate = new Vector2(0, 0);
            oceanVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            oceanVerts[2].TextureCoordinate = new Vector2(repetitions, 0);
            oceanVerts[3].TextureCoordinate = oceanVerts[1].TextureCoordinate;
            oceanVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            oceanVerts[5].TextureCoordinate = oceanVerts[2].TextureCoordinate;
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public VertexPositionTexture[] OceanVerts { get => oceanVerts; }
        public Texture2D OceanTexture { get => texture; }
    }
}
