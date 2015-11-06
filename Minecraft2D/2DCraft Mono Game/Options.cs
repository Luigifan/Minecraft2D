﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Minecraft2D.Options
{
    public class Options
    {
        public bool UseController { get; set; }
        public bool ShowDebugInformation { get; set; }
        public bool Fullscreen { get; set; }
        public bool Vsync { get; set; }
        public System.Windows.Forms.FormWindowState WindowState { get; set; }
        public Point WindowLocation { get; set; }
        public Size WindowSize { get; set; }

        public Keys JumpKey { get; set; }
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }

        public string Username { get; set; }

        public Options()
        {
            UseController = false;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;

            JumpKey = Keys.Space;
            MoveLeft = Keys.A;
            MoveRight = Keys.D;
            MoveUp = Keys.W;
            MoveDown = Keys.S;
            Vsync = true;
            Fullscreen = false;

            Username = "Player" + MainGame.RandomGenerator.Next(0, 9000);
        }
    }
}
