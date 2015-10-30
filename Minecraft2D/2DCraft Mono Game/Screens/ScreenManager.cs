﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Minecraft2D.Screens;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minecraft2D.Screens
{
    public enum GameScreens
    {
        MAIN, SPLASH, CREDITS, GAME,
        OPTIONS
    }

    public class ScreenManager
    {
        public GameScreens CurrentScreen { get; set; }

        private Minecraft2D.Screens.MainGameScreen mainGameScreen;
        private TitleScreen titleScreen;
        private OptionsScreen options;

        public ScreenManager()
        {
            mainGameScreen = new MainGameScreen();
            titleScreen = new TitleScreen();
            options = new OptionsScreen();
            MainGame.CustomContentManager = new Graphics.ContentManager();
            CurrentScreen = GameScreens.MAIN;
            LoadContent();
        }
        public void LoadContent()
        {
            if (MainGame.GlobalContentManager != null)
            {
                MainGame.CustomContentManager.AddTexture("default", MainGame.GlobalContentManager.Load<Texture2D>("default"));
                MainGame.CustomContentManager.AddSpriteFont("main-font", MainGame.GlobalContentManager.Load<BitmapFont>("mainfont"));
                MainGame.CustomContentManager.AddTexture("terrain", MainGame.GlobalContentManager.Load<Texture2D>("terrain"));
                MainGame.CustomContentManager.AddTexture("crosshair", MainGame.GlobalContentManager.Load<Texture2D>("crosshair"));
                MainGame.CustomContentManager.AddTexture("smoothlight", MainGame.GlobalContentManager.Load<Texture2D>("smoothlight"));
                MainGame.CustomContentManager.AddTexture("minecraft-logo", MainGame.GlobalContentManager.Load<Texture2D>("logo"));
                MainGame.CustomContentManager.AddTexture("mojang-logo", MainGame.GlobalContentManager.Load<Texture2D>("mojang"));
                MainGame.CustomContentManager.AddTexture("widgets", MainGame.GlobalContentManager.Load<Texture2D>("widgets"));

                MainGame.CustomContentManager.AddSoundEffect("click", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/click.ogg"));

                MainGame.CustomContentManager.AddSoundEffect("stone1", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/stone1.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("stone2", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/stone2.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("stone3", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/stone3.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("stone4", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/stone4.ogg"));

                MainGame.CustomContentManager.AddSoundEffect("gravel1", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/gravel1.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("gravel2", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/gravel2.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("gravel3", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/gravel3.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("gravel4", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/gravel4.ogg"));

                MainGame.CustomContentManager.AddSoundEffect("grass1", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/grass1.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("grass2", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/grass2.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("grass3", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/grass3.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("grass4", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/grass4.ogg"));

                MainGame.CustomContentManager.AddSoundEffect("wood1", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/wood1.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("wood2", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/wood2.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("wood3", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/wood3.ogg"));
                MainGame.CustomContentManager.AddSoundEffect("wood4", MainGame.GlobalContentManager.Load<SoundEffect>("sounds/step/wood4.ogg"));
            }
        }

        public void RecalculateMinMax()
        {
            if(CurrentScreen == GameScreens.GAME)
                if (mainGameScreen != null)
                    mainGameScreen.RecalculateMinMax();
        }

        public void PushScreen(GameScreens screen)
        {
            CurrentScreen = screen;
        }

        public void Dispose()
        {
            mainGameScreen = null;
        }

        public void Update(GameTime gameTime)
        {
            switch(CurrentScreen)
            {
                case (GameScreens.GAME):
                    mainGameScreen.Update(gameTime);
                    break;
                case (GameScreens.MAIN):
                    titleScreen.Update(gameTime);
                    break;
                case (GameScreens.OPTIONS):
                    options.Update(gameTime);
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            switch (CurrentScreen)
            {
                case GameScreens.GAME:
                    mainGameScreen.Draw(gameTime);
                    break;
                case (GameScreens.MAIN):
                    titleScreen.Draw(gameTime);
                    break;
                case (GameScreens.OPTIONS):
                    options.Draw(gameTime);
                    break;
            }
        }
    }
}
