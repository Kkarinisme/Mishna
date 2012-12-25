// Enums for use in decal

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Decal;
using Decal.Adapter;

namespace Mishna
{
    public partial class PluginCore
    {

        public enum eSetName
        {

            None = 0,
            Noble = 5,
            Ancient_Relic = 6,
            Relic_Alduressa = 7,
            Shojen = 8,
            Empyrean_Ring = 9,
            Arm_Mind_Heart = 10,
            Coat_Perfect_Light = 11,
            Leggings_Perfect_LIght = 12,
            Soldiers = 13,
            Adept = 14,
            Archers = 15,
            Defenders = 16,
            Tinkers = 17,
            Crafters = 18,
            Hearty = 19,
            Dexterous = 20,
            Wise = 21,
            Swift = 22,
            Hardened = 23,
            Reinforced = 24,
            Interlocking = 25,
            Flame_Proof = 26,
            Acid_Proof = 27,
            Cold_Proof = 28,
            Lightning_Proof = 29,
            Dedication = 30,
            Gladiatorial_Clothing = 31,
            Protective_Clothing = 32,
            Sigil_of_Defense = 35,
            Sigil_of_Destruction = 36,
            Sigil_of_Fury = 37,
            Sigil_of_Growth = 38,
            Sigil_of_Vigor = 39,
            Weave_of_Alchemy = 49,
            Weave_of_Arcane_Lore = 50,
            Weave_of_Armor_Tinkering = 51,
            Weave_of_Assess_Person = 52,
            Weave_of_Light_Weapons = 53,
            Weave_of_Missile_Weapons = 54,
            Weave_of_Cooking = 55,
            Weave_of_Creature_Enchantment = 56,
            Weave_of_MissileWeapons = 57,
            Weave_of_Finnesse_Weapons = 58,
            Weave_of_Deception = 59,
            Weave_of_Fletching = 60,
            Weave_of_Healing = 61,
            Weave_of_Item_Enchantment = 62,
            Weave_of_Item_Tinkering = 63,
            Weave_of_Leadership = 64,
            Weave_of_Life_Magic = 65,
            Weave_of_Lockpick = 66,
            Weave_of_Light_Weapon = 67,
            Weave_of_Magic_Defense = 68,
            Weave_of_Magic_Item_Tinkering = 69,
            Weave_of_Mana_Conversion = 70,
            Weave_of_Melee_Defense = 71,
            Weave_of_Missile_Defense = 72,
            Weave_of_Salvaging = 73,
            Weave_of_LightWeapons = 74,
            Weave_of_LightWeapon = 75,
            Weave_of_Heavy_Weapons = 76,
            Weave_of_Two_Handed_Combat = 78,
            Weave_of_LiteWeapon = 79,
            Weave_of_Void_Magic = 80,
            Weave_of_War_Magic = 81,
            Weave_of_Weapon_Tinkering = 82,
            Weave_of_Dirty_Fighting = 84,
            Weave_of_Dual_Wield = 85,
            Weave_of_Recklessness = 86,
            Weave_of_Shield = 87,
            Weave_of_Sneak_Attack = 88,
        }


        public enum eObjectClass
        {
            None,
            Armor,
            Clothing,
            Jewelry,
            WandStaffOrb,
            MeleeWeapon,
            MissileWeapon,
            Salvage,
            Scroll,
            HealingKit,
            Food,
            Gem,
            CraftedFletching,
            BaseCooking,
            CraftedAlchemy,
            SpellComponent,
            Key,
            Misc,
            ManaStone,
            BooksPaper,
            TradeNote,
            Money,
            Pyreal,
            Ust,
            Foci,
            Container,

        }

        public enum eDamageType
        {
            None = 0,
            Slashing = 1,
            Piercing = 2,
            Slash_Pierce = 3,
            Bludgeon = 4,
            Frost = 8,
            Fire = 16,
            Acid = 32,
            Lightning = 64

        }


        public enum eSlayer
        {
            Olthoi = 1,
            Tuskers = 8,
            Undead = 14,
            Shadow = 22,
            Skeleton = 30,
            Human = 31,
            Ghost = 77,
            Mukkir = 89,
        }


        public enum eObjEmbue
        {
            None = 0,
            Critical_Strike = 1,
            Crippling_Blow = 2,
            Armor = 4,
            Slashing = 8,
            Piercing = 16,
            Bludgeon = 32,
            Acid = 64,
            Frost = 128,
            Lightning = 256,
            Fire = 512,
            MagicD_PlusOne = 1000
        }

        public enum eobjMatName
        {
            emerald = 21,
            fire_opal = 22,
            ruby = 38,
            ivory = 51,
            bronze = 60,
            gold = 60,
            silver = 63,
        }

        public enum eObjWieldAttr
        {
            WarMagic = 34,
            TwoHand = 41,
            Heavy_Weapon = 44,
            Light_Weapon = 45,
            Finesse_Weapon = 46,
            Missile_Weapon = 47,
       }

        public enum eObjWieldType
        {
            None = 0,
            Spear = 1,
            Unarmed = 2
        }

        //public static void findSetName(string setName)
        //{
        //    switch (setName)
        //    {
        //        case "None": objSetValue = 0; break;
        //        case "Noble": objSetValue = 5; break;
        //        case "Soldiers": objSetValue = 6; break;
        //        case "Adept": objSetValue = 14; break;
        //        case "Archers": objSetValue = 15; break;
        //        case "Defenders": objSetValue = 16; break;
        //        case "Tinkers": objSetValue = 17; break;
        //        case "Crafters": objSetValue = 18; break;
        //        case "Hearty": objSetValue = 19; break;
        //        case "Dexterous": objSetValue = 20; break;
        //        case "Wise": objSetValue = 21; break;
        //        case "Swift": objSetValue = 22; break;
        //        case "Hardened": objSetValue = 23; break;
        //        case "Reinforced": objSetValue = 24; break;
        //        case "Interlocking": objSetValue = 25; break;
        //        case "Flame_Proof": objSetValue = 26; break;
        //        case "Acid_Proof": objSetValue = 27; break;
        //        case "Cold_Proof": objSetValue = 28; break;
        //        case "Lightning_Proof": objSetValue = 29; break;
        //        case "Dedication": objSetValue = 30; break;
        //        case "Gladiatorial_Clothing": objSetValue = 31; break;
        //        case "Protective_Clothing": objSetValue = 32; break;
        //    }
        //}

        public static void findArmorSetInt(int temparmorset)
        {
            // need to correlate the case number 
            //  (which is the number from the mainview.xml seen in the drop down list)
            // with the number assigned by AC for that particular set 
                // (number seen in the inventory file as ObjSet
            switch (temparmorset)
            {
                case 0: objArmorSet = 0; break;
                case 1: objArmorSet = 5; break;
                case 2: objArmorSet = 6; break;
                case 3: objArmorSet = 14; break;
                case 4: objArmorSet = 15; break;
                case 5: objArmorSet = 16; break;
                case 6: objArmorSet = 17; break;
                case 7: objArmorSet = 18; break;
                case 8: objArmorSet = 19; break;
                case 9: objArmorSet = 20; break;
                case 10: objArmorSet = 21; break;
                case 11: objArmorSet = 22; break;
                case 12: objArmorSet = 23; break;
                case 13: objArmorSet = 24; break;
                case 14: objArmorSet = 25; break;
                case 15: objArmorSet = 26; break;
                case 16: objArmorSet = 27; break;
                case 17: objArmorSet = 28; break;
                case 18: objArmorSet = 29; break;
                case 19: objArmorSet = 30; break;
                case 20: objArmorSet = 31; break;
                case 21: objArmorSet = 32; break;
                case 22: objArmorSet = 35; break;
                case 23: objArmorSet = 36; break;
                case 24: objArmorSet = 37; break;
                case 25: objArmorSet = 38; break;
                case 26: objArmorSet = 39; break;
                case 27: objArmorSet = 49; break;
                case 28: objArmorSet = 50; break;
                case 29: objArmorSet = 51; break;
                case 30: objArmorSet = 52; break;
                case 31: objArmorSet = 53; break;
                case 32: objArmorSet = 54; break;
                case 33: objArmorSet = 55; break;
                case 34: objArmorSet = 56; break;
                case 35: objArmorSet = 57; break;
                case 36: objArmorSet = 56; break;
                case 37: objArmorSet = 57; break;
                case 38: objArmorSet = 58; break;
                case 39: objArmorSet = 69; break;
                case 40: objArmorSet = 60; break;
                case 41: objArmorSet = 61; break;
                case 42: objArmorSet = 62; break;
                case 43: objArmorSet = 63; break;
                case 44: objArmorSet = 64; break;
                case 45: objArmorSet = 65; break;
                case 46: objArmorSet = 66; break;
                case 47: objArmorSet = 67; break;
                case 48: objArmorSet = 68; break;
                case 49: objArmorSet = 69; break;
                case 50: objArmorSet = 70; break;
                case 51: objArmorSet = 71; break;
                case 52: objArmorSet = 72; break;
                case 53: objArmorSet = 73; break;
                case 54: objArmorSet = 74; break;
                case 55: objArmorSet = 75; break;
                case 56: objArmorSet = 76; break;
                case 57: objArmorSet = 78; break;
                case 58: objArmorSet = 80; break;
                case 59: objArmorSet = 81; break;
                case 60: objArmorSet = 82; break;
                case 61: objArmorSet = 84; break;
                case 62: objArmorSet = 85; break;
                case 63: objArmorSet = 86; break;
                case 64: objArmorSet = 87; break;
                case 65: objArmorSet = 88; break;
                case 66: objArmorSet = 88; break;
            }
        }

        public static void findArmorSetName(long objArmorSet)
        {
            switch (objArmorSet)
            {
                case 0: objArmorSetName = "None"; break;
                case 5: objArmorSetName = "Noble"; break;
                case 6: objArmorSetName = "Soldiers"; break;
                case 14: objArmorSetName = "Adept"; break;
                case 15: objArmorSetName = "Archers"; break;
                case 16: objArmorSetName = "Defenders"; break;
                case 17: objArmorSetName = "Tinkers"; break;
                case 18: objArmorSetName = "Crafters"; break;
                case 19: objArmorSetName = "Hearty"; break;
                case 20: objArmorSetName = "Dexterous"; break;
                case 21: objArmorSetName = "Wise"; break;
                case 22: objArmorSetName = "Swift"; break;
                case 23: objArmorSetName = "Hardened"; break;
                case 24: objArmorSetName = "Reinforced"; break;
                case 25: objArmorSetName = "Interlocking"; break;
                case 26: objArmorSetName = "Frame_Proof"; break;
                case 27: objArmorSetName = "Acid_Proof"; break;
                case 28: objArmorSetName = "Cold_Proof"; break;
                case 29: objArmorSetName = "Lightning_Proof"; break;
                case 30: objArmorSetName = "Dedication"; break;
                case 31: objArmorSetName = "Gladiatorial"; break;
                case 32: objArmorSetName = "Protective"; break;
                case 35: objArmorSetName = "Sigil of Defense"; break;
                case 36: objArmorSetName = "Sigil of Destruction"; break;
                case 37: objArmorSetName = "Sigil of Fury"; break;
                case 38: objArmorSetName = "Sigil of Growth"; break;
                case 39: objArmorSetName = "Sigil of Vigor"; break;
                case 49: objArmorSetName = "Weave of Alchemy"; break;
                case 50: objArmorSetName = "Weave of Arcane Lore"; break;
                case 51: objArmorSetName = "Weave of Armor Tinkering"; break;
                case 52: objArmorSetName = "Weave of Assess Person"; break;
                case 53: objArmorSetName = "Weave of Light Weapons"; break;
                case 54: objArmorSetName = "Weave of Missile Weapons"; break;
                case 55: objArmorSetName = "Weave of Cooking"; break;
                case 56: objArmorSetName = "Weave of Creature Enchantment"; break;
                case 57: objArmorSetName = "Weave of Missile  Weapons"; break;
                case 58: objArmorSetName = "Weave of Finnesse Weapons"; break;
                case 59: objArmorSetName = "Weave of Deception"; break;
                case 60: objArmorSetName = "Weave of Fletching"; break;
                case 61: objArmorSetName = "Weave of Healing"; break;
                case 62: objArmorSetName = "Weave of Item Enchantment"; break;
                case 63: objArmorSetName = "Weave of Item Tinkering"; break;
                case 64: objArmorSetName = "Weave of Leadership"; break;
                case 65: objArmorSetName = "Weave of Life Magic"; break;
                case 66: objArmorSetName = "Weave of Lockpick"; break;
                case 67: objArmorSetName = "Weave of Light Weapons"; break;
                case 68: objArmorSetName = "Magic Defense"; break;
                case 69: objArmorSetName = "Weave of Magic Item Tinkering"; break;
                case 70: objArmorSetName = "Weave of Mana Conversion"; break;
                case 71: objArmorSetName = "Weave of Melee Defense"; break;
                case 72: objArmorSetName = "Weave of Missile Defense"; break;
                case 73: objArmorSetName = "Weave of Salvaging"; break;
                case 74: objArmorSetName = "Weave of Light Weapons"; break;
                case 75: objArmorSetName = "Weave of Light Weapons"; break;
                case 76: objArmorSetName = "Weave of Heavy Weapons"; break;
                case 78: objArmorSetName = "Weave of Two Handed Combat"; break;
                case 79: objArmorSetName = "Weave of Light Weapons"; break;
                case 80: objArmorSetName = "Weave of Void Magic"; break;
                case 81: objArmorSetName = "Weave of War Magic"; break;
                case 82: objArmorSetName = "Weave of Weapon Tinkering"; break;
                case 84: objArmorSetName = "Weave of Dirty Fighting"; break;
                case 85: objArmorSetName = "Weave of Dual Wield"; break;
                case 86: objArmorSetName = "Weave of Recklessness"; break;
                case 87: objArmorSetName = "Weave of Shield Mastery"; break;
                case 88: objArmorSetName = "Weave of Sneak Attack"; break;
        

            }
        }

        public static void findArmorLevelInt(int temparmorlevel)
        {
            switch (temparmorlevel)
            {
                case 0: objArmorLevel = 1; break;
                case 1: objArmorLevel = 0; break;
                case 2: objArmorLevel = 60; break;
                case 3: objArmorLevel = 90; break;
                case 4: objArmorLevel = 100; break;
                case 5: objArmorLevel = 150; break;
                case 6: objArmorLevel = 180; break;
                case 7: objArmorLevel = 225; break;
            }
        }

        public static void findArmorCoverage(int tempCoverage)
        {
            switch (tempCoverage)
            {
                case 0: objCovers = 0; break;
                case 1: objCovers = 256; break;
                case 2: objCovers = 512; break;
                case 3: objCovers = 1024; break;
                case 4: objCovers = 2048; break;
                case 5: objCovers = 4096; break;
                case 6: objCovers = 8192; break;
                case 7: objCovers = 16384; break;
                case 8: objCovers = 32768; break;
                case 9: objCovers = 65536; break;
                case 10: objCovers = 768; break;
                case 11: objCovers = 2304; break;
                case 12: objCovers = 2816; break;
                case 13: objCovers = 3072; break;
                case 14: objCovers = 5120; break;
                case 15: objCovers = 7168; break;
                case 16: objCovers = 13312; break;
                case 17: objCovers = 15360; break;
                case 18: objCovers = 8; break;
                case 19: objCovers = 19; break;
                case 20: objCovers = 22; break;
                case 21: objCovers = 40; break;
                case 22: objCovers = 131072; break;
            }
        }

        public static void findCoversName(long objCovers)
        {
            switch (objCovers)
            {
                case 0: objCoversName = "None"; break;
                case 8: objCoversName = "Chest"; break;
                case 19: objCoversName = "Abdomen & Upper Legs"; break;
                case 22: objCoversName = "Abdomen & Upper Legs & Lower Legs"; break;
                case 40: objCoversName = "Chest & Upper Arms"; break;
                case 104: objCoversName = "Chest & Upper Arms & Lower Arms"; break;
                case 256: objCoversName = "Upper Leg"; break;
                case 512: objCoversName = "Lower Leg"; break;
                case 1024: objCoversName = "Chest"; break;
                case 2048: objCoversName = "Abdomen"; break;
                case 4096: objCoversName = "Upper Arm"; break;
                case 8192: objCoversName = "Lower Arm"; break;
                case 16384: objCoversName = "Head"; break;
                case 32768: objCoversName = "Hands"; break;
                case 65536: objCoversName = "Feet"; break;
                case 768: objCoversName = "Upper Legs & Lower Legs"; break;
                case 2304: objCoversName = "Abdomen & Upper Legs"; break;
                case 2816: objCoversName = "Abdomen & Upper Legs & Lower Legs"; break;
                case 3072: objCoversName = "Chest & Abdomen"; break;
                case 5120: objCoversName = "Chest & Upper Arms"; break;
                case 7168: objCoversName = "Chest & Abdomen & Upper Arms"; break;
                case 13312: objCoversName = "Chest & Upper Arms & Lower Arms"; break;
                case 131072: objCoversName = "Cloak"; break;
                case 15360: objCoversName = "Chest & Abdomen & Upper Arms & Lower Arms"; break;
            }
        }


        public static void findMaterial(int tempMaterial)
        {
            switch (tempMaterial)
            {
                case 0: objMat = 0; break;
                case 1: objMat = 64; break;
                case 2: objMat = 59; break;
                case 3: objMat = 74; break;
                case 4: objMat = 61; break;
                case 5: objMat = 67; break;
                case 6: objMat = 23; break;
                case 7: objMat = 13; break;
                case 8: objMat = 15; break;
                case 9: objMat = 16; break;
                case 10: objMat = 21; break;
                case 11: objMat = 22; break;
                case 12: objMat = 26; break;
                case 13: objMat = 27; break;
                case 14: objMat = 35; break;
                case 15: objMat = 51; break;
                case 16: objMat = 52; break;
                case 17: objMat = 10; break;
                case 18: objMat = 66; break;
                case 19: objMat = 11; break;
                case 20: objMat = 12; break;
                case 21: objMat = 14; break;
                case 22: objMat = 17; break;
                case 23: objMat = 57; break;
                case 24: objMat = 58; break;
                case 25: objMat = 18; break;
                case 26: objMat = 1; break;
                case 27: objMat = 19; break;
                case 28: objMat = 20; break;
                case 29: objMat = 53; break;
                case 30: objMat = 73; break;
                case 31: objMat = 60; break;
                case 32: objMat = 24; break;
                case 33: objMat = 54; break;
                case 34: objMat = 25; break;
                case 35: objMat = 28; break;
                case 36: objMat = 29; break;
                case 37: objMat = 4; break;
                case 38: objMat = 30; break;
                case 39: objMat = 68; break;
                case 40: objMat = 31; break;
                case 41: objMat = 75; break;
                case 42: objMat = 69; break;
                case 43: objMat = 32; break;
                case 44: objMat = 33; break;
                case 45: objMat = 34; break;
                case 46: objMat = 76; break;
                case 47: objMat = 2; break;
                case 48: objMat = 62; break;
                case 49: objMat = 36; break;
                case 50: objMat = 55; break;
                case 51: objMat = 37; break;
                case 52: objMat = 38; break;
                case 53: objMat = 70; break;
                case 54: objMat = 39; break;
                case 55: objMat = 5; break;
                case 56: objMat = 71; break;
                case 57: objMat = 6; break;
                case 58: objMat = 63; break;
                case 59: objMat = 40; break;
                case 60: objMat = 65; break;
                case 61: objMat = 41; break;
                case 62: objMat = 77; break;
                case 63: objMat = 42; break;
                case 64: objMat = 43; break;
                case 65: objMat = 44; break;
                case 66: objMat = 7; break;
                case 67: objMat = 45; break;
                case 68: objMat = 46; break;
                case 69: objMat = 47; break;
                case 70: objMat = 72; break;
                case 71: objMat = 8; break;
                case 72: objMat = 48; break;
                case 73: objMat = 49; break;
                case 74: objMat = 50; break;
               }
        }

        public static void findMaterialName(long tempMat)
        {           
            switch (tempMat)
            {
                case 0: objMatName = "None"; break;
                case 1: objMatName = "Ceramic"; break;
                case 2: objMatName = "Porcelain"; break;
                case 3: objMatName = ""; break;
                case 4: objMatName = "Linen"; break;
                case 5: objMatName = "Satin"; break;
                case 6: objMatName = "Silk"; break;
                case 7: objMatName = "Velvet"; break;
                case 8: objMatName = "Wool"; break;
                case 9: objMatName = ""; break;
                case 10: objMatName = "Agate"; break;
                case 11: objMatName = "Amber"; break;
                case 12: objMatName = "Amethyst"; break;
                case 13: objMatName = "Aquamarine"; break;
                case 14: objMatName = "Azurite"; break;
                case 15: objMatName = "Black Garnet"; break;
                case 16: objMatName = "Black Opal"; break;
                case 17: objMatName = "Bloodstone"; break;
                case 18: objMatName = "Carnelian"; break;
                case 19: objMatName = "Citrine"; break;
                case 20: objMatName = "Diamond"; break;
                case 21: objMatName = "Emerald"; break;
                case 22: objMatName = "Fire Opal"; break;
                case 23: objMatName = "Green Garnet"; break;
                case 24: objMatName = "Green Jade"; break;
                case 25: objMatName = "Hematite"; break;
                case 26: objMatName = "Imperial Topaz"; break;
                case 27: objMatName = "Jet"; break;
                case 28: objMatName = "Lapis Lazuli"; break;
                case 29: objMatName = "Lavender Jade"; break;
                case 30: objMatName = "Malachite"; break;
                case 31: objMatName = "Moonstone"; break;
                case 32: objMatName = "Onyx"; break;
                case 33: objMatName = "Opal"; break;
                case 34: objMatName = "Peridot"; break;
                case 35: objMatName = "Red Garnet"; break;
                case 36: objMatName = "Red Jade"; break;
                case 37: objMatName = "Rose Quartz"; break;
                case 38: objMatName = "Ruby"; break;
                case 39: objMatName = "Sapphire"; break;
                case 40: objMatName = "Smokey Quartz"; break;
                case 41: objMatName = "Sunstone"; break;
                case 42: objMatName = "Tiger Eye"; break;
                case 43: objMatName = "Tourmaline"; break;
                case 44: objMatName = "Turquoise"; break;
                case 45: objMatName = "White Jade"; break;
                case 46: objMatName = "White Quartz"; break;
                case 47: objMatName = "White Sapphire"; break;
                case 48: objMatName = "Yellow Garnet"; break;
                case 49: objMatName = "Yellow Topaz"; break;
                case 50: objMatName = "Zircon"; break;
                case 51: objMatName = "Ivory"; break;
                case 52: objMatName = "Leather"; break;
                case 53: objMatName = "Armoredillo Hide"; break;
                case 54: objMatName = "Gromnie Hide"; break;
                case 55: objMatName = "Reed Shark Hide"; break;
                case 56: objMatName = ""; break;
                case 57: objMatName = "Brass"; break;
                case 58: objMatName = "Bronze"; break;
                case 59: objMatName = "Copper"; break;
                case 60: objMatName = "Gold"; break;
                case 61: objMatName = "Iron"; break;
                case 62: objMatName = "Pyreal"; break;
                case 63: objMatName = "Silver"; break;
                case 64: objMatName = "Steel"; break;
                case 65: objMatName = ""; break;
                case 66: objMatName = "Alabaster"; break;
                case 67: objMatName = "Granite"; break;
                case 68: objMatName = "Marble"; break;
                case 69: objMatName = "Obsidian"; break;
                case 70: objMatName = "Sandstone"; break;
                case 71: objMatName = "Serpentine"; break;
                case 72: objMatName = ""; break;
                case 73: objMatName = "Ebony"; break;
                case 74: objMatName = "Mahogany"; break;
                case 75: objMatName = "Oak"; break;
                case 76: objMatName = "Pine"; break;
                case 77: objMatName = "Teak"; break;
               }
        }

        public static void findobjSalvWork(int tempSalvWork)
        {
            switch (tempSalvWork)
            {
                case 0: objSalvWork = "None"; break;
                case 1: objSalvWork = "1,2,3,4,5,6"; break;
                case 2: objSalvWork = "7,8"; break;
                case 3: objSalvWork = "9"; break;
                case 4: objSalvWork = "10"; break;
            }
        }


 
        public static void findDamageType()
        {
            switch (objDamageTypeInt)
            {

                case 0:  objDamageType = "None"; break;
                case 1: objDamageType = "Slashing"; break;
                case 2: objDamageType = "Piercing"; break;
                case 3: objDamageType = "Slash_Pierce"; break;
                case 4: objDamageType = "Bludgeon"; break;
                case 8: objDamageType = "Frost"; break;
                case 16: objDamageType = "Fire"; break;
                case 32: objDamageType = "Acid"; break;
                case 64: objDamageType = "Lightning"; break;
            }


        }

        public static void findEmbueType(string objEmbueType)
        {
            switch (objEmbueType)
            {

                case "None":  objEmbueTypeInt = 0; break;
                case "Critical_Strike":  objEmbueTypeInt = 1; break;
                case "Crippling_Blow":  objEmbueTypeInt = 2; break;
                case "Armor":  objEmbueTypeInt = 4; break;
                case "Slashing":  objEmbueTypeInt = 8; break;
                case "Piercing": objEmbueTypeInt = 16; break;
                case "Bludgeon": objEmbueTypeInt = 32; break;
                case "Acid": objEmbueTypeInt = 64; break;
                case "Frost": objEmbueTypeInt = 128; break;
                case "Lightning": objEmbueTypeInt = 256; break;
                case "Fire": objEmbueTypeInt = 512; break;
                case "MagicD_PlusOne": objEmbueTypeInt = 1000; break;
            }


        }

        public static void findEmbueTypeStr(long objEmbueTypeInt)
        {
            switch (objEmbueTypeInt)
            {

                case 0:  objEmbueTypeStr = "None"; break;
                case 1:  objEmbueTypeStr = "Critical_Strike"; break;
                case 2:  objEmbueTypeStr = "Crippling_Blow"; break;
                case 4:  objEmbueTypeStr = "Armor"; break;
                case 8:  objEmbueTypeStr = "Slashing"; break;
                case 16:  objEmbueTypeStr = "Piercing"; break;
                case 32:  objEmbueTypeStr = "Bludgeon"; break;
                case 64:  objEmbueTypeStr = "Acid"; break;
                case 128:  objEmbueTypeStr = "Frost"; break;
                case 256:  objEmbueTypeStr = "Lightning"; break;
                case 512:  objEmbueTypeStr = "Fire"; break;
                case 1000:  objEmbueTypeStr = "MagicD_PlusOne"; break;
            }


        }

        public static void findEmbueTypeInt(int objTempEmbueTypeInt)
        {
            switch (objTempEmbueTypeInt)
            {

                case 0: objEmbueTypeInt = 0; break;
                case 1: objEmbueTypeInt = 1; break;
                case 2: objEmbueTypeInt = 2; break;
                case 3: objEmbueTypeInt = 4; break;
                case 4: objEmbueTypeInt = 8; break;
                case 5: objEmbueTypeInt = 16; break;
                case 6: objEmbueTypeInt = 32; break;
                case 7: objEmbueTypeInt = 64; break;
                case 8: objEmbueTypeInt = 128; break;
                case 9: objEmbueTypeInt = 256; break;
                case 10: objEmbueTypeInt = 512; break;
                case 11: objEmbueTypeInt = 1000; break;
            }


        }


        public static void findAttrInt(string objWieldAttr)
        {
            switch (objWieldAttr)
            {

                case "None":  objWieldAttrInt = 0; break;
                case "WarMagic":  objWieldAttrInt = 34; break;
                case "TwoHand":  objWieldAttrInt = 41; break;
                case "Heavy":  objWieldAttrInt = 44; break;
                case "Light":  objWieldAttrInt = 45; break;
                case "Finesse":  objWieldAttrInt = 46; break;
                case "Missile":  objWieldAttrInt = 47; break;
                case "Shield":  objWieldAttrInt = 48; break;
                case "Dual_Wield":  objWieldAttrInt = 49; break;
                case "Recklessness":  objWieldAttrInt = 50; break;
                case "Sneak_Attack":  objWieldAttrInt = 51; break;
                case "Dirty_Fighting":  objWieldAttrInt = 52; break;
            }


        }

        //public static void findWieldAttrStr(int objWieldAttrInt)
        //{
        //    switch (objWieldAttrInt)
        //    {

        //        case 0: objWieldAttrStr = "None"; break;
        //        case 34: objWieldAttrStr = "WarMagic"; break;
        //        case 41: objWieldAttrStr = "TwoHand"; break;
        //        case 44: objWieldAttrStr = "Heavy"; break;
        //        case 45: objWieldAttrStr = "Light"; break;
        //        case 46: objWieldAttrStr = "Finesse"; break;
        //        case 47: objWieldAttrStr = "Missile"; break;
        //        case 48: objWieldAttrStr = "Shield"; break;
        //        case 49: objWieldAttrStr = "Dual_Wield"; break;
        //        case 50: objWieldAttrStr = "Recklessness"; break;
        //        case 51: objWieldAttrStr = "Sneak_Attack"; break;
        //        case 52: objWieldAttrStr = "Dirty_Fighting"; break;
        //    }
        //}

        public static void findWieldAttrInt(int tempWieldAttrInt)
        {
            switch (tempWieldAttrInt)
            {

                case 0:   objWieldAttrInt = 0; break;
                case 1:  objWieldAttrInt = 41; break;
                case 2:  objWieldAttrInt = 44; break;
                case 3:  objWieldAttrInt = 45; break;
                case 4:  objWieldAttrInt = 46; break;
            }


        }

        public static void findDamageTypeInt(int tempDamageTypeInt)
        {
            switch (tempDamageTypeInt)
            {

                case 0: objDamageTypeInt = 0; break;
                case 1: objDamageTypeInt = 1; break;
                case 2: objDamageTypeInt = 2; break;
                case 3: objDamageTypeInt = 3; break;
                case 4: objDamageTypeInt = 4; break;
                case 5: objDamageTypeInt = 8; break;
                case 6: objDamageTypeInt = 16; break;
                case 7: objDamageTypeInt = 32; break;
                case 8: objDamageTypeInt = 64; break;
            }
        }

       public static void findLevelInt(int tempLevelInt)
        {
            switch (tempLevelInt)
            {

                case 0: objLevelInt = 1; break;
                case 1: objLevelInt = 0; break;
                case 2: objLevelInt = 250; break;
                case 3: objLevelInt = 270; break;
                case 4: objLevelInt = 290; break;
                case 5: objLevelInt = 300; break;
                case 6: objLevelInt = 310; break;
                case 7: objLevelInt = 315; break;
                case 8: objLevelInt = 325; break;
                case 9: objLevelInt = 330; break;
                case 10: objLevelInt = 335; break;
                case 11: objLevelInt = 350; break;
                case 12: objLevelInt = 355; break;
                case 13: objLevelInt = 360; break;
                case 14: objLevelInt = 370; break;
                case 15: objLevelInt = 375; break;
                case 16: objLevelInt = 400; break;
                case 17: objLevelInt = 420; break;
            }
        }


        public static void findClassName(int objClass)
        {
            switch (objClass)
            {
                case 0: objClassName = "None"; break;
                case 1: objClassName = "Armor"; break;
                case 2: objClassName = "Clothing"; break;
                case 3: objClassName = "Jewelry"; break;
                case 4: objClassName = "WandStaffOrb"; break;
                case 5: objClassName = "MeleeWeapon"; break;
                case 6: objClassName = "MissileWeapon"; break;
                case 7: objClassName = "Salvage"; break;
                case 8: objClassName = "Scroll"; break;
                case 9: objClassName = "HealingKit"; break;
                case 10: objClassName = "Food"; break;
                case 11: objClassName = "Gem"; break;
                case 12: objClassName = "CraftedFletching"; break;
                case 13: objClassName = "BaseCooking"; break;
                case 14: objClassName = "CraftedAlchemy"; break;
                case 15: objClassName = "SpellComponent"; break;
                case 16: objClassName = "Key"; break;
                case 17: objClassName = "Misc"; break;
                case 18: objClassName = "ManaStone"; break;
                case 19: objClassName = "BooksPaper"; break;
                case 20: objClassName = "TradeNote"; break;
                case 21: objClassName = "Money"; break;
                case 22: objClassName = "Pyreal"; break;
                case 23: objClassName = "Ust"; break;
                case 24: objClassName = "Foci"; break;
                case 25: objClassName = "Container"; break;
  
            }

        }

    

     

    }
}

