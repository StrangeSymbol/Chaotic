using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using ChaoticGameLib;

namespace Chaotic
{
    public partial class ChaoticForm : Form
    {
        GroupBox previousGbx;

        Creature[] creatures;
        Attack[] attacks;
        Battlegear[] battlegears;
        Mugic[] mugics;
        Location[] locations;

        public ChaoticForm()
        {
            InitializeComponent();
            previousGbx = null;
            pbChaoticCard.Image = Image.FromFile(GameMenu.sPath + "\\CardBack.png");
            //pbLocation1.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            creatures = new Creature[6];
            attacks = new Attack[20];
            battlegears = new Battlegear[6];
            mugics = new Mugic[6];
            locations = new Location[10];
            List<string> files = ChaoticEngine.LoadFile(ChaoticEngine.kDeckFile);
            if (files != null)
            {
                cbxDeck.Items.AddRange(files.ToArray());
            }
            addMouseClick(gbxCreatures);
            addMouseClick(gbxBattlegears);
            addMouseClick(gbxAttacks);
            addMouseClick(gbxMugics);
            addMouseClick(gbxLocations);
        }

        private void addMouseClick(GroupBox groupBox)
        {
            for (int i = 0; i < groupBox.Controls.Count; i++)
            {
                (groupBox.Controls[i] as PictureBox).MouseClick += pictureBox_MouseClick;
                (groupBox.Controls[i] as PictureBox).MouseEnter += pictureBox_MouseEnter;
            }
        }

        private void cbxCardType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cbxCardType.Items[cbxCardType.SelectedIndex] is string)
            {
                string cardType = cbxCardType.Items[cbxCardType.SelectedIndex] as string;
                if (previousGbx != null)
                    previousGbx.Visible = false;
                switch (cardType)
                {
                    case "Any":
                    case "Battlegear":
                        if (previousGbx != null)
                            previousGbx = null;
                        break;
                    case "Creature":
                        gbxCreatureSearch.Visible = true;
                        previousGbx = gbxCreatureSearch;
                        break;
                    case "Attack":
                        gbxAttackSearch.Visible = true;
                        previousGbx = gbxAttackSearch;
                        break;
                    case "Mugic":
                        gbxMugicSearch.Visible = true;
                        previousGbx = gbxMugicSearch;
                        break;
                    case "Location":
                        gbxLocationSearch.Visible = true;
                        previousGbx = gbxLocationSearch;
                        break;
                }
            }
        }

        private void btnAddToDeck_Click(object sender, EventArgs e)
        {
            if (lbxSearch.SelectedItem != null)
            {
                ChaoticCard card = lbxSearch.SelectedItem as ChaoticCard;
                addIfRightCardType<Creature>(card, gbxCreatures, creatures);
                addIfRightCardType<Battlegear>(card, gbxBattlegears, battlegears);
                addIfRightCardType<Attack>(card, gbxAttacks, attacks);
                addIfRightCardType<Mugic>(card, gbxMugics, mugics);
                addIfRightCardType<Location>(card, gbxLocations, locations);
            }
        }

        private void addIfRightCardType<T>(ChaoticCard card, GroupBox groupBox, T[] arr) where T : ChaoticCard
        {
            if (card is T)
            {
                string folderName = getFolderName(card);
                for (int i = 0; i < arr.Length; i++)
                {
                    if ((groupBox.Controls[arr.Length - 1 - i] as PictureBox).Image == null)
                    {
                        (groupBox.Controls[arr.Length - 1 - i] as PictureBox).Image =
                            Image.FromFile(GameMenu.sPath + "\\" + folderName + "\\" + card.GetType().Name + ".png");
                        arr[i] = card as T;
                        if (card is Location)
                            (groupBox.Controls[arr.Length - 1 - i] as PictureBox).Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<string> deck = checkDeck();
            if (deck != null)
            {
                ChaoticEngine.SaveFile(deck, cbxDeck.SelectedItem as string);
                MessageBox.Show("Saved Deck.", "Deck Saved", MessageBoxButtons.OK);
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            List<string> deck = checkDeck();
            if (deck != null)
            {
                List<string> files = ChaoticEngine.LoadFile(ChaoticEngine.kDeckFile);
                if (files == null)
                    files = new List<string>();
                if (!files.Contains(tbxSaveAs.Text))
                {
                    files.Add(tbxSaveAs.Text);
                    cbxDeck.Items.Add(tbxSaveAs.Text);
                    cbxDeck.SelectedIndex = cbxDeck.Items.Count - 1;
                    ChaoticEngine.SaveFile(deck, tbxSaveAs.Text);
                    ChaoticEngine.SaveFile(files, ChaoticEngine.kDeckFile);
                    MessageBox.Show("Saved Deck.", "Deck Saved", MessageBoxButtons.OK);
                }
                else
                    MessageBox.Show("Name for deck is already used.", "Deck Named Used", MessageBoxButtons.OK);
            }
        }

        private List<string> checkDeck()
        {
            if (!checkLegalDeckCount<Creature>(creatures) || !checkLegalDeckCount<Battlegear>(battlegears) 
                || !checkLegalDeckCount<Attack>(attacks) || !checkLegalDeckCount<Mugic>(mugics)
                || !checkLegalDeckCount<Location>(locations))
                return null;
            List<string> deck = new List<string>();
            if (!checkLegalCopies<Creature>(deck, creatures) || !checkMixedArmies(deck)
                || !checkLegalCopies<Battlegear>(deck, battlegears) || !checkLegalCopies<Mugic>(deck, mugics)
                || !checkLegalCopies<Attack>(deck, attacks) || !checkBuildCount() || !checkLegalCopies<Location>(deck, locations))
                return null;
            return deck;
        }

        private bool checkLegalDeckCount<T>(T[] arr) where T : ChaoticCard
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    MessageBox.Show("Deck is not fully playable, missing " + typeof(T).Name + "(s)", 
                        "Deck Not Playable", MessageBoxButtons.OK);
                    return false;
                }
            }
            return true;
        }
        private bool checkBuildCount()
        {
            byte buildCount = 0;
            foreach (Attack attack in attacks)
            {
                buildCount += attack.BuildNumber;
            }
            if (buildCount > 20)
            {
                MessageBox.Show("Attack Build Count of " + buildCount + " is to high, needs to be 20 or less.",
                        "Attack Build Count", MessageBoxButtons.OK);
                return false;
            }
            else
                return true;
        }
        private bool checkLegalCopies<T>(List<string> deck, T[] arr) where T : ChaoticCard
        {
            List<T> lst = new List<T>(arr);
            for (int i = 0; i < arr.Length; i++)
            {
                if (lst.FindAll(c => c.Name == arr[i].Name).Count > 2)
                {
                    MessageBox.Show("Deck has to many copies of " + arr[i].Name, "Deck Too Many Copies", MessageBoxButtons.OK);
                    return false;
                }
                else if (lst.FindAll(c => c.Name == arr[i].Name).Count > 1 && arr[i].Unique)
                {
                    MessageBox.Show("A deck can only have 1 " + arr[i].Name, "Only 1 In Deck", MessageBoxButtons.OK);
                    return false;
                }
                deck.Add(arr[i].Name);
            }
            return true;
        }
        private bool checkMixedArmies(List<string> deck)
        {
            List<Creature> creaturesLst = new List<Creature>(creatures);
            for (int i = 0; i < creatures.Length; i++)
            {
                if (creaturesLst.FindAll(c => c.CreatureTribe == creatures[i].CreatureTribe).Count < creatures.Length
                    && creatures[i].MixedArmies)
                {
                    MessageBox.Show(creatures[i].Name + " can't enter mixed armies.", "Mixed Armies", MessageBoxButtons.OK);
                    return false;
                }
            }
            return true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<string> files = ChaoticEngine.LoadFile(ChaoticEngine.kDeckFile);
            if (files.Count == 1)
            {
                MessageBox.Show("There has to be at least one Deck to be playable.", "No Deck Deleted", MessageBoxButtons.OK);
                return;
            }
            else if (cbxDeck.SelectedIndex == -1)
            {
                MessageBox.Show("There has to be a Deck selected.", "No Deck Selected", MessageBoxButtons.OK);
                return;
            }
            if ((cbxDeck.Items[cbxDeck.SelectedIndex] as string) == tbxSaveAs.Text)
            {
                btnClear_Click(sender, e);
            }
            else if (!cbxDeck.Items.Contains(tbxSaveAs.Text))
            {
                MessageBox.Show("No Deck has name '" + tbxSaveAs.Text + "' to be deleted.", "No Deck Deleted", MessageBoxButtons.OK);
                return;
            }

            cbxDeck.Items.Remove(tbxSaveAs.Text);
            files.Remove(tbxSaveAs.Text);
            ChaoticEngine.SaveFile(files, ChaoticEngine.kDeckFile);
            File.Delete(tbxSaveAs.Text + ".txt");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < creatures.Length; i++)
            {
                creatures[i] = null;
                (gbxCreatures.Controls[i] as PictureBox).Image = null;
                battlegears[i] = null;
                (gbxBattlegears.Controls[i] as PictureBox).Image = null;
                mugics[i] = null;
                (gbxMugics.Controls[i] as PictureBox).Image = null;
            }
            for (int i = 0; i < attacks.Length; i++)
            {
                attacks[i] = null;
                (gbxAttacks.Controls[i] as PictureBox).Image = null;
            }
            for (int i = 0; i < locations.Length; i++)
            {
                locations[i] = null;
                (gbxLocations.Controls[i] as PictureBox).Image = null;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<ChaoticCard> selectedCards;
            switch (cbxCardType.SelectedItem as string)
            {
                case "Creature":
                    selectedCards = ChaoticEngine.sCardDatabase.FindAll(c => c is Creature);
                    string tribe = cbxCreatureType.SelectedItem as string;

                    if (tribe != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).CreatureTribe.ToString() == tribe);
                    string type = cbxType.SelectedItem as string;
                    type = trimSpaces(type);
                    if (type != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).CardType.ToString() == type);
                    if (!cbAny.Checked)
                    {
                        if (cbFire.Checked)
                            selectedCards = selectedCards.FindAll(c => (c as Creature).Fire == true);
                        if (cbAir.Checked)
                            selectedCards = selectedCards.FindAll(c => (c as Creature).Air == true);
                        if (cbEarth.Checked)
                            selectedCards = selectedCards.FindAll(c => (c as Creature).Earth == true);
                        if (cbWater.Checked)
                            selectedCards = selectedCards.FindAll(c => (c as Creature).Water == true);
                        if (!cbFire.Checked && !cbAir.Checked && !cbEarth.Checked && !cbWater.Checked)
                            selectedCards = selectedCards.FindAll(c => (c as Creature).Fire == false 
                                && (c as Creature).Air == false && (c as Creature).Earth == false 
                                && (c as Creature).Water == false);
                    }
                    string mugicAbility = cbxMugicAbility.SelectedItem as string;
                    if (mugicAbility != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).MugicCounters == Convert.ToByte(mugicAbility));
                    string discipline = cbxCourage.SelectedItem as string;
                    if (discipline != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Courage == Convert.ToByte(discipline));
                    discipline = cbxPower.SelectedItem as string;
                    if (discipline != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Power == Convert.ToByte(discipline));
                    discipline = cbxWisdom.SelectedItem as string;
                    if (discipline != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Wisdom == Convert.ToByte(discipline));
                    discipline = cbxSpeed.SelectedItem as string;
                    if (discipline != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Speed == Convert.ToByte(discipline));
                    discipline = cbxEnergy.SelectedItem as string;
                    if (discipline != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Energy == Convert.ToByte(discipline));
                    string swift = cbxSwift.SelectedItem as string;
                    if (swift != "Any")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Swift == Convert.ToByte(swift));
                    if ((cbxNoMixedArmies.SelectedItem as string) == "Yes")
                       selectedCards = selectedCards.FindAll(c => (c as Creature).MixedArmies == true);
                    else if ((cbxNoMixedArmies.SelectedItem as string) == "No")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).MixedArmies == false);
                    if ((cbxRange.SelectedItem as string) == "Yes")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Range == true);
                    else if ((cbxRange.SelectedItem as string) == "No")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Range == false);
                    if ((cbxRecklessness.SelectedItem as string) == "Yes")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Recklessness > 0);
                    else if ((cbxRecklessness.SelectedItem as string) == "No")
                        selectedCards = selectedCards.FindAll(c => (c as Creature).Recklessness == 0);
                    break;
                case "Battlegear":
                    selectedCards = ChaoticEngine.sCardDatabase.FindAll(b => b is Battlegear);
                    break;
                case "Attack":
                    selectedCards = ChaoticEngine.sCardDatabase.FindAll(a => a is Attack);
                    string damage = cbxBuildNumber.SelectedItem as string;
                    if (damage != "Any")
                        selectedCards = selectedCards.FindAll(a => (a as Attack).BuildNumber == Convert.ToByte(damage));
                    damage = cbxBaseDamage.SelectedItem as string;
                    if (damage != "Any")
                        selectedCards = selectedCards.FindAll(a => (a as Attack).BaseDamage == Convert.ToByte(damage));
                    damage = cbxFireDamage.SelectedItem as string;
                    if (damage != "Any")
                        selectedCards = selectedCards.FindAll(a => (a as Attack).FireDamage == Convert.ToByte(damage));
                    damage = cbxAirDamage.SelectedItem as string;
                    if (damage != "Any")
                        selectedCards = selectedCards.FindAll(a => (a as Attack).AirDamage == Convert.ToByte(damage));
                    damage = cbxEarthDamage.SelectedItem as string;
                    if (damage != "Any")
                        selectedCards = selectedCards.FindAll(a => (a as Attack).EarthDamage == Convert.ToByte(damage));
                    damage = cbxWaterDamage.SelectedItem as string;
                    if (damage != "Any")
                        selectedCards = selectedCards.FindAll(a => (a as Attack).WaterDamage == Convert.ToByte(damage));
                    break;
                case "Mugic":
                    selectedCards = ChaoticEngine.sCardDatabase.FindAll(m => m is Mugic);
                    string mugicType = cbxMugicType.SelectedItem as string;
                    if (mugicType != "Any")
                        selectedCards = selectedCards.FindAll(m => (m as Mugic).MugicCasting.ToString() == mugicType);
                    string mugicCost = cbxMugicCost.SelectedItem as string;
                    if (mugicCost != "Any")
                        selectedCards = selectedCards.FindAll(m => (m as Mugic).Cost == Convert.ToByte(mugicCost));
                    break;
                case "Location":
                    selectedCards = ChaoticEngine.sCardDatabase.FindAll(l => l is Location);
                    string locationType = cbxInitiative.SelectedItem as string;
                    if (locationType != "Any")
                        selectedCards = selectedCards.FindAll(m => (m as Location).Initiative.ToString() == locationType);
                    break;
                default:
                    selectedCards = ChaoticEngine.sCardDatabase;
                    break;
            }
           
            if (cbxEffect.GetItemCheckState(0) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Unique);
            if (cbxEffect.GetItemCheckState(1) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Hive"));
            if (cbxEffect.GetItemCheckState(2) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Support Courage"));
            if (cbxEffect.GetItemCheckState(3) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Support Power"));
            if (cbxEffect.GetItemCheckState(4) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Support Wisdom"));
            if (cbxEffect.GetItemCheckState(5) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Support Speed"));
            if (cbxEffect.GetItemCheckState(6) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Intimidate Courage"));
            if (cbxEffect.GetItemCheckState(7) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Intimidate Power"));
            if (cbxEffect.GetItemCheckState(8) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Intimidate Wisdom"));
            if (cbxEffect.GetItemCheckState(9) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Intimidate Speed"));
            if (cbxEffect.GetItemCheckState(10) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Swift"));
            if (cbxEffect.GetItemCheckState(11) == CheckState.Checked)
                selectedCards = selectedCards.FindAll(c => c.Description().Contains("Range"));
   
            if (tbSearch.Text != String.Empty)
                selectedCards = selectedCards.FindAll(c => c.Description().ToLower().Contains(tbSearch.Text.ToLower()));
            selectedCards.Sort();
            lbxSearch.Items.Clear();
            for (int i = 0; i < selectedCards.Count; i++)
            {
                lbxSearch.Items.Add(selectedCards[i]);
            }
        }

        private string trimSpaces(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == ' ')
                    str.Remove(i, 1);
            }
            return str;
        }

        private void cbxDeck_SelectionChangeCommitted(object sender, EventArgs e)
        {
            List<string> deck = ChaoticEngine.LoadFile(cbxDeck.SelectedItem as string);

            int j = 0;
            j = loadCards<Creature>(creatures, deck, gbxCreatures, "Creatures", j);
            j = loadCards<Battlegear>(battlegears, deck, gbxBattlegears, "Battlegears", j);
            j = loadCards<Mugic>(mugics, deck, gbxMugics, "Mugics", j);
            j = loadCards<Attack>(attacks, deck, gbxAttacks, "Attacks", j);
            loadCards<Location>(locations, deck, gbxLocations, "Locations", j);

        }
        private int loadCards<T>(T[] arr, List<string> deck, GroupBox groupBox, string fileName, int j) where T : ChaoticCard
        {
            for (int i = 0; i < arr.Length; i++)
            {
                ChaoticCard card = ChaoticEngine.sCardDatabase.Find(c => c.Name == deck[i + j]).ShallowCopy();
                if (card is T)
                {
                    arr[i] = card as T;
                    (groupBox.Controls[arr.Length - 1 - i] as PictureBox).Image = Image.FromFile(GameMenu.sPath +
                        "\\" + fileName + "\\" + arr[i].GetType().Name + ".png");
                    if (card is Location)
                    {
                        (groupBox.Controls[arr.Length - 1 - i] as PictureBox).Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    }
                }
                else
                    throw new FileContentException("Card not as expected type.");
            }

            j += arr.Length;
            return j;
        }

        private void lbxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChaoticCard card = lbxSearch.SelectedItem as ChaoticCard;
            string folderName = getFolderName(card);
            pbChaoticCard.Image = Image.FromFile(GameMenu.sPath + "\\" + folderName + "\\" + card.GetType().Name + ".png");
            if (card is Location)
                pbChaoticCard.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            tbxDescription.Text = card.Description();
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
            int index = getCardIndex(gbxCreatures, pic);
            if (index != -1)
                return creatures[index].Description();
            index = getCardIndex(gbxBattlegears, pic);
            if (index != -1)
                return battlegears[index].Description();
            index = getCardIndex(gbxAttacks, pic);
            if (index != -1)
                return attacks[index].Description();
            index = getCardIndex(gbxMugics, pic);
            if (index != -1)
                return mugics[index].Description();
            index = getCardIndex(gbxLocations, pic);
            if (index != -1)
                return locations[index].Description();
            return String.Empty;
        }
        private int getCardIndex(GroupBox groupBox, PictureBox pic)
        {
            if (groupBox.Controls.Contains(pic))
            {
                for (int i = 0; i < groupBox.Controls.Count; i++)
                {
                    if (groupBox.Controls[i] == pic)
                        return groupBox.Controls.Count - 1 - i;
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
                int index = getCardIndex(gbxCreatures, pic);
                if (index != -1)
                {
                    creatures[index] = null;
                    pic.Image = null;
                    return;
                }
                index = getCardIndex(gbxBattlegears, pic);
                if (index != -1)
                {
                    battlegears[index] = null;
                    pic.Image = null;
                    return;
                }
                index = getCardIndex(gbxAttacks, pic);
                if (index != -1)
                {
                    attacks[index] = null;
                    pic.Image = null;
                    return;
                }
                index = getCardIndex(gbxMugics, pic);
                if (index != -1)
                {
                    mugics[index] = null;
                    pic.Image = null;
                    return;
                }
                index = getCardIndex(gbxLocations, pic);
                if (index != -1)
                {
                    locations[index] = null;
                    pic.Image = null;
                    return;
                }
            }
        }
    }
}