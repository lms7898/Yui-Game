using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace YuiGame
{
    /// <summary>
    /// The class for the boss. Not much is gonna change
    /// </summary>
    public class Boss : Enemy
    {

        /// <summary>
        /// Te check the time since the last attack
        /// </summary>
        int timeSinceLastAttack;
        int timeSinceSpawn;


        /// <summary>
        /// The new constructor, nothing's really gonna change
        /// </summary>
        /// <param name="img">I'm not sure what that is</param>
        /// <param name="sprites">The sprites for the animations</param>
        /// <param name="pos">The position</param>
        /// <param name="width">The width of the box</param>
        /// <param name="height">The height</param>
        /// <param name="ID">The ID</param>
        public Boss(Texture2D img, EnemySprites sprites, Vector2 pos, int width, int height, int ID)
            : base(img, sprites, pos, width, height, ID)
        {
            previousFace = EnemyState.Idle;
            hasGravity = true;
            jumping = false;
            speed = 1;
            timeSinceLastAttack = 500;
            timeSinceSpawn = 0;
        }

        /// <summary>
        /// It's not gonna go on platforms, is it?
        /// </summary>
        public override void Collide()
        {
        }

        /// <summary>
        /// Making sure the attack only works once
        /// </summary>
        /// <param name="player"></param>
        /// <param name="attackPower"></param>
        protected override void Attack(Player player, int attackPower)
        {
            if ((face == EnemyState.AttackRight || face == EnemyState.AttackRight)
                && frame == 7 && timeSinceLastAttack >= 2000)
            {
                base.Attack(player, attackPower);
                timeSinceLastAttack = 0;
            }
        }

        /// <summary>
        /// The new AI for the boss
        /// </summary>
        /// <param name="player">The player it needs to attack</param>
        public override void EnemyAI(Player player)
        {
            if (Vector2.Distance(position, player.Position) <= 200)
            {

                if (face != EnemyState.AttackLeft && face != EnemyState.AttackRight)
                    frame = 0;
                if (player.Position.X > position.X)
                    face = EnemyState.AttackRight;
                else
                    face = EnemyState.AttackLeft;
                Attack(player, 10);
            }
            else
            {
                if (position.X - player.Position.X > 10)
                {
                    Move(-speed, 0);
                    face = EnemyState.RunningLeft;
                }
                else if (position.X - player.Position.X < -10)
                {
                    Move(speed, 0);
                    face = EnemyState.RunningRight;
                }
                //double angle = MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(Vector2.UnitX, direction)));
            }
        }

        /// <summary>
        /// The new draw method
        /// </summary>
        /// <param name="spriteBatch">The spritebatch that's obviously gonna draw the sprites</param>
        /// <param name="gameTime">For animation</param>
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            timeSinceLastAttack += (int)(gameTime.TotalGameTime.TotalMilliseconds-timeSinceSpawn);
            //Drawing code here
            if (face == EnemyState.RunningLeft || face == EnemyState.RunningRight)
            {
                numFrames = 11;
                timePerFrame = 200;
            }
            if (face == EnemyState.AttackLeft || face == EnemyState.AttackLeft)
            {
                numFrames = 11;
                timePerFrame = 200;
            }
            if (face == EnemyState.RunningRight)
            {
                spriteBatch.Draw(sprites.Run, drawRange, new Rectangle((frame % 4) * 512, (frame / 4) * 321, 512, 321), Color.DimGray, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            if (face == EnemyState.RunningLeft)
            {
                spriteBatch.Draw(sprites.Run, drawRange, new Rectangle((frame % 4) * 512, (frame / 4) * 321, 512, 321), Color.DimGray, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            if(face==EnemyState.AttackRight)
            {
                spriteBatch.Draw(sprites.Attack, drawRange, new Rectangle((frame % 4) * 512, (frame / 4) * 321, 512, 321), Color.DimGray, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            if (face == EnemyState.AttackLeft)
            {
                spriteBatch.Draw(sprites.Attack, drawRange, new Rectangle((frame % 4) * 512, (frame / 4) * 321, 512, 321), Color.DimGray, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            if (framesElapsed != previousFramesElasped)
            {
                frame++;
            }
            if (frame >= numFrames || previousFace != face)
                frame = 0;
            previousFramesElasped = framesElapsed;
            previousFace = face;
            timeSinceSpawn = (int)(gameTime.TotalGameTime.TotalMilliseconds);
        }
    }
}
