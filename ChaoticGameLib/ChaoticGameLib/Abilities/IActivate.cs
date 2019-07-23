using System;

namespace ChaoticGameLib
{
    public interface IActivate
    {
        AbilityType Type { get; }
        void PayCost();
    }
}
