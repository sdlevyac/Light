using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Light
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int[,] grid_space;
        private int[,] grid_light;
        private int[,] grid_noise;
        private int height = 100;
        private int width = 160;
        private Random rnd;

        private static Texture2D rect;
        private int pixelWidth;// = 16;

        KeyboardState keyStateCurrent;// = Keyboard.GetState();
        MouseState mouseStateCurrent;// = Mouse.GetState();
        MouseState mouseStatePrevious;// = Mouse.GetState();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            grid_space = new int[width, height];
            grid_light = new int[width, height];
            grid_noise = new int[width, height];
            rnd = new Random(1234);
            pixelWidth = 8;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                    {
                        grid_space[i, j] = 1;
                    }
                    
                    grid_noise[i, j] = rnd.Next(10,50);
                }
            }

            mouseStateCurrent = new MouseState();
            mouseStatePrevious = new MouseState();

            //TargetElapsedTime = TimeSpan.FromSeconds(1d / 10d);// / 15d);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = width * pixelWidth;
            _graphics.PreferredBackBufferHeight = height * pixelWidth;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect = new Texture2D(GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouseStateCurrent = Mouse.GetState();

            //mouseStateCurrent.X / pixelWidth == i &&
            //mouseStateCurrent.Y / pixelWidth == j
            if (Utils.MouseOnScreen(mouseStateCurrent, width * pixelWidth, height * pixelWidth))
            {
                grid_light[mouseStateCurrent.X / pixelWidth, mouseStateCurrent.Y / pixelWidth] = 255;
                if (mouseStateCurrent.LeftButton == ButtonState.Pressed)
                {
                    grid_space[mouseStateCurrent.X / pixelWidth, mouseStateCurrent.Y / pixelWidth] = 1;
                }
                else if (mouseStateCurrent.RightButton == ButtonState.Pressed)
                {
                    grid_space[mouseStateCurrent.X / pixelWidth, mouseStateCurrent.Y / pixelWidth] = 0;
                }



            }

            //Debug.WriteLine($"{mouseStateCurrent.X},{mouseStateCurrent.Y}");
            // TODO: Add your update logic here

            mouseStatePrevious = mouseStateCurrent;
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int wall = grid_space[i, j];
                    if (mouseStateCurrent.X / pixelWidth == i &&
                        mouseStateCurrent.Y / pixelWidth == j)
                    {
                        _spriteBatch.Draw(rect, new Rectangle(i * pixelWidth, j * pixelWidth, pixelWidth, pixelWidth), new Color(0, 0, 255));

                    }
                    else if (wall == 1)
                    {
                        _spriteBatch.Draw(rect, new Rectangle(i * pixelWidth, j * pixelWidth, pixelWidth, pixelWidth), new Color(255, 0, 0));
                    }
                    else
                    {
                        int value = grid_light[i, j];
                        int noise = grid_noise[i, j];
                        value += noise;
                        _spriteBatch.Draw(rect, new Rectangle(i * pixelWidth, j * pixelWidth, pixelWidth, pixelWidth), new Color(value, value, value));
                    }
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    grid_light[i, j] = 0;
                }
            }
            if (Utils.MouseOnScreen(mouseStateCurrent, width * pixelWidth, height * pixelWidth))
            {
                Vector2 mousePos = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                //Primitives2D.DrawLine(_spriteBatch, mousePos, new Vector2(0, 0), Color.Green);
                //Primitives2D.DrawLine(_spriteBatch, mousePos, new Vector2(0, height * pixelWidth), Color.Green);
                //Primitives2D.DrawLine(_spriteBatch, mousePos, new Vector2(width * pixelWidth, 0), Color.Green);
                //Primitives2D.DrawLine(_spriteBatch, mousePos, new Vector2(width * pixelWidth, height * pixelWidth), Color.Green);
                //draw line left
                Vector2 pos = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                int count = 0;
                while (pos.X >= 0 && pos.X < width * pixelWidth && pos.Y >= 0 && pos.Y < height * pixelWidth && grid_space[(int)(pos.X / pixelWidth), (int)(pos.Y / pixelWidth)] != 1)
                {
                    count++;
                    pos.X = mousePos.X - (count * pixelWidth);
                    pos.Y = mousePos.Y - (count * pixelWidth);
                }
                Primitives2D.DrawLine(_spriteBatch, mousePos, pos, Color.Green);
                pos = new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y);
                while (pos.X >= 0 && pos.X < width * pixelWidth && pos.Y >= 0 && pos.Y < height * pixelWidth && grid_space[(int)(pos.X / pixelWidth), (int)(pos.Y / pixelWidth)] != 1)
                {
                    pos.X += pixelWidth;
                }
                Primitives2D.DrawLine(_spriteBatch, mousePos, pos, Color.Green);
            }



            //_spriteBatch.Draw(rect, new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, pixelWidth*2, pixelWidth*2), new Color(0, 0, 255));
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
