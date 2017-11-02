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
        //Sphere2 sphere2;
        List<Palm> palms = new List<Palm>();
        float palmPositionAngle = 10;
        float sphereRadius = 5;
        float oceanSize = 140.0f;
        Robot robot;
        DirLight directionalLight;
        List<DirLight> lights = new List<DirLight>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            lights.Add(new DirLight()
            {
                Direction = new Vector3(0.5f, 0, 0),
                DiffuseColor = new Vector3(1, 0, 0),
                SpecularColor = new Vector3(0, 0, 1)
            });
            lights.Add(new DirLight()
            {
                Direction = new Vector3(0, 0.5f, 0),
                DiffuseColor = new Vector3(-1, 0, 0),
                SpecularColor = new Vector3(0, 0, 1)
            });
            lights.Add(new DirLight()
            {
                Direction = new Vector3(0, 0, -1),
                DiffuseColor = new Vector3(0.02f, 0.02f, 1),
                SpecularColor = new Vector3(0, 0, 1)
            });

        }

        protected override void Initialize()
        {

            effect = new BasicEffect(graphics.GraphicsDevice);
            camera = new Camera(graphics.GraphicsDevice);

            ocean = new Ocean(oceanSize);
            sphere = new Sphere(GraphicsDevice, radius: sphereRadius, latitudes: 30, longitudes: 30, color: Color.Yellow, light:lights);
            //sphere2 = new Sphere2(Content, 0.5f, 180, sphereRadius, GraphicsDevice, new Vector3(0, 0, 0), 0, 0, 0);
            float palmHeight = (float)(sphereRadius * Math.Cos(MathHelper.ToRadians(palmPositionAngle)));
            float palmSideTranslation = (float)(sphereRadius * Math.Sin(MathHelper.ToRadians(palmPositionAngle)));
            palms.Add(new Palm(new Vector3(palmSideTranslation, 0, palmHeight), lights) { Color = Color.Green });
            palms.Add(new Palm(new Vector3(-palmSideTranslation, 0, palmHeight), lights) { Color = Color.Red });
            foreach (var palm in palms)
            {
                palm.Initialize(Content);
            }
            robot = new Robot(lights);
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

            DrawGround(ocean.OceanVerts, ocean.OceanTexture);
            sphere.Draw(camera);
            foreach (var palm in palms)
            {
                palm.Draw(camera);
            }
            //sphere2.Draw(camera.ViewMatrix, camera.ProjectionMatrix, new Vector3(0, 0, 0));
            robot.Draw(camera);
            base.Draw(gameTime);
        }



        private void DrawGround(VertexPositionTexture[] vertexData, Texture2D texture)
        {
            //var cameraPosition = new Vector3(0, 40, 20);
            effect.View = camera.ViewMatrix;

            effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = texture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList,
                    vertexData, 0, 2);
            }
        }

        //private void DrawModel(Model model, Vector3 modelPosition)
        //{
        //    foreach (var mesh in model.Meshes)
        //    {
        //        // "Effect" refers to a shader. Each mesh may
        //        // have multiple shaders applied to it for more
        //        // advanced visuals. 
        //        foreach (BasicEffect effect in mesh.Effects)
        //        {
        //            // We could set up custom lights, but this
        //            // is the quickest way to get somethign on screen:
        //            effect.EnableDefaultLighting();
        //            // This makes lighting look more realistic on
        //            // round surfaces, but at a slight performance cost:
        //            effect.PreferPerPixelLighting = true;

        //            // The world matrix can be used to position, rotate
        //            // or resize (scale) the model. Identity means that
        //            // the model is unrotated, drawn at the origin, and
        //            // its size is unchanged from the loaded content file.
        //            effect.World = Matrix.CreateScale(0.3f) * Matrix.CreateRotationX(MathHelper.PiOver2) * Matrix.CreateTranslation(modelPosition) ;

        //            // Move the camera 8 units away from the origin:
        //            //var cameraPosition = new Vector3(0, 10, 0);
        //            // Tell the camera to look at the origin:
        //            var cameraLookAtVector = Vector3.Zero;
        //            // Tell the camera that positive Z is up
        //            var cameraUpVector = Vector3.UnitZ;

        //            effect.Projection = camera.ProjectionMatrix;
        //            effect.View = camera.ViewMatrix;

        //        }

        //        // Now that we've assigned our properties on the effects we can
        //        // draw the entire mesh
        //        mesh.Draw();
        //    }
        //}

        static VertexPositionTexture[] GenerateSphere(float radius, int latitudes, int longitudes)
        {
            float latitude_increment = 360.0f / latitudes;
            float longitude_increment = 180.0f / longitudes;
            // if this causes an error, consider changing the size to [(latitude + 1)*(longitudes + 1)], but this should work.
            VertexPositionTexture[] vertices = new VertexPositionTexture[latitudes * longitudes];

            int counter = 0;

            for (float u = 0; u < 360.0f; u += latitude_increment)
            {
                for (float t = 0; t < 180.0f; t += longitude_increment)
                {

                    float rad = radius;

                    float x = (float)(rad * Math.Sin(MathHelper.ToRadians(t)) * Math.Sin(MathHelper.ToRadians(u)));
                    float y = (float)(rad * Math.Cos(MathHelper.ToRadians(t)));
                    float z = (float)(rad * Math.Sin(MathHelper.ToRadians(t)) * Math.Cos(MathHelper.ToRadians(u)));

                    //var vertexPositionTexture = new VertexPositionTexture();
                    vertices[counter].Position = new Vector3(x + 1, y + 2, z);
                    //vertices[counter].TextureCoordinate.X = x;
                    //vertices[counter].TextureCoordinate.Y = y;
                    //vertices[counter++] = vertexPositionTexture;
                    counter++;
                }
            }
            //vertices = new VertexPositionTexture[6];
            //vertices[0].Position = new Vector3(-20, -20, 0);
            //vertices[1].Position = new Vector3(-20, 20, 0);
            //vertices[2].Position = new Vector3(20, -20, 0);
            //vertices[3].Position = vertices[1].Position;
            //vertices[4].Position = new Vector3(20, 20, 0);
            //vertices[5].Position = vertices[2].Position;

            return vertices;

        }
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    //public class Game1 : Game
    //{
    //    GraphicsDeviceManager graphics;
    //    SpriteBatch spriteBatch;
    //    private Model palmModel1;
    //    private Model palmModel2;
    //    private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
    //    private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
    //    private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
    //    private Vector3 position;
    //    private float angle;

    //    public Game1()
    //    {
    //        graphics = new GraphicsDeviceManager(this);
    //        Content.RootDirectory = "Content";
    //    }

    //    /// <summary>
    //    /// Allows the game to perform any initialization it needs to before starting to run.
    //    /// This is where it can query for any required services and load any non-graphic
    //    /// related content.  Calling base.Initialize will enumerate through any components
    //    /// and initialize them as well.
    //    /// </summary>
    //    protected override void Initialize()
    //    {
    //        // TODO: Add your initialization logic here

    //        base.Initialize();
    //    }

    //    /// <summary>
    //    /// LoadContent will be called once per game and is the place to load
    //    /// all of your content.
    //    /// </summary>
    //    protected override void LoadContent()
    //    {
    //        // Create a new SpriteBatch, which can be used to draw textures.
    //        spriteBatch = new SpriteBatch(GraphicsDevice);
    //        palmModel1 = Content.Load<Model>("MY_PALM");
    //        palmModel2 = Content.Load<Model>("Palm1");
    //        position = new Vector3(0, 0, 0);
    //        angle = 0;

    //        // TODO: use this.Content to load your game content here
    //    }

    //    /// <summary>
    //    /// UnloadContent will be called once per game and is the place to unload
    //    /// game-specific content.
    //    /// </summary>
    //    protected override void UnloadContent()
    //    {
    //        // TODO: Unload any non ContentManager content here
    //        Content.Unload();
    //    }

    //    /// <summary>
    //    /// Allows the game to run logic such as updating the world,
    //    /// checking for collisions, gathering input, and playing audio.
    //    /// </summary>
    //    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    //    protected override void Update(GameTime gameTime)
    //    {
    //        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
    //            Exit();

    //        position += new Vector3(0, 0.01f, 0);
    //        angle += 0.03f;
    //        world = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(position);
    //        // TODO: Add your update logic here
    //        base.Update(gameTime);
    //    }

    //    /// <summary>
    //    /// This is called when the game should draw itself.
    //    /// </summary>
    //    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    //    protected override void Draw(GameTime gameTime)
    //    {
    //        GraphicsDevice.Clear(Color.CornflowerBlue);


    //        DrawModel(palmModel1, world, view, projection);
    //        DrawModel(palmModel2, world, view, projection);
    //        // TODO: Add your drawing code here

    //        base.Draw(gameTime);
    //    }

    //    private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
    //    {
    //        foreach (ModelMesh mesh in model.Meshes)
    //        {
    //            foreach (BasicEffect effect in mesh.Effects)
    //            {
    //                effect.World = world;
    //                effect.View = view;
    //                effect.Projection = projection;
    //            }

    //            mesh.Draw();
    //        }
    //    }
    //}
}
