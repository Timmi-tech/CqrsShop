namespace Domain.Common
{
    
    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        private Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T value) => Result<T>.Success(value);
        public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
    }

    public class Result<T>
    {
        public T? Value { get; }
        public Error? Error { get; }
        public bool IsSuccess { get; }

        private Result(T value)
        {
            Value = value;
            IsSuccess = true;
            Error = null;
        }

        private Result(Error error)
        {
            Error = error;
            IsSuccess = false;
            Value = default;
        }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(Error error) => new(error);

        // Transform into another Result
        public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> func)
            => IsSuccess ? func(Value!) : Result<TResult>.Failure(Error!);

        // Transform into another type directly
        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
            => IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
}
