using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Coursework
{
    class Player : Microsoft.Xna.Framework.Game
    {

        public bool ready = false;
        public Vector3 position = new Vector3(0,3,0);
        public float modelRotation = 0.0f;
        public Model playerModel;
        GraphicsDeviceManager graphics;
        Vector3 modelVelocity = Vector3.Zero;

        //private KeyboardState lastState;

        public Player(string modeltext, ContentManager Content)
        {
            graphics = new GraphicsDeviceManager(this);
            playerModel = Content.Load<Model>(modeltext);
        }

        public void Update()
        {
            // Add velocity to the current position.
            position += modelVelocity;

            // Bleed off velocity over time.
            modelVelocity *= 0.95f;
        }

        public void PlayerControls(Laser[] laserList)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            // Rotate the model using the left thumbstick, and scale it down
            modelRotation -= currentState.ThumbSticks.Left.X * 0.10f;

            // Create some velocity if the right trigger is down.
            Vector3 modelVelocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, 
            // using rotation.
            modelVelocityAdd.X = -(float)Math.Sin(modelRotation);
            modelVelocityAdd.Z = -(float)Math.Cos(modelRotation);

            if (keyboardState.IsKeyDown(Keys.W))
            {
                modelVelocityAdd *= 0.05f;
            }
            else
                // Now scale our direction by how hard the trigger is down.
                modelVelocityAdd *= currentState.Triggers.Right;

            // Finally, add this vector to our velocity.
            modelVelocity += modelVelocityAdd;

           
            if (keyboardState.IsKeyDown(Keys.D))
            {
                modelRotation += 0.050f;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                modelRotation -= 0.050f;
            }

            if (keyboardState.IsKeyDown(Keys.Space) || currentState.Buttons.A == ButtonState.Pressed)
            {
                //add another bullet.  Find an inactive bullet slot and use it
                //if all bullets slots are used, ignore the user input
                for (int i = 0; i < GameConstants.NumLasers; i++)
                {
                    if (!laserList[i].isActive)
                    {
                        Matrix playerTransform = Matrix.CreateRotationY(modelRotation);
                        laserList[i].direction = playerTransform.Forward;
                        laserList[i].speed = GameConstants.LaserSpeedAdjustment;
                        laserList[i].position = position + laserList[i].direction;
                        laserList[i].isActive = true;
                        //firingSound.Play();
                        break; //exit the loop     
                    }
                }
            }
        }

        public void Draw(Camera camera, float aspectRatio, Matrix[] transforms)
        {
            // Copy any parent transforms.

            playerModel.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in playerModel.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation) * Matrix.CreateTranslation(position);
                    effect.View = Matrix.CreateLookAt(camera.camPosition, camera.camLookat, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 50000.0f);
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}
