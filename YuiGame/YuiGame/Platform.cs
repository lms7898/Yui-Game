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
    public class Platform : CollidableObject // this class needs work
    {
        Rectangle sourceRange;
        //constructor
        public Platform(Texture2D img, Vector2 pos, int width, int height, int ID)
            : base(img, pos, width, height, ID)
        {
            hasGravity = false;
            sourceRange = new Rectangle(324, 72, width, height);
        }

        public override void SetPosition(int x, int y)
        {
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(image, drawRange, sourceRange, Color.White);
        }

        //method for spawning platforms?
    }
}
