using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Galaga
{
    class Menu : Sprite
    {
        string title;
        string start;
        int timer;

        public Menu()
        {
            timer = 0;
            title = "Galaga";
            start = "Press SPACE to Start...";
        }
        public void draw(SpriteBatch sb)
        {
            if (!Game1.gameStarted)
            {
                sb.DrawString(Game1.gameFont1, title, new Vector2(Game1.graphics.PreferredBackBufferWidth / 3, 50), Color.White);
                sb.DrawString(Game1.gameFont1, start, new Vector2(30, 150), Color.White);
            }
        }

        public void update(KeyboardState kbs)
        {
            timer++;
            if (kbs.IsKeyDown(Keys.Space))
            {
                Game1.gameStarted = true;
            }
            if ((timer % 60) == 0)
            {
                if (start.Equals(""))
                {
                    start = "Press SPACE to Start...";
                } else
                {
                    start = "";
                }
            }
        }
    }
}
