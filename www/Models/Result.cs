namespace www.Models
{
    public class Result
    {
        public bool Success { get; private init; }
        public string Error { get; private init; }

        public static Result SuccessResult { get; } = new() { Success = true, Error = "" };
        public static Result ErrorResult(string error) => new() { Success = false, Error = error };

        private Result()
        {}
    }
}
