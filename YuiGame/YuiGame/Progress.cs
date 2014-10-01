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
    // This class will track the progress of the game
    // I was thinking of the game class doing that but didn't
    // wanna fill it with variables
    static class Progress
    {
        public static int totalHealth;
        public static int totalMana;
        public static int currentHealth;
        public static int currentMana;
        public static int playerLevel;
        public static int currentXp;
        public static int neededXp;
        public static int skillpoints;
        
        //variables to make sure attributes dont go below their starting levels
        public static int startingHealthMax;
        public static int startingManaMax;
        public static int startingSpeed;
        public static int startingAttack;
        public static int startingRngDmg;
        public static int checkpoint;
        public static int endState;
        public static int choice;


        public static Items[] inventory;
        public static Items[] equipped;


        static int screenWidth;
        static int screenHeight;

    }
}
