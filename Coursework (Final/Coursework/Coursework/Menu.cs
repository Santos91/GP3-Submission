using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Coursework
{
    class Menu
    {

        //Variables
        private List<string> MenuItems;
        private int iterator;
        public string InfoText { get; set; }
        public string Title { get; set; }

        // Public interator to store currently selected items
        public int Iterator
        {
            get
            {
                return iterator;
            }
            set
            {
                iterator = value;
                if (iterator > MenuItems.Count - 1) iterator = MenuItems.Count - 1;
                if (iterator < 0) iterator = 0;
            }
        }

        //Main Constructor which sets up what text will be written on screen
        public Menu()
        {
            Title = "GP3 Coursework";
            MenuItems = new List<string>();
            MenuItems.Add("Play");
            MenuItems.Add("Controls");
            MenuItems.Add("Exit");
            Iterator = 0;
            InfoText = string.Empty;
        }

        //Counts the number of menu items which i previous stated
        public int GetNumberOfOptions()
        {
            return MenuItems.Count;
        }
        // returns the menu items
        public string GetItem(int index)
        {
            return MenuItems[index];
        }

        //Draws the main menu and colours the text white and when it is highlighted then the text turns red
        public void DrawMenu(SpriteBatch batch, int screenWidth, SpriteFont arial)
        {
            batch.DrawString(arial, Title, new Vector2(screenWidth / 2 - arial.MeasureString(Title).X / 2, 20), Color.White);
            int yPos = 200;
            for (int i = 0; i < GetNumberOfOptions(); i++)
            {
                Color colour = Color.White;
                if (i == Iterator)
                {
                    colour = Color.Red;
                }
                batch.DrawString(arial, GetItem(i), new Vector2(screenWidth / 2 - arial.MeasureString(GetItem(i)).X / 2, yPos), colour);
                yPos += 30;
            }
        }

        //Draws the end screen which tells the user to "Press Enter to Continue"
        public void DrawEndScreen(SpriteBatch batch, int screenWidth, SpriteFont arial)
        {
            batch.DrawString(arial, InfoText, new Vector2(screenWidth / 2 - arial.MeasureString(InfoText).X / 2, 300), Color.White);
            string prompt = "Press Enter to Continue";
            batch.DrawString(arial, prompt, new Vector2(screenWidth / 2 - arial.MeasureString(prompt).X / 2, 400), Color.White);
        }
    }
}
