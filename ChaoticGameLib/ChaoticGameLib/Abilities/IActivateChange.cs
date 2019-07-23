using System;

namespace ChaoticGameLib
{
    public interface IActivateChange : IActivate
    {
        void Ability(ref bool turnOn);
    }
}
