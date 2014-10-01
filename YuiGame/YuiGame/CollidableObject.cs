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
    // This class is for game objects that can collide
    public class CollidableObject : GameObject
    {
        //attributes
        protected Rectangle hitBox; //boxes around image sprites, used to determine collision
        //protected bool hasGravity; //does gravity act on the object?

        // a list of literally every other object 
        protected List<CollidableObject> collidesWith = new List<CollidableObject>();

        public Rectangle HitBox { get { return hitBox; } set { hitBox = value; } }
        public List<CollidableObject> CollidesWith { get { return collidesWith; } set { collidesWith = value; } }

        //new constructor
        public CollidableObject(Texture2D img, Vector2 pos, int width, int height, int ID)
            : base(img, pos, width, height, ID)
        {
            hitBox = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height); // The hit box is usually the same as the drawing rectangle, right?
        }

        //method to detect general collision with another object
        public bool IsColliding(CollidableObject obj)
        {
            //if this hitbox intersects with another hitBox
            if (hitBox.Intersects(obj.HitBox))
            {
                if (obj.ObjectID != objectID)
                    return true;
            }
            return false;
        }

        public virtual void Collide()
        {
            if (collidesWith.Count != 0)
                SetPosition((int)previousPosition.X, (int)previousPosition.Y);
        }

        public override void Move(int x, int y)
        {
            base.Move(x, y);
            hitBox.X = (int)position.X - (hitBox.Width / 2);
            hitBox.Y = (int)position.Y - (hitBox.Height / 2);
        }

        public override void SetPosition(int x, int y)
        {
            base.SetPosition(x, y);
            Move(0, 0);
        }

        public override void Gravity()
        {
            if (hasGravity)
            {

                if (isGrounded)
                {
                    gravity = 4.0f;
                }
                else
                {
                    Move(0, (int)gravity);
                    gravity += 1.0f;
                }

                if (position.Y >= ground - hitBox.Height / 2)
                {
                    SetPosition((int)position.X, ground - hitBox.Height / 2);
                    isGrounded = true;
                }
                else
                    isGrounded = false;
            }
        }
        
    }
}
