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
    public struct EnemySprites
    {
        public Texture2D Run;
        public Texture2D Attack;
        public Texture2D Jump;
    }
    public enum EnemyState { Idle, RunningLeft, RunningRight, AttackLeft, AttackRight, JumpLeft, JumpRight } //probably won't use Idle
    public class Enemy : NPC
    {
        public EnemyState face;
        public EnemyState previousFace;
        public EnemySprites sprites;
        private bool isStunned;
        private int timeSinceStun;
        private Rectangle attackRange;
        public Enemy(Texture2D img, EnemySprites sprites, Vector2 pos, int width, int height, int ID)
            : base(img, pos, width, height, ID)
        {
            speed = 3;
            face = EnemyState.Idle;
            this.sprites = sprites;
            timeSinceStun = 10000;
            attackRange = new Rectangle();
        }

        public bool Stun()
        {
            bool stun = isStunned;
            if (!isStunned)
            {
                isStunned = true;
                timeSinceStun = 0;
            }
            if (timeSinceStun == 0)
                return true;
            return false;
        }
        protected virtual void Attack(Player player, int attackPower)
        {
            player.Health -= attackPower;
        }
        public virtual void EnemyAI(Player player)
        {
            if (!isStunned)
            {
                int dist;
                if (player.IsHuman)
                    dist = 20;
                else
                    dist = 60;
                if (Vector2.Distance(player.Position, position) < dist)
                {
                    if (player.Position.X > position.X)
                        face = EnemyState.AttackRight;
                    else
                        face = EnemyState.AttackLeft;
                    Random rand = new Random();
                    if (rand.Next(10) == 0)
                    {
                        Attack(player, 1);
                    }
                }
                else
                {
                    Vector2 direction = Vector2.Normalize(player.Position - position);
                    if (direction.X > 0)
                        face = EnemyState.RunningRight;
                    else
                        face = EnemyState.RunningLeft;
                    Move((int)(direction.X * speed), 0);
                    double angle = MathHelper.ToDegrees((float)Math.Acos(Vector2.Dot(Vector2.UnitX, direction)));

                    //if the enemy is right above you, jump
                    if (angle > 60 && angle < 120)
                    {
                        if (player.Position.Y < position.Y)
                            Jump(-17);
                        else
                            platformCollisions = false;
                    }
                }
                if (jumping)
                    Jump();
                if (player.IsDead)
                    face = EnemyState.Idle;
            }
            else
            {
                face = EnemyState.Idle;
            }
            timeSinceStun++;
            if(timeSinceStun>200)
            {
                isStunned = false;
            }
            if (position.Y > 400)
                platformCollisions = true;
        }

        public override void Jump()
        {
            if ((face == EnemyState.RunningLeft) || (face == EnemyState.AttackLeft))
            {
                face = EnemyState.JumpLeft; //if the character is facing left, jump left
            }
            else if ((face == EnemyState.RunningRight) || (face == EnemyState.AttackRight))
            {
                face = EnemyState.JumpRight; //if the character is facing right, jump right
            }
            base.Jump();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            attackRange.Height = drawRange.Height + 30;
            attackRange.Width = drawRange.Width + 30;
            attackRange.Y = (int)position.Y - attackRange.Height / 2;
            attackRange.X = (int)position.X - attackRange.Width / 2;

            timePerFrame = 80;
            if (face == EnemyState.AttackLeft || face == EnemyState.AttackRight)
            {
                numFrames = 4;
                timePerFrame = 100;
            }
            if(face==EnemyState.RunningLeft||face==EnemyState.RunningRight)
            {
                numFrames = 12;
            }
            if (face == EnemyState.AttackRight)
            {
                spriteBatch.Draw(sprites.Attack, attackRange, new Rectangle(frame * 161, 0, 161, 161), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            else if (face == EnemyState.AttackLeft)
            {
                spriteBatch.Draw(sprites.Attack, attackRange, new Rectangle(frame * 161, 0, 161, 161), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else if (face == EnemyState.JumpLeft)
            {
                if (position.Y < previousPosition.Y)
                    spriteBatch.Draw(sprites.Jump, drawRange, new Rectangle(0, 0, 100, 322), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                else
                    spriteBatch.Draw(sprites.Jump, drawRange, new Rectangle(322, 0, 322, 322), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else if (face == EnemyState.JumpRight)
            {
                if (position.Y < previousPosition.Y)
                    spriteBatch.Draw(sprites.Jump, drawRange, new Rectangle(0, 0, 322, 322), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(sprites.Jump, drawRange, new Rectangle(322, 0, 322, 322), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            else if (face == EnemyState.RunningRight)
            {
                if (frame < 6)
                    spriteBatch.Draw(sprites.Run, drawRange, new Rectangle(frame * 322, 0, 322, 322), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(sprites.Run, drawRange, new Rectangle((frame - 6) * 322, 322, 322, 322), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            else if (face == EnemyState.RunningLeft)
            {
                if (frame < 6)
                    spriteBatch.Draw(sprites.Run, drawRange, new Rectangle(frame * 322, 0, 322, 322), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                else
                    spriteBatch.Draw(sprites.Run, drawRange, new Rectangle((frame-6) * 322, 322, 322, 322), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else if(face==EnemyState.Idle)
                spriteBatch.Draw(sprites.Attack, drawRange, new Rectangle(0, 0, 161, 161), Color.Red, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            //Console.WriteLine(face);
            //frame = framesElapsed % numFrames;
            if (framesElapsed != previousFramesElasped)
            {
                frame++;
            }
            if (frame >= numFrames || previousFace!=face)
                frame = 0;
            previousFramesElasped = framesElapsed;
            previousFace = face;
        }
    }
}
