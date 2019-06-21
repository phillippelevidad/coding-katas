using System;

namespace RpgCombat
{
    public class MeleeFighter : Character
    {
        private const int attackMaxRange = 2;

        public MeleeFighter() : base(attackMaxRange)
        {
        }
    }
}