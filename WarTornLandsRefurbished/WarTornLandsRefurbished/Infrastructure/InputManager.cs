﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WarTornLands.Infrastructure.Systems.InputSystem;

namespace WarTornLands.Infrastructure
{
    public enum InputMode
    {
        KEYBOARD = 0,
        GAMEPAD = 1
    }

    public class InputManager : GameComponent
    {
        private static InputManager _input;

        #region Default controls

        // Keyboard
        private readonly static Keys KeyboardDefault_Hit = Keys.O;
        private readonly static Keys KeyboardDefault_Jump = Keys.Space;
        private readonly static Keys KeyboardDefault_Interact = Keys.T;
        private readonly static Keys KeyboardDefault_UsePotion = Keys.P;
        private readonly static Keys[] KeyboardDefault_Move = { Keys.W, Keys.A, Keys.S, Keys.D };

        // GamePad

        private readonly static float GamePadTStickThreshold = .1f;

        #endregion

        // Keys /////////
        public Key ExecuteHit { get; private set; }
        public Key Jump { get; private set; }
        public Key Interact { get; private set; }
        public Key UsePotion { get; private set; }

        public DirectionInput Move { get; private set; }
        /////////////////

        private readonly static InputMode _defaultMode = InputMode.KEYBOARD;
        private readonly InputMode _mode = _defaultMode;
        private readonly List<InputKey> _inputList = new List<InputKey>();
        private KeyboardState _oldKeys = Keyboard.GetState();

        private InputManager(Game game)
            : base(game)
        {
            ExecuteHit = new Key();
            ExecuteHit.SetActivator(KeyboardDefault_Hit);
            Jump = new Key();
            Jump.SetActivator(KeyboardDefault_Jump);
            Interact = new Key();
            Interact.SetActivator(KeyboardDefault_Interact);
            UsePotion = new Key();
            UsePotion.SetActivator(KeyboardDefault_UsePotion);

            Move = new DirectionInput();
            Move.SetActivator(KeyboardDefault_Move);


            _inputList.Add(ExecuteHit);
            _inputList.Add(Jump);
            _inputList.Add(Interact);
            _inputList.Add(UsePotion);
            _inputList.Add(Move);

            foreach (InputKey ik in _inputList)
            {
                ik.SetMode(_defaultMode);
            }
        }

        public static InputManager GetInstance(Game game)
        {
            if (_input == null)
                _input = new InputManager(game);

            return _input;
        }

        public override void Update(GameTime gt)
        {
            foreach (InputKey ik in _inputList)
            {
                ik.Update(gt, _oldKeys);
            }

            _oldKeys = Keyboard.GetState();
        }

        public Type GetService()
        {
            return null;
        }

        public bool SetMapping(Microsoft.Xna.Framework.Input.Keys key, InputKey inputKey)
        {
            throw new System.NotImplementedException();
        }
    }
}
