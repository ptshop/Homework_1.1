using System;

namespace www.Models
{
    public class Result
    {
        public bool Success { get; private init; }
        public string Error { get; private init; }

        public static Result SuccessResult { get; } = new();
        public static Result FailedResult(string error) => new(error);

        public void Deconstruct(out bool success, out string error) => (success, error) = (Success, Error);

        protected Result() => (Success, Error) = (true, "");
        protected Result(string error) => (Success, Error) = (false, error);
    }

    public class Result<TValue> : Result
    {
        public TValue Value { get; private init; }

        public static new Result<TValue> SuccessResult(TValue value) => new(value);
        public static new Result<TValue> FailedResult(string error) => new(error);

        public void Deconstruct(out bool success, out TValue value, out string error) => (success, value, error) = (Success, Value, Error);

        protected Result(TValue value) : base() => Value = value;
        protected Result(string error) : base(error) => Value = default;
    }
}
