using System;

namespace ChaoticGameLib
{
    public interface ICastTargetCount<T> : ICast where T : ChaoticCard
    {
        void Ability(T card, byte count);
    }
}
