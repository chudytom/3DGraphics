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
        int longitudes = 15; //how many vertices in a circle
        int latitudes = 15; //how many circles
        Color sphereColor;
        public Sphere(GraphicsDevice graphics, float radius, int longitudes, int latitudes, Color color)
        {
            graphicd = graphics;
            this.radius = radius;
            this.longitudes = longitudes;
            this.latitudes = latitudes;
            this.sphereColor = color;
            effect = new BasicEffect(graphicd);
            nvertices = longitudes * latitudes; // 90 vertices in a circle, 90 circles in a sphere
            nindices = longitudes * latitudes * 6;
            vbuffer = new VertexBuffer(graphics, typeof(VertexPositionNormalTexture), nvertices, BufferUsage.WriteOnly);
            ibuffer = new IndexBuffer(graphics, IndexElementSize.SixteenBits, nindices, BufferUsage.WriteOnly);
            Createspherevertices();
            Createindices();
            vbuffer.SetData<VertexPositionColor>(vertices);
            ibuffer.SetData<short>(indices);
            effect.VertexColorEnabled = true;
        }
        void Createspherevertices()
        {
            vertices = new VertexPositionColor[nvertices];
            Vector3 center = new Vector3(0, 0, 0);
            Vector3 rad = new Vector3((float)Math.Abs(radius), 0, 0);
            for (int x = 0; x < latitudes; x++) //number of circles, difference between each is 360/latitudes degrees
            {
                float difx = 360.0f / latitudes;
                for (int y = 0; y < longitudes; y++) //number of veritces in a circle, difference between each is 360/longitudes degrees 
                {
                    float dify = 360.0f / longitudes;
                    Matrix zrot = Matrix.CreateRotationZ(MathHelper.ToRadians(y * dify)); 
                    Matrix yrot = Matrix.CreateRotationY(MathHelper.ToRadians(x * difx)); 
                    Vector3 point = Vector3.Transform(Vector3.Transform(rad, zrot), yrot);//transformation

                    vertices[x + y * longitudes] = new VertexPositionColor(point, sphereColor);
                }
            }
        }
        void Createindices()
        {
            indices = new short[nindices];
            int i = 0;
            for (int x = 0; x < latitudes; x++)
            {
                for (int y = 0; y < longitudes; y++)
                {
                    int s1 = x == latitudes - 1 ? 0 : x + 1;
                    int s2 = y == longitudes - 1 ? 0 : y + 1;
                    short upperLeft = (short)(x * latitudes + y);
                    short upperRight = (short)(s1 * latitudes + y);
                    short lowerLeft = (short)(x * latitudes + s2);
                    short lowerRight = (short)(s1 * longitudes + s2);
                    indices[i++] = upperLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerLeft;
                    indices[i++] = lowerLeft;
                    indices[i++] = upperRight;
                    indices[i++] = lowerRight;
                }
            }
        }

        public void Draw(Camera cam) // the camera class contains the View and Projection Matrices
        {
            effect.View = cam.ViewMatrix;
            effect.Projection = cam.ProjectionMatrix;
            graphicd.RasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame }; // Wireframe as in the picture
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicd.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleList, vertices, 0, nvertices, indices, 0, indices.Length / 3);
            }
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
