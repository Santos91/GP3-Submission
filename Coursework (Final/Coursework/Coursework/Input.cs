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
    public class Input
    {
        private KeyboardState keyboardState;
        private KeyboardState lastState;

        // Get the game pad state.
        GamePadState currentState = GamePad.GetState(PlayerIndex.One);


        public Input()
        {
            keyboardState = Keyboard.GetState();
            lastState = keyboardState;
        }

        public void Update()
        {
            lastState = keyboardState;
            keyboardState = Keyboard.GetState();
        }
        //creates a boolean to move up in the menu when pressing certain keys on the keyboard.
        public bool Up
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return keyboardState.IsKeyDown(Keys.W) && lastState.IsKeyUp(Keys.W);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.W);
                }
            }
        }
        //creates a boolean to move down in the menu when pressing certain keys on the keyboard.
        public bool Down
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return keyboardState.IsKeyDown(Keys.S) && lastState.IsKeyUp(Keys.S);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.S);
                }
            }
        }

        //Creats a boolean for the user to exit the game while playing by hitting the escape key.
        public bool Escape
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Play || Game1.gamestate == Game1.GameStates.Controls)
                {
                    return keyboardState.IsKeyDown(Keys.Escape) && lastState.IsKeyUp(Keys.Escape);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Escape);
                }
            }
        }
        //creates the boolean that allows the user to choose what option they want while in the menu.
        public bool MenuSelect
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter);
            }
        }
    }
}
