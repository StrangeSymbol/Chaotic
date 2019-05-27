using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Chaotic
{
    static class ChaoticEngine
    {
        public static bool IsACardMoving { get; set; }
        public static bool Player1Active { get; set; }
        public static bool Player1Strike { get; set; }
        public static Texture2D CoveredCard { get; set; }
        public static Texture2D OrgBackgroundSprite { get; set; }
        public static Texture2D BackgroundSprite { get; set; }
        public const int kCardWidth = 59;
        public const int kCardHeight = 82;
        public const int kCardGap = kCardHeight / 4;
        public const int kBattlegearGap = kCardHeight / 6;
        public const string kDeckFile = "Deck";
        public const string kFileFormat = ".cha";
        public static List<ChaoticGameLib.ChaoticCard> sCardDatabase;
        public static MenuStage MStage { get; set; }
        public static GameStage GStage { get; set; }
        public static BattleBoardNode sYouNode { get; set; }
        public static BattleBoardNode sEnemyNode { get; set; }
        public static bool CombatThisTurn { get; set; }
        public static bool Player1Setup { get; set; }
        public static bool Player2Setup { get; set; }
        public static bool Hive { get; set; }
        public static bool PrevHive { get; set; }
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
