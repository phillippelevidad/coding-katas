using FluentAssertions;
using RpgCombat;
using FluentAssertions.Primitives;

namespace Tests
{
    public class ResultAssertions
    {
        private readonly Result result;

        public ResultAssertions(Result result)
        {
            this.result = result;
        }

        public AndConstraint<BooleanAssertions> BeSuccessful(string because = null, params object[] becauseArgs)
        {
            return result.IsSuccess.Should().BeTrue(because, becauseArgs);
        }

        public AndConstraint<BooleanAssertions> BeFailure(string because = null, params object[] becauseArgs)
        {
            return result.IsFailure.Should().BeTrue(because, becauseArgs);
        }
    }
}