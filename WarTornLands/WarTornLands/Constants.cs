﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WarTornLands
{
    static class Constants
    {
        public static int TileSize = 64;
        
        // Player
        public const int Radius = 32;
        public const float Speed = .125f;
        public const int HitDuration = 400;
        public const float WeaponRange = 50;
        public const float WeaponStartAngle = 1.3f;
        public const float WeaponGoalAngle = (float)(Math.PI * 2 - WeaponStartAngle);
        public const float TalkDistance = 48.0f;
    }
}
