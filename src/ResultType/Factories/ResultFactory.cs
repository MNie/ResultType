namespace ResultType.Factories
{
    using System.Threading.Tasks;

    using ResultType.Results;

    public static class ResultFactory
    {
        public static Result<TPayload> CreateSuccess<TPayload>(TPayload payload) => new Result<TPayload>(payload);
        public static Result<Unit> CreateSuccess() => CreateSuccess(new Unit());
        public static Result<TPayload> CreateFailure<TPayload>(IError error) => new Result<TPayload>(error);
        public static Result<TPayload> CreateFailure<TPayload>(string msg) => new Result<TPayload>(new Error(msg));
        public static Result<Unit> CreateFailure(IError error) => new Result<Unit>(error);
        public static Result<Unit> CreateFailure(string msg) => new Result<Unit>(new Error(msg));
        public static Task<Result<TPayload>> CreateSuccessAsync<TPayload>(TPayload payload) => Task.FromResult(CreateSuccess(payload));
        public static Task<Result<Unit>> CreateSuccessAsync() => CreateSuccessAsync(new Unit());
        public static Task<Result<Unit>> CreateFailureAsync(IError error) => CreateFailureAsync<Unit>(error);
        public static Task<Result<Unit>> CreateFailureAsync(string msg) => CreateFailureAsync<Unit>(new Error(msg));
        public static Task<Result<TPayload>> CreateFailureAsync<TPayload>(IError error) => Task.FromResult(CreateFailure<TPayload>(error));
        public static Task<Result<TPayload>> CreateFailureAsync<TPayload>(string msg) => Task.FromResult(CreateFailure<TPayload>(msg));
    }
}
