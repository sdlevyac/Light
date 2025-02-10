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
        private int height = 250;
        private int width = 300;
        private Random rnd;


        Vector2 offset;
        List<Vector2> rayDirs = new List<Vector2>
        {
            new Vector2 (0, 1),
            new Vector2 (0, -1),
            new Vector2 (1, 0),
            new Vector2 (-1, 0),
            new Vector2 (1, 1),
            new Vector2 (1, -1),
            new Vector2 (-1, 1),
            new Vector2 (-1, -1),
            new Vector2 (0.1f, 1),
            new Vector2 (1, 0.1f),
            new Vector2 (-0.1f, 1),
            new Vector2 (-1, 0.1f),
            new Vector2 (0.1f, -1),
            new Vector2 (1, -0.1f),
            new Vector2 (-0.1f, -1),
            new Vector2 (-1, -0.1f),
            new Vector2 (0.2f, 1),
            new Vector2 (1, 0.2f),
            new Vector2 (-0.2f, 1),
            new Vector2 (-1, 0.2f),
            new Vector2 (0.2f, -1),
            new Vector2 (1, -0.2f),
            new Vector2 (-0.2f, -1),
            new Vector2 (-1, -0.2f),
            new Vector2 (0.3f, 1),
            new Vector2 (1, 0.3f),
            new Vector2 (-0.3f, 1),
            new Vector2 (-1, 0.3f),
            new Vector2 (0.3f, -1),
            new Vector2 (1, -0.3f),
            new Vector2 (-0.3f, -1),
            new Vector2 (-1, -0.3f),
            new Vector2 (0.4f, 1),
            new Vector2 (1, 0.4f),
            new Vector2 (-0.4f, 1),
            new Vector2 (-1, 0.4f),
            new Vector2 (0.4f, -1),
            new Vector2 (1, -0.4f),
            new Vector2 (-0.4f, -1),
            new Vector2 (-1, -0.4f),
            new Vector2 (0.5f, 1),
            new Vector2 (1, 0.5f),
            new Vector2 (-0.5f, 1),
            new Vector2 (-1, 0.5f),
            new Vector2 (0.5f, -1),
            new Vector2 (1, -0.5f),
            new Vector2 (-0.5f, -1),
            new Vector2 (-1, -0.5f),
            new Vector2 (0.6f, 1),
            new Vector2 (1, 0.6f),
            new Vector2 (-0.6f, 1),
            new Vector2 (-1, 0.6f),
            new Vector2 (0.6f, -1),
            new Vector2 (1, -0.6f),
            new Vector2 (-0.6f, -1),
            new Vector2 (-1, -0.6f),
            new Vector2 (0.7f, 1),
            new Vector2 (1, 0.7f),
            new Vector2 (-0.7f, 1),
            new Vector2 (-1, 0.7f),
            new Vector2 (0.7f, -1),
            new Vector2 (1, -0.7f),
            new Vector2 (-0.7f, -1),
            new Vector2 (-1, -0.7f),
            new Vector2 (0.8f, 1),
            new Vector2 (1, 0.8f),
            new Vector2 (-0.8f, 1),
            new Vector2 (-1, 0.8f),
            new Vector2 (0.8f, -1),
            new Vector2 (1, -0.8f),
            new Vector2 (-0.8f, -1),
            new Vector2 (-1, -0.8f),
            new Vector2 (0.9f, 1),
            new Vector2 (1, 0.9f),
            new Vector2 (-0.9f, 1),
            new Vector2 (-1, 0.9f),
            new Vector2 (0.9f, -1),
            new Vector2 (1, -0.9f),
            new Vector2 (-0.9f, -1),
            new Vector2 (-1, -0.9f),
        };

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
            pixelWidth = 4;
            offset = new Vector2(pixelWidth / 2, pixelWidth / 2);
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

            TargetElapsedTime = TimeSpan.FromSeconds(1d / 30d);// / 15d);
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

            grid_light = Processing.diffuse(grid_light, 3);

            //Debug.WriteLine($"{mouseStateCurrent.X},{mouseStateCurrent.Y}");
            // TODO: Add your update logic here

            mouseStatePrevious = mouseStateCurrent;
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            int[,] grid_light_diff = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int neighbours = 0;
                    int neighbourVals = 0;
                    for (int di = -2; di <= 2; di++)
                    {
                        for (int dj = -2; dj <= 2; dj++)
                        {
                            int ni = i + di, nj = j + dj;
                            if (ni >= 0 && ni < width &&
                                nj >= 0 && nj < height)
                            {
                                neighbours++;
                                neighbourVals += grid_light[ni, nj];
                            }
                        }
                    }
                    grid_light_diff[i, j] = neighbourVals / neighbours;
                }
            }

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
                        _spriteBatch.Draw(rect, new Rectangle(i * pixelWidth, j * pixelWidth, pixelWidth, pixelWidth), new Color(255 - grid_noise[i,j], 0, 0));
                    }
                    else
                    {
                        int value = grid_light_diff[i, j];
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
                Primitives2D.DrawCircle(_spriteBatch, new Vector2(mouseStateCurrent.X, mouseStateCurrent.Y),25,25,Color.White);
            }

            
            Vector2 startPos = new Vector2(mouseStateCurrent.X / pixelWidth, mouseStateCurrent.Y / pixelWidth);
            foreach (Vector2 dir in rayDirs)
            {
                Vector2 pos = Processing.ray(startPos, dir, width, height, grid_space, 50);
                _spriteBatch.DrawLine(startPos*pixelWidth + offset, pos*pixelWidth + offset, Color.Green);
                Primitives2D.DrawCircle(_spriteBatch, pos * pixelWidth + offset, 5, 10, Color.Green);
            }
            
            //_spriteBatch.Draw(rect, new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, pixelWidth*2, pixelWidth*2), new Color(0, 0, 255));
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
