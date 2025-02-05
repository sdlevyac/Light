using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace Light
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private int[,] grid_space;
        private int[,] grid_light;
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
                    grid_light[i, j] = rnd.Next(10,50);
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
            try
            {
                grid_light[mouseStateCurrent.X / pixelWidth, mouseStateCurrent.Y / pixelWidth] = 255;
                

            }
            catch (Exception)
            {
                Debug.WriteLine("out of range!");
            }

            Debug.WriteLine($"{mouseStateCurrent.X},{mouseStateCurrent.Y}");
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
                    grid_light[i, j] -= 1;
                    if (grid_light[i, j] < 0)
                    {
                        grid_light[i, j] = 0;
                    }
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
                        _spriteBatch.Draw(rect, new Rectangle(i * pixelWidth, j * pixelWidth, pixelWidth, pixelWidth), new Color(value, value, value));
                    }
                }
            }
            //_spriteBatch.Draw(rect, new Rectangle(mouseStateCurrent.X, mouseStateCurrent.Y, pixelWidth*2, pixelWidth*2), new Color(0, 0, 255));
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
