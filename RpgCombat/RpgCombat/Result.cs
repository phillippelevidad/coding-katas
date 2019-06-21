namespace RpgCombat
{
    public class Result
    {
        private Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Ok()
            => new Result(true, null);

        public static Result Failure(string error = null)
            => new Result(false, error);

        public bool IsSuccess { get; }
        public string Error { get; }
        public bool IsFailure => !IsSuccess;
    }
}