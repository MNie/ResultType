namespace ResultType.Operations
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Results;

    public static class InterceptOperations
    {
        [DebuggerStepThrough]
        public static IResult<TInput> Intercept<TInput>(this IResult<TInput> result, Action intercept)
        {
            intercept();
            return result;
        }
          
        [DebuggerStepThrough]
        public static Task<IResult<TInput>> InterceptAsync<TInput>(this Task<IResult<TInput>> result, Action intercept)
        {
            intercept();
            return result;
        }
    }
}