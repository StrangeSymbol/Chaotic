using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using ChaoticGameLib;

namespace Chaotic
{
    public partial class DeckSetup6On6 : Form
    {
        Creature[] creatures;
        List<Creature> creatureSelect;
        int creatureIndex;
        Battlegear[] battlegears;
        List<Battlegear> battlegearSelect;
        int battlegearIndex;
        List<Attack> attacks;
        List<Mugic> mugics;
        List<Location> locations;

        List<string> deck;

        public DeckSetup6On6(string file)
        {
            InitializeComponent();
            deck = ChaoticEngine.LoadFile(file);
            pbChaoticCard.Image = Image.FromFile(GameMenu.sPath + "\\CardBack.png");
            addMouseClick(gbxBattleboard);
            pbCreatureSelect.MouseEnter += pictureBox_MouseEnter;
            pbBattlegearSelect.MouseEnter += pictureBox_MouseEnter;

            creatures = new Creature[6];
            creatureSelect = new List<Creature>();
            battlegears = new Battlegear[6];
            battlegearSelect = new List<Battlegear>();
            attacks = new List<Attack>();
            mugics = new List<Mugic>();
            locations = new List<Location>();
            int j = 0;
            j = loadCards<Creature>(creatureSelect, 6, deck, j);
            j = loadCards<Battlegear>(battlegearSelect, 6, deck, j);
            j = loadCards<Mugic>(mugics, 6, deck, j);
            j = loadCards<Attack>(attacks, 20, deck, j);
            loadCards<Location>(locations, 10, deck, j);
            creatureIndex = 0;
            battlegearIndex = 0;
            pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\" + getFolderName(creatureSelect[creatureIndex]) +
                "\\" + creatureSelect[creatureIndex].GetType().Name + ".png");
            pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\" + getFolderName(battlegearSelect[battlegearIndex]) +
                "\\" + battlegearSelect[battlegearIndex].GetType().Name + ".png");
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
                if (index / 6 == 1)
                    return creatures[index - 6].Description();
                else if (index / 6 == 2)
                    return battlegears[index - 12].Description();
                pic.Image = null;
            }

            if (pbCreatureSelect == pic && creatureIndex != -1)
                return creatureSelect[creatureIndex].Description();
            else if (pbBattlegearSelect == pic && battlegearIndex != -1)
                return battlegearSelect[battlegearIndex].Description();
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
                    if (index / 6 == 1)
                    {
                        creatureSelect.Add(creatures[index - 6]);
                        if (creatureIndex == -1)
                        {
                            creatureIndex = 0;
                            pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                   getFolderName(creatureSelect[creatureIndex]) + "\\" +
                   creatureSelect[creatureIndex].GetType().Name + ".png");
                        }
                        creatures[index - 6] = null;
                        for (int i = 0; i < cbxFromCreature.Items.Count; i++)
                        {
                            if (Convert.ToInt32(cbxFromCreature.Items[i]) == index - 5)
                                cbxFromCreature.Items.RemoveAt(i);
                        }
                        cbxToCreature.Items.Add(index - 5);
                        cbxCreatureSelect.Items.Add(index - 5);
                    }
                    else if (index / 6 == 2)
                    {
                        battlegearSelect.Add(battlegears[index - 12]);
                        if (battlegearIndex == -1)
                        {
                            battlegearIndex = 0;
                            pbBattlegearSelect.Image = pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegearSelect[battlegearIndex]) + "\\" +
                    battlegearSelect[battlegearIndex].GetType().Name + ".png");
                        }
                        battlegears[index - 12] = null;
                        for (int i = 0; i < cbxFromBattlegear.Items.Count; i++)
                        {
                            if (Convert.ToInt32(cbxFromBattlegear.Items[i]) == index - 11)
                                cbxFromBattlegear.Items.RemoveAt(i);
                        }
                        cbxToBattlegear.Items.Add(index - 11);
                        cbxBattlegearSelect.Items.Add(index - 11);
                    }
                    pic.Image = null;
                }
            }
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
                int index = Convert.ToInt32(cbxBattlegearSelect.SelectedItem)  - 1;
                if (battlegears[index] == null)
                {
                    cbxFromBattlegear.Items.Add(cbxBattlegearSelect.SelectedItem);
                    cbxToBattlegear.Items.RemoveAt(cbxBattlegearSelect.SelectedIndex);
                    battlegears[index] = battlegearSelect[battlegearIndex];
                    (gbxBattleboard.Controls[index + 12] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegears[index]) + "\\" +
                    battlegears[index].GetType().Name + ".png");
                    battlegearSelect.RemoveAt(battlegearIndex);
                    if (battlegearIndex >= battlegearSelect.Count)
                        battlegearIndex = battlegearSelect.Count - 1;
                    cbxBattlegearSelect.Items.RemoveAt(cbxBattlegearSelect.SelectedIndex);
                    if (battlegearIndex != -1)
                    {
                        pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegearSelect[battlegearIndex]) + "\\" +
                    battlegearSelect[battlegearIndex].GetType().Name + ".png");
                    }
                    else
                    {
                        pbBattlegearSelect.Image = Image.FromFile(GameMenu.sPath + "\\CardBack.png");
                        cbxBattlegearSelect.Text = "";
                    }
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
            if (cbxCreatureSelect.SelectedItem != null && creatureIndex != -1)
            {
                int index = Convert.ToInt32(cbxCreatureSelect.SelectedItem) - 1;
                if (creatures[index] == null)
                {
                    cbxFromCreature.Items.Add(cbxCreatureSelect.SelectedItem);
                    cbxToCreature.Items.RemoveAt(cbxCreatureSelect.SelectedIndex);
                    creatures[index] = creatureSelect[creatureIndex];
                    (gbxBattleboard.Controls[index + 6] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatures[index]) + "\\" +
                    creatures[index].GetType().Name + ".png");
                    creatureSelect.RemoveAt(creatureIndex);
                    if (creatureIndex >= creatureSelect.Count)
                        creatureIndex = creatureSelect.Count - 1;
                    cbxCreatureSelect.Items.RemoveAt(cbxCreatureSelect.SelectedIndex);
                    if (creatureIndex != -1)
                    {
                        pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatureSelect[creatureIndex]) + "\\" +
                    creatureSelect[creatureIndex].GetType().Name + ".png");
                    }
                    else
                    {
                        pbCreatureSelect.Image = Image.FromFile(GameMenu.sPath + "\\CardBack.png");
                        cbxCreatureSelect.Text = "";
                    }
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
                (gbxBattleboard.Controls[toIndex + 6] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(creatures[toIndex]) + "\\" +
                    creatures[toIndex].GetType().Name + ".png");
                (gbxBattleboard.Controls[fromIndex + 6] as PictureBox).Image = null;
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
                (gbxBattleboard.Controls[toIndex + 12] as PictureBox).Image = Image.FromFile(GameMenu.sPath + "\\" +
                    getFolderName(battlegears[toIndex]) + "\\" +
                    battlegears[toIndex].GetType().Name + ".png");
                (gbxBattleboard.Controls[fromIndex + 12] as PictureBox).Image = null;
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

        private void DeckSetup6On6_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ChaoticEngine.Player1Setup && !ChaoticEngine.Player2Setup)
                ChaoticEngine.MStage = MenuStage.Test;
            else if (ChaoticEngine.MStage == MenuStage.Ready6On6 && !ChaoticEngine.Player2Setup)
            {
                ChaoticEngine.MStage = MenuStage.Test;
                ChaoticEngine.Player1Setup = false;
            }
        }
    }
}