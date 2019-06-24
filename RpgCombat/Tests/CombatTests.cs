using Xunit;
using FluentAssertions;
using RpgCombat;

namespace Tests
{
    public class CombatTests
    {
        [Fact]
        public void WhenDealingDamageToTarget5LevelsAboveDamageIsReducedBy50Pct()
        {
            var attacker = new MeleeFighter();
            var target = new MeleeFighter();

            target.RaiseLevelTo(6);
            attacker.DealDamageTo(target, 100);

            target.Health.Should().Be(950);
        }

        [Fact]
        public void WhenDealingDamageToTarget5LevelsBelowDamageIsIncreasedBy50Pct()
        {
            var attacker = new MeleeFighter();
            var target = new MeleeFighter();

            attacker.RaiseLevelTo(6);
            attacker.DealDamageTo(target, 100);

            target.Health.Should().Be(850);
        }

        [Fact]
        public void ACharacterCannotDealDamageToItself()
        {
            var character = new MeleeFighter();
            character.CanDealDamageTo(character).Should().BeFailure();
        }

        [Fact]
        public void CharactersWhenCreatedStartWithHealth1000()
        {
            var character = new MeleeFighter();
            character.Health.Should().Be(1000);
        }

        [Fact]
        public void CharactersStartAtLevelOne()
        {
            var character = new MeleeFighter();
            character.Level.Should().Be(1);
        }

        [Fact]
        public void CharactersStartAlive()
        {
            var character = new MeleeFighter();
            character.IsAlive.Should().BeTrue();
        }

        [Fact]
        public void CharactersCanDamangeOtherCharacters()
        {
            var attacker = new MeleeFighter();
            var target = new MeleeFighter();
            attacker.CanDealDamageTo(target).Should().BeSuccessful();
        }

        [Fact]
        public void CharactersDieWhenDamageTakenExceedsHealth()
        {
            var attacker = new MeleeFighter();
            var target = new MeleeFighter();

            attacker.DealDamageTo(target, 1001);

            target.IsAlive.Should().BeFalse();
        }

        [Fact]
        public void DeadCharactersCannotBeHealed()
        {
            var attacker = new MeleeFighter();
            var target = new MeleeFighter();

            attacker.DealDamageTo(target, 1001);

            target.CanHealSelf().Should().BeFailure();
            attacker.CanHeal(target).Should().BeFailure();
        }

        [Fact]
        public void HealingCannotRaiseHealthAbove1000()
        {
            var first = new MeleeFighter();
            var second = new MeleeFighter();

            first.DealDamageTo(second, 100);
            second.HealSelf(200);

            second.Health.Should().Be(1000);
        }

        [Fact]
        public void NewCharactersBelongToNoFaction()
        {
            var character = new MeleeFighter();
            character.Factions.Should().BeEmpty();
        }

        [Fact]
        public void ACharacterMayJoinOneOrMoreFactions()
        {
            var character = new MeleeFighter();

            character.JoinFaction("a");
            character.JoinFaction("b");

            character.Factions.Should().HaveCount(2);
            character.Factions.Should().Contain(new[] { "a", "b" });
        }

        [Fact]
        public void ACharacterMayLeaveOneOrMoreFactions()
        {
            var character = new MeleeFighter();

            character.JoinFaction("a");
            character.JoinFaction("b");
            character.JoinFaction("c");
            character.LeaveFaction("a");
            character.LeaveFaction("b");

            character.Factions.Should().HaveCount(1);
            character.Factions.Should().Contain(new[] { "c" });
        }

        [Fact]
        public void PlayersBelongingToTheSameFactionAreConsideredAllies()
        {
            var player1 = new MeleeFighter();
            var player2 = new MeleeFighter();

            player1.JoinFaction("a");
            player2.JoinFaction("a");

            player1.IsAlliesWith(player2).Should().BeTrue();
        }

        [Fact]
        public void AlliesCannotDealDamageToOneAnother()
        {
            var player1 = new MeleeFighter();
            var player2 = new MeleeFighter();

            player1.JoinFaction("a");
            player2.JoinFaction("a");

            player1.CanDealDamageTo(player2).Should().BeFailure();
            player2.CanDealDamageTo(player1).Should().BeFailure();
        }

        [Fact]
        public void AlliesCanHealOneAnother()
        {
            var player1 = new MeleeFighter();
            var player2 = new MeleeFighter();

            player1.JoinFaction("a");
            player2.JoinFaction("a");

            player1.CanHeal(player2).Should().BeSuccessful();
            player2.CanHeal(player1).Should().BeSuccessful();
        }
    }
}