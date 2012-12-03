using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coursework
{
    static class GameConstants
    {
        //Player
        public const int lives = 5;
        //camera constants
        public const float PlayfieldSizeX = 200f;
        public const float PlayfieldSizeZ = 200f;
        //Dalek constants
        public const int NumDaleks = 5;
        public const float DalekMinSpeed = 0f;
        public const float DalekMaxSpeed = 30f;
        public const float DalekSpeedAdjustment = 2.0f;
        public const float DalekScalar = 1.0f;
        //collision constants
        public const float DalekBoundingSphereScale = 0.25f;  //50% size
        public const float PlayerBoundingSphereScale = 0.5f;  //50% size
        public const float LaserBoundingSphereScale = 0.85f;  //50% size
        //bullet constants
        public const int NumLasers = 30;
        public const float LaserSpeedAdjustment = 5.0f;
        public const float LaserScalar = 3.0f;

    }
}
