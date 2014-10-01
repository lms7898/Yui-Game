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
    public enum yuiState { FaceRight, FaceLeft, WalkRight, WalkLeft, Idle, AttackLeft, AttackRight, RangedLeft, RangedRight, JumpingLeft, JumpingRight, Dead };

    //To store all the textures
    public struct YuiSprites
    {
        public Texture2D yuiIdle;
        public Texture2D yuiRun;
        public Texture2D yuiJump;
        public Texture2D yuiMelee;
        public Texture2D yuiRanged;
        public Texture2D yuiDeath;
        public Texture2D foxIdle;
        public Texture2D foxRun;
        public Texture2D foxJump;
        public Texture2D foxAttack;
        public Texture2D foxDeath;
    }

    // This is the player class
    // The game will have just one of these objects
    public class Player : CollidableObject
    {
        //attributes
        private int health;
        private int maxHealth;
        private int speed;
        private int mana;
        private int maxMana;
        private int genMaxMana;
        private int meleeDamage; //may or may not move
        private int rangedDamage;
        private bool isHuman;
        private bool usingMelee; //indicates using a melee attack
        private bool usingRanged; //indicates using a ranged attack
        private bool stun;
        private Rectangle foxRect;
        private Rectangle foxHitBox;
        private Rectangle yuiHitBox;
        private YuiSprites spriteSheets;

        //ranged attack stuff
        private Fireball fireball;

        // jumping stuff
        private bool jumping, platformCollisions = true; //is the character jumping?
        private double startY, jumpspeed = 0; //startY to tell us where it lands, jumpspeed to see how fast it jumps
        private double startJumpSpeed;

        //store the states the character is in (for movement)
        yuiState face;
        yuiState previousFace = yuiState.Idle;

        //stores bool for whether or not Yui is dead
        private bool isDead;

        int timeSinceLastAttack;
        const int YUI_Y = 1;
        const int YUI_HEIGHT = 161;
        const int YUI_WIDTH = 139;
        const int YUI_X_OFFSET = 2;

        //properties
        public yuiState Face { 
            get { return face; } 
            set { face = value; } 
        }

        public bool Jumping { get { return jumping; } set { jumping = value; } }
        public bool PlatformCollisions { set { platformCollisions = value; } }
        public bool IsHuman { get { return isHuman; } }
        public bool Stun { get { return stun; } set { stun = value; } }


        //constructor
        public Player(Texture2D img, YuiSprites sprites, Vector2 pos, int width, int height, int foxWidth, int foxHeight, int ID)
            : base(img, pos, width, height, ID)
        {
            // setting up the attributes
            health = 100;
            maxHealth = 100;
            mana = 60;
            maxMana = 100;
            genMaxMana = 100;
            speed = 3;
            isHuman = true;
            isDead = false;
            rangedDamage = 50;
            meleeDamage = 30;
            spriteSheets = sprites;
            foxRect = new Rectangle((int)position.X - foxWidth / 2, (int)position.Y - foxHeight / 2, foxWidth, foxHeight);
            gravity = 0.4f;
            face = yuiState.FaceRight;
            previousFramesElasped = 0;
            startY = ground;
            yuiHitBox = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width / 2, height);
            foxHitBox = foxRect;
        }

        //change the form of the character
        //even changes some of the attributes
        public void ChangeForm()
        {
            switch (isHuman)
            {
                case true:
                    isHuman = false;
                    maxMana -= 20;
                    speed += 2;
                    Progress.startingSpeed += 2;
                    hitBox = foxHitBox;
                    break;
                case false:
                    isHuman = true;
                    maxMana += 20;
                    speed -= 2;
                    Progress.startingSpeed -= 2;
                    hitBox = yuiHitBox;
                    break;
            }
        }


        //properties - some can be changed with leveling
        public int Health
        {get { return health; }set{health = value;if (health > maxHealth)health = maxHealth; } }
        public int Mana { get { return mana; } set { mana = value; if (mana > maxMana) mana = maxMana; } }
        public int MaxMana { get { return maxMana; } set { maxMana = value; } }
        public int GenMaxMana { get { return genMaxMana; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int Speed { get { return speed; } set { speed = value; } }
        public int MeleeDamage
        {
            get
            {
                return meleeDamage;
            }
            set //only when leveling up
            {
                meleeDamage = value;
            }
        }

        public int RangedDamage
        {
            get
            {
                return rangedDamage;
            }
            set //only when leveling up
            {
                rangedDamage = value;
            }
        }

        public bool UsingRanged
        {
            get
            {
                return usingRanged;
            }
            set //only when leveling up
            {
                usingRanged = value;
            }
        }

        public bool UsingMelee
        {
            get
            {
                return usingMelee;
            }
            set //only when leveling up
            {
                usingMelee = value;
            }
        }

        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set //only when Yui dies
            {
                isDead = value;
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
                else if(obj is Enemy)
                {
                    Enemy enemy = (Enemy)obj;
                    if (stun)
                    {
                        if(!isHuman)
                        if (enemy.Stun())
                            mana -= 20;
                    }
                }
            }
            if (!onPlatform)
                ground = (int)startY;
        }

        public void Jump()
        {
            if (jumping)
            {

                if ((face == yuiState.FaceLeft) || (face == yuiState.WalkLeft))
                {
                    face = yuiState.JumpingLeft; //if the character is facing left, attack left
                }
                else if ((face == yuiState.FaceRight) || (face == yuiState.WalkRight))
                {
                    face = yuiState.JumpingRight; //if the character is facing right, attack right
                }
                Move(0, (int)jumpspeed);//Making it go up
                jumpspeed += 1;//Some math (explained later)
                if (position.Y >= ground - hitBox.Height / 2)
                //If it's farther than ground
                {
                    SetPosition((int)position.X, ground - hitBox.Height / 2);//Then set it on
                    jumping = false;
                    if (face == yuiState.JumpingLeft)
                        face = yuiState.FaceLeft;
                    else if (face == yuiState.JumpingRight)
                        face = yuiState.FaceRight;
                }
            }
        }

        public void Jump(double speed)
        {
            startJumpSpeed = speed;
            if(!jumping)
            {
                jumping = true;
                jumpspeed = speed; //Give it upward thrust
            }
        }

        //ranged attack, fire fireball
        public void Fire()
        {
            // Only fire if the bullet is inactive
            if (!fireball.Active)
            {
                // Move the fireball to the Yui's position and direction
                //fireball.Direction = this.direction;
                //fireball.position.X = this.position.X;
                //fireball.position.Y = this.position.Y;
                fireball.Active = true;
            }
        }

        //the attack method
        public void Attack(NPC target)
        {
            if ((face == yuiState.FaceLeft) || (face == yuiState.WalkLeft))
            {
                face = yuiState.AttackLeft; //if the character is facing left, attack left
            }
            else if ((face == yuiState.FaceRight) || (face == yuiState.WalkRight))
            {
                face = yuiState.AttackRight; //if the character is facing right, attack right
            }
            if (Vector2.Distance(position, target.Position) < 80)
            {
                if (target is Boss)
                    target.Health -= 8;
                target.Health -= meleeDamage; //now, attack
            }
        }

        // if there are no enemies
        public void Attack()
        {
            if ((face == yuiState.FaceLeft) || (face == yuiState.WalkLeft))
            {
                face = yuiState.AttackLeft; //if the character is facing left, attack left
            }
            else if ((face == yuiState.FaceRight) || (face == yuiState.WalkRight))
            {
                face = yuiState.AttackRight; //if the character is facing right, attack right
            }
        }

        public override void Move(int x, int y)
        {
            base.Move(x, y);
            foxRect.X = (int)position.X - (foxRect.Width / 2);
            foxRect.Y = (int)position.Y - (foxRect.Height / 2);
            foxHitBox.X = (int)position.X - (foxHitBox.Width / 2);
            foxHitBox.Y = (int)position.Y - (foxHitBox.Height / 2);
            yuiHitBox.X = (int)position.X - (yuiHitBox.Width / 2);
            yuiHitBox.Y = (int)position.Y - (yuiHitBox.Height / 2);
        }
        public override void SetPosition(int x, int y)
        {
            base.SetPosition(x, y);
            Move(0, 0);
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (face == yuiState.FaceLeft || face == yuiState.FaceRight)
            {
                if (isHuman)
                    numFrames = 16;
                else
                    numFrames = 8;
                timePerFrame = 80;
            }
            else if (face == yuiState.WalkLeft || face == yuiState.WalkRight)
            {
                if (isHuman)
                    numFrames = 12;
                else
                    numFrames = 9;
                timePerFrame = 80;
            }
            else if (face == yuiState.JumpingLeft || face == yuiState.JumpingRight)
            {
                if (isHuman)
                    numFrames = 3;
                else
                    numFrames = 7;
                timePerFrame = 120;
            }
            else if (face == yuiState.AttackLeft || face == yuiState.AttackRight)
            {
                if (isHuman)
                    numFrames = 3;
                else
                    numFrames = 4;
                timePerFrame = 80;
            }
            else if(face==yuiState.RangedLeft||face==yuiState.RangedRight)
            {
                numFrames = 3;
                timePerFrame = 80;
            }
            else if (face == yuiState.Dead)
            {
                if (isHuman)
                    numFrames = 10;
                else
                    numFrames = 8;
                timePerFrame = 100;
            }
            else
                timePerFrame = 80;

            if (face == yuiState.FaceLeft) //face left, don't walk
            {
                if (isHuman)
                {
                    if (frame >= 1 && frame < 8)
                        spriteBatch.Draw(spriteSheets.yuiIdle, drawRange, new Rectangle(frame * 150, 0, 150, 161), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    else
                        spriteBatch.Draw(spriteSheets.yuiIdle, drawRange, new Rectangle((frame % 8) * 150, 161, 150, 161), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                else
                    spriteBatch.Draw(spriteSheets.foxIdle, foxRect, new Rectangle(frame * 191, 0, 191, 99), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);

            }
            else if (face == yuiState.FaceRight) //face right, don't walk
            {
                if (isHuman)
                {
                    if (frame >= 1 && frame < 8)
                        spriteBatch.Draw(spriteSheets.yuiIdle, drawRange, new Rectangle(frame * 150, 0, 150, 161), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    else
                        spriteBatch.Draw(spriteSheets.yuiIdle, drawRange, new Rectangle((frame % 8) * 150, 161, 150, 161), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                else
                    spriteBatch.Draw(spriteSheets.foxIdle, foxRect, new Rectangle(frame * 191, 0, 191, 99), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }

            if (face != yuiState.FaceLeft && face != yuiState.FaceRight)
            {
                if (isHuman)
                {
                    if (face == yuiState.WalkLeft) //walk left
                    {
                        if (frame < 6)
                            spriteBatch.Draw(spriteSheets.yuiRun, drawRange, new Rectangle(frame * 173, 0, 173, 162), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                        else
                            spriteBatch.Draw(spriteSheets.yuiRun, drawRange, new Rectangle((frame - 6) * 173, 162, 173, 162), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else if (face == yuiState.WalkRight) //walk right
                    {
                        if (frame < 6)
                            spriteBatch.Draw(spriteSheets.yuiRun, drawRange, new Rectangle(frame * 173, 0, 173, 162), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        else
                            spriteBatch.Draw(spriteSheets.yuiRun, drawRange, new Rectangle((frame - 6) * 173, 162, 173, 162), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    }
                }
                else
                {
                    if (face == yuiState.WalkLeft) //walk left
                    {
                        spriteBatch.Draw(spriteSheets.foxRun, foxRect, new Rectangle((frame) * 191, 0, 191, 85), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else if (face == yuiState.WalkRight) //walk right
                    {
                        spriteBatch.Draw(spriteSheets.foxRun, foxRect, new Rectangle((frame) * 191, 0, 191, 85), Color.White);
                    }
                }
            }

            if (face == yuiState.AttackRight) // attack right
            {
                if(isHuman)
                    spriteBatch.Draw(spriteSheets.yuiMelee, drawRange, new Rectangle(frame * 178, 0, 178, 147), Color.White);
                else
                    spriteBatch.Draw(spriteSheets.foxAttack, foxRect, new Rectangle(frame * 226, 0, 226, 136), Color.White);
            }
            if (face == yuiState.AttackLeft) // attack left
            {
                if (isHuman)
                    spriteBatch.Draw(spriteSheets.yuiMelee, drawRange, new Rectangle(frame * 178, 0, 178, 147), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                else
                    spriteBatch.Draw(spriteSheets.foxAttack, foxRect, new Rectangle(frame * 226, 0, 226, 136), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            if(face==yuiState.RangedRight)
            {
                spriteBatch.Draw(spriteSheets.yuiRanged, drawRange, new Rectangle(frame * 143, 0, 143, 162), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            if (face == yuiState.JumpingLeft) //jump left
            {
                if (isHuman)
                {
                    if (ground - (int)position.Y < 100)
                    {
                        if (position.Y > previousPosition.Y)
                            spriteBatch.Draw(spriteSheets.yuiJump, drawRange, new Rectangle(306, 0, 153, 157), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                        else
                            spriteBatch.Draw(spriteSheets.yuiJump, drawRange, new Rectangle(0, 0, 153, 157), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                        spriteBatch.Draw(spriteSheets.yuiJump, drawRange, new Rectangle(153, 0, 153, 157), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
                else
                {
                    spriteBatch.Draw(spriteSheets.foxJump, foxRect, new Rectangle(frame * 210, 0, 210, 136), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }
            }
            if (face == yuiState.JumpingRight) //jump right
            {
                if (isHuman)
                {
                    if (ground - (int)position.Y < 100)
                    {
                        if (position.Y > previousPosition.Y)
                            spriteBatch.Draw(spriteSheets.yuiJump, drawRange, new Rectangle(306, 0, 153, 157), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                        else
                            spriteBatch.Draw(spriteSheets.yuiJump, drawRange, new Rectangle(0, 0, 153, 157), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                    }
                    else
                        spriteBatch.Draw(spriteSheets.yuiJump, drawRange, new Rectangle(153, 0, 153, 157), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(spriteSheets.foxJump, foxRect, new Rectangle(frame * 210, 0, 210, 136), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                }
            }

            if (face == yuiState.Dead)
            {
                if (isHuman)
                    spriteBatch.Draw(spriteSheets.yuiDeath, drawRange, new Rectangle(frame * 188, 0, 188, 161), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
                else
                    spriteBatch.Draw(spriteSheets.foxDeath, drawRange, new Rectangle(frame * 191, 0, 191, 99), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
            if (face == yuiState.RangedLeft)
            {
                spriteBatch.Draw(spriteSheets.yuiRanged, drawRange, new Rectangle(frame * 143, 0, 143, 162), Color.White, 0, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            if (face == yuiState.RangedRight)
            {
                spriteBatch.Draw(spriteSheets.yuiRanged, drawRange, new Rectangle(frame * 143, 0, 143, 162), Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
            }

            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            //frame = framesElapsed % numFrames;

            if (previousFace != face)
                frame = 0;
            if (framesElapsed != previousFramesElasped)
            {
                if (face == yuiState.Dead)
                {
                    if (frame < numFrames - 1)
                        frame++;
                    isDead = true;
                }
                else
                {
                    frame++;
                }
            }
            if (frame >= numFrames - 1 && face != yuiState.Dead)
            {
                frame = 0;
                if (face == yuiState.RangedLeft || face == yuiState.AttackLeft)
                    face = yuiState.FaceLeft;
                else if (face == yuiState.RangedRight || face == yuiState.AttackRight)
                    face = yuiState.FaceRight;
            }
            previousFramesElasped = framesElapsed;
            previousFace = face;
        }
    }
}
