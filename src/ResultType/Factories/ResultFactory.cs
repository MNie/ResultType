namespace ResultType.Factories
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Results;

    public static class ResultFactory
    {
        public static Result<TPayload> CreateSuccess<TPayload>(TPayload payload) => new Result<TPayload>(payload);
        public static Result<Unit> CreateSuccess() => CreateSuccess(new Unit());
        public static Task<Result<TPayload>> CreateSuccessAsync<TPayload>(TPayload payload) => Task.FromResult(CreateSuccess(payload));
        public static Task<Result<Unit>> CreateSuccessAsync() => CreateSuccessAsync(new Unit());
        
        public static Result<TPayload> CreateFailure<TPayload>(IError error) => new Result<TPayload>(error);
        public static Result<TPayload> CreateFailure<TPayload>(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) => 
            new Result<TPayload>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        public static Result<Unit> CreateFailure(IError error) => new Result<Unit>(error);
        public static Result<Unit> CreateFailure(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) => 
            new Result<Unit>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        public static Task<Result<Unit>> CreateFailureAsync(IError error) => CreateFailureAsync<Unit>(error);
        public static Task<Result<Unit>> CreateFailureAsync(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) => 
            CreateFailureAsync<Unit>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        public static Task<Result<TPayload>> CreateFailureAsync<TPayload>(IError error) => Task.FromResult(CreateFailure<TPayload>(error));
        public static Task<Result<TPayload>> CreateFailureAsync<TPayload>(string msg) => Task.FromResult(CreateFailure<TPayload>(msg));
    }
}
