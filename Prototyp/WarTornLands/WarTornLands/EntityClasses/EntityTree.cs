﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.EntityClasses
{
    class EntityTree : Entity
    {
        public EntityTree(Game game, Vector2 position, Texture2D texture)
            : base(game, position, texture)
        {
            _health = 300;
            _canBeAttacked = true;
        }

        public override void OnDie()
        {
            // TODO drop wood
            (Game as Game1)._currentLevel.AddDynamics(new Entity(Game, _position, (Game as Game1)._deadTreeTexture));

            base.OnDie();
        }
    }
}
