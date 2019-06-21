using System;
using System.Collections.Generic;
using System.Linq;

namespace RpgCombat
{
    public abstract class Character
    {
        private const int maxHealth = 1000;
        private const int startingHealth = maxHealth;
        private const int startingLevel = 1;

        private readonly List<string> factions = new List<string>();

        protected Character(int attackMaxRange)
        {
            AttackMaxRange = attackMaxRange;
            Health = startingHealth;
            IsAlive = true;
            Level = startingLevel;
        }

        public int AttackMaxRange { get; }
        public int Health { get; private set; }
        public bool IsAlive { get; private set; }
        public int Level { get; private set; }

        public IReadOnlyList<string> Factions => factions.ToList();

        public Result CanDealDamageTo(Character other)
        {
            if (other == this)
                return Result.Failure("Cannot deal damage to self.");

            if (IsAlliesWith(other))
                return Result.Failure("Cannot deal damage to an ally.");

            // TODO: check range, not clear how to represent character positions

            return Result.Ok();
        }

        public void DealDamageTo(Character other, int damagePoints)
        {
            CanDealDamageTo(other)
                .OnFailure(result => throw new InvalidOperationException(result.Error));

            var modifier = GetDamageModifier(this, other);
            var actualDamage = (int)Math.Round(damagePoints * modifier);

            other.TakeDamage(actualDamage);
        }

        public Result CanHeal(Character other)
        {
            if (other != this && !IsAlliesWith(other))
                return Result.Failure("A character can only heal itself or an ally.");

            if (!other.IsAlive)
                return Result.Failure("Cannot heal a dead character.");

            return Result.Ok();
        }

        public Result CanHealSelf()
        {
            return CanHeal(this);
        }

        public void Heal(Character other, int healPoints)
        {
            CanHeal(other)
                .OnFailure(result => throw new InvalidOperationException(result.Error));

            Health = Health + healPoints > maxHealth
                ? maxHealth
                : Health + healPoints;
        }

        public void HealSelf(int healPoints)
        {
            Heal(this, healPoints);
        }

        public void JoinFaction(string faction)
        {
            if (!factions.Contains(faction))
                factions.Add(faction);
        }

        public void LeaveFaction(string faction)
        {
            if (factions.Contains(faction))
                factions.Remove(faction);
        }

        public bool IsAlliesWith(Character other)
        {
            return factions.Intersect(other.Factions).Any();
        }

        public void RaiseLevelTo(int newLevel)
        {
            Level = newLevel;
        }

        private double GetDamageModifier(Character attacker, Character target)
        {
            if (attacker.Level - target.Level >= 5)
                return 1.5;

            if (target.Level - attacker.Level >= 5)
                return 0.5;

            return 1;
        }

        private void TakeDamage(int damagePoints)
        {
            Health -= damagePoints;

            if (Health <= 0)
            {
                Health = 0;
                IsAlive = false;
            }
        }
    }
}