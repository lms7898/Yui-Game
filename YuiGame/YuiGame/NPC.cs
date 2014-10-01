using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace YuiGame
{
    // A class for all the NPCs
    public class NPC : CollidableObject
    {
        //attributes
        protected int health; //health for non-player characters - includes enemies
        protected int speed;
        Texture2D NPCsprite;

        // attributes for jumping
        protected bool jumping; //Is the character jumping?
        protected double startY, jumpspeed = 0; //startY to tell us //where it lands, jumpspeed to see how fast it jumps
        protected bool platformCollisions;

        public bool Jumping { get { return jumping; } set { jumping = value; } }

        //constructor
        public NPC(Texture2D img, Vector2 pos, int width, int height, int ID)
            : base(img, pos, width, height, ID)
        {
            // setting up the attributes
            health = 100;
            startY = ground ;//Starting position
            jumping = false;//Init jumping to false
            jumpspeed = 0;//Default no speed
            image = img;
            platformCollisions = true;
        }

        // method for jumping
        public virtual void Jump()
        {
            if (jumping)
            {
                Move(0, (int)jumpspeed);//Making it go up
                jumpspeed += 1;//Some math (explained later)
                if (position.Y >= ground-hitBox.Height/2)
                //If it's farther than ground
                {
                    SetPosition((int)position.X, ground - hitBox.Height/2);//Then set it on
                    jumping = false;
                }
            }
        }

        public void Jump(double speed)
        {
            if (!jumping)
            {
                jumping = true;
                jumpspeed = speed; //Give it upward thrust
            }
        }

        // new collision method
        public override void Collide()
        {
            bool onPlatform = false;
            foreach (CollidableObject obj in collidesWith)
            {
                if (obj is Platform)
                {
                    if ((obj.Position.Y - previousPosition.Y >= hitBox.Height / 2 + obj.HitBox.Height / 2))
                    {
                        if (platformCollisions)
                        if (Math.Abs(position.Y - obj.Position.Y) > 36 && obj.Position.Y > previousPosition.Y)
                        {
                            SetPosition((int)position.X, (int)obj.Position.Y - (hitBox.Height / 2 + obj.HitBox.Height / 2));
                            ground = (int)obj.Position.Y - (obj.HitBox.Height / 2);
                            onPlatform = true;
                        }
                    }
                }
            }
            if (!onPlatform)
                ground = (int)startY;
        }

        //properties
        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            timePerFrame = 80;
            numFrames = 10;
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);

            spriteBatch.Draw(image, drawRange, new Rectangle(frame * 185, 0, 185, 162), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

            //frame = framesElapsed % numFrames;
            if (framesElapsed != previousFramesElasped)
            {
                frame++;
            }
            previousFramesElasped = framesElapsed;

            if (frame >= numFrames)
                frame = 0;
        }
    }
}
