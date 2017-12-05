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
        BasicEffect effect;
        Texture2D chessBoardTexture;
        Vector3 cameraPosition = new Vector3(2, 30, 30);
        Model robotModel;
        Vector3 robotModelPosition;
        Camera camera;
        Ocean ocean;
        Sphere sphere;
        List<Palm> palms = new List<Palm>();
        float palmPositionAngle = 10;
        float sphereRadius = 5;
        float oceanSize = 140.0f;
        Robot robot;
        LightsManager lightsManager;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            effect = new BasicEffect(graphics.GraphicsDevice);
            camera = new Camera(graphics.GraphicsDevice);

            lightsManager = new LightsManager(prepareLights: true);
            ocean = new Ocean(oceanSize);
            sphere = new Sphere(GraphicsDevice, sphereRadius, latitudes: 30, longitudes: 30, color: Color.Red, light:lightsManager.Lights);
            float palmZTranslation = (float)(sphereRadius * Math.Cos(MathHelper.ToRadians(palmPositionAngle)));
            float palmSideTranslation = (float)(sphereRadius * Math.Sin(MathHelper.ToRadians(palmPositionAngle)));
            palms.Add(new Palm(new Vector3(palmSideTranslation, 0, palmZTranslation), lightsManager.Lights) { Color = Color.Green });
            palms.Add(new Palm(new Vector3(-palmSideTranslation, 0, palmZTranslation), lightsManager.Lights) { Color = Color.Red });
            foreach (var palm in palms)
            {
                palm.Initialize(Content);
            }
            robot = new Robot(lightsManager.Lights);
            robot.Initialize(Content);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            robotModel = Content.Load<Model>("robot");

            using (var stream = TitleContainer.OpenStream("Content/chessboard.png"))
            {
                chessBoardTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
            using (var stream = TitleContainer.OpenStream("Content/Ocean.jpg"))
            {
                ocean.SetTexture(Texture2D.FromStream(this.GraphicsDevice, stream));
            }
            robotModelPosition = new Vector3(0, 0, 3);
            sphere.SetTexture(chessBoardTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            robot.Update(gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            ocean.Draw(camera);
            sphere.Draw(camera);
            foreach (var palm in palms)
            {
                palm.Draw(camera);
            }
            robot.Draw(camera);
            base.Draw(gameTime);
        }
    }
}
