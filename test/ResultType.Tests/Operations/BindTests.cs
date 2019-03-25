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
        private static readonly Result<string> firstSuccessResult = ResultFactory.CreateSuccess(first);
        private static readonly Result<string> firstFailureResult = ResultFactory.CreateFailure<string>(error);
        private static Task<Result<string>> firstSuccessResultAsync => Task.FromResult(firstSuccessResult);
        private static Task<Result<string>> firstFailureResultAsync => Task.FromResult(firstFailureResult);
        private static readonly Result<string> secondResult = ResultFactory.CreateSuccess(second);
        private static Task<Result<string>> secondResultAsync => Task.FromResult(secondResult);

        [Fact]
        public void Bind_WhenFirstEndWithSuccess() =>
            firstSuccessResult.Bind(() => secondResult).Payload.ShouldBe(second);

        [Fact]
        public async Task Bind_WhenFirstEndWithSuccessAsync() =>
            (await firstSuccessResult.BindAsync(() => secondResultAsync)).Payload.ShouldBe(second);

        [Fact]
        public void Bind_WhenFirstEndWithFailure() =>
            firstFailureResult.Bind(() => secondResult).Error.Message.ShouldBe(error);

        [Fact]
        public async Task Bind_WhenFirstEndWithFailureAsync() =>
            (await firstFailureResult.BindAsync(() => secondResultAsync)).Error.Message.ShouldBe(error);

        [Fact]
        public async Task Bind_WhenFirstAsyncEndWithSuccessAsync() =>
            (await firstSuccessResultAsync.BindAsync(() => secondResultAsync)).Payload.ShouldBe(second);

        [Fact]
        public async Task Bind_WhenFirstAsyncEndWithFailureAsync() =>
            (await firstFailureResultAsync.BindAsync(() => secondResultAsync)).Error.Message.ShouldBe(error);

        [Fact]
        public void BindWithOnError_WhenFirstEndWithFailure() =>
            firstFailureResult.Bind(() => secondResult, () => firstSuccessResult).Payload.ShouldBe(first);

        [Fact]
        public async Task BindWithOnError_WhenFirstEndWithFailureAsync() =>
            (await firstFailureResult.BindAsync(() => secondResultAsync, () => firstSuccessResultAsync)).Payload.ShouldBe(first);

        [Fact]
        public void BindWithInput_WhenFirstEndWithSuccess() =>
            firstSuccessResult.Bind((input) => ResultFactory.CreateSuccess(input)).Payload.ShouldBe(first);

        [Fact]
        public async Task BindWithInput_WhenFirstEndWithSuccessAsync() =>
            (await firstSuccessResult.BindAsync((input) => Task.FromResult(ResultFactory.CreateSuccess(input)))).Payload.ShouldBe(first);

        [Fact]
        public async Task BindAsync_WhenFirstEndWithSuccessAsync() =>
            (await firstSuccessResultAsync.BindAsync((input) => Task.FromResult(ResultFactory.CreateSuccess(input)))).Payload.ShouldBe(first);

        [Fact]
        public void Bind_OnFailureResult_PropagateError()
        {
            var exception = new ArgumentException("bad arg");
            var failure = new FailureError("some fail", exception);
            var badResult = ResultFactory.CreateFailure<string>(failure);

            var result = badResult.Bind(() => ResultFactory.CreateSuccess(true));

            (result.Error as FailureError).Exception.ShouldBe(exception);
        }

        [Fact]
        public async Task BindAsync_OnFailureResult_PropagateError()
        {
            var exception = new ArgumentException("bad arg");
            var failure = new FailureError("some fail", exception);
            var badResult = await ResultFactory.CreateFailureAsync<string>(failure);

            var result = await badResult.BindAsync(async () => await ResultFactory.CreateSuccessAsync(""));

            (result.Error as FailureError).Exception.ShouldBe(exception);
        }
        
        [Fact]
        public void Bind_WhenOnFailureParameterWasUsedAndResultReturnsAnError_PropagateError()
        {
            var badResult = ResultFactory.CreateFailure<string>("first failure");
            var result = badResult.Bind(onSuccess: (x) => ResultFactory.CreateSuccess(), onFailure: (x) => ResultFactory.CreateFailure($"{x.Message} and second failure"));
            result.Error.Message.ShouldBe("first failure and second failure");
        }
        
        [Fact]
        public void Bind_WhenOnSuccessParameterWasUsedAndResultReturnsValidData_PropagatePayload()
        {
            var success = ResultFactory.CreateSuccess("first success");
            var result = success.Bind(
                onSuccess: (x) => ResultFactory.CreateSuccess($"{x} and second one"), 
                onFailure: (x) => ResultFactory.CreateFailure<string>($"{x.Message} and second failure")
            );
            result.Payload.ShouldBe("first success and second one");
        }
        
        [Fact]
        public async Task BindAsync_OnTaskWhenOnFailureParameterWasUsedAndResultReturnsSuccess_ReturnNotAnErrorMessage()
        {
            var badResult = ResultFactory.CreateFailureAsync<string>("first failure");

            var result = await badResult.BindAsync(onSuccess: (string x) => x.ToSuccessAsync(), onFailure: _ => "not an error".ToSuccessAsync());

            result.Payload.ShouldBe("not an error");
        }
        
        [Fact]
        public async Task BindAsync_WhenOnFailureParameterWasUsedAndResultReturnsSuccess_ReturnNotAnErrorMessage()
        {
            var badResult = ResultFactory.CreateFailure<string>("first failure");

            var result = await badResult.BindAsync(onSuccess: (string x) => x.ToSuccessAsync(), onFailure: _ => "not an error".ToSuccessAsync());

            result.Payload.ShouldBe("not an error");
        }
        
        [Fact]
        public async Task BindAsync_OnSuccessResult_DoSomethingWithResult()
        {
            var success = "piesek leszek".ToSuccessAsync();
            var result = await success.BindAsync(async msg => await msg.ToSuccessAsync(),
                async error => ResultFactory.CreateFailure<string>(error));
            
            result.Payload.ShouldBe("piesek leszek");
        }
    }
}
