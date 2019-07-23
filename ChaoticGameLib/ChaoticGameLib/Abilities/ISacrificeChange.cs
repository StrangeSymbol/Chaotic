using System;

namespace ChaoticGameLib
{
    public interface ISacrificeChange : ISacrifice
    {
        void Ability(ref bool change);
    }
}
