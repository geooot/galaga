using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Galaga
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D projectileTexture;
        List<Sprite> sprites;
        public const int WIDTH = 600;
        public const int HEIGHT = 600;
        int gameTimer = 0;

        Enemy tester;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            reset();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Content.Load<Texture2D>("scrolling_space");
            Sprite back = new Background(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            sprites.Add(back);
            // TODO: use this.Content to load your game content here
            projectileTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            projectileTexture.SetData(new Color[] { Color.White });
            Enemy.load(this, WIDTH, HEIGHT);
            tester = new Enemy(150, 150);
        }


        public void reset()
        {
            sprites = new List<Sprite>();
            gameTimer = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState curr = Keyboard.GetState();

            // TODO: Add your update logic here
            foreach (Sprite spr in sprites) {
                spr.update(curr);
            }
            Enemy.deviation();
            gameTimer++;
            tester.update(curr);

            if (gameTimer%(60*4)==0)
            {
                Console.WriteLine("---ACTIVATE---");
                tester.dive();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (Sprite spr in sprites)
            {
                spr.draw(spriteBatch);
            }

            tester.draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
