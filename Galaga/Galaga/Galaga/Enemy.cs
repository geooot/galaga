using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Galaga
{
    class Enemy : Sprite
    {
        private Rectangle rect;
        private Texture2D text;
        private static Texture2D[] textures;
        private static Random rand;
        private bool diving;

        public Enemy(int x, int y)
        {
            text = textures[rand.Next(0, textures.Length)];
            rect = new Rectangle(0,0,100, 100);
            rect.X = x;
            rect.Y = y;
            diving = false;
        }

        public static void load (Game1 game)
        {
            textures = new Texture2D[1];
            textures[0] = game.Content.Load<Texture2D>("AjitEnemy");
            rand = new Random();
        }
        public void dive()
        {
            diving = true;
        }


        public void draw(SpriteBatch sb)
        {
            sb.Draw(text, rect, Color.White);
        }

        public void update(KeyboardState kbs)
        {
            //
        }
    }
}
