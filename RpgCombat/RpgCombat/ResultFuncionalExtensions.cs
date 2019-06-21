using System;

namespace RpgCombat
{
    public static class ResultFuncionalExtensions
    {
        public static Result OnFailure(this Result result, Action<Result> action)
        {
            if (result.IsFailure)
                action.Invoke(result);

            return result;
        }
    }
}