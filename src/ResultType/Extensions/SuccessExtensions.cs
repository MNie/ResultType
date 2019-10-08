namespace ResultType.Extensions
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Factories;
    using Results;

    public static class SuccessExtensions
    {
        public static IResult<Unit> Success() 
            => ResultFactory.CreateSuccess();
        
        public static IResult<TType> ToSuccess<TType>(this TType obj) 
            => ResultFactory.CreateSuccess<TType>(obj);
        
        public static Task<IResult<TType>> ToSuccessAsync<TType>(this TType obj) 
            => ResultFactory.CreateSuccessAsync<TType>(obj);

        public static async Task<IResult<TType>> ToSuccessAsync<TType>(this Task<TType> obj) 
            => await ResultFactory.CreateSuccessAsync<TType>(await obj);
        
        public static IResult<TType> ToSuccessWhen<TType>(this TType obj, Predicate<TType> predicate, string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => 
                predicate(obj) ? ResultFactory.CreateSuccess(obj) : ResultFactory.CreateFailure<TType>(new Error(msg, memberName, sourceFilePath, sourceLineNumber));

        public static IResult<TType> ToSuccessWhen<TType>(this TType obj, Predicate<TType> predicate, IError err, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0) 
            => 
                predicate(obj) ? ResultFactory.CreateSuccess(obj) : ResultFactory.CreateFailure<TType>(err);
    }
}