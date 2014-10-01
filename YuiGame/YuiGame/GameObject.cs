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
    // The base class
    // All other classes inherit from this one
    public class GameObject
    {
        // Field for managing objects
        protected int objectID;

        //Field for position
        protected Vector2 position;
        protected Vector2 previousPosition;
        protected bool visible; // should the object be drawn
        protected Texture2D image; //the image for the game object...
        protected Vector2 center;
        protected bool canMove;

        //stuff for gravity
        protected bool hasGravity;
        protected float gravity;
        protected Vector2 velocity;
        protected bool isGrounded;
        public int ground;
        
        //stuff for animation, may be moved to another class
        protected int frame;
        protected double timePerFrame;
        protected int numFrames;
        protected int framesElapsed;
        protected int previousFramesElasped;
        protected Rectangle drawRange;

        //Properties
        public Texture2D Image
        {
            get { return image; }
        }

        public bool CanMove { get { return canMove; } set { canMove = false; } }

        public Vector2 Position
        {
            get { return position; }
        }
        public Vector2 PreviousPosition { get { return previousPosition; } set { previousPosition = value; } }
        public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }

        public int ObjectID { get { return objectID; } }

        public int Frame { get { return frame; } set { frame = value; } }
        public int NumFrames { get { return numFrames; } set { numFrames = value; } }

        // Constructor which sets up coordinates
        public GameObject(Texture2D img, Vector2 pos, int width, int height, int ID)
        {
            objectID = ID;
            image = img;
            position = pos;
            drawRange = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);
            visible = true;
            gravity = 1.0f;
            ground = 520;
            objectID = ID;
            hasGravity = true;
            isGrounded = true;
            canMove = true;
            previousFramesElasped = 0;
        }

        //method for movement, will change depending on the type of object
        //moves the by (x, y), x and y can be negative
        public virtual void Move(int x, int y)
        {
            if (canMove)
            {
                position += new Vector2(x, y);
                drawRange.X = (int)position.X - (drawRange.Width / 2);
                drawRange.Y = (int)position.Y - (drawRange.Height / 2);
            }
        }

        public virtual void SetPosition(int x, int y)
        {
            position = new Vector2(x, y);
            Move(0, 0);
        }
        public bool IsMoving()
        {
            if (previousPosition != position)
                return true;
            return false;
        }
        public virtual void Gravity()
        {
            if(hasGravity)
            {

                if(isGrounded)
                {
                    gravity = 4.0f;
                }
                else
                {
                    Move(0, (int)gravity);
                    gravity += 1.0f;
                }

                if (position.Y >= ground)
                {
                    SetPosition((int)position.X, ground);
                    isGrounded = true;
                }
                else
                    isGrounded = false;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // only draw if the object needs to be visible
            if (visible)
            {
                // The default drawing for if the object is not moving
                // No animation involved
                    spriteBatch.Draw(image, drawRange, Color.White);
            }
        }
    }
}