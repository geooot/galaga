using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Galaga
{
    class Score : Sprite
    {
        long score;

        public Score()
        {
            score = 0;
        }

        public void draw(SpriteBatch sb)
        {
            sb.DrawString(Game1.gameFont1, "Score: " + score, new Vector2(0, 0), Color.White);
        }

        public void update(KeyboardState kbs)
        {

        }

        public void incrementScore()
        {
            score++;
        }

        public void incrementScore(long hahahahahahahaha)
        {
            score += hahahahahahahaha;
        }

        public long getScore()
        {
            return score;
        }
    }
}
