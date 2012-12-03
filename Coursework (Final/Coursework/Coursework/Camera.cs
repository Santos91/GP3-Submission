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
    public class Camera
    {
        //--------------------------------------------------------------------------------------
        // Added for the creation of a camera
        //--------------------------------------------------------------------------------------

        public Matrix camRotationMatrixY; //Rotation Matrix for camera to reflect movement around Y Axis
        public Matrix camRotationMatrixX;
        public Vector3 camPosition; //Position of Camera in world
        public Vector3 camLookat; //Where the camera is looking or pointing at
        public Vector3 camTransformX;
        public Vector3 camTransformY;//Used for repositioning the camer after it has been rotated
        public float camRotationSpeed; //Defines the amount of rotation
        public float camYaw; //Cumulative rotation on Y
        public float camPitch;

        public bool firstperson = false;
        public bool thirdperson = true;

        public Matrix projectionMatrix;
        public Matrix worldMatrix;
        public Matrix viewMatrix; //Cameras view


        public void InitializeCamera(float aspectRatio)
        {

            camPosition = new Vector3(0.0f, 5.0f, 10.0f);
            camRotationSpeed = 100.0f;
            viewMatrix = Matrix.CreateLookAt(camPosition, Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), aspectRatio, 1.0f, 10000.0f);
            worldMatrix = Matrix.Identity;
        }

        //----------------------------------------------------------------------------
        // Updates camera view
        //----------------------------------------------------------------------------
        public void camUpdate(Vector3 Position)
        {
            camRotationMatrixY = Matrix.CreateRotationY(camYaw);
            camRotationMatrixX = Matrix.CreateRotationX(camPitch);
            camTransformX = Vector3.Transform(Vector3.Forward, camRotationMatrixX);
            camTransformY = Vector3.Transform(Vector3.Forward, camRotationMatrixY);
            camLookat = camPosition + camTransformX + camTransformY;

            ChangeCamera();
            CameraModes(Position);
            viewMatrix = Matrix.CreateLookAt(camPosition, camLookat, Vector3.Up);
        }

        // checks to see if the specific key has been pressed which then changes the boolean linked to that key press.
        public void ChangeCamera()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.F) || currentState.Buttons.X == ButtonState.Pressed)
            {
                firstperson = true;
                thirdperson = false;
            }
            if (keyboardState.IsKeyDown(Keys.T) || currentState.Buttons.B == ButtonState.Pressed)
            {
                thirdperson = true;
                firstperson = false;
            }

        }

        //sets what view the camera has depending on what boolean is set to true
        public void CameraModes(Vector3 Position)
        {
            if (thirdperson == true)
            {
                camPosition = new Vector3(Position.X + 10, Position.Y + 4, Position.Z + 2);
                camLookat = Position + camTransformY;
            }
            else if (firstperson == true)
            {
                camPosition = new Vector3(Position.X + 5, Position.Y + 2, Position.Z);
                camLookat = new Vector3(Position.X + 800, Position.Y, Position.Z);
            }
        }
    }
}
