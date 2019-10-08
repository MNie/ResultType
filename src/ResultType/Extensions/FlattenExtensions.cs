namespace ResultType.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Factories;
    using Results;

    public static class FlattenExtensions
    {
        public static IResult<IEnumerable<T>> Flatten<T>(this IEnumerable<IResult<T>> results)
        {
            var resultList = results.ToArray();
            var errors = resultList.Where(x => x is Failure<T>).Select(failure => ((Failure<T>)failure).Error).ToList();
            var payloads = resultList.Where(x => x is Success<T>).Select(failure => ((Success<T>)failure).Payload);
            return errors.Any()
                ? ResultFactory.CreateFailure<IEnumerable<T>>(new AggregateError(errors))
                : ResultFactory.CreateSuccess(payloads);
        }
        
        public static async Task<IResult<IEnumerable<T>>> FlattenAsync<T>(this IEnumerable<Task<IResult<T>>> results)
        {
            var resultList = results.ToList();
            
            await Task.WhenAll(resultList); 

            var errors = resultList.Where(x => x.Result is Failure<T>).Select(failure => ((Failure<T>)failure.Result).Error).ToList();
            var payloads = resultList.Where(x => x.Result is Success<T>).Select(failure => ((Success<T>)failure.Result).Payload);
            
            return errors.Any()
                ? ResultFactory.CreateFailure<IEnumerable<T>>(new AggregateError(errors))
                : ResultFactory.CreateSuccess(payloads);
        }
    }
}