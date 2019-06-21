using RpgCombat;

namespace Tests
{
    public static class ResultAssertionExtensions
    {
        public static ResultAssertions Should(this Result result)
        {
            return new ResultAssertions(result);
        }
    }
}