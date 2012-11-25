﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities.Modules.Draw;
using WarTornLands.Entities.Modules.Interact;
using WarTornLands.Entities.Modules.Collide;

namespace WarTornLands.Entities.Modules.Die
{
    public class ExplodeAndLoot:BaseModule, IDieModule
    {
        private PickUpOnCollide _pick;
        private AnimatedDrawer _explosion;

        public ExplodeAndLoot(WarTornLands.PlayerClasses.Items loot)
        {
            _pick = new PickUpOnCollide(loot);
            _explosion = AnimatedDrawer.Explosion;
        }
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_owner.Health <= 0)
            {
                _owner.AddModule(_explosion);
                _owner.AddModule(_pick);
            }
        }
    }
}