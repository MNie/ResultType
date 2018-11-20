namespace ResultType.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Factories;
    using Results;

    public static class ResultExtensions
    {
        public static Result<TType> ToSuccess<TType>(this TType obj) 
            => ResultFactory.CreateSuccess<TType>(obj);
        
        public static Result<TType> ToFailure<TType>(this Exception obj, string msg) 
            => ResultFactory.CreateFailure<TType>(new Error(msg));
        
        public static Result<TType> ToFailure<TType>(this Exception obj) 
            => ResultFactory.CreateFailure<TType>(new Error(obj.Message));
        
        public static Task<Result<TType>> ToSuccessAsync<TType>(this TType obj) 
            => ResultFactory.CreateSuccessAsync<TType>(obj);
        
        public static Task<Result<TType>> ToFailureAsync<TType>(this Exception obj, string msg) 
            => ResultFactory.CreateFailureAsync<TType>(new Error(msg));
        
        public static Task<Result<TType>> ToFailureAsync<TType>(this Exception obj) 
            => ResultFactory.CreateFailureAsync<TType>(new Error(obj.Message));
        
        public static async Task<Result<TType>> ToSuccessAsync<TType>(this Task<TType> obj) 
            => await ResultFactory.CreateSuccessAsync<TType>(await obj);

        public static async Task<Result<TType>> ToFailureAsync<TType>(this Task<Exception> obj, string msg) 
            => await ResultFactory.CreateFailureAsync<TType>(new Error(msg));

        public static async Task<Result<TType>> ToFailureAsync<TType>(this Task<Exception> obj)
        {
            var ex = await obj;
            return await ResultFactory.CreateFailureAsync<TType>(new Error(ex.Message));
        } 
        
    }
}