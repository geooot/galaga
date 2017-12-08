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
        public Rectangle rect;
        private Texture2D text;
        private static Texture2D[] textures;
        private static Random rand;
        private bool diving;
        public int spotX = -100;
        public int spotY = -100;
        public int state = 0; //0 = swooping to spot, 1 = in horde sidetoside, 2 = diving, 3 = dead
        private static int xDeviation = 0;
        private static int[] screenDimensions;
        public static int speed = 0;
        double xSpeed = 0;
        int ySpeed = 0;
        private static int accuracy = 0;
        public static int timer = 0;
        private static int interval = 1;
        private static int direction = 0;
        private static int boundary = 0;
        private double acceleration = 0;
        double xPos = -100;
        double yPos = -100;


        public Enemy(int x, int y)
        {
            state = 0;
            xDeviation = 0;
            text = textures[rand.Next(0, textures.Length)];
            rect = new Rectangle(0,0,100, 100);
            spotX = x;
            spotY = y;
            diving = false;
            rect.X = rand.Next(0, screenDimensions[0]-rect.Width);
            xPos = rect.X;
            timer = 0;
            direction = 1;
            acceleration = 0;
        }

        public static void load (Game1 game, int width, int height)
        {
            textures = new Texture2D[1];
            textures[0] = game.Content.Load<Texture2D>("AjitEnemy");
            rand = new Random();
            screenDimensions = new int[2];
            screenDimensions[0] = width;
            screenDimensions[1] = height;
            speed = 5;
            accuracy = 3;
            timer = 0;
            boundary = 25;
        }
        public void dive()
        {
            if (state==1)
            {
                diving = true;
                ySpeed = speed;
                xSpeed = 0;
                //acceleration = rand.NextDouble()*.3-.15;
                acceleration = 2.0 * (rand.Next(screenDimensions[0]) - (spotX+rect.Width/2.0)) * ySpeed * ySpeed / ((screenDimensions[1]-rect.Y)* 1.0*(screenDimensions[1] - rect.Y));
                Console.WriteLine(acceleration);
                state = 2;
            }
            
        }

        public void kill()
        {
            state = 3;
        }

        public void revive()
        {
            state = 0;
            rect.X = rand.Next(0, screenDimensions[0] - rect.Width);
        }



        public void draw(SpriteBatch sb)
        {
            if (state !=3 )
            {
                sb.Draw(text, rect, Color.White);
            }
            
        }

        public static void deviation ()
        {
            timer++;
            if (timer % interval == 0)
            {
                xDeviation += direction;
                if (Math.Abs(xDeviation) >= boundary)
                {
                    direction *= -1;
                }
            }
        }

      

        public void update(KeyboardState kbs)
        {

           // Console.WriteLine(state);
            switch(state)
            {
                case 0:
                    double angle = Math.Atan((spotY - rect.Y) * 1.0 / (spotX + xDeviation - rect.X));
                    ySpeed = (int)(speed * Math.Sin(angle));
                    xSpeed = (int)(speed * Math.Cos(angle));
                    if (spotY>rect.Y)
                    {
                        ySpeed = Math.Abs(ySpeed);
                    } else
                    {
                        ySpeed = -Math.Abs(ySpeed);
                    }
                    if (spotX > rect.X)
                    {
                        xSpeed = Math.Abs(xSpeed);
                    }
                    else
                    {
                        xSpeed = -Math.Abs(xSpeed);
                    }
                    rect.Y += ySpeed;
                    rect.X += (int)xSpeed;
                    xPos = rect.X;
                    yPos = rect.Y;
                    if (Math.Abs(rect.Y-spotY)<accuracy && Math.Abs(rect.Y - spotY) < accuracy)
                    {
                        state = 1;
                    }

                    break;
                case 1:
                    rect.X = spotX + xDeviation;
                    xPos = rect.X;
                    break;
                case 2:
                    xSpeed += acceleration;
                    rect.Y += ySpeed;
                    xPos += xSpeed;
                    rect.X = (int)xPos;

                    if (rect.Y>screenDimensions[1])
                    {
                        rect.X = rand.Next(0, screenDimensions[0] - rect.Width);
                        rect.Y = 0 - rect.Height;
                        state = 0;
                    }
                    break;
            }
        }
    }
}
