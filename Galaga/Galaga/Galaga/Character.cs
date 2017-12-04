using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Galaga
{
    class Character: Sprite
    {
        Texture2D texture;
        Rectangle pos;
        int minX, maxX, speed;

        public Character(Texture2D t, Rectangle pos, int minX, int maxX, int speed)
        {
            this.pos = pos;
            texture = t;
            this.minX = minX;
            this.maxX = maxX;
            this.speed = speed;
        }

        public Character(Texture2D t, int x, int y, int w, int h, int minX, int maxX, int speed)
        {
            pos = new Rectangle(x, y, w, h);
            texture = t;
            this.minX = minX;
            this.maxX = maxX;
            this.speed = speed;
        }

        public void update(KeyboardState kbs)
        {
            if (kbs.IsKeyDown(Keys.Right))
            {
                pos.X += speed;
            }else if (kbs.IsKeyDown(Keys.Left))
            {
                pos.X -= speed;
            }

            if(pos.X < 0)
                pos.X = 0;

            if(pos.X > maxX)
                pos.X = maxX;
           
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, pos, Color.White);
        }
    }
}
