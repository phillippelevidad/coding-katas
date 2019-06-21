namespace RpgCombat
{
    public class RangedFighter : Character
    {
        private const int attackMaxRange = 20;

        public RangedFighter(int attackMaxRange) : base(attackMaxRange)
        {
        }
    }
}