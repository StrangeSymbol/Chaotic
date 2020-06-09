/*
 *  Coded by: Ambrose Emmett-Iwaniw
 *  The following code is (c) copyright 2020, StrangeSymbol, Inc. ALL RIGHTS RESERVED
 */
using ChaoticGameLib;

namespace Chaotic
{
    static class CheckSystem
    {
        /// <summary>
        /// This checks whether any mugic in your hand can be cast.
        /// </summary>
        /// <param name="hand">The Mugic cards you currently have.</param>
        /// <param name="creatureSpaces">All the creatures in the game.</param>
        /// <param name="discardPile">Your discard pile for Creatures, Battlegears, and Mugics.</param>
        /// <param name="activeLoc">The current active location.</param>
        /// <returns></returns>
        public static bool CheckAnyMugicPlayable(MugicHand hand, BattleBoardNode[] creatureSpaces,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            foreach (Mugic mugic in hand)
            {
                if (CheckMugicPlayable(hand.IsPlayer1, mugic, creatureSpaces, discardPile, activeLoc))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// This checks whether this mugic can be cast.
        /// </summary>
        /// <param name="isPlayer1">Whether this is player 1.</param>
        /// <param name="mugic">The mugic to be cast.</param>
        /// <param name="creatureSpaces">All the creatures in the game.</param>
        /// <param name="discardPile">Your discard pile for Creatures, Battlegears, and Mugics.</param>
        /// <param name="activeLoc">The current active location.</param>
        /// <returns>Whether this mugic can be cast properly.</returns>
        public static bool CheckMugicPlayable(bool isPlayer1, Mugic mugic, BattleBoardNode[] creatureSpaces,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode != null && creatureSpaces[i].IsPlayer1 == isPlayer1 &&
                        (!ChaoticEngine.GenericMugicOnly || mugic.MugicCasting == MugicType.Generic) &&
                        !(activeLoc.LocationActive is ChaoticGameLib.Locations.DranakisThreshold) &&
                        !(activeLoc.LocationActive is ChaoticGameLib.Locations.LakeKenIPo &&
                        (mugic is ICastDispel || mugic is ChaoticGameLib.Mugics.MugicReprise)) && 
                        mugic.CheckCanPayMugicCost(creatureSpaces[i].CreatureNode, activeLoc.LocationActive) && 
                        CheckAvailableTarget(mugic, creatureSpaces, discardPile))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks whether there is a target for this mugic.
        /// </summary>
        /// <param name="mugic">The mugic to be check to cast.</param>
        /// <param name="creatureSpaces">All the creatures in the game.</param>
        /// <param name="discardPile">Your discard pile for Creatures, Battlegears, and Mugics.</param>
        /// <returns>Whether there is a target.</returns>
        public static bool CheckAvailableTarget(Mugic mugic, BattleBoardNode[] creatureSpaces, DiscardPile<ChaoticCard> discardPile)
        {
            if (mugic is ChaoticGameLib.Mugics.MugicReprise)
            {
                for (int i = 0; i < discardPile.Count; i++)
                {
                    if (discardPile[i] is Mugic)
                        return true;
                }
            }
            else if (mugic is ChaoticGameLib.Mugics.SongOfRevival_UnderWorld_)
            {
                for (int i = 0; i < discardPile.Count; i++)
                {
                    if (discardPile[i] is Creature)
                    {
                        Creature c = discardPile[i] as Creature;
                        if (c.CreatureTribe == Tribe.UnderWorld)
                            return true;
                    }
                }
            }
            else if (mugic is ChaoticGameLib.Mugics.SongOfDeflection && Burst.Alive)
            {
                if (Burst.Peek().Action == AbilityAction.Cast && Burst.Peek()[0] is Mugic)
                    return Burst.Peek().Count == 3; // Mugic has a single target.
                else if (Burst.Peek()[0] is Battlegear)
                    return Burst.Peek().Count == 3;
                else if (Burst.Peek()[0] is Creature)
                    return Burst.Peek().Count == 2;
            }
            else if (mugic is ChaoticGameLib.Mugics.MelodyOfMirage && Burst.Alive)
            {
                return Burst.Peek()[0] is Attack;
            }
            else if ((mugic as ICast).Type == AbilityType.TargetEngaged)
            {
                return ChaoticEngine.sYouNode!= null && mugic.CheckPlayable(ChaoticEngine.sYouNode.CreatureNode)
                    && ChaoticEngine.sEnemyNode != null && mugic.CheckPlayable(ChaoticEngine.sEnemyNode.CreatureNode);
            }
            else if ((mugic as ICast).Type == AbilityType.TargetLocation)
            {
                return true; // TODO: either the activeLocations have a location not negated.
            }
            else if (mugic is ICastDispel)
            {
                ICastDispel dispelMugic = mugic as ICastDispel;
                if (Burst.Alive && Burst.Peek().Action == AbilityAction.Cast && Burst.Peek()[0] is Mugic)
                {
                    Mugic dispel = Burst.Peek()[0] as Mugic;
                    return dispelMugic.CheckDispel(dispel);
                }
            }
            else
            {
                for (int i = 0; i < creatureSpaces.Length; i++)
                {
                    if (creatureSpaces[i].CreatureNode != null && mugic.CheckPlayable(creatureSpaces[i].CreatureNode))
                        return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks whether any Creature can play an effect.
        /// </summary>
        /// <param name="isPlayer1">Is this player 1.</param>
        /// <param name="creatureSpaces">The creature spaces of the board.</param>
        /// <param name="discardPile">The discard pile of isPlayer1.</param>
        /// <returns>Whether there is a Creature that can play an effect.</returns>
        public static bool CheckAnyCreature(bool isPlayer1, BattleBoardNode[] creatureSpaces,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode != null && creatureSpaces[i].IsPlayer1 == isPlayer1 && 
                    (CheckCreatureAbility(isPlayer1, creatureSpaces[i].CreatureNode, creatureSpaces, discardPile, activeLoc) || 
                    CheckCreatureSacrifice(creatureSpaces[i].CreatureNode, creatureSpaces, discardPile, activeLoc)))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Checks if a Creature can activate an effect.
        /// </summary>
        /// <param name="isPlayer1">Is this Player 1.</param>
        /// <param name="creature">The Creature the player is checking can activate an effect.</param>
        /// <param name="creatureSpaces">The creature spaces of the board.</param>
        /// <returns>Whether there is an ability to activate.</returns>
        public static bool CheckCreatureAbility(bool isPlayer1, Creature creature,
            BattleBoardNode[] creatureSpaces, DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            if (!creature.Negate && !(activeLoc.LocationActive is ChaoticGameLib.Locations.DranakisThreshold) &&
                creature.CheckAbility(ChaoticEngine.Hive))
            {
                if (creature is ChaoticGameLib.Creatures.Najarin &&
                    !(activeLoc.LocationActive is ChaoticGameLib.Locations.LakeKenIPo))
                {
                    for (int j = 0; j < discardPile.Count; j++)
                    {
                        if (discardPile[j] is Mugic)
                            return true;
                    }
                }
                else if (creature is IActivateDispel)
                {
                    if (Burst.Alive)
                    {
                        IActivateDispel actDispel = creature as IActivateDispel;
                        if (Burst.Peek().Action == AbilityAction.Cast && Burst.Peek()[0] is Mugic)
                        {
                            Mugic dispel = Burst.Peek()[0] as Mugic;
                            return (actDispel.CheckDispel(dispel) && !(activeLoc.LocationActive is 
                                ChaoticGameLib.Locations.LakeKenIPo)) || Burst.Peek().IsTarget(creature);
                        }
                        else
                            Burst.Peek().IsTarget(creature);
                    }
                }
                else
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode != null &&
                            creature.CheckAbilityTarget(creatureSpaces[i].CreatureNode, creatureSpaces[i].IsPlayer1 == isPlayer1))
                            return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Checks for a specific Creature to sacrifice self to play effect.
        /// </summary>
        /// <param name="creature">The Creature to activate effect by sacrificing.</param>
        /// <param name="creatureSpaces">All the Creatureson the board.</param>
        /// <param name="discardPile">The discard pile for this player.</param>
        /// <returns>Whether Creature can play effect.</returns>
        public static bool CheckCreatureSacrifice(Creature creature, BattleBoardNode[] creatureSpaces, 
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            if (!creature.Negate && !(activeLoc.LocationActive is ChaoticGameLib.Locations.DranakisThreshold) && 
                creature.CheckSacrifice(ChaoticEngine.Hive))
            {
                if (creature is ChaoticGameLib.Creatures.Kannen)
                {
                    for (int j = 0; j < discardPile.Count; j++)
                    {
                        if (discardPile[j] is Creature)
                        {
                            Creature c = discardPile[j] as Creature;
                            if (c.CardType == CreatureType.Mandiblor || c.CardType == CreatureType.MandiblorMuge)
                                return true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode != null && creature.CheckSacrificeTarget(creatureSpaces[i].CreatureNode))
                            return true;
                    }
                }
            }
            return false;
        }

        public static bool CheckAnyBattlegearAbility(bool isPlayer1, BattleBoardNode[] creatureSpaces, ActiveLocation activeLoc)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode != null && creatureSpaces[i].IsPlayer1 == isPlayer1 &&
                    CheckBattlegearAbility(creatureSpaces[i].CreatureNode, creatureSpaces, activeLoc))
                   return true;
            }
            return false;
        }

        public static bool CheckBattlegearAbility(Creature creatureEquipped, BattleBoardNode[] creatureSpaces,
            ActiveLocation activeLoc)
        {
            if (!(activeLoc.LocationActive is ChaoticGameLib.Locations.DranakisThreshold))
            {
                if (creatureEquipped.Battlegear is ChaoticGameLib.Battlegears.MipedianCactus && !creatureEquipped.Battlegear.Negate
                    && creatureEquipped.Battlegear.IsFaceUp && creatureEquipped.CreatureTribe == Tribe.Mipedian)
                {
                    return creatureEquipped.MugicCounters >= 1 && !creatureEquipped.CanMoveAnywhere;
                }
            }
            return false;
        }

        public static bool CheckAnyBattlegearSacrifice(bool isPlayer1, BattleBoardNode[] creatureSpaces,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            for (int i = 0; i < creatureSpaces.Length; i++)
            {
                if (creatureSpaces[i].CreatureNode != null && creatureSpaces[i].IsPlayer1 == isPlayer1 &&
                    creatureSpaces[i].CreatureNode.Battlegear != null &&
                    CheckBattlegearSacrifice(creatureSpaces[i].CreatureNode, creatureSpaces, discardPile, activeLoc))
                    return true;
            }
            return false;
        }

        public static bool CheckBattlegearSacrifice(Creature creature, BattleBoardNode[] creatureSpaces,
            DiscardPile<ChaoticCard> discardPile, ActiveLocation activeLoc)
        {
            if (!creature.Battlegear.Negate && !(activeLoc.LocationActive is ChaoticGameLib.Locations.DranakisThreshold) &&
                creature.Battlegear.CheckSacrifice(creature))
            {
                if (creature.Battlegear is ChaoticGameLib.Battlegears.TalismanOfTheMandiblor && creature.Battlegear.IsFaceUp &&
                creature.CreatureTribe == Tribe.Danian)
                {
                    for (int j = 0; j < discardPile.Count; j++)
                    {
                        if (discardPile[j] is Creature)
                        {
                            Creature c = discardPile[j] as Creature;
                            if (c.CardType == CreatureType.Mandiblor || c.CardType == CreatureType.MandiblorMuge)
                                return true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < creatureSpaces.Length; i++)
                    {
                        if (creatureSpaces[i].CreatureNode != null &&
                            creature.Battlegear.CheckSacrificeTarget(creatureSpaces[i].CreatureNode))
                            return true;
                    }
                }
            }
            return false;
        }
    }
}
