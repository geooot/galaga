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

        const int FIRE_TIMEOUT = 10;
        public const int GAME_WIDTH = 600;
        public const int GAME_HEIGHT = 600;

        SpriteBatch spriteBatch;
        Texture2D tempTexture;
        public static SpriteFont gameFont1;
        List<Sprite> sprites;

        List<Projectile> projectiles;

        KeyboardState oldKb = Keyboard.GetState();

        Character mainCharacter;
        Menu menu;
        Score score;

        int fireTimeOut = FIRE_TIMEOUT;
        int fireTimer = 0;
        int gameTimer = 0;

        Enemy tester;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;

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

            score = new Score();
            reset();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            this.Content.Load<Texture2D>("scrolling_space");
            Sprite back = new Background(this, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            sprites.Add(back);
            
            gameFont1 = Content.Load<SpriteFont>("GameFont1");
            menu = new Menu();
            

            // TODO: use this.Content to load your game content here
            tempTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            tempTexture.SetData(new Color[] { Color.White });
            Enemy.load(this, GAME_WIDTH, GAME_HEIGHT);
            tester = new Enemy(150, 150);
        }


        public void reset()
        {
            sprites = new List<Sprite>();
            mainCharacter = new Character(tempTexture, new Rectangle(268, 468, 64, 64), 0, graphics.PreferredBackBufferWidth - 64, CHARACTER_SPEED);

            gameStarted = false;
            score.incrementScore(score.getScore() * -1);
            projectiles = new List<Projectile>();
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
            
            //update all other sprites
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

            //update main character
            mainCharacter.update(curr);
            menu.update(curr);
            score.update(curr);
            // if shooting

            if(curr.IsKeyDown(Keys.Space) && !oldKb.IsKeyDown(Keys.Space))
            {
                fireTimer = FIRE_TIMEOUT;
                fireTimeOut = FIRE_TIMEOUT;
            }

            if (curr.IsKeyDown(Keys.Space))
            {
                if (fireTimer >= fireTimeOut)
                {
                    Rectangle pPos = mainCharacter.pos;
                    pPos.Width = 6;
                    pPos.Height = 20;
                    pPos.X += 29;
                    pPos.Y -= pPos.Width;

                    projectiles.Add(new Projectile(tempTexture, pPos, 0, -5));
                    fireTimeOut += 5;
                    fireTimer = 0;
                }

                fireTimer++;
            }

            //update all projectiles
            List<Projectile> toBeDeleted = new List<Projectile>();
            foreach(Projectile p in projectiles)
            {
                if (p.isOutOfBounds())
                    toBeDeleted.Add(p);
                p.update(curr);
            }

            foreach(Projectile p in toBeDeleted)
            {
                projectiles.Remove(p);
            }


            oldKb = curr;

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

            // draw other sprites
            foreach (Sprite spr in sprites)
            {
                spr.draw(spriteBatch);
            }

            //draw main character
            mainCharacter.draw(spriteBatch);
            menu.draw(spriteBatch);
            
            //draw projectiles
            foreach (Projectile p in projectiles)
            {
                p.draw(spriteBatch);
            }

            tester.draw(spriteBatch);
            score.draw(spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
