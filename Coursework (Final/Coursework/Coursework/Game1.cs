using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Coursework
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Creates an insatnce for skybox class
        Skybox skybox;
        
        //creates private instance for the player class
        private Player player;

        //creates an instance for Terrain class
        Terrain landscape;

        // create an array of enemy daleks
        private Model mdlDalek;
        private Matrix[] mdDalekTransforms;
        private Daleks[] dalekList = new Daleks[GameConstants.NumDaleks];

        // creates an array for the laser
        private Model mdlLaser;
        private Matrix[] mdlLaserTransforms;
        private Laser[] laserList = new Laser[GameConstants.NumLasers];

        //private variable to assign random to positions or time
        private Random random = new Random();
        
        //variable for the background music
        Song bkgMusic;
        //soundeffect called click for the noise when navigating through the menu
        SoundEffect click;

        float aspectRatio; // The aspect ratio determines how to scale 3d to 2d projection.

        //Creates instance of the camera class
        Camera camera;

        // Initialises basic effect for drawing model
        BasicEffect basicEffect;
        
        //creates instance of the menu class
        private Menu menu;
        //creates an instance of the input class
        private Input input;

        // Texture for Control Screen
        Texture2D backGround;
      
        public static GameStates gamestate;

        SpriteFont fontToUse;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        // Creates the GameStates
        public enum GameStates 
        {
             Menu,
             Play,
             Controls,
             Exit
        }


        #region Initialize
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        protected override void Initialize()
        {
            //Intialises the variable previously created as a new menu
            menu = new Menu();
            //sets the gamestate at menu
            gamestate = GameStates.Menu;
            //Intialises the variable previously created as a new input
            input = new Input();
            
            // Gets aspectRatio using the Graphics device
            aspectRatio = GraphicsDevice.Viewport.AspectRatio;

            //Intialises the variable previously created as a new Camera
            camera = new Camera();
            camera.InitializeCamera(aspectRatio);

            //Intialises the variable previously created as a new Terrain
            landscape = new Terrain(GraphicsDevice);

            //Initialises the method that deals with the effects
            InitializeEffect();

            //Initialises the method that resets the enemy daleks
            ResetDaleks();

            base.Initialize();
        }
        #endregion

        #region Load Content
        /// LoadContent will be called once per game and is the place to load all of your content.
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Assigns the fontToUse variable to the font.
            fontToUse = Content.Load<SpriteFont>(".\\Fonts\\DrWho");
            //Links the player variable to the model which will be controlled by the user/player
            player = new Player("Models\\dalek", Content);

            // Loads all assets for daleks and lasers
            mdlDalek = Content.Load<Model>(".\\Models\\dalek");
            mdDalekTransforms = SetupEffectTransformDefaults(mdlDalek);
            mdlLaser = Content.Load<Model>(".\\Models\\laser");
            mdlLaserTransforms = SetupEffectTransformDefaults(mdlLaser);

            // Loads Audio
            bkgMusic = Content.Load<Song>(".\\Audio\\Bluebell");
            click = Content.Load<SoundEffect>("Audio\\Click");

            //Loads Skybox
            skybox = new Skybox("Skybox/Sunset", Content);

            //load heightMap and heightMapTexture to create landscape
            landscape.SetHeightMapData(Content.Load<Texture2D>("Textures\\HeightMap"), Content.Load<Texture2D>("Textures\\TextureMap"));

            //Loads the background texture for the memu
            backGround = Content.Load<Texture2D>("Textures\\Background");

            // TODO: use this.Content to load your game content here
        }
        #endregion

        //This method sets up each effect transformation for any model that i create. Does this by passing in the mesh and uses the camera matrices.
        private Matrix[] SetupEffectTransformDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = camera.projectionMatrix;
                    effect.View = camera.viewMatrix;
                }
            }
            return absoluteTransforms;
        }

        //This method is dedicated to drawing each model, it passes in the model name, the transformations the model with go through and then passes in the matrix for the transformation.
        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {

            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.View = camera.viewMatrix;
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }

        #region Unload Content
        // UnloadContent will be called once per game and is the place to unload all content.
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        // Method to spawn daleks, called at beginning of game
        private void ResetDaleks()
        {
            float xStart;
            float zStart;
            // Loops through dalek list
            for (int i = 0; i < GameConstants.NumDaleks; i++)
            {
                // Gives daleks position using randomised xStart and zStart
                xStart = -((float)-GameConstants.PlayfieldSizeX + 0);
                zStart = -(float)random.NextDouble() * GameConstants.PlayfieldSizeZ;
                dalekList[i].position = new Vector3(xStart, 2.0f, zStart);
                // Creates a random angle in which the dalek will move
                double angle = random.NextDouble() * 2 * Math.PI;
                dalekList[i].direction.X = -(float)Math.Sin(angle);
                dalekList[i].direction.Z = (float)Math.Cos(angle);
                // Gives speed using random factor 
                dalekList[i].speed = GameConstants.DalekMinSpeed +(float)random.NextDouble() * GameConstants.DalekMaxSpeed;
                dalekList[i].isActive = true;               
            }
        }

        #region Update
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        protected override void Update(GameTime gameTime)
        {
            input.Update();
            

            if (gamestate == GameStates.Play)
            {
                player.PlayerControls(laserList);
                camera.camUpdate(player.position);
                
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    gamestate = GameStates.Menu;


                // Get user input.
                UpdateInput();
                //Get the player controls method
                player.PlayerControls(laserList);
                //gets the upate method from the player class
                player.Update();
                //gets the collisions
                CheckCollisions();

                //Updates dalek list
                float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                for (int i = 0; i < GameConstants.NumDaleks; i++)
                {
                    dalekList[i].Update(timeDelta);
                }
                //Updates laser list
                for (int i = 0; i < GameConstants.NumLasers; i++)
                {
                    if (laserList[i].isActive)
                    {
                        laserList[i].Update(timeDelta);
                    }
                }


            }
            //Allows the user to exit back to the menu screen
            else if (gamestate == GameStates.Controls)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    gamestate = GameStates.Menu;
            }
            // creates what happens when in the menu state by retriving the users input that is set up within the input class and moves up and down the options accordingly while playing the sound effect.
            else if (gamestate == GameStates.Menu)
            {
                if (input.Down)
                {
                    //move selection up and play sound effect
                    menu.Iterator++;
                    click.Play();
                }
                else if (input.Up)
                {
                    //move selection down and play sound effect
                    menu.Iterator--;
                    click.Play();
                }

                if (input.MenuSelect)
                {
                    if (menu.Iterator == 0)
                    {
                        gamestate = GameStates.Play;
                    }
                    else if (menu.Iterator == 1)
                    {
                        gamestate = GameStates.Controls;
                    }
                    else if (menu.Iterator == 2)
                    {
                        this.Exit();
                    }
                    menu.Iterator = 0;
                }
            }
            else if (gamestate == GameStates.Exit)
            {
                if (input.MenuSelect)
                {
                    gamestate = GameStates.Menu;
                }
            }

            base.Update(gameTime);
        }
        #endregion

        #region Input Update
        protected void UpdateInput()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            // Get the keyboard state.
            KeyboardState currentKeyState = Keyboard.GetState();

                // The following allow the user to toggle the sound on, off or resume by pressing specific buttons on keyboard or on the xbox 360 controller.
                if (currentKeyState.IsKeyDown(Keys.P) || currentState.DPad.Down == ButtonState.Pressed)
                {
                    MediaPlayer.Pause();
                }
                else if (currentKeyState.IsKeyDown(Keys.R) || currentState.DPad.Up == ButtonState.Pressed)
                {
                    MediaPlayer.Resume();
                }
                else if (currentKeyState.IsKeyDown(Keys.O) || currentState.DPad.Right == ButtonState.Pressed)
                {
                    MediaPlayer.Play(bkgMusic);
                }


        }
        #endregion

        //The following method looks for collisions between the daleks, player and the laser.
        //it does this by creating a bounding sphere around each object and checking for intersections
        private void CheckCollisions()
        {
            BoundingSphere PlayerSphere = new BoundingSphere(player.position,player.playerModel.Meshes[0].BoundingSphere.Radius * GameConstants.PlayerBoundingSphereScale);

            for (int i = 0; i < dalekList.Length; i++)
            {
                if (dalekList[i].isActive)
                {
                    //Gives bounding sphere to the dalek
                    BoundingSphere dalekSphereA = new BoundingSphere(dalekList[i].position, mdlDalek.Meshes[0].BoundingSphere.Radius * GameConstants.DalekBoundingSphereScale);

                    for (int k = 0; k < laserList.Length; k++)
                    {
                        if (laserList[k].isActive)
                        {
                            //Gives bounding sphere to laser
                            BoundingSphere laserSphere = new BoundingSphere(laserList[k].position, mdlLaser.Meshes[0].BoundingSphere.Radius * GameConstants.LaserBoundingSphereScale);
                            if (dalekSphereA.Intersects(laserSphere))
                            {
                                
                                dalekList[i].isActive = false;
                                laserList[k].isActive = false;
                                 break; //no need to check other bullets
                            }
                        }
                        if (dalekSphereA.Intersects(PlayerSphere)) //Check collision between Dalek and Player
                        {
                            dalekList[i].direction *= -1.0f;
                            break; //no need to check other bullets
                        }

                    }
                }
            }

        }
        //Sets up effect and takes all the camera's matrices
        private void InitializeEffect()
        {
            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.View = camera.viewMatrix;
            basicEffect.Projection = camera.projectionMatrix;
            basicEffect.World = camera.worldMatrix;
        }

        // This method sets up the effects that are required for the terrain to be drawn.
        public void DrawLandscape()
        {
            landscape.basicEffect.CurrentTechnique.Passes[0].Apply();
            SetEffects(landscape.basicEffect);
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            // Sets up the fog.
            basicEffect.FogEnabled = true;
            basicEffect.FogColor = new Vector3(0.15f);
            basicEffect.FogStart = 100;
            basicEffect.FogEnd = 320;

            foreach (EffectPass pass in landscape.basicEffect.CurrentTechnique.Passes)
            {
                //calls the draw method from terrain class
                landscape.Draw();
            }
        }

        public void SetEffects(BasicEffect basicEffect)
        {
            basicEffect.View = Matrix.CreateLookAt(camera.camPosition, camera.camLookat, Vector3.Up);
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 50000.0f);
            basicEffect.World = camera.worldMatrix;
        }

        private void writeText()
        {
            spriteBatch.Begin();
            // Draw instructions
            Vector2 guidepos = new Vector2(50, 50);
            string guide = "Instructions\nPress Left/Right Arrow to change direction of the camera";
            spriteBatch.DrawString(fontToUse, guide, guidepos, Color.Black);
            spriteBatch.End();
        }

        #region Draw Method
        // This is called when the game should draw itself.
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            
            // Basically says if the menu state = Menu the draw everything associated with the menu which was previously mentioned.
            if (gamestate == GameStates.Menu)
            {
                spriteBatch.Begin();
                menu.DrawMenu(spriteBatch, graphics.GraphicsDevice.Viewport.Width, fontToUse);
                spriteBatch.End();
            }
            // draw code for game if gamestate is in play state
            else if (gamestate == GameStates.Play)
            {
                

                // creates transformation matrix for the playermodel.
                Matrix[] transforms = new Matrix[player.playerModel.Bones.Count];
                //calls the draw method which is within the player class.
                player.Draw(camera, aspectRatio, transforms);

                //draws dalek list
                for (int i = 0; i < GameConstants.NumDaleks; i++)
                {
                    if (dalekList[i].isActive)
                    {
                        Matrix dalekTransform = Matrix.CreateScale(GameConstants.DalekScalar) * Matrix.CreateTranslation(dalekList[i].position);
                        DrawModel(mdlDalek, dalekTransform, mdDalekTransforms);
                    }
                }
                //Draws laser list
                for (int i = 0; i < GameConstants.NumLasers; i++)
                {
                    if (laserList[i].isActive)
                    {
                        Matrix laserTransform = Matrix.CreateScale(GameConstants.LaserScalar) * Matrix.CreateTranslation(laserList[i].position);
                        DrawModel(mdlLaser, laserTransform, mdlLaserTransforms);
                    }
                }
                //calls the drawlandscape method
                DrawLandscape();
                graphics.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
                // Draw skybox which passes in the camera
                skybox.Draw(camera);
                graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                // to get landscape viewable
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            // Drawing code for Control screen
            else if (gamestate == GameStates.Controls)
            {
                spriteBatch.Begin();
                Rectangle screenRectangle = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
                spriteBatch.Draw(backGround, screenRectangle, Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);

        }
        #endregion


    }
}
