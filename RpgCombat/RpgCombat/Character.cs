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

        public Result CanDealDamageTo(Character target)
        {
            if (target == this)
                return Result.Failure("Cannot deal damage to self.");

            if (IsAlliesWith(target))
                return Result.Failure("Cannot deal damage to an ally.");

            // TODO: check range, not clear how to represent character positions

            return Result.Ok();
        }

        public void DealDamageTo(Character target, int damagePoints)
        {
            CanDealDamageTo(target)
                .OnFailure(result => throw new InvalidOperationException(result.Error));

            var modifier = GetDamageModifier(this, target);
            var actualDamage = (int)Math.Round(damagePoints * modifier);

            target.TakeDamage(actualDamage);
        }

        public Result CanHeal(Character target)
        {
            if (target != this && !IsAlliesWith(target))
                return Result.Failure("A character can only heal itself or an ally.");

            if (!target.IsAlive)
                return Result.Failure("Cannot heal a dead character.");

            return Result.Ok();
        }

        public Result CanHealSelf()
        {
            return CanHeal(this);
        }

        public void Heal(Character target, int healPoints)
        {
            CanHeal(target)
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