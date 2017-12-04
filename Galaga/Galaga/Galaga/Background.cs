using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Galaga
{
    class Background : Sprite
    {
        private Rectangle rect;
        private Texture2D text;
        private int width = 0;
        private int height = 0;
        private int dividerY = 0;
        private int speed = 1;

        public Background (Game1 game, int w, int h)
        {
            width = w;
            height = h;
            text = game.Content.Load<Texture2D>("scrolling_space");
            rect = new Rectangle(0, 0, width, height);
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(text, rect, Color.White);
            sb.Draw(text, new Rectangle(0, rect.Y - height, width, height), Color.White);
        }

        public void update(KeyboardState kbs)
        {
            rect.Y += speed;
            if (rect.Y >= height)
            {
                rect.Y = 0;
            }
        }
    }
}
