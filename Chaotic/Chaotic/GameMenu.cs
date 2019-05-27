using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChaoticGameLib;
using ChaoticGameLib.Attacks;
using ChaoticGameLib.Creatures;
using ChaoticGameLib.Battlegears;
using ChaoticGameLib.Mugics;
using ChaoticGameLib.Locations;

namespace Chaotic
{
    enum MenuStage
    {
        Test = 1, DeckEdit, MainMenu, InGame, OneOnOne, ThreeOnThree, SixOnSix, Wait1On1, Ready1On1, Wait3On3, Ready3On3,
        Wait6On6, Ready6On6,
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameMenu : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D title;

        MenuStage selectedStage;

        MenuButton[] mainButtons;
        MenuButton[] testButtons;

        BattleBoard battleBoard;

        int j;
        double elapsedTime;

        public static string sPath;

        public GameMenu()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 680;
            this.Window.Title = "Chaotic: Dawn Of Perim";
            graphics.ApplyChanges();
            sPath = System.IO.Path.GetFullPath("CardBack.png");
            sPath = System.IO.Path.GetDirectoryName(sPath);
            int integer = sPath.LastIndexOf("Chaotic");
            sPath = sPath.Remove(integer);
            sPath += "ChaoticContent";
            ChaoticEngine.MStage = MenuStage.MainMenu;
            mainButtons = new MenuButton[2];
            testButtons = new MenuButton[4];
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

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

            title = Content.Load<Texture2D>(@"Menu/ChaoticSymbol");

            loadMenuButtons(graphics);

            Texture2D overlay = Content.Load<Texture2D>("CardOutline");
            ChaoticEngine.sCardDatabase = new List<ChaoticCard>() 
            { new Arias(Content.Load<Texture2D>(@"Creatures\Arias"), overlay, 50, 55, 65, 30, 55),
              new Attacat(Content.Load<Texture2D>(@"Creatures\Attacat"), overlay, 50, 30, 65, 60, 105),
              new Blazier(Content.Load<Texture2D>(@"Creatures\Blazier"), overlay, 40, 35, 40, 60, 25),
              new Blugon(Content.Load<Texture2D>(@"Creatures\Blugon"), overlay, 40, 35, 65, 70, 45),
              new Bodal(Content.Load<Texture2D>(@"Creatures\Bodal"), overlay, 45, 40, 40, 80, 60),
              new Crawsectus(Content.Load<Texture2D>(@"Creatures\Crawsectus"), overlay, 50, 60, 45, 40, 25),
              new Donmar(Content.Load<Texture2D>(@"Creatures\Donmar"), overlay, 45, 45, 65, 65, 50),
              new Dractyl(Content.Load<Texture2D>(@"Creatures\Dractyl"), overlay, 40, 25, 70, 50, 70),
              new Frafdo(Content.Load<Texture2D>(@"Creatures\Frafdo"), overlay, 35, 85, 80, 45, 75),
              new Gespedan(Content.Load<Texture2D>(@"Creatures\Gespedan"), overlay, 35, 45, 50, 35, 100),
              new Heptadd(Content.Load<Texture2D>(@"Creatures\Heptadd"), overlay, 55, 55, 60, 50, 40),
              new Intress(Content.Load<Texture2D>(@"Creatures\Intress"), overlay, 40, 40, 35, 40, 55),
              new Laarina(Content.Load<Texture2D>(@"Creatures\Laarina"), overlay, 30, 35, 25, 30, 30),
              new Maglax(Content.Load<Texture2D>(@"Creatures\Maglax"), overlay, 40, 70, 60, 25, 30),
              new Maxxor(Content.Load<Texture2D>(@"Creatures\Maxxor"), overlay, 60, 100, 65, 80, 50),
              new Najarin(Content.Load<Texture2D>(@"Creatures\Najarin"), overlay, 30, 60, 30, 90, 35),
              new Owis(Content.Load<Texture2D>(@"Creatures\Owis"), overlay, 45, 65, 25, 55, 30),
              new Psimion(Content.Load<Texture2D>(@"Creatures\Psimion"), overlay, 45, 45, 30, 55, 40),
              new Rellim(Content.Load<Texture2D>(@"Creatures\Rellim"), overlay, 50, 50, 65, 60, 40),
              new Slurhk(Content.Load<Texture2D>(@"Creatures\Slurhk"), overlay, 50, 45, 35, 35, 40),
              new Staluk(Content.Load<Texture2D>(@"Creatures\Staluk"), overlay, 50, 35, 45, 25, 40),
              new TangathToborn(Content.Load<Texture2D>(@"Creatures\TangathToborn"), overlay, 30, 40, 45, 40, 30),
              new Tartarek(Content.Load<Texture2D>(@"Creatures\Tartarek"), overlay, 35, 90, 45, 90, 25),
              new Velreth(Content.Load<Texture2D>(@"Creatures\Velreth"), overlay, 45, 55, 80, 40, 25),
              new Vidav(Content.Load<Texture2D>(@"Creatures\Vidav"), overlay, 35, 45, 30, 50, 35),
              new Xaerv(Content.Load<Texture2D>(@"Creatures\Xaerv"), overlay, 30, 40, 25, 45, 60),
              new Yokkis(Content.Load<Texture2D>(@"Creatures\Yokkis"), overlay, 50, 20, 20, 20, 20),
              new Zalic(Content.Load<Texture2D>(@"Creatures\Zalic"), overlay, 45, 45, 55, 40, 40),
              new BarathBeyond(Content.Load<Texture2D>(@"Creatures\BarathBeyond"), overlay, 60, 40, 85, 15, 65),
              new BorthMajar(Content.Load<Texture2D>(@"Creatures\BorthMajar"), overlay, 45, 45, 90, 85, 40),
              new Chaor(Content.Load<Texture2D>(@"Creatures\Chaor"), overlay, 70, 95, 90, 70, 60),
              new Dardemus(Content.Load<Texture2D>(@"Creatures\Dardemus"), overlay, 50, 60, 75, 20, 65),
              new Drakness(Content.Load<Texture2D>(@"Creatures\Drakness"), overlay, 45, 75, 40, 85, 55),
              new Ghuul(Content.Load<Texture2D>(@"Creatures\Ghuul"), overlay, 35, 50, 85, 15, 35),
              new Grook(Content.Load<Texture2D>(@"Creatures\Grook"), overlay, 50, 30, 100, 20, 60),
              new Hearring(Content.Load<Texture2D>(@"Creatures\Hearring"), overlay, 50, 70, 30, 55, 45),
              new Kerric(Content.Load<Texture2D>(@"Creatures\Kerric"), overlay, 50, 50, 30, 45, 65),
              new Khybon(Content.Load<Texture2D>(@"Creatures\Khybon"), overlay, 40, 75, 25, 85, 20),
              new Klasp(Content.Load<Texture2D>(@"Creatures\Klasp"), overlay, 65, 65, 90, 30, 65),
              new Krekk(Content.Load<Texture2D>(@"Creatures\Krekk"), overlay, 40, 10, 85, 20, 60),
              new Kughar(Content.Load<Texture2D>(@"Creatures\Kughar"), overlay, 50, 65, 85, 25, 45),
              new LordVanBloot(Content.Load<Texture2D>(@"Creatures\LordVanBloot"), overlay, 65, 75, 115, 50, 95),
              new Magmon(Content.Load<Texture2D>(@"Creatures\Magmon"), overlay, 55, 75, 60, 20, 35),
              new Miklon(Content.Load<Texture2D>(@"Creatures\Miklon"), overlay, 50, 20, 75, 30, 60),
              new Nauthilax(Content.Load<Texture2D>(@"Creatures\Nauthilax"), overlay, 60, 65, 60, 45, 50),
              new Pyrithion(Content.Load<Texture2D>(@"Creatures\Pyrithion"), overlay, 50, 45, 80, 40, 65),
              new Rarran(Content.Load<Texture2D>(@"Creatures\Rarran"), overlay, 50, 65, 60, 30, 60),
              new Rothar(Content.Load<Texture2D>(@"Creatures\Rothar"), overlay, 70, 75, 95, 25, 50),
              new Skithia(Content.Load<Texture2D>(@"Creatures\Skithia"), overlay, 35, 65, 25, 55, 40),
              new Skreeth(Content.Load<Texture2D>(@"Creatures\Skreeth"), overlay, 55, 80, 65, 60, 20),
              new Solvis(Content.Load<Texture2D>(@"Creatures\Solvis"), overlay, 40, 45, 60, 65, 35),
              new Takinom(Content.Load<Texture2D>(@"Creatures\Takinom"), overlay, 40, 60, 65, 20, 95),
              new Toxis(Content.Load<Texture2D>(@"Creatures\Skreeth"), overlay, 50, 45, 70, 40, 50),
              new Ulmar(Content.Load<Texture2D>(@"Creatures\Ulmar"), overlay, 25, 40, 20, 70, 35),
              new Xield(Content.Load<Texture2D>(@"Creatures\Xield"), overlay, 20, 90, 40, 35, 15),
              new Zaur(Content.Load<Texture2D>(@"Creatures\Zaur"), overlay, 50, 65, 75, 35, 25),
              new Ekuud(Content.Load<Texture2D>(@"Creatures\Ekuud"), overlay, 50, 40, 60, 20, 45),
              new Formicidor(Content.Load<Texture2D>(@"Creatures\Formicidor"), overlay, 40, 60, 40, 35, 40),
              new Galin(Content.Load<Texture2D>(@"Creatures\Galin"), overlay, 50, 65, 70, 40, 65),
              new Hota(Content.Load<Texture2D>(@"Creatures\Hota"), overlay, 30, 40, 55, 35, 30),
              new Ibiaan(Content.Load<Texture2D>(@"Creatures\Ibiaan"), overlay, 50, 45, 60, 65, 30),
              new Junda(Content.Load<Texture2D>(@"Creatures\Junda"), overlay, 50, 35, 55, 50, 40),
              new Kannen(Content.Load<Texture2D>(@"Creatures\Kannen"), overlay, 40, 25, 20, 70, 25),
              new Kebna(Content.Load<Texture2D>(@"Creatures\Kebna"), overlay, 45, 65, 30, 55, 35),
              new Lhad(Content.Load<Texture2D>(@"Creatures\Lhad"), overlay, 40, 65, 65, 70, 25),
              new Lore(Content.Load<Texture2D>(@"Creatures\Lore"), overlay, 25, 30, 35, 70, 30),
              new Mallash(Content.Load<Texture2D>(@"Creatures\Mallash"), overlay, 30, 30, 25, 45, 35),
              new OduBathax(Content.Load<Texture2D>(@"Creatures\OduBathax"), overlay, 30, 45, 60, 40, 45),
              new Skartalas(Content.Load<Texture2D>(@"Creatures\Skartalas"), overlay, 40, 55, 45, 40, 55),
              new ValaniiLevaan(Content.Load<Texture2D>(@"Creatures\ValaniiLevaan"), overlay, 50, 50, 50, 15, 25),
              new Wamma(Content.Load<Texture2D>(@"Creatures\Wamma"), overlay, 50, 40, 55, 30, 25),
              new Ario(Content.Load<Texture2D>(@"Creatures\Ario"), overlay, 40, 50, 55, 25, 55),
              new Biondu(Content.Load<Texture2D>(@"Creatures\Biondu"), overlay, 40, 45, 40, 30, 35),
              new Brathe(Content.Load<Texture2D>(@"Creatures\Brathe"), overlay, 55, 55, 45, 65, 55),
              new Malvadine(Content.Load<Texture2D>(@"Creatures\Malvadine"), overlay, 50, 50, 75, 40, 55),
              new MarquisDarini(Content.Load<Texture2D>(@"Creatures\MarquisDarini"), overlay, 40, 45, 40, 65, 40),
              new PrinceMudeenu(Content.Load<Texture2D>(@"Creatures\PrinceMudeenu"), overlay, 45, 55, 35, 70, 30),
              new Qwun(Content.Load<Texture2D>(@"Creatures\Qwun"), overlay, 35, 55, 40, 55, 90),
              new Shimmark(Content.Load<Texture2D>(@"Creatures\Shimmark"), overlay, 45, 60, 50, 30, 70),
              new Siado(Content.Load<Texture2D>(@"Creatures\Siado"), overlay, 30, 60, 45, 60, 55),
              new Sobtjek(Content.Load<Texture2D>(@"Creatures\Sobtjek"), overlay, 30, 40, 25, 65, 40),
              new Tiaane(Content.Load<Texture2D>(@"Creatures\Tiaane"), overlay, 40, 65, 40, 50, 20),
              new Ubliqun(Content.Load<Texture2D>(@"Creatures\Ubliqun"), overlay, 35, 35, 45, 60, 60),
              new Uro(Content.Load<Texture2D>(@"Creatures\Uro"), overlay, 45, 60, 60, 45, 60),
              new Vinta(Content.Load<Texture2D>(@"Creatures\Vinta"), overlay, 35, 35, 60, 35, 40),
              new Zhade(Content.Load<Texture2D>(@"Creatures\Zhade"), overlay, 30, 65, 60, 40, 60),
              new Allmageddon(Content.Load<Texture2D>(@"Attacks\Allmageddon"), overlay),
              new AshTorrent(Content.Load<Texture2D>(@"Attacks\AshTorrent"), overlay),
              new CoilCrush(Content.Load<Texture2D>(@"Attacks\CoilCrush"), overlay),
              new Degenervate(Content.Load<Texture2D>(@"Attacks\Degenervate"), overlay),
              new Delerium(Content.Load<Texture2D>(@"Attacks\Delerium"), overlay),
              new Ektospasm(Content.Load<Texture2D>(@"Attacks\Ektospasm"), overlay),
              new EmberSwarm(Content.Load<Texture2D>(@"Attacks\EmberSwarm"), overlay),
              new Evaporize(Content.Load<Texture2D>(@"Attacks\Evaporize"), overlay),
              new Fearocity(Content.Load<Texture2D>(@"Attacks\Fearocity"), overlay),
              new FlameOrb(Content.Load<Texture2D>(@"Attacks\FlameOrb"), overlay),
              new FlashKick(Content.Load<Texture2D>(@"Attacks\FlashKick"), overlay),
              new FlashMend(Content.Load<Texture2D>(@"Attacks\FlashMend"), overlay),
              new Flashwarp(Content.Load<Texture2D>(@"Attacks\Flashwarp"), overlay),
              new FrostBlight(Content.Load<Texture2D>(@"Attacks\FrostBlight"), overlay),
              new HailStorm(Content.Load<Texture2D>(@"Attacks\HailStorm"), overlay),
              new HiveCall(Content.Load<Texture2D>(@"Attacks\HiveCall"), overlay),
              new Incinerase(Content.Load<Texture2D>(@"Attacks\Incinerase"), overlay),
              new InfernoGust(Content.Load<Texture2D>(@"Attacks\InfernoGust"), overlay),
              new IronBalls(Content.Load<Texture2D>(@"Attacks\IronBalls"), overlay),
              new Lavalanche(Content.Load<Texture2D>(@"Attacks\Lavalanche"), overlay),
              new LightningBurst(Content.Load<Texture2D>(@"Attacks\LightningBurst"), overlay),
              new LuckyShot(Content.Load<Texture2D>(@"Attacks\LuckyShot"), overlay),
              new MegaRoar(Content.Load<Texture2D>(@"Attacks\MegaRoar"), overlay),
              new Mirthquake(Content.Load<Texture2D>(@"Attacks\Mirthquake"), overlay),
              new ParalEyes(Content.Load<Texture2D>(@"Attacks\ParalEyes"), overlay),
              new PebbleStorm(Content.Load<Texture2D>(@"Attacks\PebbleStorm"), overlay),
              new PowerPulse(Content.Load<Texture2D>(@"Attacks\PowerPulse"), overlay),
              new QuickExit(Content.Load<Texture2D>(@"Attacks\QuickExit"), overlay),
              new RipTide(Content.Load<Texture2D>(@"Attacks\RipTide"), overlay),
              new RockWave(Content.Load<Texture2D>(@"Attacks\RockWave"), overlay),
              new Rustoxic(Content.Load<Texture2D>(@"Attacks\Rustoxic"), overlay),
              new ShadowStrike(Content.Load<Texture2D>(@"Attacks\ShadowStrike"), overlay),
              new ShriekShock(Content.Load<Texture2D>(@"Attacks\ShriekShock"), overlay),
              new SkeletalStrike(Content.Load<Texture2D>(@"Attacks\SkeletalStrike"), overlay),
              new SleepSting(Content.Load<Texture2D>(@"Attacks\SleepSting"), overlay),
              new SludgeGush(Content.Load<Texture2D>(@"Attacks\SludgeGush"), overlay),
              new SpiritGust(Content.Load<Texture2D>(@"Attacks\SpiritGust"), overlay),
              new SqueezePlay(Content.Load<Texture2D>(@"Attacks\SqueezePlay"), overlay),
              new SteamRage(Content.Load<Texture2D>(@"Attacks\SteamRage"), overlay),
              new TelekineticBolt(Content.Load<Texture2D>(@"Attacks\TelekineticBolt"), overlay),
              new ThunderShout(Content.Load<Texture2D>(@"Attacks\ThunderShout"), overlay),
              new TornadoTackle(Content.Load<Texture2D>(@"Attacks\TornadoTackle"), overlay),
              new TorrentOfFlame(Content.Load<Texture2D>(@"Attacks\TorrentOfFlame"), overlay),
              new ToxicGust(Content.Load<Texture2D>(@"Attacks\ToxicGust"), overlay),
              new Unsanity(Content.Load<Texture2D>(@"Attacks\Unsanity"), overlay),
              new Velocitrap(Content.Load<Texture2D>(@"Attacks\Velocitrap"), overlay),
              new VineSnare(Content.Load<Texture2D>(@"Attacks\VineSnare"), overlay),
              new Viperlash(Content.Load<Texture2D>(@"Attacks\Viperlash"), overlay),
              new Windslash(Content.Load<Texture2D>(@"Attacks\Windslash"), overlay),
              new AquaShield(Content.Load<Texture2D>(@"Battlegears\AquaShield"), overlay),
              new Cyclance(Content.Load<Texture2D>(@"Battlegears\Cyclance"), overlay),
              new DiamondOfVlaric(Content.Load<Texture2D>(@"Battlegears\DiamondOfVlaric"), overlay),
              new DragonPulse(Content.Load<Texture2D>(@"Battlegears\DragonPulse"), overlay),
              new ElixirOfTenacity(Content.Load<Texture2D>(@"Battlegears\ElixirOfTenacity"), overlay),
              new FluxBauble(Content.Load<Texture2D>(@"Battlegears\FluxBauble"), overlay),
              new GauntletsOfMight(Content.Load<Texture2D>(@"Battlegears\GauntletsOfMight"), overlay),
              new Liquilizer(Content.Load<Texture2D>(@"Battlegears\Liquilizer"), overlay),
              new MipedianCactus(Content.Load<Texture2D>(@"Battlegears\MipedianCactus"), overlay),
              new Mowercycle(Content.Load<Texture2D>(@"Battlegears\Mowercycle"), overlay),
              new MugiciansLyre(Content.Load<Texture2D>(@"Battlegears\MugiciansLyre"), overlay),
              new NexusFuse(Content.Load<Texture2D>(@"Battlegears\NexusFuse"), overlay),
              new OrbOfForesight(Content.Load<Texture2D>(@"Battlegears\OrbOfForesight"), overlay),
              new PhobiaMask(Content.Load<Texture2D>(@"Battlegears\PhobiaMask"), overlay),
              new PrismOfVacuity(Content.Load<Texture2D>(@"Battlegears\PrismOfVacuity"), overlay),
              new Pyroblaster(Content.Load<Texture2D>(@"Battlegears\Pyroblaster"), overlay),
              new RingOfNaarin(Content.Load<Texture2D>(@"Battlegears\RingOfNaarin"), overlay),
              new RiverlandStar(Content.Load<Texture2D>(@"Battlegears\RiverlandStar"), overlay),
              new SkeletalSteed(Content.Load<Texture2D>(@"Battlegears\SkeletalSteed"), overlay),
              new SpectralViewer(Content.Load<Texture2D>(@"Battlegears\SpectralViewer"), overlay),
              new StaffOfWisdom(Content.Load<Texture2D>(@"Battlegears\StaffOfWisdom"), overlay),
              new StoneMail(Content.Load<Texture2D>(@"Battlegears\StoneMail"), overlay),
              new TalismanOfTheMandiblor(Content.Load<Texture2D>(@"Battlegears\TalismanOfTheMandiblor"), overlay),
              new TorrentKrinth(Content.Load<Texture2D>(@"Battlegears\TorrentKrinth"), overlay),
              new Torwegg(Content.Load<Texture2D>(@"Battlegears\Torwegg"), overlay),
              new Viledriver(Content.Load<Texture2D>(@"Battlegears\Viledriver"), overlay),
              new VlaricShard(Content.Load<Texture2D>(@"Battlegears\VlaricShard"), overlay),
              new WhepCrack(Content.Load<Texture2D>(@"Battlegears\WhepCrack"), overlay),
              new WindStrider(Content.Load<Texture2D>(@"Battlegears\WindStrider"), overlay),
              new Decrescendo(Content.Load<Texture2D>(@"Mugics\Decrescendo"), overlay),
              new EmberFlourish(Content.Load<Texture2D>(@"Mugics\EmberFlourish"), overlay),
              new Fortissimo(Content.Load<Texture2D>(@"Mugics\Fortissimo"), overlay),
              new GeoFlourish(Content.Load<Texture2D>(@"Mugics\GeoFlourish"), overlay),
              new InterludeOfConsequence(Content.Load<Texture2D>(@"Mugics\InterludeOfConsequence"), overlay),
              new MinorFlourish(Content.Load<Texture2D>(@"Mugics\MinorFlourish"), overlay),
              new SongOfEmbernova(Content.Load<Texture2D>(@"Mugics\SongOfEmbernova"), overlay),
              new SongOfFuturesight(Content.Load<Texture2D>(@"Mugics\SongOfFuturesight"), overlay),
              new SongOfGeonova(Content.Load<Texture2D>(@"Mugics\SongOfGeonova"), overlay),
              new SongOfTruesight(Content.Load<Texture2D>(@"Mugics\SongOfTruesight"), overlay),
              new CascadeOfSymphony(Content.Load<Texture2D>(@"Mugics\CascadeOfSymphony"), overlay),
              new HymnOfTheElements(Content.Load<Texture2D>(@"Mugics\HymnOfTheElements"), overlay),
              new MugicReprise(Content.Load<Texture2D>(@"Mugics\MugicReprise"), overlay),
              new OverWorldAria(Content.Load<Texture2D>(@"Mugics\OverWorldAria"), overlay),
              new RefrainOfDenial(Content.Load<Texture2D>(@"Mugics\RefrainOfDenial"), overlay),
              new SongOfFocus(Content.Load<Texture2D>(@"Mugics\SongOfFocus"), overlay),
              new SongOfResurgence(Content.Load<Texture2D>(@"Mugics\SongOfResurgence"), overlay),
              new SongOfStasis(Content.Load<Texture2D>(@"Mugics\SongOfStasis"), overlay),
              new CanonOfCasualty(Content.Load<Texture2D>(@"Mugics\CanonOfCasualty"), overlay),
              new DiscordOfDisarming(Content.Load<Texture2D>(@"Mugics\DiscordOfDisarming"), overlay),
              new MelodyOfMalady(Content.Load<Texture2D>(@"Mugics\MelodyOfMalady"), overlay),
              new RefrainOfDenial_OverWorld_(Content.Load<Texture2D>(@"Mugics\RefrainOfDenial_OverWorld_"), overlay),
              new SongOfAsperity(Content.Load<Texture2D>(@"Mugics\SongOfAsperity"), overlay),
              new SongOfFury(Content.Load<Texture2D>(@"Mugics\SongOfFury"), overlay),
              new SongOfRevival_UnderWorld_(Content.Load<Texture2D>(@"Mugics\SongOfRevival_UnderWorld_"), overlay),
              new SongOfTreachery(Content.Load<Texture2D>(@"Mugics\SongOfTreachery"), overlay),

              new FanfareOfTheVanishing(Content.Load<Texture2D>(@"Mugics\FanfareOfTheVanishing"), overlay),
              new MelodyOfMirage(Content.Load<Texture2D>(@"Mugics\MelodyOfMirage"), overlay),
              new NotesOfNeverwhere(Content.Load<Texture2D>(@"Mugics\NotesOfNeverwhere"), overlay),
              new SongOfDeflection(Content.Load<Texture2D>(@"Mugics\SongOfDeflection"), overlay),
              new SongOfRecovery(Content.Load<Texture2D>(@"Mugics\SongOfRecovery"), overlay),
              new TrillsOfDiminution(Content.Load<Texture2D>(@"Mugics\TrillsOfDiminution"), overlay),
              new CastleBodhran(Content.Load<Texture2D>(@"Locations\CastleBodhran"),
                  Content.Load<Texture2D>(@"Backgrounds\CastleBodhran"), overlay),
              new CastlePillar(Content.Load<Texture2D>(@"Locations\CastlePillar"),
                  Content.Load<Texture2D>(@"Backgrounds\CastlePillar"), overlay),
              new CordacFalls(Content.Load<Texture2D>(@"Locations\CordacFalls"),
                  Content.Load<Texture2D>(@"Backgrounds\CordacFalls"), overlay),
              new CordacFallsPlungepool(Content.Load<Texture2D>(@"Locations\CordacFallsPlungepool"),
                  Content.Load<Texture2D>(@"Backgrounds\CordacFallsPlungepool"), overlay),
              new CrystalCave(Content.Load<Texture2D>(@"Locations\CrystalCave"),
                  Content.Load<Texture2D>(@"Backgrounds\CrystalCave"), overlay),
              new DoorsOfTheDeepmines(Content.Load<Texture2D>(@"Locations\DoorsOfTheDeepmines"),
                  Content.Load<Texture2D>(@"Backgrounds\DoorsOfTheDeepmines"), overlay),
              new DranakisThreshold(Content.Load<Texture2D>(@"Locations\DranakisThreshold"),
                  Content.Load<Texture2D>(@"Backgrounds\DranakisThreshold"), overlay),
              new Everrain(Content.Load<Texture2D>(@"Locations\Everrain"),
                  Content.Load<Texture2D>(@"Backgrounds\Everrain"), overlay),
              new EyeOfTheMaelstrom(Content.Load<Texture2D>(@"Locations\EyeOfTheMaelstrom"),
                  Content.Load<Texture2D>(@"Backgrounds\EyeOfTheMaelstrom"), overlay),
              new FearValley(Content.Load<Texture2D>(@"Locations\FearValley"),
                  Content.Load<Texture2D>(@"Backgrounds\FearValley"), overlay),
              new ForestOfLife(Content.Load<Texture2D>(@"Locations\ForestOfLife"),
                  Content.Load<Texture2D>(@"Backgrounds\ForestOfLife"), overlay),
              new Gigantempopolis(Content.Load<Texture2D>(@"Locations\Gigantempopolis"),
                  Content.Load<Texture2D>(@"Backgrounds\Gigantempopolis"), overlay),
              new GlacierPlains(Content.Load<Texture2D>(@"Locations\GlacierPlains"),
                  Content.Load<Texture2D>(@"Backgrounds\GlacierPlains"), overlay),
              new GloomuckSwamp(Content.Load<Texture2D>(@"Locations\GloomuckSwamp"),
                  Content.Load<Texture2D>(@"Backgrounds\GloomuckSwamp"), overlay),
              new GothosTower(Content.Load<Texture2D>(@"Locations\GothosTower"),
                  Content.Load<Texture2D>(@"Backgrounds\GothosTower"), overlay),
              new IronPillar(Content.Load<Texture2D>(@"Locations\IronPillar"),
                  Content.Load<Texture2D>(@"Backgrounds\IronPillar"), overlay),
              new KiruCity(Content.Load<Texture2D>(@"Locations\KiruCity"),
                  Content.Load<Texture2D>(@"Backgrounds\KiruCity"), overlay),
              new LakeKenIPo(Content.Load<Texture2D>(@"Locations\LakeKenIPo"),
                  Content.Load<Texture2D>(@"Backgrounds\LakeKenIPo"), overlay),
              new LavaPond(Content.Load<Texture2D>(@"Locations\LavaPond"),
                  Content.Load<Texture2D>(@"Backgrounds\LavaPond"), overlay),
              new MipedimOasis(Content.Load<Texture2D>(@"Locations\MipedimOasis"),
                  Content.Load<Texture2D>(@"Backgrounds\MipedimOasis"), overlay),
              new MountPillar(Content.Load<Texture2D>(@"Locations\MountPillar"),
                  Content.Load<Texture2D>(@"Backgrounds\MountPillar"), overlay),
              new RavanaughRidge(Content.Load<Texture2D>(@"Locations\RavanaughRidge"),
                  Content.Load<Texture2D>(@"Backgrounds\RavanaughRidge"), overlay),
              new Riverlands(Content.Load<Texture2D>(@"Locations\Riverlands"),
                  Content.Load<Texture2D>(@"Backgrounds\Riverlands"), overlay),
              new RunicGrove(Content.Load<Texture2D>(@"Locations\RunicGrove"),
                  Content.Load<Texture2D>(@"Backgrounds\RunicGrove"), overlay),
              new StonePillar(Content.Load<Texture2D>(@"Locations\StonePillar"),
                  Content.Load<Texture2D>(@"Backgrounds\StonePillar"), overlay),
              new StormTunnel(Content.Load<Texture2D>(@"Locations\StormTunnel"),
                  Content.Load<Texture2D>(@"Backgrounds\StormTunnel"), overlay),
              new StrongholdMorn(Content.Load<Texture2D>(@"Locations\StrongholdMorn"),
                  Content.Load<Texture2D>(@"Backgrounds\StrongholdMorn"), overlay),
              new UnderworldCity(Content.Load<Texture2D>(@"Locations\UnderworldCity"),
                  Content.Load<Texture2D>(@"Backgrounds\UnderworldCity"), overlay),
              new UnderworldColosseum(Content.Load<Texture2D>(@"Locations\UnderworldColosseum"),
                  Content.Load<Texture2D>(@"Backgrounds\UnderworldColosseum"), overlay),
              new WoodenPillar(Content.Load<Texture2D>(@"Locations\WoodenPillar"),
                  Content.Load<Texture2D>(@"Backgrounds\WoodenPillar"), overlay),
              };
        }

        private void loadMenuButtons(GraphicsDeviceManager graphics)
        {
            Texture2D overlay = Content.Load<Texture2D>(@"Menu/MenuButtonCover");
            loadMainButtons(graphics, overlay);
            loadTestButtons(graphics, overlay);
            overlay = Content.Load<Texture2D>(@"Menu/backBtnCover");
            testButtons[3] = new MenuButton(Content.Load<Texture2D>(@"Menu/SuitBackButton"),
                new Vector2(20, 20), overlay, MenuStage.MainMenu);
        }

        private void loadMainButtons(GraphicsDeviceManager graphics, Texture2D overlay)
        {
            float btnPosX = graphics.PreferredBackBufferWidth / 2 - overlay.Width / 2;
            float btnSpace = overlay.Height / 2;
            mainButtons[0] = new MenuButton(Content.Load<Texture2D>(@"Menu/TestButton"), new Vector2(btnPosX, title.Height + btnSpace),
                overlay, MenuStage.Test);
            mainButtons[1] = new MenuButton(Content.Load<Texture2D>(@"Menu/EditButton"), new Vector2(btnPosX,
                mainButtons[0].Position.Y + btnSpace + overlay.Height), overlay, MenuStage.DeckEdit);
        }

        private void loadTestButtons(GraphicsDeviceManager graphics, Texture2D overlay)
        {
            float btnPosX = graphics.PreferredBackBufferWidth / 2 - overlay.Width / 2;
            float btnSpace = overlay.Height / 2;
            testButtons[0] = new MenuButton(Content.Load<Texture2D>(@"Menu/OneOnOneButton"), new Vector2(btnPosX,
                title.Height + btnSpace), overlay, MenuStage.OneOnOne);
            testButtons[1] = new MenuButton(Content.Load<Texture2D>(@"Menu/ThreeOnThreeButton"), new Vector2(btnPosX,
                testButtons[0].Position.Y + overlay.Height + btnSpace), overlay, MenuStage.ThreeOnThree);
            testButtons[2] = new MenuButton(Content.Load<Texture2D>(@"Menu/SixOnSixButton"), new Vector2(btnPosX,
               testButtons[1].Position.Y + overlay.Height + btnSpace), overlay, MenuStage.SixOnSix);
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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            MouseState mouse = Mouse.GetState();
            switch (ChaoticEngine.MStage)
            {
                case MenuStage.Test:
                    UpdateButtons(testButtons, gameTime, mouse);
                    break;
                case MenuStage.DeckEdit:
                    System.Windows.Forms.Application.EnableVisualStyles();
                    ChaoticEngine.MStage = MenuStage.MainMenu;
                    new ChaoticForm().ShowDialog();
                    break;
                case MenuStage.MainMenu:
                    UpdateButtons(mainButtons, gameTime, mouse);
                    break;
                case MenuStage.InGame:
                    break;
                case MenuStage.OneOnOne:
                    ChaoticEngine.MStage = MenuStage.Wait1On1;
                    System.Windows.Forms.Application.EnableVisualStyles();
                    new DeckSetup1On1("Test1").ShowDialog();
                    break;
                case MenuStage.ThreeOnThree:
                    ChaoticEngine.MStage = MenuStage.Wait3On3;
                    System.Windows.Forms.Application.EnableVisualStyles();
                    new DeckSetup3On3("Test1").ShowDialog();
                    break;
                case MenuStage.SixOnSix:
                    ChaoticEngine.MStage = MenuStage.Wait6On6;
                    System.Windows.Forms.Application.EnableVisualStyles();
                    new DeckSetup6On6("Test1").ShowDialog();
                    break;
                case MenuStage.Wait1On1:
                    if (ChaoticEngine.Player1Setup)
                    {
                        ChaoticEngine.MStage = MenuStage.Ready1On1;
                        System.Windows.Forms.Application.EnableVisualStyles();
                        new DeckSetup1On1("Test1").ShowDialog();
                    }
                    break;
                case MenuStage.Ready1On1:
                    if (ChaoticEngine.Player2Setup)
                    {
                        ChaoticEngine.Player1Setup = false;
                        ChaoticEngine.Player2Setup = false;
                        battleBoard = new BattleBoard(this, graphics, CreatureNumber.OneOnOne);
                        Components.Add(battleBoard);
                        battleBoard.Enabled = true;
                        battleBoard.Visible = true;
                        ChaoticEngine.MStage = MenuStage.InGame;
                    }
                    break;
                case MenuStage.Wait3On3:
                    if (ChaoticEngine.Player1Setup)
                    {
                        ChaoticEngine.MStage = MenuStage.Ready3On3;
                        System.Windows.Forms.Application.EnableVisualStyles();
                        new DeckSetup3On3("Test1").ShowDialog();
                    }
                    break;
                case MenuStage.Ready3On3:
                    if (ChaoticEngine.Player2Setup)
                    {
                        ChaoticEngine.Player1Setup = false;
                        ChaoticEngine.Player2Setup = false;
                        battleBoard = new BattleBoard(this, graphics, CreatureNumber.ThreeOnThree);
                        Components.Add(battleBoard);
                        battleBoard.Enabled = true;
                        battleBoard.Visible = true;
                        ChaoticEngine.MStage = MenuStage.InGame;
                    }
                    break;
                case MenuStage.Wait6On6:
                    if (ChaoticEngine.Player1Setup)
                    {
                        ChaoticEngine.MStage = MenuStage.Ready6On6;
                        System.Windows.Forms.Application.EnableVisualStyles();
                        new DeckSetup6On6("Test1").ShowDialog();
                    }
                    break;
                case MenuStage.Ready6On6:
                    if (ChaoticEngine.Player2Setup)
                    {
                        ChaoticEngine.Player1Setup = false;
                        ChaoticEngine.Player2Setup = false;
                        battleBoard = new BattleBoard(this, graphics, CreatureNumber.SixOnSix);
                        Components.Add(battleBoard);
                        battleBoard.Enabled = true;
                        battleBoard.Visible = true;
                        ChaoticEngine.MStage = MenuStage.InGame;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateButtons(MenuButton[] buttons, GameTime gameTime, MouseState mouse)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (buttons[i].CollisionRectangle.Contains(new Point(mouse.X, mouse.Y)))
                    {
                        buttons[i].IsClicked = true;
                        buttons[i].IsCovered = true;
                        elapsedTime = gameTime.TotalGameTime.TotalMilliseconds;
                        selectedStage = buttons[i].Stage;
                        j = i;
                        break;
                    }
                }
                else
                    buttons[i].IsCovered = false;
            }
            if (elapsedTime != 0 && gameTime.TotalGameTime.TotalMilliseconds - elapsedTime >= 100f)
            {
                ChaoticEngine.MStage = selectedStage;
                buttons[j].IsClicked = false;
                elapsedTime = 0f;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(title, new Vector2(graphics.PreferredBackBufferWidth / 2 - title.Width / 2, 0), Color.White);
            switch (ChaoticEngine.MStage)
            {
                case MenuStage.Test:
                    DrawButtons(spriteBatch, testButtons);
                    break;
                case MenuStage.DeckEdit:
                    break;
                case MenuStage.MainMenu:
                    DrawButtons(spriteBatch, mainButtons);
                    break;
                case MenuStage.InGame:
                    break;
                default:
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawButtons(SpriteBatch spriteBatch, MenuButton[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Draw(spriteBatch);
            }
        }
    }
}