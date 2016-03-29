﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Minecraft2DRebirth.Graphics;

namespace Minecraft2DRebirth.Entity
{
    public class IAnimatedEntity : IEntity
    {
        /// <summary>
        /// The amount of frames in one animation.
        /// </summary>
        public int FrameCount { get; set; }

        /// <summary>
        /// Zero based index of the vertical index of animations.
        /// Ex: top row of sprites is FrameIndex=0
        /// </summary>
        public int YFrameIndex { get; set; }

        public float AnimationFPS { get; set; }

        public string SheetName { get; set; }

        /// <summary>
        /// The size of one sprite.
        /// </summary>
        public Vector2 SpriteSize { get; set; }

        /// <summary>
        /// The current frame being drawn in the animation.
        /// </summary>
        public int CurrentFrameIndex { get; internal set; }

        public bool Animating { get; internal set; } = true;

        private int ElapsedTime;

        public override void Draw(Graphics.Graphics graphics)
        {
            Rectangle sourceRectangle = new Rectangle
            (
                (int)SpriteSize.X * CurrentFrameIndex,
                (int)SpriteSize.Y * YFrameIndex,
                (int)SpriteSize.X,
                (int)SpriteSize.Y
            );
            Rectangle destinationRectangle = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)Math.Round(SpriteSize.X * Constants.SpriteScale),
                (int)Math.Round(SpriteSize.Y * Constants.SpriteScale)
            );
            var texture = graphics.GetTexture2DByName(SheetName);
            graphics.GetSpriteBatch().Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (Animating)
            {
                ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (ElapsedTime > AnimationFPS)
                {
                    CurrentFrameIndex++;
                    ElapsedTime = 0;
                    if (CurrentFrameIndex > FrameCount - 1)
                        CurrentFrameIndex = 0;
                }
            }
        }
    }
}