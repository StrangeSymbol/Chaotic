/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chaotic
{
    static class ChaoticEngine
    {
        public static bool IsACardMoving { get; set; }
        public static bool Player1Active { get; set; }
        public static bool Player1Strike { get; set; }
        public static Texture2D OrgBackgroundSprite { get; set; }
        public static Texture2D BackgroundSprite { get; set; }
        public const int kCardWidth = 59;
        public const int kCardHeight = 82;
        public const int kCardGap = kCardHeight / 4;
        public const int kBattlegearGap = kCardHeight / 6;
        public const string kDeckFile = "Deck";
        public const string kFileFormat = ".cha";
        public const int kCardCoveredWidth = 175;
        public const int kCardCoveredHeight = 245;
        public static List<ChaoticGameLib.ChaoticCard> sCardDatabase;
        public static MenuStage MStage { get; set; }
        public static GameStage GStage { get; set; }
        public static BattleBoardNode sYouNode { get; set; }
        public static BattleBoardNode sEnemyNode { get; set; }
        public static int ReturnSelectedIndex1 { get; set; }
        public static int ReturnSelectedIndex2 { get; set; }
        public static bool CombatThisTurn { get; set; }
        public static bool Player1Setup { get; set; }
        public static bool Player2Setup { get; set; }
        public static bool Hive { get; set; }
        public static bool PrevHive { get; set; }
        public static bool GenericMugicOnly { get; set; }
        public static CardHighlighter Highlighter { get; set; } // When a card is added to burst puts a sheen on that card. 
        public static ChaoticMessageBox MsgBox { get; set; } // Checks whether a player wants to add to Burst.
        public static Ability CurrentAbility { get; set; }
        public static BattleBoardNode SelectedNode { get; set; }
        public static bool HasMarquisDarini { get; set; }
        
        public static MouseState PrevState { get; set; }
        public static List<ChaoticGameLib.Creature> sCreatures1;
        public static List<ChaoticGameLib.Battlegear> sBattlegears1;
        public static List<ChaoticGameLib.Attack> sAttacks1;
        public static List<ChaoticGameLib.Mugic> sMugics1;
        public static List<ChaoticGameLib.Location> sLocations1;
        public static List<ChaoticGameLib.Creature> sCreatures2;
        public static List<ChaoticGameLib.Battlegear> sBattlegears2;
        public static List<ChaoticGameLib.Attack> sAttacks2;
        public static List<ChaoticGameLib.Mugic> sMugics2;
        public static List<ChaoticGameLib.Location> sLocations2;

        public static void SaveFile(List<string> lst, string file)
        {
            if (File.Exists(file + kFileFormat))
                File.Delete(file + kFileFormat);
            using (FileStream w = new FileStream(file + kFileFormat, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(w, lst);
            }
        }

        public static List<string> LoadFile(string file)
        {
            if (File.Exists(file + kFileFormat))
            {
                using (FileStream r = new FileStream(file + kFileFormat, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (List<string>)formatter.Deserialize(r);
                }
            }
            else
                return null;
        }
    }
}
