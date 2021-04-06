namespace www.Models
{
    public record Result<T>
    {
        public bool Success { get; private init; }
        public T Value { get; private init; }
        public string Error { get; private init; }

        public static Result<T> SuccessResult(T value) => new() { Success = true, Value = value, Error = "" };
        public static Result<T> ErrorResult(string error) => new() { Success = false, Error = error };

        private Result()
        {}
    }
}
