using ResultType.Results;

namespace ResultType.Tests.Operations
{
    using System;
    using System.Threading.Tasks;

    using ResultType.Factories;
    using ResultType.Operations;
    using Results;
    using ResultType.Extensions;
    using Shouldly;

    using Xunit;

    public class BindTests
    {
        private const string first = "first";
        private const string error = "error";
        private const string second = "second";
        private static readonly IResult<string> firstSuccessResult = ResultFactory.CreateSuccess(first);
        private static readonly IResult<string> firstFailureResult = ResultFactory.CreateFailure<string>(error);
        private static Task<IResult<string>> firstSuccessResultAsync => Task.FromResult(firstSuccessResult);
        private static Task<IResult<string>> firstFailureResultAsync => Task.FromResult(firstFailureResult);
        private static readonly IResult<string> secondResult = ResultFactory.CreateSuccess(second);
        private static Task<IResult<string>> secondResultAsync => Task.FromResult(secondResult);

        [Fact]
        public void Bind_WhenFirstEndWithSuccess() =>
            firstSuccessResult.Bind(() => secondResult).Map(x => x, _ => throw new Exception("error")).ShouldBe(second);

        [Fact]
        public async Task Bind_WhenFirstEndWithSuccessAsync() =>
            (await firstSuccessResult.BindAsync(() => secondResultAsync)).Map(x => x, _ => throw new Exception("error")).ShouldBe(second);

        [Fact]
        public void Bind_WhenFirstEndWithFailure() =>
            firstFailureResult.Bind(() => secondResult).Map(_ => throw new Exception("error"), x => x).Message.ShouldBe(error);

        [Fact]
        public async Task Bind_WhenFirstEndWithFailureAsync() =>
            (await firstFailureResult.BindAsync(() => secondResultAsync)).Map(_ => throw new Exception("error"), x => x).Message.ShouldBe(error);

        [Fact]
        public async Task Bind_WhenFirstAsyncEndWithSuccessAsync() =>
            (await firstSuccessResultAsync.BindAsync(() => secondResultAsync)).Map(x => x, _ => throw new Exception("error")).ShouldBe(second);

        [Fact]
        public async Task Bind_WhenFirstAsyncEndWithFailureAsync() =>
            (await firstFailureResultAsync.BindAsync(() => secondResultAsync)).Map(_ => throw new Exception("error"), x => x).Message.ShouldBe(error);

        [Fact]
        public void BindWithOnError_WhenFirstEndWithFailure() =>
            firstFailureResult.Bind(() => secondResult, () => firstSuccessResult).Map(x => x, _ => throw new Exception("error")).ShouldBe(first);

        [Fact]
        public async Task BindWithOnError_WhenFirstEndWithFailureAsync() =>
            (await firstFailureResult.BindAsync(() => secondResultAsync, () => firstSuccessResultAsync)).Map(x => x, _ => throw new Exception("error")).ShouldBe(first);

        [Fact]
        public void BindWithInput_WhenFirstEndWithSuccess() =>
            firstSuccessResult.Bind((input) => ResultFactory.CreateSuccess(input)).Map(x => x, _ => throw new Exception("error")).ShouldBe(first);

        [Fact]
        public async Task BindWithInput_WhenFirstEndWithSuccessAsync() =>
            (await firstSuccessResult.BindAsync((input) => Task.FromResult(ResultFactory.CreateSuccess(input)))).Map(x => x, _ => throw new Exception("error")).ShouldBe(first);

        [Fact]
        public async Task BindAsync_WhenFirstEndWithSuccessAsync() =>
            (await firstSuccessResultAsync.BindAsync((input) => Task.FromResult(ResultFactory.CreateSuccess(input)))).Map(x => x, _ => throw new Exception("error")).ShouldBe(first);

        [Fact]
        public void Bind_OnFailureResult_PropagateError()
        {
            var exception = new ArgumentException("bad arg");
            var failure = new FailureError("some fail", exception);
            var badResult = ResultFactory.CreateFailure<string>(failure);

            var result = badResult.Bind(() => ResultFactory.CreateSuccess(true));

            (result.Map(_ => throw new Exception("error"), x => x) as FailureError).Exception.ShouldBe(exception);
        }

        [Fact]
        public async Task BindAsync_OnFailureResult_PropagateError()
        {
            var exception = new ArgumentException("bad arg");
            var failure = new FailureError("some fail", exception);
            var badResult = await ResultFactory.CreateFailureAsync<string>(failure);

            var result = await badResult.BindAsync(async () => await ResultFactory.CreateSuccessAsync(""));

            (result.Map(_ => throw new Exception("error"), x => x) as FailureError).Exception.ShouldBe(exception);
        }
        
        [Fact]
        public void Bind_WhenOnFailureParameterWasUsedAndResultReturnsAnError_PropagateError()
        {
            var badResult = ResultFactory.CreateFailure<string>("first failure");
            var result = badResult.Bind(onSuccess: (x) => ResultFactory.CreateSuccess(), onFailure: (x) => ResultFactory.CreateFailure($"{x.Message} and second failure"));
            result.Map(_ => throw new Exception("error"), x => x).Message.ShouldBe("first failure and second failure");
        }
        
        [Fact]
        public void Bind_WhenOnSuccessParameterWasUsedAndResultReturnsValidData_PropagatePayload()
        {
            var success = ResultFactory.CreateSuccess("first success");
            var result = success.Bind(
                onSuccess: (x) => ResultFactory.CreateSuccess($"{x} and second one"), 
                onFailure: (x) => ResultFactory.CreateFailure<string>($"{x.Message} and second failure")
            );
            result.Map(x => x, _ => throw new Exception("error")).ShouldBe("first success and second one");
        }
        
        [Fact]
        public async Task BindAsync_OnTaskWhenOnFailureParameterWasUsedAndResultReturnsSuccess_ReturnNotAnErrorMessage()
        {
            var badResult = ResultFactory.CreateFailureAsync<string>("first failure");

            var result = await badResult.BindAsync(onSuccess: (string x) => x.ToSuccessAsync(), onFailure: _ => "not an error".ToSuccessAsync());

            result.Map(x => x, _ => throw new Exception("error")).ShouldBe("not an error");
        }
        
        [Fact]
        public async Task BindAsync_WhenOnFailureParameterWasUsedAndResultReturnsSuccess_ReturnNotAnErrorMessage()
        {
            var badResult = ResultFactory.CreateFailure<string>("first failure");

            var result = await badResult.BindAsync(onSuccess: (string x) => x.ToSuccessAsync(), onFailure: _ => "not an error".ToSuccessAsync());

            result.Map(x => x, _ => throw new Exception("error")).ShouldBe("not an error");
        }
        
        [Fact]
        public async Task BindAsync_OnSuccessResult_DoSomethingWithResult()
        {
            var success = "piesek leszek".ToSuccessAsync();
            var result = await success.BindAsync(async msg => await msg.ToSuccessAsync(),
                ResultFactory.CreateFailureAsync<string>);
            
            result.Map(x => x, _ => throw new Exception("error")).ShouldBe("piesek leszek");
        }
    }
}
