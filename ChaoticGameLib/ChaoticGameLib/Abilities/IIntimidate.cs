using System;

namespace ChaoticGameLib
{
    public interface IIntimidate
    {
        AbilityType Type { get; }
        void Ability(Creature opposing);
    }
}
