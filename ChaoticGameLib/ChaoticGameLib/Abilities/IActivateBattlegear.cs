using System;

namespace ChaoticGameLib
{
    public interface IActivateBattlegear : IActivate
    {
        void PayCost(Creature creature);
        void Ability(Creature creature);
    }
}
