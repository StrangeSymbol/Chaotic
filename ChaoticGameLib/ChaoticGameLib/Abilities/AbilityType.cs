using System;

namespace ChaoticGameLib
{
    public enum AbilityType : byte
    {
        TargetSelf, TargetSelfChange, TargetCreature, TargetCreatureTwo, SacrificeTargetCreature,
        ReturnMugic, ReturnCreature, TargetLocationDeck, TargetAttackLocationDeck, DispelMugic,
        TargetEquipped, TargetSelectElemental, DestroyTargetBattlegear, TargetAttack, ShuffleTargetDeck, Attack,
        Recklessness, TargetEngaged, TargetDanianCount, TargetEquippedCreature, ChangeTarget, Triggered, TargetLocation,
    };
}
