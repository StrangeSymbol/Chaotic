using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using ChaoticGameLib;

namespace Chaotic
{
    public partial class DeckSetup3On3 : Form
    {
        int creatureIndex;
        Creature[] creatures;
        List<Creature> creatureSelect;
        int battlegearIndex;
        Battlegear[] battlegears;
        List<Battlegear> battlegearSelect;
        int mugicIndex;
        Mugic[] mugics;
        List<Mugic> mugicSelect;
        int attackIndex;
        Attack[] attacks;
        List<Attack> attackSelect;
        int locationIndex;
        Location[] locations;
        List<Location> locationSelect;

        List<string> deck;

        public DeckSetup3On3(string file)
        {
            InitializeComponent();
            deck = ChaoticEngine.LoadFile(file);
            pbChaoticCard.Image = Image.FromFile(GameMenu.sPath + "\\CardBack.png");
            addMouseClick(gbxBattleboard);
            addMouseClick(gbxAttacks);
            addMouseClick(gbxMugics);
            addMouseClick(gbxLocations);
            pbCreatureSelect.MouseEnter += pictureBox_MouseEnter;
            pbBattlegearSelect.MouseEnter += pictureBox_MouseEnter;
            pbMugicSelect.MouseEnter += pictureBox_MouseEnter;
            pbAttackSelect.MouseEnter += pictureBox_MouseEnter;
            pbLocationSelect.MouseEnter += pictureBox_MouseEnter;

            creatures = new Creature[3];
            creatureSelect = new List<Creature>();
            battlegears = new Battlegear[3];
            battlegearSelect = new List<Battlegear>();
            mugics = new Mugic[3];
            mugicSelect = new List<Mugic>();
            attacks = new Attack[10];
            attackSelect = new List<Attack>();
            locations = new Location[5];
            locationSelect = new List<Location>();
            int j = 0;
            j = loadCards<Creature>(creatureSelect, 6, deck, j);
            j = loadCards<Battlegear>(battlegearSelect, 6, deck, j);
            j = loadCards<Mugic>(mugicSelect, 6, deck, j);
            j = loadCards<Attack>(attackSelect, 20, deck, j);
            loadCards<Location>(locationSelect, 10, deck, j);
            creatureIndex = 0;
            battlegearIndex = 0;
            mugicIndex = 0;
            attackIndex = 0;
            locationIndex = 0;
            pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\" + getFolderName(creatureSelect[creatureIndex]) +
                "\\" + creatureSelect[creatureIndex].GetType().Name + ".png");
            pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\" + getFolderName(battlegearSelect[battlegearIndex]) +
                "\\" + battlegearSelect[battlegearIndex].GetType().Name + ".png");
            pbMugicSelect.Image = Image.FromFile(GameMenu.sPath + "\\" + getFolderName(mugicSelect[mugicIndex]) +
                "\\" + mugicSelect[mugicIndex].GetType().Name + ".png");
            pbAttackSelect.Image = Image.FromFile(GameMenu.sPath + "\\" + getFolderName(attackSelect[attackIndex]) +
                "\\" + attackSelect[attackIndex].GetType().Name + ".png");
            pbLocationSelect.Image = Image.FromFile(GameMenu.sPath + "\\" + getFolderName(locationSelect[locationIndex]) +
                "\\" + locationSelect[locationIndex].GetType().Name + ".png");
            pbLocationSelect.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        private void addMouseClick(GroupBox groupBox)
        {
            for (int i = 0; i < groupBox.Controls.Count; i++)
            {
                if (groupBox.Controls[i] is PictureBox)
                {
                    (groupBox.Controls[i] as PictureBox).MouseClick += pictureBox_MouseClick;
                    (groupBox.Controls[i] as PictureBox).MouseEnter += pictureBox_MouseEnter;
                }
            }
        }

        private int loadCards<T>(List<T> lst, int count, List<string> deck, int j) where T : ChaoticCard
        {
            for (int i = 0; i < count; i++)
            {
                ChaoticCard card = ChaoticEngine.sCardDatabase.Find(c => c.Name == deck[i + j]).ShallowCopy();
                if (card is T)
                    lst.Add(card as T);
                else
                    throw new FileContentException("Card not as expected type.");
            }

            j += count;
            return j;
        }

        private string getFolderName(ChaoticCard card)
        {
            if (card is Creature)
                return "Creatures";
            else if (card is Battlegear)
                return "Battlegears";
            else if (card is Attack)
                return "Attacks";
            else if (card is Mugic)
                return "Mugics";
            else if (card is Location)
                return "Locations";
            else
                return String.Empty;
        }

        private string getDescription(PictureBox pic)
        {
            int index = getCardIndex(gbxBattleboard, pic);
            if (index != -1)
            {
                if (index / 3 == 1)
                    return creatures[index - 3].Description();
                else if (index / 3 == 2)
                    return battlegears[index - 6].Description();
                pic.Image = null;
            }
            index = getCardIndex(gbxAttacks, pic);
            if (index != -1)
                return attacks[index].Description();
            index = getCardIndex(gbxMugics, pic);
            if (index != -1)
                return mugics[index].Description();
            index = getCardIndex(gbxLocations, pic);
            if (index != -1)
                return locations[index].Description();

            if (pbCreatureSelect == pic && creatureIndex != -1)
                return creatureSelect[creatureIndex].Description();
            else if (pbBattlegearSelect == pic && battlegearIndex != -1)
                return battlegearSelect[battlegearIndex].Description();
            else if (pbMugicSelect == pic && mugicIndex != -1)
                return mugicSelect[mugicIndex].Description();
            else if (pbAttackSelect == pic && attackIndex != -1)
                return attackSelect[attackIndex].Description();
            else if (pbLocationSelect == pic && locationIndex != -1)
                return locationSelect[locationIndex].Description();
            return String.Empty;
        }
        private int getCardIndex(GroupBox groupBox, PictureBox pic)
        {
            if (groupBox.Controls.Contains(pic))
            {
                for (int i = 0; i < groupBox.Controls.Count; i++)
                {
                    if (groupBox.Controls[i] == pic)
                        return i;
                }
            }
            return -1;
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (pic.Image != null)
            {
                pbChaoticCard.Image = pic.Image;
                tbxDescription.Text = getDescription(pic);
            }
        }
        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            if (e.Button == MouseButtons.Right && pic.Image != null)
            {
                int index = getCardIndex(gbxBattleboard, pic);
                if (index != -1)
                {
                    if (index / 3 == 1)
                    {
                        creatureSelect.Add(creatures[index - 3]);
                        creatures[index - 3] = null;
                        for (int i = 0; i < cbxFromCreature.Items.Count; i++)
                        {
                            if (Convert.ToInt32(cbxFromCreature.Items[i]) == index - 2)
                                cbxFromCreature.Items.RemoveAt(i);
                        }
                        cbxToCreature.Items.Add(index - 2);
                        cbxCreatureSelect.Items.Add(index - 2);
                    }
                    else if (index / 3 == 2)
                    {
                        battlegearSelect.Add(battlegears[index - 6]);
                        battlegears[index - 6] = null;
                        for (int i = 0; i < cbxFromBattlegear.Items.Count; i++)
                        {
                            if (Convert.ToInt32(cbxFromBattlegear.Items[i]) == index - 5)
                                cbxFromBattlegear.Items.RemoveAt(i);
                        }
                        cbxToBattlegear.Items.Add(index - 5);
                        cbxBattlegearSelect.Items.Add(index - 5);
                    }
                    pic.Image = null;
                }
                index = getCardIndex(gbxAttacks, pic);
                if (index != -1)
                {
                    attackSelect.Add(attacks[index]);
                    attacks[index] = null;
                    pic.Image = null;
                }
                index = getCardIndex(gbxMugics, pic);
                if (index != -1)
                {
                    mugicSelect.Add(mugics[index]);
                    mugics[index] = null;
                    cbxMugicSelect.Items.Add(index + 1);
                    pic.Image = null;
                }
                index = getCardIndex(gbxLocations, pic);
                if (index != -1)
                {
                    locationSelect.Add(locations[index]);
                    locations[index] = null;
                    pic.Image = null;
                }
            }
        }

        private void sort(ComboBox comboBox)
        {
            List<int> lst = new List<int>();
            for (int i = 0; i < comboBox.Items.Count; i++)
                lst.Add(Convert.ToInt32(comboBox.Items[i]));
            lst.Sort();
            comboBox.Items.Clear();
            for (int i = 0; i < lst.Count; i++)
                comboBox.Items.Add(lst[i]);
        }

        private void btnRBattlegear_Click(object sender, EventArgs e)
        {
            if (battlegearIndex < battlegearSelect.Count - 1)
            {
                battlegearIndex++;
                pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegearSelect[battlegearIndex]) + "\\" +
                    battlegearSelect[battlegearIndex].GetType().Name + ".png");
            }
        }

        private void btnLBattlegear_Click(object sender, EventArgs e)
        {
            if (battlegearIndex > 0)
            {
                battlegearIndex--;
                pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegearSelect[battlegearIndex]) + "\\" +
                    battlegearSelect[battlegearIndex].GetType().Name + ".png");
            }
        }

        private void btnAddBattlegear_Click(object sender, EventArgs e)
        {
            if (cbxBattlegearSelect.SelectedItem != null && battlegearIndex != -1)
            {
                int index = Convert.ToInt32(cbxBattlegearSelect.SelectedItem) - 1;
                if (battlegears[index] == null)
                {
                    cbxFromBattlegear.Items.Add(cbxBattlegearSelect.SelectedItem);
                    cbxToBattlegear.Items.RemoveAt(cbxBattlegearSelect.SelectedIndex);
                    battlegears[index] = battlegearSelect[battlegearIndex];
                    (gbxBattleboard.Controls[index + 6] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegears[index]) + "\\" +
                    battlegears[index].GetType().Name + ".png");
                    battlegearSelect.RemoveAt(battlegearIndex);
                    if (battlegearIndex >= battlegearSelect.Count)
                        battlegearIndex = battlegearSelect.Count - 1;
                    cbxBattlegearSelect.Items.RemoveAt(cbxBattlegearSelect.SelectedIndex);
                    pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegearSelect[battlegearIndex]) + "\\" +
                    battlegearSelect[battlegearIndex].GetType().Name + ".png");
                }
            }
        }

        private void btnRCreature_Click(object sender, EventArgs e)
        {
            if (creatureIndex < creatureSelect.Count - 1)
            {
                creatureIndex++;
                pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatureSelect[creatureIndex]) + "\\" +
                    creatureSelect[creatureIndex].GetType().Name + ".png");
            }
        }

        private void btnLCreature_Click(object sender, EventArgs e)
        {
            if (creatureIndex > 0)
            {
                creatureIndex--;
                pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatureSelect[creatureIndex]) + "\\" +
                    creatureSelect[creatureIndex].GetType().Name + ".png");
            }
        }

        private void btnAddCreature_Click(object sender, EventArgs e)
        {
            if (cbxCreatureSelect.SelectedItem != null)
            {
                int index = Convert.ToInt32(cbxCreatureSelect.SelectedItem) - 1;
                if (creatures[index] == null)
                {
                    cbxFromCreature.Items.Add(cbxCreatureSelect.SelectedItem);
                    cbxToCreature.Items.RemoveAt(cbxCreatureSelect.SelectedIndex);
                    creatures[index] = creatureSelect[creatureIndex];
                    (gbxBattleboard.Controls[index + 3] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatures[index]) + "\\" +
                    creatures[index].GetType().Name + ".png");
                    creatureSelect.RemoveAt(creatureIndex);
                    if (creatureIndex >= creatureSelect.Count)
                        creatureIndex = creatureSelect.Count - 1;
                    cbxCreatureSelect.Items.RemoveAt(cbxCreatureSelect.SelectedIndex);
                    pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatureSelect[creatureIndex]) + "\\" +
                    creatureSelect[creatureIndex].GetType().Name + ".png");
                }
            }
        }

        private void btnSwitchCreature_Click(object sender, EventArgs e)
        {
            if (cbxFromCreature.SelectedItem != null && cbxToCreature.SelectedItem != null)
            {
                int toIndex = Convert.ToInt32(cbxToCreature.SelectedItem) - 1;
                int fromIndex = Convert.ToInt32(cbxFromCreature.SelectedItem) - 1;
                creatures[toIndex] = creatures[fromIndex];
                creatures[fromIndex] = null;
                (gbxBattleboard.Controls[toIndex + 3] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatures[toIndex]) + "\\" +
                    creatures[toIndex].GetType().Name + ".png");
                (gbxBattleboard.Controls[fromIndex + 3] as PictureBox).Image = null;
                cbxToCreature.Items.Add(cbxFromCreature.SelectedItem);
                cbxFromCreature.Items.Add(cbxToCreature.SelectedItem);
                cbxCreatureSelect.Items.Add(cbxFromCreature.SelectedItem);
                cbxCreatureSelect.Items.RemoveAt(cbxToCreature.SelectedIndex);
                cbxToCreature.Items.RemoveAt(cbxToCreature.SelectedIndex);
                cbxFromCreature.Items.RemoveAt(cbxFromCreature.SelectedIndex);
            }
        }

        private void btnSwitchBattlegear_Click(object sender, EventArgs e)
        {
            if (cbxFromBattlegear.SelectedItem != null && cbxToBattlegear.SelectedItem != null)
            {
                int toIndex = Convert.ToInt32(cbxToBattlegear.SelectedItem) - 1;
                int fromIndex = Convert.ToInt32(cbxFromBattlegear.SelectedItem) - 1;
                battlegears[toIndex] = battlegears[fromIndex];
                battlegears[fromIndex] = null;
                (gbxBattleboard.Controls[toIndex + 6] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegears[toIndex]) + "\\" +
                    battlegears[toIndex].GetType().Name + ".png");
                (gbxBattleboard.Controls[fromIndex + 6] as PictureBox).Image = null;
                cbxToBattlegear.Items.Add(cbxFromBattlegear.SelectedItem);
                cbxFromBattlegear.Items.Add(cbxToBattlegear.SelectedItem);
                cbxBattlegearSelect.Items.Add(cbxFromBattlegear.SelectedItem);
                cbxBattlegearSelect.Items.RemoveAt(cbxToBattlegear.SelectedIndex);
                cbxToBattlegear.Items.RemoveAt(cbxToBattlegear.SelectedIndex);
                cbxFromBattlegear.Items.RemoveAt(cbxFromBattlegear.SelectedIndex);
            }
        }

        private void btnLetsGetChaotic_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < creatures.Length; i++)
            {
                if (creatures[i] == null)
                {
                    MessageBox.Show("Missing a Creature card.", "Battleboard Missing Creature", MessageBoxButtons.OK);
                    return;
                }
            }

            for (int i = 0; i < battlegears.Length; i++)
            {
                if (battlegears[i] == null)
                {
                    MessageBox.Show("Missing a Battlegear card.", "Battleboard Missing Battlegear", MessageBoxButtons.OK);
                    return;
                }
            }

            for (byte i = 0; i < mugics.Length; i++)
            {
                if (mugics[i] == null)
                {
                    MessageBox.Show("Missing a Mugic card.", "Missing Mugic", MessageBoxButtons.OK);
                    return;
                }
            }

            for (byte i = 0; i < attacks.Length; i++)
            {
                if (attacks[i] == null)
                {
                    MessageBox.Show("Missing a Attack card.", "Missing Attack", MessageBoxButtons.OK);
                    return;
                }
            }
            int buildCount = attacks.Sum(c => c.BuildNumber);
            if (buildCount > 10)
            {
                MessageBox.Show("The build count: " + buildCount + " is to high, must have build count less or equal to 10.",
                    "Build Count To High.", MessageBoxButtons.OK);
                return;
            }

            for (byte i = 0; i < locations.Length; i++)
            {
                if (locations[i] == null)
                {
                    MessageBox.Show("Missing a Location card.", "Missing Location", MessageBoxButtons.OK);
                    return;
                }
            }

            if (!ChaoticEngine.Player1Setup)
            {
                ChaoticEngine.sCreatures1 = new List<Creature>();
                ChaoticEngine.sCreatures1.AddRange(creatures);
                ChaoticEngine.sBattlegears1 = new List<Battlegear>();
                ChaoticEngine.sBattlegears1.AddRange(battlegears);
                ChaoticEngine.sAttacks1 = new List<Attack>();
                ChaoticEngine.sAttacks1.AddRange(attacks);
                ChaoticEngine.sMugics1 = new List<Mugic>();
                ChaoticEngine.sMugics1.AddRange(mugics);
                ChaoticEngine.sLocations1 = new List<Location>();
                ChaoticEngine.sLocations1.AddRange(locations);
                ChaoticEngine.Player1Setup = true;
            }

            else if (!ChaoticEngine.Player2Setup)
            {
                ChaoticEngine.sCreatures2 = new List<Creature>();
                ChaoticEngine.sCreatures2.AddRange(creatures);
                ChaoticEngine.sBattlegears2 = new List<Battlegear>();
                ChaoticEngine.sBattlegears2.AddRange(battlegears);
                ChaoticEngine.sAttacks2 = new List<Attack>();
                ChaoticEngine.sAttacks2.AddRange(attacks);
                ChaoticEngine.sMugics2 = new List<Mugic>();
                ChaoticEngine.sMugics2.AddRange(mugics);
                ChaoticEngine.sLocations2 = new List<Location>();
                ChaoticEngine.sLocations2.AddRange(locations);
                ChaoticEngine.Player2Setup = true;
            }
            this.Close();
        }

        private void btnLAttack_Click(object sender, EventArgs e)
        {
            if (attackIndex > 0)
            {
                attackIndex--;
                pbAttackSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(attackSelect[attackIndex]) + "\\" +
                    attackSelect[attackIndex].GetType().Name + ".png");
            }
        }

        private void btnRAttack_Click(object sender, EventArgs e)
        {
            if (attackIndex < attackSelect.Count - 1)
            {
                attackIndex++;
                pbAttackSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(attackSelect[attackIndex]) + "\\" +
                    attackSelect[attackIndex].GetType().Name + ".png");
            }
        }

        private void btnAddAttack_Click(object sender, EventArgs e)
        {
            byte index = 0;
            for (byte i = 0; i < attacks.Length; i++)
            {
                if (attacks[i] == null)
                    index = i;
            }
            if (attacks[index] == null)
            {
                attacks[index] = attackSelect[attackIndex];
                (gbxAttacks.Controls[index] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                getFolderName(attacks[index]) + "\\" +
                attacks[index].GetType().Name + ".png");
                attackSelect.RemoveAt(attackIndex);
                if (attackIndex >= attackSelect.Count)
                    attackIndex = attackSelect.Count - 1;
                pbAttackSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                getFolderName(attackSelect[attackIndex]) + "\\" +
                attackSelect[attackIndex].GetType().Name + ".png");
            }
        }

        private void btnLMugic_Click(object sender, EventArgs e)
        {
            if (mugicIndex > 0)
            {
                mugicIndex--;
                pbMugicSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(mugicSelect[mugicIndex]) + "\\" +
                    mugicSelect[mugicIndex].GetType().Name + ".png");
            }
        }

        private void btnRMugic_Click(object sender, EventArgs e)
        {
            if (mugicIndex < mugicSelect.Count - 1)
            {
                mugicIndex++;
                pbMugicSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(mugicSelect[mugicIndex]) + "\\" +
                    mugicSelect[mugicIndex].GetType().Name + ".png");
            }
        }

        private void btnAddMugic_Click(object sender, EventArgs e)
        {
            if (cbxMugicSelect.SelectedItem != null)
            {
                int index = Convert.ToInt32(cbxMugicSelect.SelectedItem) - 1;
                if (mugics[index] == null)
                {
                    mugics[index] = mugicSelect[mugicIndex];
                    (gbxMugics.Controls[index] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(mugics[index]) + "\\" +
                    mugics[index].GetType().Name + ".png");
                    mugicSelect.RemoveAt(mugicIndex);
                    if (mugicIndex >= mugicSelect.Count)
                        mugicIndex = mugicSelect.Count - 1;
                    cbxMugicSelect.Items.RemoveAt(cbxMugicSelect.SelectedIndex);
                    pbMugicSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(mugicSelect[mugicIndex]) + "\\" +
                    mugicSelect[mugicIndex].GetType().Name + ".png");
                }
            }
        }

        private void btnLLocation_Click(object sender, EventArgs e)
        {
            if (locationIndex > 0)
            {
                locationIndex--;
                pbLocationSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(locationSelect[locationIndex]) + "\\" +
                    locationSelect[locationIndex].GetType().Name + ".png");
                pbLocationSelect.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
        }

        private void btnRLocation_Click(object sender, EventArgs e)
        {
            if (locationIndex < locationSelect.Count - 1)
            {
                locationIndex++;
                pbLocationSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(locationSelect[locationIndex]) + "\\" +
                    locationSelect[locationIndex].GetType().Name + ".png");
                pbLocationSelect.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
        }

        private void btnAddLocation_Click(object sender, EventArgs e)
        {
            byte index = 0;
            for (byte i = 0; i < locations.Length; i++)
            {
                if (locations[i] == null)
                    index = i;
            }
            if (locations[index] == null)
            {
                locations[index] = locationSelect[locationIndex];
                (gbxLocations.Controls[index] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                getFolderName(locations[index]) + "\\" +
                locations[index].GetType().Name + ".png");
                (gbxLocations.Controls[index] as PictureBox).Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                locationSelect.RemoveAt(locationIndex);
                if (locationIndex >= locationSelect.Count)
                    locationIndex = locationSelect.Count - 1;
                pbLocationSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                getFolderName(locationSelect[locationIndex]) + "\\" +
                locationSelect[locationIndex].GetType().Name + ".png");
                pbLocationSelect.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
        }

        private void DeckSetup3On3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ChaoticEngine.Player1Setup && !ChaoticEngine.Player2Setup)
                ChaoticEngine.MStage = MenuStage.Test;
            else if (ChaoticEngine.MStage == MenuStage.Ready3On3 && !ChaoticEngine.Player2Setup)
            {
                ChaoticEngine.MStage = MenuStage.Test;
                ChaoticEngine.Player1Setup = false;
            }
        }
    }
}