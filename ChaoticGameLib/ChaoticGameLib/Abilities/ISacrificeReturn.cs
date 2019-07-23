using System;

namespace ChaoticGameLib
{
    public interface ISacrificeReturn : ISacrifice
    {
        bool CheckReturnable(Creature card);
    }
}
