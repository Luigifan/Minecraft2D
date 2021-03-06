﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RockSolidEngine.Graphics;
using RockSolidEngine.Input;
using RockSolidEngine.Native;
using RockSolidEngine.Screens;
using System;

namespace RockSolidEngine
{
    public static class Constants
    {
        /// <summary>
        /// The default pixel size of the tiles.
        /// </summary>
        public const int TileSize = 32;

        /// <summary>
        /// The default texture size on the sheet.
        /// </summary>
        public const int SpriteSize = 16;

        public const int SpriteScale = TileSize / SpriteSize;

        /// <summary>
        /// The scale of the game.
        /// </summary>
        public const float Scale = 1f;

        /// <summary>
        /// The cursor size.
        /// </summary>
        public const int CursorSize = 8;
    }

    public class Minecraft2D : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        #region My shit
        public static Graphics.Graphics graphics;
        public static InputHelper InputHelper;

        /// <summary>
        /// If true, the game will scale its default resolution to match the current vs. updating screen bounds.
        /// </summary>
        public static bool ScaleGame { get; set; } = true;
        #endregion

        public Minecraft2D()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
			this.Window.AllowUserResizing = true;
            Window.Title = "MonoGame Test Application";

            this.Window.ClientSizeChanged += (sender, e) =>
            {
                Console.WriteLine("Resized: {0}x{1}", _graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
            };
        }

        //This is called first
        protected override void Initialize()
        {
            base.Initialize();

            Console.WriteLine($"OS: {OperatingSystemDetermination.GetUnixName()}");

            Console.WriteLine("--Graphics Information--");
            Console.WriteLine($"Backbuffer: {_graphics.PreferredBackBufferWidth} x {_graphics.PreferredBackBufferHeight}");
        }

        //This is called second
        protected override void LoadContent()
        {
            Console.WriteLine("Load Content");

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics = new Graphics.Graphics(this, _spriteBatch, this.Content, _graphics);
            graphics.LoadContent();

            InputHelper = new InputHelper(this); //this will not copy. my C++ instincts are kicking in ;)
#if DEBUG
            graphics.DebugModeStuff();
#endif
        }

        protected override void UnloadContent() => graphics.UnloadContent();

        protected override void Update(GameTime gameTime)
        {
            //For now
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
            InputHelper.Update();
            graphics.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            graphics.Draw();
        }
    }
}
