using System;

namespace ChaoticGameLib
{
    public interface ICastReturn : ICast
    {
        bool CheckReturnable(Creature card);
    }
}
