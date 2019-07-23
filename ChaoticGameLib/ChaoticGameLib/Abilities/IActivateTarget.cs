using System;

namespace ChaoticGameLib
{
    public interface IActivateTarget<T> : IActivate where T : ChaoticCard
    {
        void Ability(T card);
    }
}
