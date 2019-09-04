using Microsoft.Xna.Framework.Graphics;

namespace ChaoticGameLib.Mugics
{
    public class ChorusOfTheHive : Mugic, ICastChange
    {
        public ChorusOfTheHive(Texture2D sprite, Texture2D overlay) : base(sprite, overlay, MugicType.Danian, 1) { }
        
        public override string Description()
        {
            return base.Description() + " Activate Hive until the end of the turn." +
                " What repels some attracts others. Is preference learned or ingrained?";
        }

        bool ICastChange.Ability()
        {
            return true;
        }

        AbilityType ICast.Type
        {
            get { return AbilityType.TargetSelfChange; }
        }
    }
}