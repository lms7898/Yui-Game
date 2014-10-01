#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Threading;
using System.IO;
using System.Diagnostics;
#endregion

namespace YuiGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    public class Game1 : Game
    {
        #region Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //0 is start, 1 is game, 2 is pause
        private int gameMode;

        //variables for speaking
        bool isTalking = false;
        List<string> phrases = new List<string>();
        string talkName;
        string currentPhrase;
        int phraseIndex;
        
        
        //Location of the camera relative to the map
        private int mapX;
        private int mapY;

        //Location of the character relative to the map
        //will almost always be the same as the camera except for at the edge of the map
        private int cX;
        private int cY;

        //background for the game
        Texture2D backgroundArt;
        Texture2D bossroomArt;
        ScrollingBackground gameBackground = new ScrollingBackground();

        //temp storage for settings
        int mh, mm, sh, sm, sp;

        //collections of game objects
        private List<GameObject> gameObjects = new List<GameObject>();
        private int maxID = 0;

        Boss boss;

        // list of collidable objects
        private List<CollidableObject> collidableObjects = new List<CollidableObject>();

        //list of enemies
        private List<Enemy> enemies = new List<Enemy>();

        //player object
        Player player;

        Platform testPlatform;
        Platform Platform1;
        Platform Platform2;
        Platform Platform3;
        Platform Platform4;
        Platform Platform5;
        Platform Platform6;
        Platform Platform7;
        Platform Platform8;
        Platform Platform9;
        Platform Platform10;
        Platform Platform11;
        Platform Platform12;
        Platform Platform13;
        Platform Platform14;
        Platform Platform15;
        Platform Platform16;
        Platform Platform17;
        Platform Platform18;
        Platform Platform19;
        Platform Platform20;
        Platform Platform21;
        Platform Platform22;
        Platform Platform23;
        Platform Platform24;
        Platform Platform25;
        Platform Platform26;
        Platform Platform27;
        Platform Platform28;
        Platform Platform29;
        Platform Platform30;

        NPC tanuki;
        Texture2D tanukiSprite;

        Texture2D platformSprite;

        //all of the start menu options and an int for each
        private Dictionary<int, string> menuSelect = new Dictionary<int, string>();
        //what option is currently selected
        private int startMenuSelected;
        //works the same as start menu
        private Dictionary<int, string> pauseMenuSelect = new Dictionary<int, string>();
        private int pauseMenuSelected;

        private Dictionary<int, string> optionSelect = new Dictionary<int, string>();
        private int optionSelected;
        private bool showCredits;
        private bool showHow;


        //object used to get what buttons are being pressed
        private KeyboardState kState;
        private KeyboardState previousKState; //to store the previous state of the keyboard (mainly for tapping buttons
        private MouseState mState;
        private MouseState previousMState;

        //ammount of time the menu sleeps after a button is pressed
        const int MENUSLEEPTIME = 100;

        /*Texture2D YuiSpriteSheet; //the image for the running animation
        Texture2D attackSpriteSheet; //spritesheet for attacking
        Texture2D foxWalkingSpriteSheet;
        Texture2D foxIdleSpriteSheet;
        Texture2D yuiDeadSpriteSheet;*/
        YuiSprites sprites;
        EnemySprites enemySprites;
        EnemySprites bossSprites;

        Keys lastKey = 0; //store previous pressed keys

        //stuff for animation
        int frame;
        double timePerFrame = 100;
        int numFrames = 12;
        int framesElapsed;
        int timeSinceLastAttack;
        const int YUI_Y = 1;
        const int YUI_HEIGHT = 161;
        const int YUI_WIDTH = 139;
        const int YUI_X_OFFSET = 2;

        //punching bag stuff
        Texture2D bagSprite;

        //variable for testing purposes
        int once = 0;

        //variable for displaying items
        int inventoryIndex = -1;
        int equipIndex = -1;

        //fireball obj
        Fireball fireballObj;

        //list all the stuff that can be dropped in order of rareness
        List<Items> droppedItems = new List<Items>();

        #endregion

        #region GameTextures
        Texture2D healthBar;
        Texture2D manaBar;
        Texture2D lvlCircle;
        Texture2D barSurrounding;
        Texture2D upArrow;

        Texture2D pauseScreeninventorytab;
        Texture2D PauseScreeninventoryTabSelect;
        Texture2D PauseScreencharacterTab;
        Texture2D PauseScreencharacterTabSelect;
        Texture2D pauseScreenOptionsTab;
        Texture2D pauseScreenOptionsSelect;
        Texture2D pauseScreenBackGround;
        SpriteFont mainFont;

        Texture2D mainMenuBackgroun;
        Texture2D mainMenuCredits;
        Texture2D mainMenuCreditsSelect;
        Texture2D mainMenuExit;
        Texture2D mainMenuExitSelect;
        Texture2D mainMenuStart;
        Texture2D mainMenuStartSelect;
        Texture2D mainMenuHowTo;
        Texture2D mainMenuHowToSelect;
        
        Texture2D fireball;

        //items
        Texture2D food01;
        Texture2D potion01;
        Texture2D potion02;
        Texture2D potion03;
        Texture2D inventoryBox;
        Texture2D necklace01;
        Texture2D necklace02;
        Texture2D other01;
        Texture2D ring01;
        Texture2D ring02;
        Texture2D ring03;
        Texture2D talisman01;
        Texture2D talisman02;
        Texture2D soul;

        int manaregenDelay;
        #endregion

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            gameMode = 0;
            startMenuSelected = 0;
            pauseMenuSelected = 0;
            menuSelect.Add(0, "Start");
            menuSelect.Add(1, "How to Play");
            menuSelect.Add(2, "Credits");
            menuSelect.Add(3, "Quit");
            showCredits = false;
            showHow = false;

            pauseMenuSelect.Add(0, "Inventory");
            pauseMenuSelect.Add(1, "Character");
            pauseMenuSelect.Add(2, "Options");
            pauseMenuSelected = 0;

            optionSelect.Add(0, "Resume");
            optionSelect.Add(1, "Quit");
            optionSelected = 0;

            //LOAD SETTINGS//
            ReadSettings();
            //////////////////////
            Progress.playerLevel = 0;
            Progress.totalHealth = mh;
            Progress.totalMana = mm;
            Progress.currentHealth = sh;
            Progress.currentMana = sm;
            Progress.neededXp = 5 ^ Progress.playerLevel;
            Progress.currentXp = 0;
            Progress.inventory = new Items[25];
            Progress.skillpoints = sp;
            

            //set up equpied array
            //0 - necklace, 1 and 2 rings, 3 talisman
            Progress.equipped = new Items[4];

            manaregenDelay = 0;
            

            previousKState = Keyboard.GetState();
            previousMState = Mouse.GetState();

            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            backgroundArt = Content.Load<Texture2D>("backdrop2");
            bossroomArt = Content.Load<Texture2D>("bossroom");
            manaBar = Content.Load<Texture2D>("ManaBar");
            healthBar = Content.Load<Texture2D>("HealthBar");
            lvlCircle = Content.Load<Texture2D>("Gui/LevelCircle");
            barSurrounding = Content.Load<Texture2D>("Gui/HealthManaBar1");
            mainFont = Content.Load<SpriteFont>("mainFont");

            //pause menu
            pauseScreenBackGround = Content.Load<Texture2D>("GUI/MenuandTabs/Menu");
            PauseScreencharacterTab = Content.Load<Texture2D>("GUI/MenuandTabs/CharacterTab");
            PauseScreencharacterTabSelect = Content.Load<Texture2D>("GUI/MenuandTabs/CharacterTabSelect");
            pauseScreeninventorytab = Content.Load<Texture2D>("GUI/MenuandTabs/InventoryTab");
            PauseScreeninventoryTabSelect = Content.Load<Texture2D>("GUI/MenuandTabs/InventoryTabSelect");
            pauseScreenOptionsTab = Content.Load<Texture2D>("GUI/MenuandTabs/OptionsTab");
            pauseScreenOptionsSelect = Content.Load<Texture2D>("GUI/MenuandTabs/OptionsTabSelect");

            //main menu stuff
            mainMenuBackgroun = Content.Load<Texture2D>("GUI/MainMenuStuff/MainMenu");
            mainMenuCredits = Content.Load<Texture2D>("GUI/MainMenuStuff/Credits");
            mainMenuCreditsSelect = Content.Load<Texture2D>("GUI/MainMenuStuff/CreditsSelect");
            mainMenuExit = Content.Load<Texture2D>("GUI/MainMenuStuff/Exit");
            mainMenuExitSelect = Content.Load<Texture2D>("GUI/MainMenuStuff/ExitSelect");
            mainMenuHowTo = Content.Load<Texture2D>("GUI/MainMenuStuff/Howto");
            mainMenuHowToSelect = Content.Load<Texture2D>("GUI/MainMenuStuff/HowtoSelect");
            mainMenuStart = Content.Load<Texture2D>("GUI/MainMenuStuff/Start");
            mainMenuStartSelect = Content.Load<Texture2D>("GUI/MainMenuStuff/Startselect");

            // The sprites for Yui
            sprites.yuiIdle = this.Content.Load<Texture2D>("YuiIdleSpriteSheet");
            sprites.yuiRun = Content.Load<Texture2D>("RunningSpriteSheet");
            sprites.yuiMelee = this.Content.Load<Texture2D>("YuiMeleeAttack");
            sprites.foxRun = Content.Load<Texture2D>("FoxRunningSpriteSheet");
            sprites.foxIdle = Content.Load<Texture2D>("FoxIdleSpriteSheet");
            sprites.yuiDeath = Content.Load<Texture2D>("YuiDeathSpriteSheet");
            sprites.yuiRanged = Content.Load<Texture2D>("YuiRangedAttack");
            sprites.yuiJump = Content.Load<Texture2D>("YuiJumpSpriteSheet");
            sprites.yuiRun = Content.Load<Texture2D>("YuiRunningSpriteSheet");
            sprites.foxDeath = Content.Load<Texture2D>("FoxDeathSpriteSheet");
            sprites.foxJump = Content.Load<Texture2D>("FoxJumpingSpriteSheet");
            sprites.foxAttack = Content.Load<Texture2D>("FoxAttackSpriteSheet");

            enemySprites.Attack = Content.Load<Texture2D>("EnemyAttackSpriteSheet");
            enemySprites.Jump = Content.Load<Texture2D>("EnemyJumpSpriteSheet");
            enemySprites.Run = Content.Load<Texture2D>("EnemyRunningSpriteSheet");

            // the boss
            bossSprites.Run = Content.Load<Texture2D>("BossWalkingSpriteSheet");
            bossSprites.Attack = Content.Load<Texture2D>("BossAttackSpriteSheet");

            //the npc
            tanukiSprite = Content.Load<Texture2D>("TanukiSpriteSheet");

            bagSprite = Content.Load<Texture2D>("PunchingBag");
            upArrow = Content.Load<Texture2D>("UpArrow");
            platformSprite = Content.Load<Texture2D>("platforms");

            //load item textures
            food01 = Content.Load<Texture2D>("Items/Etc17");


            potion01 = Content.Load<Texture2D>("Items/Potion03");
            potion02 = Content.Load<Texture2D>("Items/Potion04");
            potion03 = Content.Load<Texture2D>("Items/Potion05");
            
            inventoryBox = Content.Load<Texture2D>("Items/InventoryBox");
            
            necklace01 = Content.Load<Texture2D>("Items/necklace2");
            necklace02 = Content.Load<Texture2D>("Items/Accessory04");
            
            other01 = Content.Load<Texture2D>("Items/Etc01");
            
            ring01 = Content.Load<Texture2D>("Items/Accessory02a");
            ring02 = Content.Load<Texture2D>("Items/Accessory02b");
            ring03 = Content.Load<Texture2D>("Items/Accessory01");

            talisman01 = Content.Load<Texture2D>("Items/Etc13");
            //talisman02 = Content.Load<Texture2D>("Items/Accessory03");

            soul = Content.Load<Texture2D>("Items/singleSoul");

            fireball = Content.Load<Texture2D>("Projectile1");

            //creating the player object and punching bag
            player = new Player(sprites.yuiIdle, sprites, new Vector2(100, 500), 150, 162, 191, 99, maxID);
            player.MaxHealth = mh;
            player.MaxMana = mm;
            player.Health = sh;
            player.Mana = sm;

            //adding them to the list of gameobjects
            gameObjects.Add(player);
            collidableObjects.Add(player);

            // the boss
            boss = new Boss(bagSprite, bossSprites, new Vector2(11694, 300), 512, 321, maxID++);
            gameObjects.Add(boss);
            collidableObjects.Add(boss);

            gameBackground.Load(GraphicsDevice, backgroundArt, bossroomArt, player);

            //give yui some items to test out inventory
            Progress.inventory[0] = new Items(food01, new Vector2(0, 0), 32, 32, maxID++, "Bread", "food");
            Progress.inventory[1] = new Items(potion02, new Vector2(0, 0), 32, 32, maxID++, "Potion of Healing", "food");
            Progress.inventory[2] = new Items(potion01, new Vector2(0, 0), 32, 32, maxID++, "Potion of Mana", "food");
            Progress.inventory[3] = new Items(potion03, new Vector2(0, 0), 32, 32, maxID++, "Ether potion", "food");

            Progress.inventory[4] = new Items(ring01, new Vector2(0, 0), 32, 32, maxID++, "Ring of Restoration", "ring");
            Progress.inventory[5] = new Items(ring02, new Vector2(0, 0), 32, 32, maxID++, "Ring of Mana", "ring");
            Progress.inventory[6] = new Items(ring03, new Vector2(0, 0), 32, 32, maxID++, "Ring of Fastness", "ring");

            Progress.inventory[7] = new Items(necklace02, new Vector2(0, 0), 32, 32, maxID++, "Necklace of Power", "necklace");
            Progress.inventory[8] = new Items(necklace02, new Vector2(0, 0), 32, 32, maxID++, "Necklace of Magic", "necklace");

            Progress.inventory[9] = new Items(other01, new Vector2(0, 0), 32, 32, maxID++, "Scroll of Knowledge", "talisman");
            //Progress.inventory[10] = new Items(other01, new Vector2(0, 0), 32, 32, maxID++, "Sharp Earrings", "talisman");

            droppedItems.Add(new Items(food01, new Vector2(0, 0), 32, 32, maxID++, "Bread", "food"));
            droppedItems.Add(new Items(potion02, new Vector2(0, 0), 32, 32, maxID++, "Potion of Healing", "food"));
            droppedItems.Add(Progress.inventory[2] = new Items(potion01, new Vector2(0, 0), 32, 32, maxID++, "Potion of Mana", "food"));
            droppedItems.Add(new Items(potion01, new Vector2(0, 0), 32, 32, maxID++, "Potion of Mana", "food"));
            droppedItems.Add(new Items(potion03, new Vector2(0, 0), 32, 32, maxID++, "Ether potion", "food"));
            droppedItems.Add(new Items(ring02, new Vector2(0, 0), 32, 32, maxID++, "Ring of Mana", "ring"));
            droppedItems.Add(new Items(necklace02, new Vector2(0, 0), 32, 32, maxID++, "Necklace of Magic", "necklace"));
            
            
            
            //setting the starting attributes to make sure they dont go too low
            Progress.startingAttack = player.MeleeDamage;
            Progress.startingHealthMax = player.MaxHealth;
            Progress.startingManaMax = player.MaxMana;
            Progress.startingSpeed = player.Speed;
            Progress.startingRngDmg = player.RangedDamage;
            Progress.checkpoint = 0;

            //create the fireball for YUI to use
            fireballObj = new Fireball(player, fireball, new Vector2(100, 100), 50, 50, maxID++);
            Add(fireballObj);


            //create platforms
            testPlatform = new Platform(platformSprite, new Vector2(500, 460), 144, 144, maxID++);
            Add(testPlatform);

            Platform1 = new Platform(platformSprite, new Vector2(1050, 460), 144, 144, maxID++);
            Add(Platform1);
            Platform2 = new Platform(platformSprite, new Vector2(1170, 460), 144, 144, maxID++);
            Add(Platform2);
            Platform3 = new Platform(platformSprite, new Vector2(1270, 460), 144, 144, maxID++);
            Add(Platform3);

            Platform4 = new Platform(platformSprite, new Vector2(2050, 460), 144, 144, maxID++);
            Add(Platform4);
            Platform5 = new Platform(platformSprite, new Vector2(2050, 340), 144, 144, maxID++);
            Add(Platform5);
            Platform6 = new Platform(platformSprite, new Vector2(2050, 220), 144, 144, maxID++);
            Add(Platform6);

            Platform7 = new Platform(platformSprite, new Vector2(2550, 460), 144, 72, maxID++);
            Add(Platform7);
            Platform8 = new Platform(platformSprite, new Vector2(2700, 460), 144, 72, maxID++);
            Add(Platform8);

            Platform9 = new Platform(platformSprite, new Vector2(3050, 340), 144, 72, maxID++);
            Add(Platform9);
            Platform10 = new Platform(platformSprite, new Vector2(3170, 220), 144, 72, maxID++);
            Add(Platform10);
            Platform11 = new Platform(platformSprite, new Vector2(3270, 100), 144, 72, maxID++);
            Add(Platform11);

            Platform13 = new Platform(platformSprite, new Vector2(3770, 460), 144, 72, maxID++);
            Add(Platform13);

            Platform14 = new Platform(platformSprite, new Vector2(4770, 460), 144, 72, maxID++);
            Add(Platform14);

            Platform15 = new Platform(platformSprite, new Vector2(5670, 460), 144, 144, maxID++);
            Add(Platform15);
            Platform16 = new Platform(platformSprite, new Vector2(5820, 400), 144, 144, maxID++);
            Add(Platform16);
            Platform17 = new Platform(platformSprite, new Vector2(5960, 400), 144, 144, maxID++);
            Add(Platform17);
            Platform18 = new Platform(platformSprite, new Vector2(6100, 460), 144, 144, maxID++);
            Add(Platform18);

            Platform19 = new Platform(platformSprite, new Vector2(6770, 460), 144, 144, maxID++);
            Add(Platform19);
            Platform20 = new Platform(platformSprite, new Vector2(6914, 460), 144, 144, maxID++);
            Add(Platform20);
            Platform21 = new Platform(platformSprite, new Vector2(7058, 430), 144, 72, maxID++);
            Add(Platform21);
            Platform22 = new Platform(platformSprite, new Vector2(7202, 430), 144, 72, maxID++);
            Add(Platform22);
            Platform22 = new Platform(platformSprite, new Vector2(7274, 500), 144, 72, maxID++);
            Add(Platform22);

            Platform23 = new Platform(platformSprite, new Vector2(8070, 460), 144, 144, maxID++);
            Add(Platform23);
            Platform24 = new Platform(platformSprite, new Vector2(8214, 340), 144, 144, maxID++);
            Add(Platform24);
            Platform25 = new Platform(platformSprite, new Vector2(8070, 220), 144, 144, maxID++);
            Add(Platform25);

            Platform26 = new Platform(platformSprite, new Vector2(10500, 460), 144, 144, maxID++);
            Add(Platform26);
            Platform27 = new Platform(platformSprite, new Vector2(10644, 460), 144, 144, maxID++);
            Add(Platform27);
            Platform28 = new Platform(platformSprite, new Vector2(10644, 340), 144, 144, maxID++);
            Add(Platform28);

            Platform29 = new Platform(platformSprite, new Vector2(11444, 460), 144, 144, maxID++);
            Add(Platform29);
            Platform30 = new Platform(platformSprite, new Vector2(11944, 460), 144, 144, maxID++);
            Add(Platform30);

            //create NPC
            tanuki = new NPC(tanukiSprite, new Vector2(1500, 460), 185, 162, maxID++);
            Add(tanuki);

            // TODO: use this.Content to load your game content here

            Progress.checkpoint = 0;
            Progress.endState = 0;
            

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            // Get the current keyboard state here
            kState = Keyboard.GetState();
            mState = Mouse.GetState();


            //YuiCollide = new CollidableObject(YuiHitbox);
            //punchBagCollide = new NPC(bagSprite, bagHitbox);

            //player died
            if ((player.IsDead))
            {
                //over the main menu button
                if ((mState.X >= 250) && (mState.X <= 250 + 200) && ((mState.Y >= 350) && (mState.Y <= 350 + 100)))
                {
                    if (mState.LeftButton == ButtonState.Pressed)
                    {
                        Process.Start("YuiGame.exe");
                        Exit();
                    }
                }
                if ((mState.X >= 450) && (mState.X <= 450 + 200) && ((mState.Y >= 350) && (mState.Y <= 350 + 100)))
                {
                    if (mState.LeftButton == ButtonState.Pressed)
                    {
                        Exit();
                    }
                }
            }
            if (Progress.checkpoint == 2 && Progress.endState == 1)
            {
                //over the main menu button
                if ((mState.X >= 250) && (mState.X <= 250 + 400) && ((mState.Y >= 350) && (mState.Y <= 350 + 100)))
                {
                    if (mState.LeftButton == ButtonState.Pressed)
                    {
                        Progress.choice = 0;
                        Progress.endState = 2;
                    }
                }
                if ((mState.X >= 450) && (mState.X <= 450 + 400) && ((mState.Y >= 350) && (mState.Y <= 350 + 100)))
                {
                    Progress.choice = 1;
                    Progress.endState = 2;
                }
            }
            
            if (gameMode == 0)
            {
                startMenu();
            }
            else if (gameMode == 1)
            {


                //dialoge
                if (isTalking == true)
                {
                    if (kState.IsKeyUp(Keys.Enter) && previousKState.IsKeyDown(Keys.Enter))
                    {
                        if (phraseIndex >= phrases.Count)
                        {
                            isTalking = false;
                        }
                        if (phraseIndex < phrases.Count)
                        {
                            currentPhrase = phrases[phraseIndex];
                            phraseIndex++;
                        }
                    }
                }
                if (!isTalking)
                {
                    if (player.Face != yuiState.Dead)
                    {
                        if (Progress.checkpoint == 0)
                        {
                            //A call to the input method does wonders to your game
                            Input();

                            if (once == 0)
                            {
                                say("Test.txt");
                                once = 1;
                            }

                            //get mana back after a few ticks
                            if (manaregenDelay < 10)
                            {
                                manaregenDelay += 1;
                            }

                            if (manaregenDelay >= 10)
                            {
                                player.Mana += Convert.ToInt32(Math.Floor(player.MaxMana / 100.0));
                                manaregenDelay = 0;
                            }

                            if (player.Health <= 0)
                            {
                                player.Face = yuiState.Dead;
                                player.Frame = 0;
                            }

                            //Spawn, delete, implement AI, and basically just manage enemies
                            ManageEnemies();

                            foreach (GameObject obj in gameObjects)
                            {
                                if (!(obj is Player) && !(obj is Enemy))
                                {
                                    obj.Gravity();
                                }
                                if (obj is Boss)
                                    obj.Gravity();
                            }

                            foreach (Enemy enemy in enemies)
                            {
                                if (!enemy.Jumping)
                                {
                                    enemy.Gravity();
                                }
                            }
                            if (!player.Jumping)
                                player.Gravity();
                            if (gameBackground.CameraX > 11260)
                                Progress.checkpoint = 1;

                            gameBackground.Update(gameObjects);
                        }
                        else if(Progress.checkpoint==1)
                        {
                            //A call to the input method does wonders to your game
                            Input();

                            //get mana back after a few ticks
                            if (manaregenDelay < 10)
                            {
                                manaregenDelay += 1;
                            }

                            if (manaregenDelay >= 10)
                            {
                                player.Mana += Convert.ToInt32(Math.Floor(player.MaxMana / 100.0));
                                manaregenDelay = 0;
                            }

                            if (player.Health <= 0)
                            {
                                player.Face = yuiState.Dead;
                                player.Frame = 0;
                            }

                            //Spawn, delete, implement AI, and basically just manage enemies
                            ManageEnemies();
                            boss.EnemyAI(player);
                            if(boss.Health<=0)
                            {
                                gameObjects.Remove(boss);
                                collidableObjects.Remove(boss);
                                for (int i = 1; i < enemies.Count; i++)
                                    Delete(enemies[i].ObjectID);
                                    Progress.checkpoint = 2;
                            }

                            foreach (GameObject obj in gameObjects)
                            {
                                if (!(obj is Player) && !(obj is Enemy))
                                {
                                    obj.Gravity();
                                }
                                if (obj is Boss)
                                    obj.Gravity();
                            }

                            foreach (Enemy enemy in enemies)
                            {
                                if (!enemy.Jumping)
                                {
                                    enemy.Gravity();
                                }
                            }
                            if (!player.Jumping)
                                player.Gravity();

                            gameBackground.Update(gameObjects);
                        }
                        else if(Progress.checkpoint==2)
                        {
                            if (Progress.endState == 0)
                            {
                                say("EndChoice.txt");
                                Progress.endState = 1;
                            }
                            if(Progress.endState==2)
                            {
                                if (Progress.choice == 1)
                                {
                                    say("ChoiceGood.txt");
                                    Thread.Sleep(10000);
                                    Exit();
                                }
                                else
                                {
                                    say("ChoiceBad.txt");
                                    Thread.Sleep(10000);
                                    Exit();
                                }
                            }
                        }
                        CheckCollisions();

                        foreach (CollidableObject obj in collidableObjects)
                        {
                            obj.Collide();
                        }

                        foreach (GameObject obj in gameObjects)
                        {
                            obj.PreviousPosition = obj.Position;
                        }
                    }
                }
            }
            else if (gameMode == 2)
            {
                pauseMenu();
            }

            // Calculate the frame to draw based on the time
            //  - The variable "frame" will contain either 1, 2 or 3 (the "walking" frames)
            //  - Frame 0 is the "standing" frame
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames + 1;

            /*//check for collisions
            YuiCollide.IsColliding(bagHitbox);
            YuiCollide.WalkInto(bagHitbox);*/

            previousKState = kState; //store the previous state
            previousMState = mState;

            //update fireball location
            fireballObj.Move();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //checks to see if object is always drawn
            //compares the location of the object being drawn onthe map to the location of the camera
            //calls the draw method

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();


            //menu screen
            if (gameMode == 0)
            {
                spriteBatch.Draw(mainMenuBackgroun, new Rectangle(0, 0, 834, 620), Color.White);
                if (showCredits == false && showHow == false)
                {
                    for (int i = 0; i <= 3; i++)
                    {
                        if (i == startMenuSelected)
                        {
                            switch (i)
                            {
                                case 0:
                                    spriteBatch.Draw(mainMenuStartSelect, new Rectangle(100, 255, 211, 55), Color.White);
                                    break;
                                case 1:
                                    spriteBatch.Draw(mainMenuHowToSelect, new Rectangle(150, 350, 211, 55), Color.White);
                                    break;
                                case 2:
                                    spriteBatch.Draw(mainMenuCreditsSelect, new Rectangle(250, 425, 211, 55), Color.White);
                                    break;
                                case 3:
                                    spriteBatch.Draw(mainMenuExitSelect, new Rectangle(300, 500, 211, 55), Color.White);
                                    break;
                            }
                        }
                        else
                        {
                            switch (i)
                            {
                                case 0:
                                    spriteBatch.Draw(mainMenuStart, new Rectangle(100, 255, 211, 55), Color.White);
                                    break;
                                case 1:
                                    spriteBatch.Draw(mainMenuHowTo, new Rectangle(150, 350, 211, 55), Color.White);
                                    break;
                                case 2:
                                    spriteBatch.Draw(mainMenuCredits, new Rectangle(250, 425, 211, 55), Color.White);
                                    break;
                                case 3:
                                    spriteBatch.Draw(mainMenuExit, new Rectangle(300, 500, 211, 55), Color.White);
                                    break;
                            }
                        }
                    }
                }
                //draws the credits
                else if (showCredits == true)
                {
                    spriteBatch.DrawString(mainFont, "Team Foxtrot", new Vector2(400, 250), Color.Black);
                    spriteBatch.DrawString(mainFont, "Kash Mutt", new Vector2(400, 350), Color.Black);
                    spriteBatch.DrawString(mainFont, "Nick Tancredi", new Vector2(400, 400), Color.Black);
                    spriteBatch.DrawString(mainFont, "L.a. Stapleford", new Vector2(400, 450), Color.Black);
                    spriteBatch.DrawString(mainFont, "Steven Siewert", new Vector2(400, 500), Color.Black);
                }
                else if (showHow == true)
                {
                    spriteBatch.DrawString(mainFont, "A to move left", new Vector2(400, 50), Color.Black);
                    spriteBatch.DrawString(mainFont, "D to move right", new Vector2(400, 100), Color.Black);
                    spriteBatch.DrawString(mainFont, "Left click to melee attack", new Vector2(400, 150), Color.Black);
                    spriteBatch.DrawString(mainFont, "Right click for ranged attack", new Vector2(400, 200), Color.Black);
                    spriteBatch.DrawString(mainFont, "Ctrl to change form", new Vector2(400, 250), Color.Black);
                    spriteBatch.DrawString(mainFont, "Enter to interact", new Vector2(400, 300), Color.Black);
                    spriteBatch.DrawString(mainFont, "Esc to pause, level up,", new Vector2(400, 350), Color.Black);
                    spriteBatch.DrawString(mainFont, "and access inventory", new Vector2(400, 380), Color.Black);
                    spriteBatch.DrawString(mainFont, "Space to jump", new Vector2(400, 430), Color.Black);
                    spriteBatch.DrawString(mainFont, "Alt to drop from platform", new Vector2(400, 470), Color.Black);

                }
            }

            //in the actual game
            if (gameMode == 1)
            {
                
                //drawing the background
                gameBackground.Draw(spriteBatch);

                // draw all the game objects
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    gameObjects[i].Draw(spriteBatch, gameTime);
                }

                if (isTalking == true)
                {
                    spriteBatch.DrawString(mainFont, talkName + "\n" + currentPhrase, new Vector2(30, 400), Color.White);
                }


                #region HUD
                spriteBatch.Draw(lvlCircle, new Rectangle(20, 20, 64, 64), Color.White);
                spriteBatch.DrawString(mainFont, "" + Progress.playerLevel, new Vector2(46, 36), Color.Gold);
                spriteBatch.Draw(healthBar, new Rectangle(84, 20, 320 * player.Health / player.MaxHealth, 32), Color.White);
                spriteBatch.Draw(manaBar, new Rectangle(84, 60, 320 * player.Mana / player.MaxMana, 32), Color.White);
                spriteBatch.Draw(barSurrounding, new Rectangle(84, 20, 320, 32), Color.White);
                spriteBatch.Draw(barSurrounding, new Rectangle(84, 60, 320, 32), Color.White);
                #endregion

                //if player dies
                if ((player.IsDead))
                {
                    spriteBatch.DrawString(mainFont, "You have died.", new Vector2(300, 300), Color.White);
                    spriteBatch.DrawString(mainFont, "Main Menu", new Vector2(250, 350), Color.White);
                    spriteBatch.DrawString(mainFont, "Exit", new Vector2(450, 350), Color.White);
                }
                if(Progress.endState==1)
                {
                    spriteBatch.DrawString(mainFont, "What do you wanna do?", new Vector2(300, 300), Color.White);
                    spriteBatch.DrawString(mainFont, "Take it", new Vector2(250, 350), Color.White);
                    spriteBatch.DrawString(mainFont, "Noooo!", new Vector2(450, 350), Color.White);
                }
            }

            #region PauseMenuDraw
            if (gameMode == 2)
            {
                spriteBatch.Draw(pauseScreenBackGround, new Rectangle(0, 0, 834, 620), Color.White);
                for (int i = 0; i <= 2; i++)
                {
                    if (i == pauseMenuSelected)
                    {
                        switch (i)
                        {
                            case 0:
                                spriteBatch.Draw(PauseScreeninventoryTabSelect, new Rectangle(80, 50, 211, 55), Color.White);
                                break;
                            case 1:
                                spriteBatch.Draw(PauseScreencharacterTabSelect, new Rectangle(310, 50, 211, 55), Color.White);
                                break;
                            case 2:
                                spriteBatch.Draw(pauseScreenOptionsSelect, new Rectangle(540, 50, 211, 55), Color.White);
                                break;
                        }
                    }
                    else
                    {
                        switch (i)
                        {
                            case 0:
                                spriteBatch.Draw(pauseScreeninventorytab, new Rectangle(80, 50, 211, 55), Color.White);
                                break;
                            case 1:
                                spriteBatch.Draw(PauseScreencharacterTab, new Rectangle(310, 50, 211, 55), Color.White);
                                break;
                            case 2:
                                spriteBatch.Draw(pauseScreenOptionsTab, new Rectangle(540, 50, 211, 55), Color.White);
                                break;
                        }
                    }
                }

                //inventory screen
                if (pauseMenuSelected == 0)
                {

                    //draw inventory squares
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            spriteBatch.Draw(inventoryBox, new Rectangle(250 + 84 * i, 150 + 84 * j, 64, 64), Color.White);
                            //draw inventory sqaure
                            if (Progress.inventory[(i + 5 * j)] != null)
                            {
                                spriteBatch.Draw(Progress.inventory[(i + 5 * j)].Image, new Rectangle(257 + 84 * i, 157 + 84 * j, 50, 50), Color.White);
                            }
                        }
                    }

                    //draw the description and name of current object in the inventory
                    if (inventoryIndex > -1)
                    {
                        if (Progress.inventory[inventoryIndex] != null)
                        {
                            spriteBatch.DrawString(mainFont, Progress.inventory[inventoryIndex].Name + "\n" + Progress.inventory[inventoryIndex].Description, new Vector2(mState.X + 10, mState.Y), Color.Black);
                        }
                    }

                    //draw equiped items
                    for (int i = 0; i < 4; i++)
                    {
                        spriteBatch.Draw(inventoryBox, new Rectangle(100, 150 + 84 * i, 64, 64), Color.White);
                        if (Progress.equipped[i] != null)
                        {
                            spriteBatch.Draw(Progress.equipped[i].Image, new Rectangle(107, 157 + 84 * i, 50, 50), Color.White);
                        }
                    }

                    //draw the description and name of current object in the euipped
                    if (equipIndex > -1)
                    {
                        if (Progress.equipped[equipIndex] != null)
                        {
                            spriteBatch.DrawString(mainFont, Progress.equipped[equipIndex].Name + "\n" + Progress.equipped[equipIndex].Description, new Vector2(mState.X + 10, mState.Y), Color.Black);
                        }
                    }

                }
                //character screen
                if (pauseMenuSelected == 1)
                {
                    //draws health
                    spriteBatch.DrawString(mainFont, "Health " + player.Health + " / " + player.MaxHealth, new Vector2(20, 190), Color.Red);
                    spriteBatch.Draw(healthBar, new Rectangle(20, 220, 320 * player.Health / player.MaxHealth, 32), Color.White);
                    spriteBatch.Draw(barSurrounding, new Rectangle(20, 220, 320, 32), Color.White);

                    //draws mana
                    spriteBatch.DrawString(mainFont, "Mana " + player.Mana + " / " + player.MaxMana, new Vector2(20, 290), Color.Blue);
                    spriteBatch.Draw(manaBar, new Rectangle(20, 320, 320 * player.Mana / player.MaxMana, 32), Color.White);
                    spriteBatch.Draw(barSurrounding, new Rectangle(20, 320, 320, 32), Color.White);

                    //draws exp
                    spriteBatch.DrawString(mainFont, "XP " + Progress.currentXp + " / " + Progress.neededXp, new Vector2(20, 390), Color.Green);
                    spriteBatch.Draw(manaBar, new Rectangle(20, 420, 320 * Progress.currentXp / Progress.neededXp, 32), Color.Green);
                    spriteBatch.Draw(barSurrounding, new Rectangle(20, 420, 320, 32), Color.White);

                    //draw skillpoints
                    spriteBatch.DrawString(mainFont, "Skillpoints: " + Progress.skillpoints, new Vector2(20, 490), Color.White);

                    //draw all of the attributes
                    spriteBatch.DrawString(mainFont, "Attack: " + player.MeleeDamage, new Vector2(400, 125), Color.White);
                    spriteBatch.DrawString(mainFont, "Speed: " + player.Speed, new Vector2(400, 225), Color.White);
                    spriteBatch.DrawString(mainFont, "Health: " + player.MaxHealth, new Vector2(400, 325), Color.White);
                    spriteBatch.DrawString(mainFont, "Mana: " + player.MaxMana, new Vector2(400, 425), Color.White);
                    spriteBatch.DrawString(mainFont, "Intellegence: " + player.RangedDamage, new Vector2(400, 525), Color.White);
                    //drawing the arrows for leveling
                    if (Progress.skillpoints > 0)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            spriteBatch.Draw(upArrow, new Rectangle(650, (125 + (100 * i)), 15, 15), Color.White);
                        }
                    }
                    //draw the down arrows
                    for (int i = 0; i < 5; i++)
                    {
                        spriteBatch.Draw(upArrow, new Rectangle(650, (145 + (100 * i)), 15, 15), null, Color.White
                            , 0f, new Vector2(0, 0), SpriteEffects.FlipVertically, 0f);
                    }


                }
                //options screen
                if (pauseMenuSelected == 2)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        if (i == optionSelected) spriteBatch.DrawString(mainFont, optionSelect[i], new Vector2(300, 200 + i * 100), Color.White);
                        else spriteBatch.DrawString(mainFont, optionSelect[i], new Vector2(300, 200 + i * 100), Color.Black);
                    }
                }
            }
            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }

        //method to check collisions?
        protected void CheckCollisions()
        {
            foreach(CollidableObject obj1 in collidableObjects)
            {
                obj1.CollidesWith.Clear();
                foreach(CollidableObject obj2 in collidableObjects)
                {
                    if (obj1.IsColliding(obj2))
                    {
                        obj1.CollidesWith.Add(obj2);
                    }
                }
            }
            //check for fireball colissions
            if (fireballObj.Active == true)
            {
                //game object is player fireball
                for (int i = 1; i < collidableObjects.Count; i++)
                {
                    if (collidableObjects[i] is NPC)
                    {
                        NPC obj1 = (NPC)collidableObjects[i];
                        if (fireballObj.IsColliding(obj1))
                        {
                            obj1.Health -= player.RangedDamage;
                            fireballObj.Active = false;
                        }
                    }
                }
            }
            for (int i = 0; i < collidableObjects.Count; i++)
            {
                //make sure the collidable object is an item
                if (collidableObjects[i] is Items)
                {
                    Items obj = (Items)collidableObjects[i];
                    //check to see if yui is colliding with it
                    if (obj.IsColliding(collidableObjects[0]))
                    {
                        //find an empty spot in inventory
                        if (obj.Type == "soul")
                        {
                            obj.activate(player);
                            Delete(obj.ObjectID);
                        }
                        else
                        {
                            for (int j = 0; j < 25; j++)
                            {
                                if (Progress.inventory[j] == null)
                                {
                                    Progress.inventory[j] = obj;
                                    Delete(obj.ObjectID);
                                    break;
                                }
                            }
                        }
                        
                    }
                }
            }
        }

        // the parameterized overloaded version of it (or whatever you wanna call it)
        private void CheckCollisions(CollidableObject obj1)
        {
            obj1.CollidesWith.Clear();
            foreach (CollidableObject obj2 in collidableObjects)
            {
                if (obj1.IsColliding(obj2))
                {
                    obj1.CollidesWith.Add(obj2);
                }
            }
        }

        //method to manage enemies
        protected void ManageEnemies()
        {
            Random rand = new Random();


            if(rand.Next(400)==0) // a 1/100th chance that an enemy will spawn.... might adjust
            {
                // making sure the enemy is inside the screen
                int x = rand.Next(graphics.PreferredBackBufferWidth - 160) + 80;
                int y = 450;

                Vector2 enemyPosition = new Vector2(x, y);
                if (Vector2.Distance(enemyPosition, player.Position) > 20) //only spawn if the enemy is 20 pixels away
                {
                        // finally, create the enemy
                        Add(new Enemy(bagSprite, enemySprites, enemyPosition, 161, 161, maxID++));
                }
            }

            for(int i=0;i<enemies.Count;) // go through each enemy
            {
                if (enemies[i].Health <= 0) //if the enemy is out of health
                {
                    if(true) // chances of enemy dropping the item
                    {
                        Items item = new Items(soul, enemies[i].Position, 32, 32, maxID++, "Small Soul", "soul");
                        for (int j = droppedItems.Count - 1; j >= 0; j--)
                        {
                            double chance = ((j+1.0)/Convert.ToDouble(droppedItems.Count)) * 100.0;
                            if (rand.Next(-20, 101) >= chance)
                            {
                                string itemName = droppedItems[j].Name;
                                string itemType = droppedItems[j].Type;
                                Texture2D image = droppedItems[j].Image;
                                Items tempItem = new Items(image, new Vector2 (enemies[i].Position.X + 10, enemies[i].Position.Y), 32, 32, maxID++, itemName, itemType);
                                Add(tempItem);
                                //only drop one item
                                break;
                            }
                        }
                        Add(item);
                    }
                    Delete(enemies[i].ObjectID); // get rid of it
                    
                }
                else
                {
                    enemies[i].EnemyAI(player); //implement the AI (need to work on that)
                    i++;
                }
            }

        }
        

        //method to act based on input
        protected void Input()
        {

            Keys[] pressedKeys = kState.GetPressedKeys();

            if (gameMode == 1)
            {

                
                //character levels up
                if (Progress.currentXp >= Progress.neededXp)
                {
                    Progress.playerLevel++;
                    Progress.skillpoints += 5;
                    Progress.neededXp = (5 ^ Progress.playerLevel) + 4 * Progress.playerLevel;
                }

                if (kState.GetPressedKeys().Length == 0) //if nothing was pressed
                {
                    if (lastKey == Keys.A) //if last pressed key was left, keep facing left
                    {

                        if (player.Face != yuiState.AttackLeft && player.Face != yuiState.RangedLeft)
                            player.Face = yuiState.FaceLeft;
                    }
                    if (lastKey == Keys.D) //if last pressed key was right, keep facing right
                    {

                        if (player.Face != yuiState.AttackRight && player.Face != yuiState.RangedRight)
                            player.Face = yuiState.FaceRight;
                    }

                }
                else
                {
                    //check pressed keys
                    for (int i = 0; i < pressedKeys.Length; i++)
                    {
                        //if escape key is pressed open the pause menu
                        if (pressedKeys[i] == Keys.Escape)
                        {
                            gameMode = 2;
                        }

                        if (pressedKeys[i] == Keys.A) //if left key is pressed, player.Face left
                        {
                            if (player.Face != yuiState.AttackLeft && player.Face != yuiState.RangedLeft)
                            player.Face = yuiState.FaceLeft;
                        }
                        else if (pressedKeys[i] == Keys.D) //if right key is pressed, player.Face right
                        {
                            if (player.Face != yuiState.AttackRight && player.Face != yuiState.RangedRight)
                            player.Face = yuiState.FaceRight;
                        }
                        else if (pressedKeys[i] == Keys.Enter) //if Enter is pressed, try to interact with something
                        {
                            if (player.IsColliding(tanuki))
                            {
                                if (isTalking == false)
                                {
                                    say("TanukiDialogue1.txt");
                                }
                            }
                        }

                        if (lastKey != 0) //if not the first time key has been pressed
                        {
                            if (pressedKeys[i] != lastKey) //if the current pressed key doesn't match the last key that was pressed
                            {
                                if (pressedKeys[i] == Keys.A) //change the direction Yui's facing to left
                                {
                                    if (player.Face != yuiState.AttackLeft && player.Face != yuiState.RangedLeft)
                                    player.Face = yuiState.FaceLeft;
                                }
                                else if (pressedKeys[i] == Keys.D) //change the direction Yui's facing to right
                                {
                                    if (player.Face != yuiState.AttackRight && player.Face != yuiState.RangedRight)
                                    player.Face = yuiState.FaceRight;
                                }

                            }
                            else if (pressedKeys[i] == lastKey) //if the current pressed key does match the last key that was pressed
                            {
                                if (pressedKeys[i] == Keys.A) //make Yui walk to the left
                                {
                                    if (player.Face != yuiState.AttackLeft && player.Face != yuiState.RangedLeft)
                                    player.Face = yuiState.WalkLeft;
                                    /*if (player.Position.X <= 80)
                                    {
                                        gameBackground.ScrollLeft(player.Speed);
                                    }*/
                                }
                                else if (pressedKeys[i] == Keys.D) //make Yui walk to the right
                                {
                                    if (player.Face != yuiState.AttackRight && player.Face != yuiState.RangedRight)
                                    player.Face = yuiState.WalkRight;
                                    /*if (player.Position.X > 700)
                                    {
                                        gameBackground.ScrollRight(player.Speed);
                                    }*/
                                }
                            }
                        }


                        if (mState.LeftButton != ButtonState.Pressed)
                        {
                            if (player.Face == yuiState.WalkLeft) //if Yui is walking to the left, decrease her X location
                            {
                                if (player.Position.X <= 80)//keep yui from going off the screen
                                {
                                    player.Move(0, 0);
                                }
                                else
                                {
                                    player.Move(-player.Speed, 0);
                                }

                            }
                            else if (player.Face == yuiState.WalkRight) //if Yui is walking to the right, increase her X location
                            {
                                if (player.Position.X >= 720)//keep yui from going off the screen
                                {
                                    player.Move(0, 0);
                                }
                                else
                                {
                                    player.Move(player.Speed, 0);
                                }
                            }
                        }




                        //store the current pressed key as the last key that was pressed
                        lastKey = pressedKeys[i];

                    }
                }

                if (kState.IsKeyUp(Keys.LeftControl) && previousKState.IsKeyDown(Keys.LeftControl))
                {
                    player.ChangeForm();
                }

                if (mState.LeftButton == ButtonState.Pressed
                    && previousMState.LeftButton != ButtonState.Pressed) //if the left mouse buton is pressed
                {
                    foreach (Enemy enemy in enemies) //for every enemy
                    {
                        player.Attack(enemy); //attack
                    }
                    player.Attack(boss);
                    player.Attack();
                }
                
                //shoots a fireball
                if (mState.RightButton == ButtonState.Pressed)
                {
                    if (player.IsHuman && previousMState.RightButton != ButtonState.Pressed)
                    {
                        if (fireballObj.Active == false &&
                            player.Face != yuiState.RangedLeft && player.Face != yuiState.RangedRight)
                        {
                            if (player.Mana >= 50)
                            {
                                if (player.Face == yuiState.FaceLeft || player.Face == yuiState.WalkLeft)
                                    player.Face = yuiState.RangedLeft;
                                else if (player.Face == yuiState.FaceLeft || player.Face == yuiState.WalkLeft)
                                    player.Face = yuiState.RangedRight;
                                player.Mana -= 50;
                                fireballObj.fire();
                            }
                        }
                    }
                    else if (!player.IsHuman &&
                        (player.Face == yuiState.WalkLeft || player.Face == yuiState.WalkRight))
                    {
                        if (player.Mana >= 20)
                            player.Stun = true;
                        else
                            player.Stun = false;
                    }
                    else
                        player.Stun = false;
                }
                else
                    player.Stun = false;

                //ranged attack - will switch to right mouse button
                /*
                if (kState.IsKeyDown(Keys.F))
                {
                    player.Fire(); //ranged attack
                }
                 */

                //jumping stuff
                if (player.Jumping)
                {
                    player.Jump();
                    if (kState.IsKeyDown(Keys.A))
                        player.Move(-player.Speed, 0);
                    if (kState.IsKeyDown(Keys.D))
                        player.Move(player.Speed, 0);
                }
                else
                {
                    if (kState.IsKeyDown(Keys.Space))
                    {
                        player.Jump(-17);
                    }
                    if(kState.IsKeyDown(Keys.LeftAlt))
                    {
                        player.PlatformCollisions = false;
                    }
                    else
                    {
                        player.PlatformCollisions = true;
                    }
                }
            }
        }

        protected void startMenu()
        {
            if (showCredits == false && showHow == false)
            {
                //moves what is currently selected 
                if (((kState.IsKeyDown(Keys.W)) || (kState.IsKeyDown(Keys.Up)))
                    && (previousKState.IsKeyUp(Keys.W) && previousKState.IsKeyUp(Keys.Up)))
                {
                    if (startMenuSelected == 0) startMenuSelected = 4;
                    else startMenuSelected--;
                }
                else if (((kState.IsKeyDown(Keys.S)) || (kState.IsKeyDown(Keys.Down)))
                    && (previousKState.IsKeyUp(Keys.S) && previousKState.IsKeyUp(Keys.Down)))
                {
                    if (startMenuSelected == 4) startMenuSelected = 0;
                    else startMenuSelected++;
                }
                else if (kState.IsKeyDown(Keys.Enter))
                {
                    switch (startMenuSelected)
                    {
                        //play game
                        case 0:
                            gameMode = 1;
                            break;
                        case 1:
                            showHow = true;
                            break;
                        //credits
                        case 2:
                            showCredits = true;
                            break;
                        //exit Game
                        case 3:
                            Exit();
                            break;
                    }
                }
            }
            if (showCredits == true)
            {
                if (kState.IsKeyDown(Keys.Escape)) showCredits = false;
            }
            if (showHow == true)
            {
                if (kState.IsKeyDown(Keys.Escape)) showHow = false;
            }
        }

        protected void pauseMenu()
        {
            if (((kState.IsKeyDown(Keys.D)) || (kState.IsKeyDown(Keys.Right)))
                && (previousKState.IsKeyUp(Keys.D) && previousKState.IsKeyUp(Keys.Right)))
            {
                if (pauseMenuSelected == 2) pauseMenuSelected = 0;
                else pauseMenuSelected++;
            }
            if (((kState.IsKeyDown(Keys.A)) || (kState.IsKeyDown(Keys.Left)))
                && (previousKState.IsKeyUp(Keys.A) && previousKState.IsKeyUp(Keys.Left)))
            {
                if (pauseMenuSelected == 0) pauseMenuSelected = 2;
                else pauseMenuSelected--;
            }

            //inventory menu
            if (pauseMenuSelected == 0)
            {
                #region inventoryItems
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        //mouse is over something in the inventory
                        if ((mState.X >= 250 + 84 * i) && (mState.X <= 314 + 84 * i) && ((mState.Y >= 150 + 84 * j) && (mState.Y <= 214 + 84 * j)))
                        {
                            //makes sure the thing it is over is not empty
                            inventoryIndex = i + 5 * j;
                            if (Progress.inventory[inventoryIndex] != null)
                            {
                                //player clicks on the item
                                if (mState.LeftButton == ButtonState.Pressed && previousMState.LeftButton != ButtonState.Pressed)
                                {
                                    if (Progress.inventory[inventoryIndex].Type == "food")
                                    {
                                        Progress.inventory[inventoryIndex].activate(player);
                                        Progress.inventory[inventoryIndex] = null;
                                    }

                                    else if (Progress.inventory[inventoryIndex].Type == "necklace")
                                    {
                                        //need a temp item in the special case that inventory is full and an object is already equipped
                                        Items tempItem = Progress.inventory[inventoryIndex];
                                        Progress.inventory[inventoryIndex] = null;

                                        //special case - there is already an object there
                                        if (Progress.equipped[0] != null)
                                        {
                                            Progress.equipped[0].unEquip(player);
                                            
                                            //go through inventory until an empty spot is found
                                            for (int s = 0; s < 25; s++)
                                            {
                                                if (Progress.inventory[s] == null)
                                                {
                                                    Progress.inventory[s] = Progress.equipped[0];
                                                    break;
                                                }
                                            }
                                        }

                                        Progress.equipped[0] = tempItem;
                                        tempItem.equip(player);

                                    }
                                    //object is a ring
                                    else if (Progress.inventory[inventoryIndex].Type == "ring")
                                    {
                                        Items tempItem = Progress.inventory[inventoryIndex];
                                        Progress.inventory[inventoryIndex] = null;

                                        //there is no ring in the first slot
                                        if (Progress.equipped[1] == null)
                                        {
                                            Progress.equipped[1] = tempItem;
                                            tempItem.equip(player);
                                            
                                        }

                                        //there is nothing in the second ring slot
                                        else if (Progress.equipped[2] == null)
                                        {
                                            Progress.equipped[2] = tempItem;
                                            tempItem.equip(player);
                                        }

                                        //both slots are filled
                                        else
                                        {
                                            //go through inventory until an empty spot is found
                                            for (int s = 0; s < 25; s++)
                                            {
                                                if (Progress.inventory[s] == null)
                                                {
                                                    Progress.inventory[s] = Progress.equipped[2];
                                                    Progress.equipped[2].unEquip(player);
                                                    Progress.equipped[2] = Progress.equipped[1];
                                                    Progress.equipped[1] = tempItem;
                                                    tempItem.equip(player);
                                                    break;
                                                }
                                            }

                                        }

                                    }
                                    //object is a talisman
                                    else if (Progress.inventory[inventoryIndex].Type == "talisman")
                                    {
                                        //need a temp item in the special case that inventory is full and an object is already equipped
                                        Items tempItem = Progress.inventory[inventoryIndex];
                                        Progress.inventory[inventoryIndex] = null;

                                        //special case - there is already an object there
                                        if (Progress.equipped[3] != null)
                                        {
                                            Progress.equipped[3].unEquip(player);

                                            //go through inventory until an empty spot is found
                                            for (int s = 0; s < 25; s++)
                                            {
                                                if (Progress.inventory[s] == null)
                                                {
                                                    Progress.inventory[s] = Progress.equipped[3];
                                                    break;
                                                }
                                            }
                                        }

                                        Progress.equipped[3] = tempItem;
                                        tempItem.equip(player);
                                    }
                                }
                                //dropping stuff
                                if (mState.RightButton == ButtonState.Pressed && previousMState.RightButton != ButtonState.Pressed)
                                {
                                    Progress.inventory[inventoryIndex] = null;
                                }

                            }
                        }
                        
                    }
                }
                if (((mState.X >= 250) && (mState.X <= 314 + 84 * 4) && ((mState.Y >= 150) && (mState.Y <= 214 + 84 * 4))) == false)
                {
                    inventoryIndex = -1;
                }
                #endregion
                #region equipped Items
                for (int i = 0; i < 4; i++)
                {
                    //check to see if mouse is over an item
                    if ((mState.X >= 100 && mState.X <= 164) && (mState.Y >= (150 + 84 * i) && mState.Y <= (214 + 84 * i)))
                    {
                        equipIndex = i;
                        if (mState.LeftButton == ButtonState.Pressed && previousMState.LeftButton != ButtonState.Pressed)
                        {
                            //check to see if the index is valid
                            if (Progress.equipped[equipIndex] != null)
                            {
                                for (int j = 0; j < Progress.inventory.Count(); j++)
                                {
                                    //check if the inventory isnt full
                                    if (Progress.inventory[j] == null)
                                    {
                                        Progress.equipped[equipIndex].unEquip(player);
                                        Progress.inventory[j] = Progress.equipped[equipIndex];
                                        Progress.equipped[equipIndex] = null;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                //sets the index to -1 if the mouse is not over the inventory
                if ((mState.X >= 100 && mState.X <= 164) == false || (mState.Y >= (150) && mState.Y <= (214 + 84 * 3)) == false)
                {
                    equipIndex = -1;
                }
                #endregion
            }
            
            
            
            //level up screen
            if (pauseMenuSelected == 1)
            {
                //up arrows
                for (int i = 0; i < 5; i++)
                {
                    //checks if player has skillpoints
                    if (Progress.skillpoints > 0)
                    {
                        //checks the location of the mouse to see if it is on a button
                        if ((mState.X >= 650 && mState.X <= 665) && ((mState.Y >= 125 + (100 * i) && (mState.Y <= 140 + (100 * i)))))
                        {
                            if (mState.LeftButton == ButtonState.Pressed && previousMState.LeftButton != ButtonState.Pressed)
                            {
                                Progress.skillpoints--;
                                switch (i)
                                {
                                    case 0:
                                        player.MeleeDamage = player.MeleeDamage + 30;
                                        break;
                                    case 1:
                                        player.Speed = player.Speed + 1;
                                        break;
                                    case 2:
                                        player.MaxHealth = player.MaxHealth + 10;
                                        break;
                                    case 3:
                                        player.MaxMana = player.MaxMana + 10;
                                        break;
                                    case 4:
                                        player.RangedDamage = player.RangedDamage + 5;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                  //checks the location of the mouse to see if it is on a button
                  if ((mState.X >= 650 && mState.X <= 665) && ((mState.Y >= 145 + (100 * i) && (mState.Y <= 160 + (100 * i)))))
                   {
                     if (mState.LeftButton == ButtonState.Pressed && previousMState.LeftButton != ButtonState.Pressed)
                     {
                        switch (i)
                        {
                          case 0:
                                if (player.MeleeDamage > Progress.startingAttack)
                                {
                                    player.MeleeDamage = player.MeleeDamage - 30;
                                    Progress.skillpoints++;
                                }
                             break;
                          case 1:
                             if (player.Speed > Progress.startingSpeed)
                             {
                                 player.Speed = player.Speed - 1;
                                 Progress.skillpoints++;
                             }
                             break;
                          case 2:
                             if (player.MaxHealth > Progress.startingHealthMax)
                             {
                                 player.MaxHealth = player.MaxHealth - 10;
                                 Progress.skillpoints++;
                             }
                             break;
                          case 3:
                             if (player.MaxMana > Progress.startingManaMax)
                             {
                                 player.MaxMana = player.MaxMana - 10;
                                 Progress.skillpoints++;
                             }
                             break;
                          case 4:
                             if (player.RangedDamage > Progress.startingRngDmg)
                             {
                                 player.RangedDamage = player.RangedDamage - 5;
                                 Progress.skillpoints++;
                             }
                             break;
                          default:
                             break;
                         }
                      }
                   }
                }
            }

            //options screen 
            if (pauseMenuSelected == 2)
            {

                if (((kState.IsKeyDown(Keys.W)) || (kState.IsKeyDown(Keys.Up)))
                    && (previousKState.IsKeyUp(Keys.W) && previousKState.IsKeyUp(Keys.Up)))
                {
                    if (optionSelected == 1) optionSelected = 0;
                    else optionSelected++;
                }
                if (((kState.IsKeyDown(Keys.S)) || (kState.IsKeyDown(Keys.Down)))
                    && (previousKState.IsKeyUp(Keys.S) && previousKState.IsKeyUp(Keys.Down)))
                {
                    if (optionSelected == 0) optionSelected = 1;
                    else optionSelected--;
                }
                else if (kState.IsKeyDown(Keys.Enter))
                {
                    switch (optionSelected)
                    {
                        //resume gmae
                        case 0:
                            gameMode = 1;
                            break;
                        //exit game
                        case 1:
                            Exit();
                            break;
                    }
                }
            }
        }

        protected void gameOverScreen()
        {
            //fill in with GameOver stuff
        }

        protected void say(string file)
        {
            isTalking = true;
            phrases.Clear();
            phraseIndex = 0;
            try
            {
                StreamReader input = new StreamReader(file);
                talkName = input.ReadLine();
                string phrase = "";
                int i = 0; //keeps track of lines
                string phrasePart;

                while ((phrasePart = input.ReadLine()) != null)
                {

                    if (i < 3)
                    {
                        phrase = phrase + "\n" + phrasePart;
                        i++;
                    }
                    //when three lines are read in add it to the list
                    if (i >= 3)
                    {
                        i = 0;
                        phrases.Add(phrase);
                        phrase = "";
                    }
                }
                //add the last phrase if it is less than 3 lines
                if (i != 0)
                {
                    phrases.Add(phrase);
                }

                input.Close();

            }
            catch (IOException ioe)
            {
                talkName = "console";
                phrases.Add("invalid file");
                Console.WriteLine("Message: " + ioe.Message);
                Console.WriteLine("\nStack Trace: " + ioe.StackTrace);
            }
            currentPhrase = phrases[0];



        }

        // Adds the objects to the right lists
        protected void Add(GameObject gameObject)
        {
            if (gameObject is Platform)
            {
                for (int i = 0; i < gameObjects.Count - 1; i++)
                {
                    if (gameObjects[i] is Platform || !(gameObjects[i] is Platform) && gameObjects[i+1] is Platform)
                    {
                        gameObjects.Insert(0, gameObject);
                        collidableObjects.Add((CollidableObject)gameObject);
                        return;
                    }
                }
                gameObjects.Insert(0, gameObject);
                collidableObjects.Add((CollidableObject)gameObject);
            }

            else if (gameObject is NPC)
            {
                for (int i = 0; i < gameObjects.Count-1; i++)
                {
                    if ((gameObjects[i] is Player))
                    {
                        if(enemies.Count==0 && !(gameObject is Enemy))
                        {
                            gameObjects.Insert(i, gameObject);
                            collidableObjects.Add((CollidableObject)gameObject);
                        }
                        if (gameObjects[i + 1] is NPC)
                        {
                            if (gameObjects[i + 1] is Enemy)
                            {
                                if (gameObject is Enemy)
                                {
                                    gameObjects.Insert(i+1, gameObject);
                                    collidableObjects.Add((CollidableObject)gameObject);
                                    enemies.Add((Enemy)gameObject);
                                    return;
                                }
                                else
                                    continue;
                            }
                            else
                            {
                                if (!(gameObject is Enemy))
                                {
                                    gameObjects.Insert(i+1, gameObject);
                                    collidableObjects.Add((CollidableObject)gameObject);
                                    return;
                                }
                                else
                                    continue;
                            }
                        }
                        else
                        {
                            gameObjects.Insert(i+1, gameObject);
                            collidableObjects.Add((CollidableObject)gameObject);
                            if (gameObject is Enemy)
                                enemies.Add((Enemy)gameObject);
                            return;
                        }
                    }
                }
            }
            else if (gameObject is Fireball)
            {
                gameObjects.Add(gameObject);
            }

            else if (gameObject is Items)
            {
                gameObjects.Add(gameObject);
                collidableObjects.Add((CollidableObject)gameObject);
            }
            else
            {
                gameObjects.Insert(0, gameObject);
                if (gameObject is CollidableObject)
                    collidableObjects.Add((CollidableObject)gameObject);
            }
        }

        //removes every reference to the object
        protected void Delete(int ID)
        {
            foreach(GameObject gameObject in gameObjects)
            {
                if(gameObject.ObjectID==ID)
                {
                    gameObjects.Remove(gameObject);
                    if (gameObject is CollidableObject)
                    {
                        collidableObjects.Remove((CollidableObject)gameObject);
                        if (gameObject is Enemy)
                            enemies.Remove((Enemy)gameObject);
                        return;
                    }
                    else
                        return;
                }
            }
        }




        //for reading the settings file
        public void ReadSettings()
        {
            StreamReader input = null;
            try
            {
                //opening the file
                input = new StreamReader("settings.txt");

                //loop to read the file
                string text = "";
                string[] data = new string[5];
                int[] newdata = new int[5];
                while ((text = input.ReadLine()) != null)
                {
                    //split the settings up
                    data = text.Split();
                }
                for (int i = 0; i < data.Length; i++)
                {
                    newdata[i] = int.Parse(data[i]);
                }
                sh = newdata[0];
                sm = newdata[1];
                mh = newdata[2];
                mm = newdata[3];
                sp = newdata[4];
            }
            catch (IOException ioe)
            {
                Console.WriteLine("Settings input Message: " + ioe.Message);
                Console.WriteLine("Settings input Stack Trace: " + ioe.StackTrace);
            }
        }
    }
}
