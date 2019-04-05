using System;
using System.Collections.Generic;
using System.IO;
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
        public static List<ChaoticGameLib.ChaoticCard> sCardDatabase;
        public static MenuStage MStage { get; set; }
        public static GameStage GStage { get; set; }
        public static BattleBoardNode sYouNode { get; set; }
        public static BattleBoardNode sEnemyNode { get; set; }
        public static bool CombatThisTurn { get; set; }
        public static bool Player1Setup { get; set; }
        public static bool Player2Setup { get; set; }
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
            if (File.Exists(file + ".txt"))
                File.Delete(file + ".txt");
            using (StreamWriter w = new StreamWriter(file + ".txt"))
            {
                foreach (string line in lst)
                {
                    w.WriteLine(line);
                }
            }
        }

        public static List<string> LoadFile(string file)
        {
            if (File.Exists(file + ".txt"))
            {
                using (StreamReader r = File.OpenText(file + ".txt"))
                {
                    List<string> loadText = new List<string>();
                    string line = "";
                    while (line != null)
                    {
                        line = r.ReadLine();
                        if (line != null)
                            loadText.Add(line);
                    }
                    return loadText;
                }
            }
            else
                return null;
        }
    }
}
