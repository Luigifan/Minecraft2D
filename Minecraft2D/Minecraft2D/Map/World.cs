﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Minecraft2D.Map
{
    public class World
    {
        private Tile[,] tiles = new Tile[256, 100];
        public float[,] Lightmap { get; set; }

        private Random ran = new Random((int)DateTime.Now.Millisecond * 69);
        private PresetBlocks presets = new PresetBlocks();
        private long worldTime = 6000;
        public Color BlockTint = Color.White; //Full daytime
        public Color SkyColor = Color.CornflowerBlue;

        public Vector2 WorldSize { get; set; }
        public long WorldTime { get { return worldTime; } set{ worldTime = value; } }

        public World()
        {
            Lightmap = new float[256, 100];
            WorldSize = new Vector2(tiles.GetLength(1) * 32, tiles.GetLength(0) * 32);
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    if(y > 32)
                    {
                        if(y == 33)
                        {
                            Tile t = PresetBlocks.Grass.AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                        else if(y <= 37)
                        {
                            Tile t = PresetBlocks.Dirt.AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                        else if(y == 255)
                        {
                            //TODO: bedrock
                            Tile t = PresetBlocks.Dirt.AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                        else
                        {
                            Tile t = PresetBlocks.Stone.AsTile();
                            t.Position = new Vector2(x * 32, y * 32);
                            tiles[y, x] = t;
                            t = null;
                        }
                    }
                    else
                    {
                        Tile t = PresetBlocks.Air.AsTile();
                        t.Position = new Vector2(x * 32, y * 32);
                        tiles[y, x] = t;
                        t = null;
                    }
                }
            }
            if (File.Exists("World1.wld"))
                LoadWorld("World1.wld");
        }

        private void GenerateLightmap()
        {
            for (int x = 0; x < tiles.GetLength(1); x++)
            {
                for (int y = 0; y < tiles.GetLength(0); y++)
                {
                    if (tiles[y, x].Type == TileType.Air)
                        Lightmap[y, x] = 1f;
                }
            }
        }

        public void SaveWorld(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                for (int x = 0; x < tiles.GetLength(1); x++)
                {
                    for (int y = 0; y < tiles.GetLength(0); y++)
                    {
                        string format = string.Format("{0}:{1}:{2}", (int)tiles[y, x].Type, tiles[y, x].Position.X, tiles[y, x].Position.Y);
                        sw.WriteLine(format);
                    }
                }
                sw.Flush();
            }
        }

        public void LoadWorld(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                string input = "";
                while (!sr.EndOfStream)
                {
                    input = sr.ReadLine();
                    if (input.Trim() != String.Empty)
                    {
                        string[] split = input.Split(new char[] { ':' }, 3);
                        int x, y, typeEnumIndex;
                        x = (int)Math.Floor((double)Int32.Parse(split[1]) / 32);
                        y = (int)Math.Floor((double)Int32.Parse(split[2]) / 32);
                        typeEnumIndex = Int32.Parse(split[0]);
                        Tile t = PresetBlocks.Air.AsTile();
                        t.Position = new Vector2(x * 32, y * 32);
                        switch ((TileType)typeEnumIndex)
                        {
                            case TileType.Air:
                                t = PresetBlocks.Air.AsTile();
                                t.Position = new Vector2(x * 32, y * 32);
                                break;
                            case TileType.Stone:
                                t = PresetBlocks.Stone.AsTile();
                                t.Position = new Vector2(x * 32, y * 32);
                                break;
                            case TileType.Grass:
                                t = PresetBlocks.Grass.AsTile();
                                t.Position = new Vector2(x * 32, y * 32);
                                break;
                            case TileType.Dirt:
                                t = PresetBlocks.Dirt.AsTile();
                                t.Position = new Vector2(x * 32, y * 32);
                                break;
                        }
                        tiles[y, x] = t;
                    }
                }
            }
        }


        public void Update(GameTime gameTime)
        {
            WorldTime++;
            if (WorldTime > 24000)
                WorldTime = 0;

            if(WorldTime > 6000 && WorldTime < 12000)
            {
                BlockTint = Color.White;
                SkyColor = Color.CornflowerBlue;
            }
            else if(WorldTime > 12000 && WorldTime < 18000)
            {
                BlockTint = Color.DarkGray;
                SkyColor = Color.DimGray;
            }
            else if(WorldTime > 18000 && WorldTime < 24000)
            {
                BlockTint = Color.DarkGray;
                SkyColor = Color.Black;
            }
            else if(WorldTime > 0 && WorldTime < 6000)
            {
                BlockTint = Color.DarkOrange;
                SkyColor = Color.Orange;
            }
            
        }
        
        public Rectangle viewportRect { get; set; }

        /// <summary>
        /// Absolute x and y values are used here
        /// </summary>
        public Tile GetTile(int x, int y)
        {
            int tX = (int)Math.Floor((double)(x + 16) / 32); //add half of the cross hair so it's centered
            int tY = (int)Math.Floor((double)(y + 16) / 32);
            if (tX > tiles.GetLength(1) - 1 || tX < 0 || tY > tiles.GetLength(0) - 1 || tY < 0)
                return null;
            else
                return tiles[tY, tX];
        }

        /// <summary>
        /// Absolute x and y values are used here.
        /// Also sets the tile position.
        /// </summary>
        public void SetTile(int x, int y, Tile toReplace)
        {
            int tX = (int)Math.Floor((double)(x + 16) / 32); //add half of the cross hair so it's centered
            int tY = (int)Math.Floor((double)(y + 16) / 32);
            if (tX > tiles.GetLength(1) || tX < 0 || tY > tiles.GetLength(0) || tY < 0)
                return;
            else
            {
                tiles[tY, tX] = toReplace;
                tiles[tY, tX].Position = new Vector2(tX * 32, tY * 32);

                if (tiles[tY, tX].Type == TileType.Air)
                    Lightmap[tY, tX] = 1f;
                else
                    Lightmap[tY, tX] = 0f;
            }
        }

        public void Draw(GameTime gameTime)
        {
            MainGame.GlobalSpriteBatch.Begin(SpriteSortMode.Texture,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone, null, MainGame.GameCamera.get_transformation(MainGame.GlobalSpriteBatch.GraphicsDevice));

            List<Tile> tilesToBeRendered = new List<Tile>();

            viewportRect = new Rectangle((int)MainGame.GameCamera.Pos.X - (MainGame.GlobalGraphicsDevice.Viewport.Width / 2),
                    (int)MainGame.GameCamera.Pos.Y - (MainGame.GlobalGraphicsDevice.Viewport.Height / 2),
                    MainGame.GlobalGraphicsDevice.Viewport.Width, MainGame.GlobalGraphicsDevice.Viewport.Height);
            foreach (var block in tiles)
            {
                Rectangle objectBounds = new Rectangle((int)block.Position.X, (int)block.Position.Y, 34, 34);
                int x, y;
                x = (int)Math.Floor((double)block.Position.X / 32);
                y = (int)Math.Floor((double)block.Position.Y / 32);
                if (viewportRect.Intersects(objectBounds))
                {
                    tilesToBeRendered.Add(block);
                    if(block.Type == TileType.Air)
                        Lightmap[y, x] = 1f; //it'll either be 1f or 0f i guess
                }
            }
            
            foreach (Tile block in tilesToBeRendered)
            {
                //if (block.Type == TileType.Air)
                //    AddLight(stationaryLightMap, (int)Math.Floor(block.Position.X / 32), (int)Math.Floor(block.Position.Y / 32));
                if (block.Type != TileType.Air)
                    if (!block.IsBackground)
                    {
                        int x = (int)Math.Floor(block.Position.X / 32);
                        int y = (int)Math.Floor(block.Position.Y / 32);
                        
                        MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"),
                            new Rectangle(Convert.ToInt32(block.Position.X), Convert.ToInt32(block.Position.Y), 32, 32), //scaling to 32
                            new Rectangle(block.TextureRegion.X, block.TextureRegion.Y, 16, 16),
                            Color.White,
                            0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0f);
                    }
                    else
                    {
                        MainGame.GlobalSpriteBatch.Draw(MainGame.CustomContentManager.GetTexture("terrain"),
                            new Rectangle(Convert.ToInt32(block.Position.X), Convert.ToInt32(block.Position.Y), 32, 32), //scaling to 32
                            new Rectangle(block.TextureRegion.X, block.TextureRegion.Y, 16, 16),
                            Color.Gray,
                            0f,
                            Vector2.Zero,
                            SpriteEffects.None,
                            0f);
                    }
            }
            MainGame.GlobalSpriteBatch.End();
        }
    }
}
