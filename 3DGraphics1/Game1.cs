using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirstProject
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        private Texture2D _chessBoardTexture;
        Texture2D _oceanTexture1;
        Vector3 cameraPosition = new Vector3(2, 30, 30);
        Vector3 robotModelPosition;
        Camera _camera;
        BasicOcean basicOcean;
        private Sphere sphere;
        private List<Palm> palms = new List<Palm>();
        float palmPositionAngle = 10;
        float sphereRadius = 5;
        float oceanSize = 140.0f;
        private Robot _robot;
        private LightsManager lightsManager;
        private Effect _specularEffect;
        private Effect _textureEffect;
        private Effect _oceanEffect;
        private Skybox skybox;
        private PrimitiveOcean _primitiveOcean;
        private Ocean _ocean;
        private Texture2D _oceanTexture2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            _specularEffect = Content.Load<Effect>("Shaders/Specular");
            _textureEffect = Content.Load<Effect>("Shaders/Texture");
            _oceanEffect = Content.Load<Effect>("Shaders/NewOceanShader");
            _camera = new Camera(graphics.GraphicsDevice);
            skybox = new Skybox(Content);
            _primitiveOcean = new PrimitiveOcean(graphics.GraphicsDevice, _camera, Content);

            lightsManager = new LightsManager(prepareLights: true);
            basicOcean = new BasicOcean(oceanSize);
            sphere = new Sphere(GraphicsDevice, sphereRadius, latitudes: 30, longitudes: 30, color: Color.Red, effect: _specularEffect);
            float palmZTranslation = (float)(sphereRadius * Math.Cos(MathHelper.ToRadians(palmPositionAngle)));
            float palmSideTranslation = (float)(sphereRadius * Math.Sin(MathHelper.ToRadians(palmPositionAngle)));
            palms.Add(new Palm(new Vector3(0 , palmZTranslation, palmSideTranslation), _textureEffect));
            palms.Add(new Palm(new Vector3(0, palmZTranslation, -palmSideTranslation), _textureEffect));
            foreach (var palm in palms)
            {
                palm.Initialize(Content, graphics);
            }
            _robot = new Robot(_textureEffect);
            _robot.Initialize(Content);
            _ocean = new Ocean(_oceanEffect);
            _ocean.SetScale(100);
            _ocean.Initialize(Content, graphics);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            using (var stream = TitleContainer.OpenStream("Content/chessboard.png"))
            {
                _chessBoardTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
            using (var stream = TitleContainer.OpenStream("Content/Ocean.jpg"))
            {
                _oceanTexture1 = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
            using (var stream = TitleContainer.OpenStream("Content/Images/sea1.jpg"))
            {
                _oceanTexture2 = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
    
            robotModelPosition = new Vector3(0, 0, 3);
            _robot.SetTexture(_chessBoardTexture);
            _ocean.SetTexture(_oceanTexture2);
            //foreach (var palm in palms)
            //{
            //    palm.SetTexture(_oceanTexture1);
            //}
            sphere.SetTexture(_oceanTexture1);
        }

        protected override void Update(GameTime gameTime)
        {
            _robot.Update(gameTime);
            _ocean.Update(gameTime);
            _camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            sphere.Draw(_camera);
            foreach (var palm in palms)
            {
                palm.Draw(_camera);
            }
            _robot.Draw(_camera);
            _ocean.Draw(_camera);


            graphics.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.CullClockwiseFace };
            skybox.Draw(_camera);
            graphics.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.CullCounterClockwiseFace };
            //graphics.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.CullClockwiseFace };
            //ocean.Draw(gameTime, skybox.Texture);
            //graphics.GraphicsDevice.RasterizerState = new RasterizerState() { CullMode = CullMode.CullCounterClockwiseFace };
            base.Draw(gameTime);
        }
    }
}
