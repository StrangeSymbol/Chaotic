using System;

namespace ChaoticGameLib
{
    public interface ICastTargetTwo<T> : ICast where T : ChaoticCard
    {
        void Ability(T card1, T card2);
    }
}
