﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Entities;

namespace WarTornLandsRefurbished.Entities.Modules.Interact
{
    interface IInteractModule
    {
        public struct InteractInformation
        {
        }

        public void Interact(Entity invoker, Entity target, InteractInformation information);
    }
}
