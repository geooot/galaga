using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaga
{
    interface Sprite
    {
        void update(KeyboardState kbs);
        void draw(SpriteBatch sb);
    }
}
