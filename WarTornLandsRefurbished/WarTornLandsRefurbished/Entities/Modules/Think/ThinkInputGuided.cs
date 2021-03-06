﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarTornLands.Counter;
using WarTornLands;
using Microsoft.Xna.Framework;
using WarTornLands.Entities;
using WarTornLands.Entities.Modules.Think.Parts;
using WarTornLands.Infrastructure;

namespace WarTornLands.Entities.Modules.Think
{
    class ThinkInputGuided : BaseModule, IThinkModule
    {
        public float Speed { get; set; }
        public float Radius { get; set; }

        private CounterManager _cm;
        private InputManager _input;

        // Parts
        private JumpAbility _jump;
        private SwingHitAbility _swing;
        private InteractAbility _interact;
        private bool _frozen;

        public ThinkInputGuided(float speed = .125f)
        {
            Speed = speed;

            _jump = new JumpAbility();
            _swing = new SwingHitAbility(700, 1);
            _interact = new InteractAbility();

            // Subscribe to Input events
            _input = InputManager.Instance;
            _input.KUsePotion.Pressed += new EventHandler(OnUsePotion);
            _input.KHit.Pressed += new EventHandler(OnExecuteHit);
            _input.KInteract.Pressed += new EventHandler(OnInteract);
            _input.KJump.Pressed += new EventHandler(OnJump);
        }

        public void Update(GameTime gameTime)
        {
            if (!_frozen)
            {
                Vector2 oldPos = _owner.Position;
                Vector2 moveDirection = _input.KMove.Value;
                _owner.Position =
                    CollisionManager.Instance.TryMove(
                    _owner.Position,
                    moveDirection * Speed * gameTime.ElapsedGameTime.Milliseconds,
                    Radius,
                    _owner
                    ) + _owner.Position;

                _swing.Update(gameTime);
                CalcFacing(moveDirection);
            }
        }

        public void Freeze()
        {
            _frozen = true;
        }

        public void Thaw()
        {
            _frozen = false;
        }

        public override void SetOwner(Entity owner)
        {
            base.SetOwner(owner);
            _jump.SetOwner(owner);
            _swing.SetOwner(owner);
            _interact.SetOwner(owner);

            _cm = owner.CM;
            _cm.Bang += new EventHandler<BangEventArgs>(OnBang);

        }

        private void CalcFacing(Vector2 moveDirection)
        {
            // true = Y direction over X
            // false = X direction over Y
            if (true)
            {
                if (Math.Abs(moveDirection.X) >= Math.Abs(moveDirection.Y))
                    XFacing(moveDirection);
                else
                    YFacing(moveDirection);
            }
            else
            {
                YFacing(moveDirection);
                XFacing(moveDirection);
            }
        }

        private void XFacing(Vector2 moveDirection)
        {
            if (moveDirection.X > 0)
            {
                _owner.Face = Facing.RIGHT;
            }
            if (moveDirection.X < 0)
            {
                _owner.Face = Facing.LEFT;
            }
        }

        private void YFacing(Vector2 moveDirection)
        {
            if (moveDirection.Y > 0)
            {
                _owner.Face = Facing.DOWN;
            }
            if (moveDirection.Y < 0)
            {
                _owner.Face = Facing.UP;
            }
        }


        #region Subscribed events

        private void OnBang(object sender, BangEventArgs e)
        {

        }

        private void OnUsePotion(object sender, EventArgs e)
        {

        }

        private void OnExecuteHit(object sender, EventArgs e)
        {
            _swing.TryExecute();
        }

        private void OnInteract(object sender, EventArgs e)
        {
            _interact.TryExecute();   
        }

        private void OnJump(object sender, EventArgs e)
        {
            _jump.TryExecute();
        }

        #endregion
    }
}
