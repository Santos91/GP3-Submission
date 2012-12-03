using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Coursework
{
    struct Laser
    {
        //creates a position for the laser
        public Vector3 position;
        //creates a direction for the laser
        public Vector3 direction;
        //creates a speed for the laser to move at
        public float speed;
        //creates a boolean to check if the laser is active or not
        public bool isActive;

        public void Update(float delta)
        {
            //sets the position to be positive or equal to the direction multiplied by 
            //the speed which is then multiplied by the speed adjust which is set in the GameConstant class.
            position += direction * speed * GameConstants.LaserSpeedAdjustment * delta;
            //Checks if the position of x and z are more than certain numbers which will then change the 
            // is active boolean to false if they are. This stops the laser keep going into infinite space
            if (position.X > GameConstants.PlayfieldSizeX + 80 ||
                position.X < -GameConstants.PlayfieldSizeX + 90 ||
                position.Z > GameConstants.PlayfieldSizeZ ||
                position.Z < -GameConstants.PlayfieldSizeZ)
                isActive = false;
        }
    }
}
