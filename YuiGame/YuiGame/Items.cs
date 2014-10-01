using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace YuiGame
{
    public class Items : CollidableObject
    {
        private string description;
        private string name;
        //food, ring, necklace, talisman, soul, other
        private string type;

        public Items(Texture2D img, Vector2 pos, int width, int height, int ID, string nm, string tp)
            : base(img, pos, width, height, ID)
        {
            // setting up the attributes
            name = nm;
            description = "unassigned description";
            type = tp;
            //consumables
            if (name == "Bread") { description = "heals by 10"; }
            if (name == "Potion of Healing") { description = "heals by 50"; }
            if (name == "Ether potion") { description = "a bottle filled\n with ether"; }
            if (name == "Potion of Mana") { description = "increases mana\n by 100"; }
            //rings
            if (name == "Ring of Restoration") { description = "increases max\n health by 10"; }
            if (name == "Ring of Mana") { description = "increases max\n mana by 10"; }
            if (name == "Ring of Fastness") { description = "increases speed\n by 1"; }
            //Necklaces
            if (name == "Necklace of Power") { description = "increases physical\n damage by 5"; }
            if (name == "Necklace of Magic") { description = "increases magic\n attack by 5"; }
            //talisman
            if (name == "Scroll of Knowledge") { description = "increases max\n mana by 20"; }
            if (name == "Sharp Earrings") { description = "increase attack\n by 5"; }

        }

        public void equip(Player p)
        {
            //rings
            if (name == "Ring of Restoration") { p.MaxHealth = p.MaxHealth + 10; }
            if (name == "Ring of Mana") { p.MaxMana = p.MaxMana + 10; }
            if (name == "Ring of Fastness") { p.Speed = p.Speed + 1; }
            //necklaces
            if (name == "Necklace of Power") { p.MeleeDamage = p.MeleeDamage + 5; }
            if (name == "Necklace of Magic") { p.RangedDamage += 5; }
            //talismans
            if (name == "Scroll of Knowledge") { p.MaxMana = p.MaxMana + 20; }
            if (name == "Sharp Earrings") { p.MeleeDamage += 5; }
        }

        public void unEquip(Player p)
        {
            //rings
            if (name == "Ring of Restoration") { p.MaxHealth = p.MaxHealth - 10; }
            if (name == "Ring of Mana") { p.MaxMana = p.MaxMana - 10; }
            if (name == "Ring of Fastness") { p.Speed = p.Speed - 1; }
            //necklaces
            if (name == "Necklace of Power") { p.MeleeDamage = p.MeleeDamage - 5; }
            if (name == "Necklace of Magic") { p.RangedDamage -= 5; }
            //talisman
            if (name == "Scroll of Knowledge") { p.MaxMana = p.MaxMana - 5; }
            if (name == "Sharp Earrings") { p.MeleeDamage -= 5; }
        }

        public void activate(Player p)
        {
            if (name == "Food of testing") { p.Health = p.Health + 10; }
            if (name == "Potion of Healing") { p.Health += p.Health + 50; }
            if (name == "Ether potion") { p.Health = 0; }
            if (name == "Potion of Mana") { p.Mana += 100; }
            if (name == "Small Soul") { Progress.currentXp = Progress.currentXp + 1; }
        }

        public override void Collide()
        {
            //base.Collide();
        }

        //properties
        public string Description { get { return description; } }
        public string Name { get { return name; } }
        public string Type { get { return type; } }

    }
}
