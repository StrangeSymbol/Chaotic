using System;

namespace ChaoticGameLib
{
    public interface ICastTarget<T> : ICast where T : ChaoticCard
    {
        void Ability(T card);
    }
}
