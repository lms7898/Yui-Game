using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace YuiGame
{
    public class ScrollingBackground
    {
        // class ScrollingBackground
        private Player player;
        private bool isScrolling;
        private Vector2 screenpos, origin, texturesize, btexturesize, totalscreen;
        private Texture2D mytexture,mytexture2;
        private int screenheight, screenwidth;
        private int cameraX;
        public int CameraX { get { return cameraX; } }
        public void Load(GraphicsDevice device, Texture2D backgroundTexture, Texture2D bossroomTexture, Player plr)
        {
            player = plr;
            mytexture = backgroundTexture;
            mytexture2 = bossroomTexture;
            screenheight = device.Viewport.Height;
            screenwidth = device.Viewport.Width;
            // Set the origin to draw from the top left corner
            origin = new Vector2(0, 0);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(0, 0);
            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(mytexture.Width, 0);
            btexturesize = new Vector2(mytexture2.Width, 0);
            isScrolling = false;
            cameraX = 0;
        }
        // ScrollingBackground.Update
        public void Update(List<GameObject> gameObjects)
        {
            if (player.Position.X < 200 && cameraX > 200)
            {
                ScrollLeft(player.Speed);
                cameraX -= player.Speed;
                foreach (GameObject obj in gameObjects)
                {
                    obj.Move(player.Speed, 0);
                }
            }
            if (player.Position.X > 400 && cameraX <= 11400)
            {
                ScrollRight(player.Speed);
                cameraX += player.Speed;
                foreach (GameObject obj in gameObjects)
                {
                    obj.Move(-player.Speed, 0);
                }
            }
        }
        // ScrollingBackground.Draw
        public void Draw(SpriteBatch batch)
        {
            // Draw the texture, if it is still onscreen.
            if (screenpos.X < screenwidth)
            {
                batch.Draw(mytexture, screenpos, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
            // Draw the texture a bunch of times behind one another
            // to create the scrolling illusion.
            for (int i = 0; i < 7; i++)
            {
                batch.Draw(mytexture, screenpos + texturesize * i, null,
                     Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            }
            //and behind incase for some reason you manage to go backwards
            batch.Draw(mytexture, screenpos - texturesize, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f);
            //drawing the boss area
            batch.Draw(mytexture2, screenpos + new Vector2(11275, 0), null, Color.White, 0, origin, 1, SpriteEffects.None, 0f);
        }

        //background scrolling methods
        public void ScrollLeft(int speed)
        {
            if (screenpos.X >= 0)
            {
                screenpos.X = 0;
            }
            screenpos.X += speed;
        }

        public void ScrollRight(int speed)
        {
            screenpos.X -= speed;
        }

        public void ScrollDown(float gravity)
        {
            screenpos.Y += gravity;
        }

        public void ScrollUp(float gravity)
        {
            screenpos.Y -= gravity;
        }
    }
}

