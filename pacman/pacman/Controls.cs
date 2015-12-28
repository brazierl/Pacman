using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pacman
{
    class Controls
    {
        public const Keys UP = Keys.Up;
        public const Keys DOWN = Keys.Down;
        public const Keys LEFT = Keys.Left;
        public const Keys RIGHT = Keys.Right;

        public static bool CheckActionUp(){
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(UP);
        }
        public static bool CheckActionDown()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(DOWN);
        }
        public static bool CheckActionLeft()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(LEFT);
        }
        public static bool CheckActionRight()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.IsKeyDown(RIGHT);
        }
        public static bool CheckAction()
        {
            KeyboardState keyboard = Keyboard.GetState();
            return keyboard.GetPressedKeys().Length>0;
        }
    }
}
