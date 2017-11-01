using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FirstProject
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        VertexPositionTexture[] floorVerts;
        BasicEffect effect;
        Texture2D chessBoardTexture;
        Vector3 cameraPosition = new Vector3(2, 30, 30);
        Model robotModel;
        Vector3 robotModelPosition;
        Robot robot;
        Camera camera;

        // This is the model instance that we'll load
        // our XNB into:

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            floorVerts = new VertexPositionTexture[6];
            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);
            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            float repetitions = 20.0f;
            //floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            //floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            //floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);
            //floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            //floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            //floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            effect = new BasicEffect(graphics.GraphicsDevice);

            robot = new Robot();
            robot.Initialize(Content);

            camera = new Camera(graphics.GraphicsDevice);
            // We’ll be assigning texture values later
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Notice that loading a model is very similar
            // to loading any other XNB (like a Texture2D).
            // The only difference is the generic type.
            robotModel = Content.Load<Model>("robot");
            using (var stream = TitleContainer.OpenStream("Content/chessboard.png"))
            {
                chessBoardTexture = Texture2D.FromStream(this.GraphicsDevice, stream);
            }
                robotModelPosition = new Vector3(0, 0, 3);

            
        }

        protected override void Update(GameTime gameTime)
        {
            robot.Update(gameTime);
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //DrawUsetPrimitives(floorVerts);
            //DrawUsetPrimitives(GenerateSphere(30, 20, 20));
            //float aspectRatio =
            //    graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
            robot.Draw(camera);

            var sphere = new Sphere(GraphicsDevice, radius: 5, longitudes:20, latitudes:20, color:Color.LightGoldenrodYellow);
            sphere.Draw(camera);

            base.Draw(gameTime);
        }


        private void DrawModel(Model model, Vector3 modelPosition)
        {
            foreach (var mesh in model.Meshes)
            {
                // "Effect" refers to a shader. Each mesh may
                // have multiple shaders applied to it for more
                // advanced visuals. 
                foreach (BasicEffect effect in mesh.Effects)
                {
                    // We could set up custom lights, but this
                    // is the quickest way to get somethign on screen:
                    effect.EnableDefaultLighting();
                    // This makes lighting look more realistic on
                    // round surfaces, but at a slight performance cost:
                    effect.PreferPerPixelLighting = true;

                    // The world matrix can be used to position, rotate
                    // or resize (scale) the model. Identity means that
                    // the model is unrotated, drawn at the origin, and
                    // its size is unchanged from the loaded content file.
                    effect.World = Matrix.CreateTranslation(modelPosition);

                    // Move the camera 8 units away from the origin:
                    //var cameraPosition = new Vector3(0, 10, 0);
                    // Tell the camera to look at the origin:
                    var cameraLookAtVector = Vector3.Zero;
                    // Tell the camera that positive Z is up
                    var cameraUpVector = Vector3.UnitZ;

                    effect.View = Matrix.CreateLookAt(
                    cameraPosition, cameraLookAtVector, cameraUpVector);

                    // We want the aspect ratio of our display to match
                    // the entire screen's aspect ratio:
                    float aspectRatio =
                    graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
                    // Field of view measures how wide of a view our camera has.
                    // Increasing this value means it has a wider view, making everything
                    // on screen smaller. This is conceptually the same as "zooming out".
                    // It also 
                    float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
                    // Anything closer than this will not be drawn (will be clipped)
                    float nearClipPlane = 1;
                    // Anything further than this will not be drawn (will be clipped)
                    float farClipPlane = 200;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                    fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

                }

                // Now that we've assigned our properties on the effects we can
                // draw the entire mesh
                mesh.Draw();
            }
        }

        private void DrawUsetPrimitives(VertexPositionTexture[] vertexData)
        {
            //var cameraPosition = new Vector3(0, 40, 20);
            effect.View = camera.ViewMatrix;

            effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = chessBoardTexture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList,
                    vertexData, 0, 2);
            }
        }

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
