using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace YuiGame
{
    public class Fireball:CollidableObject
    {
        // Attributes
        private bool active;
        public int FireballSpeed = 10;
        private Player Yui;
        float fireStart;
        private string direction;

        // Gets and sets the active state of the fireball
        public bool Active { get { return active; } set { active = value; } }

        
        //constructor, takes fireball's x and y value relative to Yui, as well as her direction
        public Fireball(Player yui, Texture2D img, Vector2 pos, int width, int height, int ID)
            : base(img, pos, width, height, ID)
        {
            // Start out inactive
            active = false;

            this.hasGravity = false;

            //get Yui's stats
            Yui = yui;
        }
        
        
        //moves fireball in direction Yui is facing
        public void Move()
        {
            

            // Is the fireball active?
            if (active)
            {
                // Move in direction Yui's facing when she fires
                if (direction == "Left")
                {
                    Move(-FireballSpeed, 0);
                }
                else if (direction == "Right")
                {
                    Move(FireballSpeed, 0);
                }

                //Don't go past range
                if (this.position.X > fireStart + 300 || this.position.X < fireStart - 300)
                    active = false;
                
            }
        }

        public void fire()
        {
            //keep track of fireball's starting position
            SetPosition((int)Yui.Position.X, (int)Yui.Position.Y);
            fireStart = this.position.X;
            active = true;
            /*
            if (Yui.Face == yuiState.FaceLeft || Yui.Face == yuiState.JumpingLeft || Yui.Face == yuiState.WalkLeft || Yui.Face == yuiState.AttackLeft)
            {
                direction = "Left"; //firing to left
            }
             */
            if (Yui.Face == yuiState.FaceRight || Yui.Face == yuiState.JumpingRight || Yui.Face == yuiState.WalkRight || Yui.Face == yuiState.AttackRight)
            {
                direction = "Right"; //firing to right
            }
            else
            {
                direction = "Left";
            }
        }

        //code for drawing the fireball

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Only draw if we're active
            if (active)
            {
                if (direction == "Right")
                    spriteBatch.Draw(image, drawRange, new Rectangle(0, 0, 25, 28), Color.White);
                else
                    spriteBatch.Draw(image, drawRange, new Rectangle(0, 0, 25, 28), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                //fireball not visible, doesn't exist yet
            }
        }
    }
}
