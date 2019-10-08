namespace ResultType.Factories
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using Results;

    public static class ResultFactory
    {
        public static IResult<TPayload> CreateSuccess<TPayload>(TPayload payload) => new Success<TPayload>(payload);
        public static IResult<Unit> CreateSuccess() => CreateSuccess(new Unit());
        public static Task<IResult<TPayload>> CreateSuccessAsync<TPayload>(TPayload payload) => Task.FromResult(CreateSuccess(payload));
        public static Task<IResult<Unit>> CreateSuccessAsync() => CreateSuccessAsync(new Unit());
        
        public static IResult<TPayload> CreateFailure<TPayload>(IError error) => new Failure<TPayload>(error);
        public static IResult<TPayload> CreateFailure<TPayload>(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) => 
            new Failure<TPayload>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        public static IResult<Unit> CreateFailure(IError error) => new Failure<Unit>(error);
        public static IResult<Unit> CreateFailure(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) => 
            new Failure<Unit>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        public static Task<IResult<Unit>> CreateFailureAsync(IError error) => CreateFailureAsync<Unit>(error);
        public static Task<IResult<Unit>> CreateFailureAsync(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) => 
            CreateFailureAsync<Unit>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));
        public static Task<IResult<TPayload>> CreateFailureAsync<TPayload>(IError error) => Task.FromResult(CreateFailure<TPayload>(error));
        public static Task<IResult<TPayload>> CreateFailureAsync<TPayload>(string msg) => Task.FromResult(CreateFailure<TPayload>(msg));
    }
}
