using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaga
{
    class Projectile
    {
        int xVel, yVel;
        Texture2D texture;
        Rectangle pos;

        public Projectile(Texture2D t, Rectangle pos, int xVel, int yVel)
        {
            this.pos = pos;
            this.xVel = xVel;
            this.yVel = yVel;
            texture = t;
        }

        public Projectile(Texture2D t, int x, int y, int w, int h, int xVel, int yVel)
        {
            pos = new Rectangle(x,y,w,h);
            this.xVel = xVel;
            this.yVel = yVel;
            texture = t;
        }

        public void draw(SpriteBatch sp)
        {
            sp.Draw(texture, pos, Color.White);
        }

        public void update(KeyboardState kb)
        {
            pos.X += xVel;
            pos.Y += yVel;
        }

    }
}
