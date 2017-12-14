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
        const int MIN_DIVE_TIME = 120;
        const int MAX_DIVE_TIME = 240;
        public const int GAME_WIDTH = 600;
        public const int GAME_HEIGHT = 600;

        SpriteBatch spriteBatch;
        Texture2D tempTexture;
        public static SpriteFont gameFont1;
        List<Sprite> sprites = new List<Sprite>();
        Texture2D playerText;

        List<Projectile> projectiles;

        Random r = new Random();
        KeyboardState oldKb = Keyboard.GetState();

        Character mainCharacter;
        Menu menu;
        Score score;

        int fireTimeOut = FIRE_TIMEOUT;
        int fireTimer = 0;
        int gameTimer = 0;
        int lives = 3;
        int nextDiveTime = 0;
        List<Enemy> enemies = new List<Enemy>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;
            nextDiveTime = r.Next(MIN_DIVE_TIME, MAX_DIVE_TIME);
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
            playerText = this.Content.Load<Texture2D>("usa");
            score = new Score();

            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D bg = this.Content.Load<Texture2D>("scrolling_space");
            Sprite back = new Background(bg, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            sprites.Add(back);
            
            gameFont1 = Content.Load<SpriteFont>("GameFont1");
            menu = new Menu();
            

            // TODO: use this.Content to load your game content here
            tempTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            tempTexture.SetData(new Color[] { Color.White });
            Enemy.load(this, GAME_WIDTH, GAME_HEIGHT);
            reset();


        }


        public void reset()
        {
            
            mainCharacter = new Character(playerText, new Rectangle(275, 468, 100 , 70), 0, graphics.PreferredBackBufferWidth - 64, CHARACTER_SPEED);

            gameStarted = false;
           // score.incrementScore(score.getScore() * -1);
            projectiles = new List<Projectile>();
            gameTimer = 0;
            enemies = new List<Enemy>();
            lives = 3;

            int xPos = 0;
            int yPos = 10;
            for (int x = 0; x < 18; x++)
            {
                enemies.Add(new Enemy(xPos, yPos));
                xPos += 100;
                if (xPos == 600)
                {
                    yPos += 100;
                    xPos = 0;
                }

            }
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

            // update enemy
            List<Enemy> stateZeroes = new List<Enemy>();
            foreach(Enemy e in enemies)
            {
                e.update(curr);
                if (e.state == 1)
                    stateZeroes.Add(e);
            }

            if (gameTimer % nextDiveTime == 0 && gameStarted && stateZeroes.Count > 0)
            {
                stateZeroes[r.Next(0, stateZeroes.Count - 1)].dive();
                nextDiveTime = r.Next(MIN_DIVE_TIME, MAX_DIVE_TIME);
            }


            //update main character
            mainCharacter.update(curr);
            if (curr.IsKeyDown(Keys.Space) && !oldKb.IsKeyDown(Keys.Space) && gameStarted == false)
            {
                score.incrementScore(score.getScore() * -1);
            }
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


            //enemies and player
            for (int e = 0; e < enemies.Count; e++)
            {
                if (mainCharacter.pos.Intersects(enemies[e].rect) && enemies[e].state != 3)
                {
                    lives--;
                    enemies[e].kill();
                }
            }

            //enemy revival
            bool allDead = true;
            for (int e = 0; e < enemies.Count; e++)
            {
                if (enemies[e].state != 3)
                {
                    allDead = false;
                    break;
                }
            }
            if (allDead)
            {
                Enemy.speed++;
                for (int e = 0; e < enemies.Count; e++)
                {
                    enemies[e].revive();
                }
            }
            //player lives
            if (lives<=0)
            {
                reset();//game restarts
            }
            
            oldKb = curr;

            //collisions
            //projectiles and enemies
            for (int i = 0; i < projectiles.Count; i++)
            {
                for (int e = 0; e < enemies.Count; e++)
                {
                    if (projectiles[i].pos.Intersects(enemies[e].rect) && enemies[e].state != 3)
                    {
                        enemies[e].kill();
                        projectiles.Remove(projectiles[i]);
                        score.incrementScore(10);
                        i--;
                        break;
                    }
                }
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

            foreach (Enemy e in enemies)
            {
                e.draw(spriteBatch);
            }

            score.draw(spriteBatch);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
