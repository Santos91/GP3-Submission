using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Coursework
{
    struct Daleks
    {
        //creates a position for the dalek
        public Vector3 position;
        //creates a direction for the dalek
        public Vector3 direction;
        //creates a speed for the dalek to move at
        public float speed;
        //creates a boolean to check if the laser is active or not
        public bool isActive;

        public void Update(float delta)
        {
            //sets the position to be positive or equal to the direction multiplied by 
            //the speed which is then multiplied by the speed adjust which is set in the GameConstant class.
            position += direction * speed * GameConstants.DalekSpeedAdjustment * delta;
            
            //If the dalek goes outwidth the playfield then invert direction so it stays within the boundaries
            if (position.X > GameConstants.PlayfieldSizeX + 80)
                direction = -direction;
            if (position.X < -GameConstants.PlayfieldSizeX + 90)
                direction = -direction;
            if (position.Z > GameConstants.PlayfieldSizeZ)
                direction = -direction;
            if (position.Z < -GameConstants.PlayfieldSizeZ)
                direction = -direction;
        }
    }
}
