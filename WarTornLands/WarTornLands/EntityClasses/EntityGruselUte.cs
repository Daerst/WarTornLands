﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WarTornLands.EntityClasses
{
    class EntityGruselUte : Entity
    {
        public EntityGruselUte(Game game, Vector2 position, Texture2D texture) : base(game, position, texture)
        {
            _health = 100;
            _canbeattacked = true;
        }

        public override void Update(GameTime gameTime)
        {
            





            base.Update(gameTime);
        }

        public override void OnDie()
        {
            // TODO drop

            base.OnDie();
        }
    }
}
