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
        const int CHARACTER_SPEED = 5;
        public static bool gameStarted;
        

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D tempTexture;
        public static SpriteFont gameFont1;
        List<Sprite> sprites;
        List<Projectile> projectiles;


        Character mainCharacter;
        Menu menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
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
            tempTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            tempTexture.SetData(new Color[] { Color.White });

            reset();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Content.Load<Texture2D>("scrolling_space");
            Sprite back = new Background(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            sprites.Add(back);
            gameFont1 = Content.Load<SpriteFont>("GameFont1");
            menu = new Menu();
        }


        public void reset()
        {
            sprites = new List<Sprite>();
            mainCharacter = new Character(tempTexture, new Rectangle(268, 468, 64, 64), 0, graphics.PreferredBackBufferWidth - 64, CHARACTER_SPEED);
            gameStarted = false;
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

            mainCharacter.update(curr);
            menu.update(curr);
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

            mainCharacter.draw(spriteBatch);
            menu.draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
