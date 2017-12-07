using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FirstProject
{
    /// <summary>
    /// Handles all of the aspects of working with a skybox.
    /// </summary>
    public class Skybox
    {
        private readonly Model _skyBox;
        private Effect _skyBoxEffect;
        private const float Size = 50f;
        private readonly TextureCube _skyBoxTexture;
        public TextureCube Texture => _skyBoxTexture;

        public Skybox(ContentManager content)
        {
            _skyBox = content.Load<Model>("Skyboxes/cube");
            _skyBoxTexture = content.Load<TextureCube>("Skyboxes/Islands");
            _skyBoxEffect = content.Load<Effect>("Shaders/SkyboxShader");
        }
        public void Draw(Camera camera)
        {
            foreach (var _ in _skyBoxEffect.CurrentTechnique.Passes)
            {
                foreach (var mesh in _skyBox.Meshes)
                {
                    foreach (var part in mesh.MeshParts)
                    {
                        part.Effect = _skyBoxEffect;
                        part.Effect.Parameters["World"].SetValue(Matrix.CreateScale(Size) * Matrix.CreateTranslation(camera.Position));
                        part.Effect.Parameters["View"].SetValue(camera.ViewMatrix);
                        part.Effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        part.Effect.Parameters["SkyBoxTexture"].SetValue(_skyBoxTexture);
                        part.Effect.Parameters["CameraPosition"].SetValue(camera.Position);
                    }
                    mesh.Draw();
                }
            }
        }
    }
}