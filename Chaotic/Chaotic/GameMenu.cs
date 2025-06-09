/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
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
        Wait6On6, Ready6On6, SplashScreen,
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameMenu : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D title;
        Texture2D splashScreen;

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
            ChaoticEngine.MStage = MenuStage.SplashScreen;
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
            if (DateTime.Now.Month == 3) // Check that it is March.
                splashScreen = Content.Load<Texture2D>(@"SplashScreens\SplashScreenMarch");
            else if (DateTime.Now.Month == 10) // Check that it is October.
                splashScreen = Content.Load<Texture2D>(@"SplashScreens\SplashScreenOctober");
            else if (DateTime.Now.Month == 12) // Check that it is December.
                splashScreen = Content.Load<Texture2D>(@"SplashScreens\SplashScreenDecember");
            else
                splashScreen = Content.Load<Texture2D>(@"SplashScreens\SplashScreen");

            loadMenuButtons(graphics);

            Texture2D overlay = Content.Load<Texture2D>("CardOutline");
            Texture2D negate = Content.Load<Texture2D>("negate");
            ChaoticEngine.sCardDatabase = new List<ChaoticCard>() 
            { new Arias(Content.Load<Texture2D>(@"Creatures\Arias"), overlay, negate, 50, 55, 65, 30, 55),
              new Attacat(Content.Load<Texture2D>(@"Creatures\Attacat"), overlay, negate, 50, 30, 65, 60, 105),
              new Blazier(Content.Load<Texture2D>(@"Creatures\Blazier"), overlay, negate, 40, 35, 40, 60, 25),
              new Blugon(Content.Load<Texture2D>(@"Creatures\Blugon"), overlay, negate, 40, 35, 65, 70, 45),
              new Bodal(Content.Load<Texture2D>(@"Creatures\Bodal"), overlay, negate, 45, 40, 40, 80, 60),
              new Crawsectus(Content.Load<Texture2D>(@"Creatures\Crawsectus"), overlay, negate, 50, 60, 45, 40, 25),
              new Donmar(Content.Load<Texture2D>(@"Creatures\Donmar"), overlay, negate, 45, 45, 65, 65, 50),
              new Dractyl(Content.Load<Texture2D>(@"Creatures\Dractyl"), overlay, negate, 40, 25, 70, 50, 70),
              new Frafdo(Content.Load<Texture2D>(@"Creatures\Frafdo"), overlay, negate, 35, 85, 80, 45, 75),
              new Gespedan(Content.Load<Texture2D>(@"Creatures\Gespedan"), overlay, negate, 35, 45, 50, 35, 100),
              new Heptadd(Content.Load<Texture2D>(@"Creatures\Heptadd"), overlay, negate, 55, 55, 60, 50, 40),
              new Intress(Content.Load<Texture2D>(@"Creatures\Intress"), overlay, negate, 40, 40, 35, 40, 55),
              new Laarina(Content.Load<Texture2D>(@"Creatures\Laarina"), overlay, negate, 30, 35, 25, 30, 30),
              new Maglax(Content.Load<Texture2D>(@"Creatures\Maglax"), overlay, negate, 40, 70, 60, 25, 30),
              new Maxxor(Content.Load<Texture2D>(@"Creatures\Maxxor"), overlay, negate, 60, 100, 65, 80, 50),
              new Najarin(Content.Load<Texture2D>(@"Creatures\Najarin"), overlay, negate, 30, 60, 30, 90, 35),
              new Owis(Content.Load<Texture2D>(@"Creatures\Owis"), overlay, negate, 45, 65, 25, 55, 30),
              new Psimion(Content.Load<Texture2D>(@"Creatures\Psimion"), overlay, negate, 45, 45, 30, 55, 40),
              new Rellim(Content.Load<Texture2D>(@"Creatures\Rellim"), overlay, negate, 50, 50, 65, 60, 40),
              new Slurhk(Content.Load<Texture2D>(@"Creatures\Slurhk"), overlay, negate, 50, 45, 35, 35, 40),
              new Staluk(Content.Load<Texture2D>(@"Creatures\Staluk"), overlay, negate, 50, 35, 45, 25, 40),
              new TangathToborn(Content.Load<Texture2D>(@"Creatures\TangathToborn"), overlay, negate, 30, 40, 45, 40, 30),
              new Tartarek(Content.Load<Texture2D>(@"Creatures\Tartarek"), overlay, negate, 35, 90, 45, 90, 25),
              new Velreth(Content.Load<Texture2D>(@"Creatures\Velreth"), overlay, negate, 45, 55, 80, 40, 25),
              new Vidav(Content.Load<Texture2D>(@"Creatures\Vidav"), overlay, negate, 35, 45, 30, 50, 35),
              new Xaerv(Content.Load<Texture2D>(@"Creatures\Xaerv"), overlay, negate, 30, 40, 25, 45, 60),
              new Yokkis(Content.Load<Texture2D>(@"Creatures\Yokkis"), overlay, negate, 50, 20, 20, 20, 20),
              new Zalic(Content.Load<Texture2D>(@"Creatures\Zalic"), overlay, negate, 45, 45, 55, 40, 40),
              new Nebres(Content.Load<Texture2D>(@"Creatures\Nebres"), overlay, negate, 50, 70, 80, 45, 50),
              new Neffa(Content.Load<Texture2D>(@"Creatures\Neffa"), overlay, negate, 40, 50, 25, 80, 35),
              new Hoton(Content.Load<Texture2D>(@"Creatures\Hoton"), overlay, negate, 40, 50, 25, 80, 35),
              new Sukoval(Content.Load<Texture2D>(@"Creatures\Sukoval"), overlay, negate, 45, 30, 55, 80, 55),
              new Raimusa(Content.Load<Texture2D>(@"Creatures\Raimusa"), overlay, negate, 50, 60, 65, 40, 45),
              new Ulfhedinn(Content.Load<Texture2D>(@"Creatures\Ulfhedinn"), overlay, negate, 50, 70, 80, 45, 50),
              new Ettala(Content.Load<Texture2D>(@"Creatures\Ettala"), overlay, negate, 35, 50, 50, 20, 30),
              new Antidaeon(Content.Load<Texture2D>(@"Creatures\Antidaeon"), overlay, negate, 40, 35, 60, 40, 70),
              new Quadore(Content.Load<Texture2D>(@"Creatures\Quadore"), overlay, negate, 40, 60, 20, 65, 60),
              new Prantix(Content.Load<Texture2D>(@"Creatures\Prantix"), overlay, negate, 30, 40, 50, 30, 40),
              new Mommark(Content.Load<Texture2D>(@"Creatures\Mommark"), overlay, negate, 30, 50, 20, 45, 35),
              new Mezzmarr(Content.Load<Texture2D>(@"Creatures\Mezzmarr"), overlay, negate, 45, 30, 55, 55, 55),
              new BarathBeyond(Content.Load<Texture2D>(@"Creatures\BarathBeyond"), overlay, negate, 60, 40, 85, 15, 65),
              new BorthMajar(Content.Load<Texture2D>(@"Creatures\BorthMajar"), overlay, negate, 45, 45, 90, 85, 40),
              new Chaor(Content.Load<Texture2D>(@"Creatures\Chaor"), overlay, negate, 70, 95, 90, 70, 60),
              new Dardemus(Content.Load<Texture2D>(@"Creatures\Dardemus"), overlay, negate, 50, 60, 75, 20, 65),
              new Drakness(Content.Load<Texture2D>(@"Creatures\Drakness"), overlay, negate, 45, 75, 40, 85, 55),
              new Ghuul(Content.Load<Texture2D>(@"Creatures\Ghuul"), overlay, negate, 35, 50, 85, 15, 35),
              new Grook(Content.Load<Texture2D>(@"Creatures\Grook"), overlay, negate, 50, 30, 100, 20, 60),
              new Hearring(Content.Load<Texture2D>(@"Creatures\Hearring"), overlay, negate, 50, 70, 30, 55, 45),
              new Kerric(Content.Load<Texture2D>(@"Creatures\Kerric"), overlay, negate, 50, 50, 30, 45, 65),
              new Khybon(Content.Load<Texture2D>(@"Creatures\Khybon"), overlay, negate, 40, 75, 25, 85, 20),
              new Klasp(Content.Load<Texture2D>(@"Creatures\Klasp"), overlay, negate, 65, 65, 90, 30, 65),
              new Krekk(Content.Load<Texture2D>(@"Creatures\Krekk"), overlay, negate, 40, 10, 85, 20, 60),
              new Kughar(Content.Load<Texture2D>(@"Creatures\Kughar"), overlay, negate, 50, 65, 85, 25, 45),
              new LordVanBloot(Content.Load<Texture2D>(@"Creatures\LordVanBloot"), overlay, negate, 65, 75, 115, 50, 95),
              new Magmon(Content.Load<Texture2D>(@"Creatures\Magmon"), overlay, negate, 55, 75, 60, 20, 35),
              new Miklon(Content.Load<Texture2D>(@"Creatures\Miklon"), overlay, negate, 50, 20, 75, 30, 60),
              new Nauthilax(Content.Load<Texture2D>(@"Creatures\Nauthilax"), overlay, negate, 60, 65, 60, 45, 50),
              new Pyrithion(Content.Load<Texture2D>(@"Creatures\Pyrithion"), overlay, negate, 50, 45, 80, 40, 65),
              new Rarran(Content.Load<Texture2D>(@"Creatures\Rarran"), overlay, negate, 50, 65, 60, 30, 60),
              new Rothar(Content.Load<Texture2D>(@"Creatures\Rothar"), overlay, negate, 70, 75, 95, 25, 50),
              new Skithia(Content.Load<Texture2D>(@"Creatures\Skithia"), overlay, negate, 35, 65, 25, 55, 40),
              new Skreeth(Content.Load<Texture2D>(@"Creatures\Skreeth"), overlay, negate, 55, 80, 65, 60, 20),
              new Solvis(Content.Load<Texture2D>(@"Creatures\Solvis"), overlay, negate, 40, 45, 60, 65, 35),
              new Takinom(Content.Load<Texture2D>(@"Creatures\Takinom"), overlay, negate, 40, 60, 65, 20, 95),
              new Toxis(Content.Load<Texture2D>(@"Creatures\Skreeth"), overlay, negate, 50, 45, 70, 40, 50),
              new Ulmar(Content.Load<Texture2D>(@"Creatures\Ulmar"), overlay, negate, 25, 40, 20, 70, 35),
              new Xield(Content.Load<Texture2D>(@"Creatures\Xield"), overlay, negate, 20, 90, 40, 35, 15),
              new Zaur(Content.Load<Texture2D>(@"Creatures\Zaur"), overlay, negate, 50, 65, 75, 35, 25),
              new Chargola(Content.Load<Texture2D>(@"Creatures\Chargola"), overlay, negate, 60, 75, 85, 65, 50),
              new Banshor(Content.Load<Texture2D>(@"Creatures\Banshor"), overlay, negate, 60, 75, 85, 65, 50),
              new Cerbie(Content.Load<Texture2D>(@"Creatures\Cerbie"), overlay, negate, 60, 50, 95, 25, 45),
              new Agitos(Content.Load<Texture2D>(@"Creatures\Agitos"), overlay, negate, 65, 65, 40, 85, 35),
              new Mishmoshmish(Content.Load<Texture2D>(@"Creatures\Mishmoshmish"), overlay, negate, 45, 35, 60, 45, 65),
              new Dyrtax(Content.Load<Texture2D>(@"Creatures\Dyrtax"), overlay, negate, 45, 35, 65, 45, 65),
              new Getterek(Content.Load<Texture2D>(@"Creatures\Getterek"), overlay, negate, 60, 50, 95, 25, 45),
              new Ekuud(Content.Load<Texture2D>(@"Creatures\Ekuud"), overlay, negate, 50, 40, 60, 20, 45),
              new Formicidor(Content.Load<Texture2D>(@"Creatures\Formicidor"), overlay, negate, 40, 60, 40, 35, 40),
              new Galin(Content.Load<Texture2D>(@"Creatures\Galin"), overlay, negate, 50, 65, 70, 40, 65),
              new Hota(Content.Load<Texture2D>(@"Creatures\Hota"), overlay, negate, 30, 40, 55, 35, 30),
              new Ibiaan(Content.Load<Texture2D>(@"Creatures\Ibiaan"), overlay, negate, 50, 45, 60, 65, 30),
              new Junda(Content.Load<Texture2D>(@"Creatures\Junda"), overlay, negate, 50, 35, 55, 50, 40),
              new Kannen(Content.Load<Texture2D>(@"Creatures\Kannen"), overlay, negate, 40, 25, 20, 70, 25),
              new Kebna(Content.Load<Texture2D>(@"Creatures\Kebna"), overlay, negate, 45, 65, 30, 55, 35),
              new Lhad(Content.Load<Texture2D>(@"Creatures\Lhad"), overlay, negate, 40, 65, 65, 70, 25),
              new Lore(Content.Load<Texture2D>(@"Creatures\Lore"), overlay, negate, 25, 30, 35, 70, 30),
              new Mallash(Content.Load<Texture2D>(@"Creatures\Mallash"), overlay, negate, 30, 30, 25, 45, 35),
              new OduBathax(Content.Load<Texture2D>(@"Creatures\OduBathax"), overlay, negate, 30, 45, 60, 40, 45),
              new Skartalas(Content.Load<Texture2D>(@"Creatures\Skartalas"), overlay, negate, 40, 55, 45, 40, 55),
              new ValaniiLevaan(Content.Load<Texture2D>(@"Creatures\ValaniiLevaan"), overlay, negate, 50, 50, 50, 15, 25),
              new Wamma(Content.Load<Texture2D>(@"Creatures\Wamma"), overlay, negate, 50, 40, 55, 30, 25),
              new Tarteme(Content.Load<Texture2D>(@"Creatures\Tarteme"), overlay, negate, 40, 50, 65, 35, 40),
              new Ario(Content.Load<Texture2D>(@"Creatures\Ario"), overlay, negate, 40, 50, 55, 25, 55),
              new Biondu(Content.Load<Texture2D>(@"Creatures\Biondu"), overlay, negate, 40, 45, 40, 30, 35),
              new Brathe(Content.Load<Texture2D>(@"Creatures\Brathe"), overlay, negate, 55, 55, 45, 65, 55),
              new Malvadine(Content.Load<Texture2D>(@"Creatures\Malvadine"), overlay, negate, 50, 50, 75, 40, 55),
              new MarquisDarini(Content.Load<Texture2D>(@"Creatures\MarquisDarini"), overlay, negate, 40, 45, 40, 65, 40),
              new PrinceMudeenu(Content.Load<Texture2D>(@"Creatures\PrinceMudeenu"), overlay, negate, 45, 55, 35, 70, 30),
              new Qwun(Content.Load<Texture2D>(@"Creatures\Qwun"), overlay, negate, 35, 55, 40, 55, 90),
              new Shimmark(Content.Load<Texture2D>(@"Creatures\Shimmark"), overlay, negate, 45, 60, 50, 30, 70),
              new Siado(Content.Load<Texture2D>(@"Creatures\Siado"), overlay, negate, 30, 60, 45, 60, 55),
              new Sobtjek(Content.Load<Texture2D>(@"Creatures\Sobtjek"), overlay, negate, 30, 40, 25, 65, 40),
              new Tiaane(Content.Load<Texture2D>(@"Creatures\Tiaane"), overlay, negate, 40, 65, 40, 50, 20),
              new Ubliqun(Content.Load<Texture2D>(@"Creatures\Ubliqun"), overlay, negate, 35, 35, 45, 60, 60),
              new Uro(Content.Load<Texture2D>(@"Creatures\Uro"), overlay, negate, 45, 60, 60, 45, 60),
              new Vinta(Content.Load<Texture2D>(@"Creatures\Vinta"), overlay, negate, 35, 35, 60, 35, 40),
              new Zhade(Content.Load<Texture2D>(@"Creatures\Zhade"), overlay, negate, 30, 65, 60, 40, 60),
              new Ghatup(Content.Load<Texture2D>(@"Creatures\Ghatup"), overlay, negate, 40, 60, 20, 65, 60),
              new Allmageddon(Content.Load<Texture2D>(@"Attacks\Allmageddon"), overlay, negate),
              new AshTorrent(Content.Load<Texture2D>(@"Attacks\AshTorrent"), overlay, negate),
              new CoilCrush(Content.Load<Texture2D>(@"Attacks\CoilCrush"), overlay, negate),
              new Degenervate(Content.Load<Texture2D>(@"Attacks\Degenervate"), overlay, negate),
              new Delerium(Content.Load<Texture2D>(@"Attacks\Delerium"), overlay, negate),
              new Ektospasm(Content.Load<Texture2D>(@"Attacks\Ektospasm"), overlay, negate),
              new EmberSwarm(Content.Load<Texture2D>(@"Attacks\EmberSwarm"), overlay, negate),
              new Evaporize(Content.Load<Texture2D>(@"Attacks\Evaporize"), overlay, negate),
              new Fearocity(Content.Load<Texture2D>(@"Attacks\Fearocity"), overlay, negate),
              new FlameOrb(Content.Load<Texture2D>(@"Attacks\FlameOrb"), overlay, negate),
              new FlashKick(Content.Load<Texture2D>(@"Attacks\FlashKick"), overlay, negate),
              new FlashMend(Content.Load<Texture2D>(@"Attacks\FlashMend"), overlay, negate),
              new Flashwarp(Content.Load<Texture2D>(@"Attacks\Flashwarp"), overlay, negate),
              new FrostBlight(Content.Load<Texture2D>(@"Attacks\FrostBlight"), overlay, negate),
              new HailStorm(Content.Load<Texture2D>(@"Attacks\HailStorm"), overlay, negate),
              new HiveCall(Content.Load<Texture2D>(@"Attacks\HiveCall"), overlay, negate),
              new Incinerase(Content.Load<Texture2D>(@"Attacks\Incinerase"), overlay, negate),
              new InfernoGust(Content.Load<Texture2D>(@"Attacks\InfernoGust"), overlay, negate),
              new IronBalls(Content.Load<Texture2D>(@"Attacks\IronBalls"), overlay, negate),
              new Lavalanche(Content.Load<Texture2D>(@"Attacks\Lavalanche"), overlay, negate),
              new LightningBurst(Content.Load<Texture2D>(@"Attacks\LightningBurst"), overlay, negate),
              new LuckyShot(Content.Load<Texture2D>(@"Attacks\LuckyShot"), overlay, negate),
              new MegaRoar(Content.Load<Texture2D>(@"Attacks\MegaRoar"), overlay, negate),
              new Mirthquake(Content.Load<Texture2D>(@"Attacks\Mirthquake"), overlay, negate),
              new ParalEyes(Content.Load<Texture2D>(@"Attacks\ParalEyes"), overlay, negate),
              new PebbleStorm(Content.Load<Texture2D>(@"Attacks\PebbleStorm"), overlay, negate),
              new PowerPulse(Content.Load<Texture2D>(@"Attacks\PowerPulse"), overlay, negate),
              new QuickExit(Content.Load<Texture2D>(@"Attacks\QuickExit"), overlay, negate),
              new RipTide(Content.Load<Texture2D>(@"Attacks\RipTide"), overlay, negate),
              new RockWave(Content.Load<Texture2D>(@"Attacks\RockWave"), overlay, negate),
              new Rustoxic(Content.Load<Texture2D>(@"Attacks\Rustoxic"), overlay, negate),
              new ShadowStrike(Content.Load<Texture2D>(@"Attacks\ShadowStrike"), overlay, negate),
              new ShriekShock(Content.Load<Texture2D>(@"Attacks\ShriekShock"), overlay, negate),
              new SkeletalStrike(Content.Load<Texture2D>(@"Attacks\SkeletalStrike"), overlay, negate),
              new SleepSting(Content.Load<Texture2D>(@"Attacks\SleepSting"), overlay, negate),
              new SludgeGush(Content.Load<Texture2D>(@"Attacks\SludgeGush"), overlay, negate),
              new SpiritGust(Content.Load<Texture2D>(@"Attacks\SpiritGust"), overlay, negate),
              new SqueezePlay(Content.Load<Texture2D>(@"Attacks\SqueezePlay"), overlay, negate),
              new SteamRage(Content.Load<Texture2D>(@"Attacks\SteamRage"), overlay, negate),
              new TelekineticBolt(Content.Load<Texture2D>(@"Attacks\TelekineticBolt"), overlay, negate),
              new ThunderShout(Content.Load<Texture2D>(@"Attacks\ThunderShout"), overlay, negate),
              new TornadoTackle(Content.Load<Texture2D>(@"Attacks\TornadoTackle"), overlay, negate),
              new TorrentOfFlame(Content.Load<Texture2D>(@"Attacks\TorrentOfFlame"), overlay, negate),
              new ToxicGust(Content.Load<Texture2D>(@"Attacks\ToxicGust"), overlay, negate),
              new Unsanity(Content.Load<Texture2D>(@"Attacks\Unsanity"), overlay, negate),
              new Velocitrap(Content.Load<Texture2D>(@"Attacks\Velocitrap"), overlay, negate),
              new VineSnare(Content.Load<Texture2D>(@"Attacks\VineSnare"), overlay, negate),
              new Viperlash(Content.Load<Texture2D>(@"Attacks\Viperlash"), overlay, negate),
              new Windslash(Content.Load<Texture2D>(@"Attacks\Windslash"), overlay, negate),
              new Catacollision(Content.Load<Texture2D>(@"Attacks\Catacollision"), overlay, negate),
              new FrighteningMuck(Content.Load<Texture2D>(@"Attacks\FrighteningMuck"), overlay, negate),
              new Nexdoors(Content.Load<Texture2D>(@"Attacks\Nexdoors"), overlay, negate),
              new AquaShield(Content.Load<Texture2D>(@"Battlegears\AquaShield"), overlay, negate),
              new Cyclance(Content.Load<Texture2D>(@"Battlegears\Cyclance"), overlay, negate),
              new DiamondOfVlaric(Content.Load<Texture2D>(@"Battlegears\DiamondOfVlaric"), overlay, negate),
              new DragonPulse(Content.Load<Texture2D>(@"Battlegears\DragonPulse"), overlay, negate),
              new ElixirOfTenacity(Content.Load<Texture2D>(@"Battlegears\ElixirOfTenacity"), overlay, negate),
              new FluxBauble(Content.Load<Texture2D>(@"Battlegears\FluxBauble"), overlay, negate),
              new GauntletsOfMight(Content.Load<Texture2D>(@"Battlegears\GauntletsOfMight"), overlay, negate),
              new Liquilizer(Content.Load<Texture2D>(@"Battlegears\Liquilizer"), overlay, negate),
              new MipedianCactus(Content.Load<Texture2D>(@"Battlegears\MipedianCactus"), overlay, negate),
              new Mowercycle(Content.Load<Texture2D>(@"Battlegears\Mowercycle"), overlay, negate),
              new MugiciansLyre(Content.Load<Texture2D>(@"Battlegears\MugiciansLyre"), overlay, negate),
              new NexusFuse(Content.Load<Texture2D>(@"Battlegears\NexusFuse"), overlay, negate),
              new OrbOfForesight(Content.Load<Texture2D>(@"Battlegears\OrbOfForesight"), overlay, negate),
              new PhobiaMask(Content.Load<Texture2D>(@"Battlegears\PhobiaMask"), overlay, negate),
              new PrismOfVacuity(Content.Load<Texture2D>(@"Battlegears\PrismOfVacuity"), overlay, negate),
              new Pyroblaster(Content.Load<Texture2D>(@"Battlegears\Pyroblaster"), overlay, negate),
              new RingOfNaarin(Content.Load<Texture2D>(@"Battlegears\RingOfNaarin"), overlay, negate),
              new RiverlandStar(Content.Load<Texture2D>(@"Battlegears\RiverlandStar"), overlay, negate),
              new SkeletalSteed(Content.Load<Texture2D>(@"Battlegears\SkeletalSteed"), overlay, negate),
              new SpectralViewer(Content.Load<Texture2D>(@"Battlegears\SpectralViewer"), overlay, negate),
              new StaffOfWisdom(Content.Load<Texture2D>(@"Battlegears\StaffOfWisdom"), overlay, negate),
              new StoneMail(Content.Load<Texture2D>(@"Battlegears\StoneMail"), overlay, negate),
              new TalismanOfTheMandiblor(Content.Load<Texture2D>(@"Battlegears\TalismanOfTheMandiblor"), overlay, negate),
              new TorrentKrinth(Content.Load<Texture2D>(@"Battlegears\TorrentKrinth"), overlay, negate),
              new Torwegg(Content.Load<Texture2D>(@"Battlegears\Torwegg"), overlay, negate),
              new Viledriver(Content.Load<Texture2D>(@"Battlegears\Viledriver"), overlay, negate),
              new VlaricShard(Content.Load<Texture2D>(@"Battlegears\VlaricShard"), overlay, negate),
              new WhepCrack(Content.Load<Texture2D>(@"Battlegears\WhepCrack"), overlay, negate),
              new WindStrider(Content.Load<Texture2D>(@"Battlegears\WindStrider"), overlay, negate),
              new Decrescendo(Content.Load<Texture2D>(@"Mugics\Decrescendo"), overlay, negate),
              new EmberFlourish(Content.Load<Texture2D>(@"Mugics\EmberFlourish"), overlay, negate),
              new Fortissimo(Content.Load<Texture2D>(@"Mugics\Fortissimo"), overlay, negate),
              new GeoFlourish(Content.Load<Texture2D>(@"Mugics\GeoFlourish"), overlay, negate),
              new InterludeOfConsequence(Content.Load<Texture2D>(@"Mugics\InterludeOfConsequence"), overlay, negate),
              new MinorFlourish(Content.Load<Texture2D>(@"Mugics\MinorFlourish"), overlay, negate),
              new SongOfEmbernova(Content.Load<Texture2D>(@"Mugics\SongOfEmbernova"), overlay, negate),
              new SongOfFuturesight(Content.Load<Texture2D>(@"Mugics\SongOfFuturesight"), overlay, negate),
              new SongOfGeonova(Content.Load<Texture2D>(@"Mugics\SongOfGeonova"), overlay, negate),
              new SongOfTruesight(Content.Load<Texture2D>(@"Mugics\SongOfTruesight"), overlay, negate),
              new CascadeOfSymphony(Content.Load<Texture2D>(@"Mugics\CascadeOfSymphony"), overlay, negate),
              new HymnOfTheElements(Content.Load<Texture2D>(@"Mugics\HymnOfTheElements"), overlay, negate),
              new MugicReprise(Content.Load<Texture2D>(@"Mugics\MugicReprise"), overlay, negate),
              new OverWorldAria(Content.Load<Texture2D>(@"Mugics\OverWorldAria"), overlay, negate),
              new RefrainOfDenial(Content.Load<Texture2D>(@"Mugics\RefrainOfDenial"), overlay, negate),
              new SongOfFocus(Content.Load<Texture2D>(@"Mugics\SongOfFocus"), overlay, negate),
              new SongOfResurgence(Content.Load<Texture2D>(@"Mugics\SongOfResurgence"), overlay, negate),
              new SongOfStasis(Content.Load<Texture2D>(@"Mugics\SongOfStasis"), overlay, negate),
              new CanonOfCasualty(Content.Load<Texture2D>(@"Mugics\CanonOfCasualty"), overlay, negate),
              new DiscordOfDisarming(Content.Load<Texture2D>(@"Mugics\DiscordOfDisarming"), overlay, negate),
              new MelodyOfMalady(Content.Load<Texture2D>(@"Mugics\MelodyOfMalady"), overlay, negate),
              new RefrainOfDenial_OverWorld_(Content.Load<Texture2D>(@"Mugics\RefrainOfDenial_OverWorld_"), overlay, negate),
              new SongOfAsperity(Content.Load<Texture2D>(@"Mugics\SongOfAsperity"), overlay, negate),
              new SongOfFury(Content.Load<Texture2D>(@"Mugics\SongOfFury"), overlay, negate),
              new SongOfRevival_UnderWorld_(Content.Load<Texture2D>(@"Mugics\SongOfRevival_UnderWorld_"), overlay, negate),
              new SongOfTreachery(Content.Load<Texture2D>(@"Mugics\SongOfTreachery"), overlay, negate),
              new ChorusOfTheHive(Content.Load<Texture2D>(@"Mugics\ChorusOfTheHive"), overlay, negate),
              new RefrainOfDenial_OverWorld0UnderWorld_(Content.Load<Texture2D>(@"Mugics\RefrainOfDenial_OverWorld0UnderWorld_"), overlay, negate),
              new SongOfMandiblor(Content.Load<Texture2D>(@"Mugics\SongOfMandiblor"), overlay, negate),
              new SongOfResistance(Content.Load<Texture2D>(@"Mugics\SongOfResistance"), overlay, negate),
              new SongOfSurprisal(Content.Load<Texture2D>(@"Mugics\SongOfSurprisal"), overlay, negate),
              new SongOfSymmetry(Content.Load<Texture2D>(@"Mugics\SongOfSymmetry"), overlay, negate),
              new SwarmSong(Content.Load<Texture2D>(@"Mugics\SwarmSong"), overlay, negate),
              new FanfareOfTheVanishing(Content.Load<Texture2D>(@"Mugics\FanfareOfTheVanishing"), overlay, negate),
              new MelodyOfMirage(Content.Load<Texture2D>(@"Mugics\MelodyOfMirage"), overlay, negate),
              new NotesOfNeverwhere(Content.Load<Texture2D>(@"Mugics\NotesOfNeverwhere"), overlay, negate),
              new SongOfDeflection(Content.Load<Texture2D>(@"Mugics\SongOfDeflection"), overlay, negate),
              new SongOfRecovery(Content.Load<Texture2D>(@"Mugics\SongOfRecovery"), overlay, negate),
              new TrillsOfDiminution(Content.Load<Texture2D>(@"Mugics\TrillsOfDiminution"), overlay, negate),
              new SongOfTransportation(Content.Load<Texture2D>(@"Mugics\SongOfTransportation"), overlay, negate),
              new CastleBodhran(Content.Load<Texture2D>(@"Locations\CastleBodhran"),
                  Content.Load<Texture2D>(@"Backgrounds\CastleBodhran"), overlay, negate),
              new CastlePillar(Content.Load<Texture2D>(@"Locations\CastlePillar"),
                  Content.Load<Texture2D>(@"Backgrounds\CastlePillar"), overlay, negate),
              new CordacFalls(Content.Load<Texture2D>(@"Locations\CordacFalls"),
                  Content.Load<Texture2D>(@"Backgrounds\CordacFalls"), overlay, negate),
              new CordacFallsPlungepool(Content.Load<Texture2D>(@"Locations\CordacFallsPlungepool"),
                  Content.Load<Texture2D>(@"Backgrounds\CordacFallsPlungepool"), overlay, negate),
              new CrystalCave(Content.Load<Texture2D>(@"Locations\CrystalCave"),
                  Content.Load<Texture2D>(@"Backgrounds\CrystalCave"), overlay, negate),
              new DoorsOfTheDeepmines(Content.Load<Texture2D>(@"Locations\DoorsOfTheDeepmines"),
                  Content.Load<Texture2D>(@"Backgrounds\DoorsOfTheDeepmines"), overlay, negate),
              new DranakisThreshold(Content.Load<Texture2D>(@"Locations\DranakisThreshold"),
                  Content.Load<Texture2D>(@"Backgrounds\DranakisThreshold"), overlay, negate),
              new Everrain(Content.Load<Texture2D>(@"Locations\Everrain"),
                  Content.Load<Texture2D>(@"Backgrounds\Everrain"), overlay, negate),
              new EyeOfTheMaelstrom(Content.Load<Texture2D>(@"Locations\EyeOfTheMaelstrom"),
                  Content.Load<Texture2D>(@"Backgrounds\EyeOfTheMaelstrom"), overlay, negate),
              new FearValley(Content.Load<Texture2D>(@"Locations\FearValley"),
                  Content.Load<Texture2D>(@"Backgrounds\FearValley"), overlay, negate),
              new ForestOfLife(Content.Load<Texture2D>(@"Locations\ForestOfLife"),
                  Content.Load<Texture2D>(@"Backgrounds\ForestOfLife"), overlay, negate),
              new Gigantempopolis(Content.Load<Texture2D>(@"Locations\Gigantempopolis"),
                  Content.Load<Texture2D>(@"Backgrounds\Gigantempopolis"), overlay, negate),
              new GlacierPlains(Content.Load<Texture2D>(@"Locations\GlacierPlains"),
                  Content.Load<Texture2D>(@"Backgrounds\GlacierPlains"), overlay, negate),
              new GloomuckSwamp(Content.Load<Texture2D>(@"Locations\GloomuckSwamp"),
                  Content.Load<Texture2D>(@"Backgrounds\GloomuckSwamp"), overlay, negate),
              new GothosTower(Content.Load<Texture2D>(@"Locations\GothosTower"),
                  Content.Load<Texture2D>(@"Backgrounds\GothosTower"), overlay, negate),
              new IronPillar(Content.Load<Texture2D>(@"Locations\IronPillar"),
                  Content.Load<Texture2D>(@"Backgrounds\IronPillar"), overlay, negate),
              new KiruCity(Content.Load<Texture2D>(@"Locations\KiruCity"),
                  Content.Load<Texture2D>(@"Backgrounds\KiruCity"), overlay, negate),
              new LakeKenIPo(Content.Load<Texture2D>(@"Locations\LakeKenIPo"),
                  Content.Load<Texture2D>(@"Backgrounds\LakeKenIPo"), overlay, negate),
              new LavaPond(Content.Load<Texture2D>(@"Locations\LavaPond"),
                  Content.Load<Texture2D>(@"Backgrounds\LavaPond"), overlay, negate),
              new MipedimOasis(Content.Load<Texture2D>(@"Locations\MipedimOasis"),
                  Content.Load<Texture2D>(@"Backgrounds\MipedimOasis"), overlay, negate),
              new MountPillar(Content.Load<Texture2D>(@"Locations\MountPillar"),
                  Content.Load<Texture2D>(@"Backgrounds\MountPillar"), overlay, negate),
              new RavanaughRidge(Content.Load<Texture2D>(@"Locations\RavanaughRidge"),
                  Content.Load<Texture2D>(@"Backgrounds\RavanaughRidge"), overlay, negate),
              new Riverlands(Content.Load<Texture2D>(@"Locations\Riverlands"),
                  Content.Load<Texture2D>(@"Backgrounds\Riverlands"), overlay, negate),
              new RunicGrove(Content.Load<Texture2D>(@"Locations\RunicGrove"),
                  Content.Load<Texture2D>(@"Backgrounds\RunicGrove"), overlay, negate),
              new StonePillar(Content.Load<Texture2D>(@"Locations\StonePillar"),
                  Content.Load<Texture2D>(@"Backgrounds\StonePillar"), overlay, negate),
              new StormTunnel(Content.Load<Texture2D>(@"Locations\StormTunnel"),
                  Content.Load<Texture2D>(@"Backgrounds\StormTunnel"), overlay, negate),
              new StrongholdMorn(Content.Load<Texture2D>(@"Locations\StrongholdMorn"),
                  Content.Load<Texture2D>(@"Backgrounds\StrongholdMorn"), overlay, negate),
              new UnderworldCity(Content.Load<Texture2D>(@"Locations\UnderworldCity"),
                  Content.Load<Texture2D>(@"Backgrounds\UnderworldCity"), overlay, negate),
              new UnderworldColosseum(Content.Load<Texture2D>(@"Locations\UnderworldColosseum"),
                  Content.Load<Texture2D>(@"Backgrounds\UnderworldColosseum"), overlay, negate),
              new WoodenPillar(Content.Load<Texture2D>(@"Locations\WoodenPillar"),
                  Content.Load<Texture2D>(@"Backgrounds\WoodenPillar"), overlay, negate),
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
                case MenuStage.SplashScreen:
                    if (gameTime.TotalGameTime.Seconds > 2)
                        ChaoticEngine.MStage = MenuStage.MainMenu;
                    break;
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
                    #if DEBUG
                        ChaoticEngine.MStage = MenuStage.Ready1On1;
                        List<string> deck = ChaoticEngine.LoadFile("Test1");
                        ChaoticEngine.sCreatures1 = ChaoticEngine.LoadCards<Creature>(1, 0, deck);
                        ChaoticEngine.sBattlegears1 = ChaoticEngine.LoadCards<Battlegear>(1, 6, deck);
                        ChaoticEngine.sMugics1 = ChaoticEngine.LoadCards<Mugic>(1, 12, deck);
                        ChaoticEngine.sAttacks1 = ChaoticEngine.LoadCards<Attack>(10, 18, deck);
                        ChaoticEngine.sLocations1 = ChaoticEngine.LoadCards<Location>(5, 38, deck);
                        deck = ChaoticEngine.LoadFile("OverWorld Starter Deck");
                        ChaoticEngine.sCreatures2 = ChaoticEngine.LoadCards<Creature>(1, 0, deck);
                        ChaoticEngine.sBattlegears2 = ChaoticEngine.LoadCards<Battlegear>(1, 6, deck);
                        ChaoticEngine.sMugics2 = ChaoticEngine.LoadCards<Mugic>(1, 12, deck);
                        ChaoticEngine.sAttacks2 = ChaoticEngine.LoadCards<Attack>(10, 18, deck);
                        ChaoticEngine.sLocations2 = ChaoticEngine.LoadCards<Location>(5, 38, deck);
                        ChaoticEngine.Player2Setup = true;
                    #else
                        ChaoticEngine.MStage = MenuStage.Wait1On1;
                        System.Windows.Forms.Application.EnableVisualStyles();
                        new DeckSetup1On1("Test1").ShowDialog();
                    #endif
                    break;
                case MenuStage.ThreeOnThree:
                    #if DEBUG
                        ChaoticEngine.MStage = MenuStage.Ready3On3;
                        deck = ChaoticEngine.LoadFile("Test1");
                        ChaoticEngine.sCreatures1 = ChaoticEngine.LoadCards<Creature>(3, 0, deck);
                        ChaoticEngine.sBattlegears1 = ChaoticEngine.LoadCards<Battlegear>(3, 6, deck);
                        ChaoticEngine.sMugics1 = ChaoticEngine.LoadCards<Mugic>(3, 12, deck);
                        ChaoticEngine.sAttacks1 = ChaoticEngine.LoadCards<Attack>(10, 18, deck);
                        ChaoticEngine.sLocations1 = ChaoticEngine.LoadCards<Location>(5, 38, deck);
                        deck = ChaoticEngine.LoadFile("OverWorld Starter Deck");
                        ChaoticEngine.sCreatures2 = ChaoticEngine.LoadCards<Creature>(3, 0, deck);
                        ChaoticEngine.sBattlegears2 = ChaoticEngine.LoadCards<Battlegear>(3, 6, deck);
                        ChaoticEngine.sMugics2 = ChaoticEngine.LoadCards<Mugic>(3, 12, deck);
                        ChaoticEngine.sAttacks2 = ChaoticEngine.LoadCards<Attack>(10, 18, deck);
                        ChaoticEngine.sLocations2 = ChaoticEngine.LoadCards<Location>(5, 38, deck);
                        ChaoticEngine.Player2Setup = true;
                    #else
                        ChaoticEngine.MStage = MenuStage.Wait3On3;
                        System.Windows.Forms.Application.EnableVisualStyles();
                        new DeckSetup3On3("Test1").ShowDialog();
                    #endif
                    break;
                case MenuStage.SixOnSix:
                    #if DEBUG
                        ChaoticEngine.MStage = MenuStage.Ready6On6;
                        deck = ChaoticEngine.LoadFile("Test1");
                        ChaoticEngine.sCreatures1 = ChaoticEngine.LoadCards<Creature>(6, 0, deck);
                        ChaoticEngine.sBattlegears1 = ChaoticEngine.LoadCards<Battlegear>(6, 6, deck);
                        ChaoticEngine.sMugics1 = ChaoticEngine.LoadCards<Mugic>(6, 12, deck);
                        ChaoticEngine.sAttacks1 = ChaoticEngine.LoadCards<Attack>(20, 18, deck);
                        ChaoticEngine.sLocations1 = ChaoticEngine.LoadCards<Location>(10, 38, deck);
                        deck = ChaoticEngine.LoadFile("OverWorld Starter Deck");
                        ChaoticEngine.sCreatures2 = ChaoticEngine.LoadCards<Creature>(6, 0, deck);
                        ChaoticEngine.sBattlegears2 = ChaoticEngine.LoadCards<Battlegear>(6, 6, deck);
                        ChaoticEngine.sMugics2 = ChaoticEngine.LoadCards<Mugic>(6, 12, deck);
                        ChaoticEngine.sAttacks2 = ChaoticEngine.LoadCards<Attack>(20, 18, deck);
                        ChaoticEngine.sLocations2 = ChaoticEngine.LoadCards<Location>(10, 38, deck);
                        ChaoticEngine.Player2Setup = true;
                    #else
                        ChaoticEngine.MStage = MenuStage.Wait6On6;
                        System.Windows.Forms.Application.EnableVisualStyles();
                        new DeckSetup6On6("Test1").ShowDialog();
                    #endif
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
                case MenuStage.SplashScreen:
                    spriteBatch.Draw(splashScreen, new Rectangle(0, 0,
                        graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                    break;
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