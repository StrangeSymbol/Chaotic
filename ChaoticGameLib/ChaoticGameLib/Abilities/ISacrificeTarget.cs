using System;

namespace ChaoticGameLib
{
    public interface ISacrificeTarget<T> : ISacrifice where T : ChaoticCard
    {
        void Ability(T card);
    }
}
