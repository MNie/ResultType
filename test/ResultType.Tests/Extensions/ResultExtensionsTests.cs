namespace ResultType.Tests.Extensions
{
    using System;
    using System.Threading.Tasks;
    using ResultType.Extensions;
    using ResultType.Factories;
    using ResultType.Results;
    using Shouldly;
    using Xunit;
    using static ResultType.Extensions.ResultExtensions;
    
    public class ResultExtensionsTests
    {
        [Fact]
        public void Success_ReturnSuccess() => Success().IsSuccess.ShouldBeTrue();
        [Fact]
        public void ToSuccess_ReturnSuccessResult() =>
            12.ToSuccess().IsSuccess.ShouldBeTrue();

        [Fact]
        public async Task ToSuccessAsync_ReturnSuccessResult() =>
            (await 12.ToSuccessAsync()).IsSuccess.ShouldBeTrue();
        
        [Fact]
        public async Task ToSuccessAsync_WhenArgumentIsTask_ReturnSuccessResult() =>
            (await Task.FromResult(12).ToSuccessAsync()).IsSuccess.ShouldBeTrue();
        
        [Fact]
        public async Task ToFailureAsync_WhenArgumentIsExceptionWithoutMsg_ReturnFailureResult() =>
            ((Error) (await Task.FromResult(new Exception("error")).ToFailureAsync<Unit>()).Error)
            .Message
            .ShouldBe("error");
        
        [Fact]
        public async Task ToFailureAsync_ReturnSuccessResult() =>
            ((Error) (await (new Exception("error").ToFailureAsync<Unit>("dd"))).Error)
            .Message
            .ShouldBe("dd");
        
        [Fact]
        public void ToFailure_WhenArgumentIsException_ReturnFailureResult() =>
            ((Error) (new Exception("error").ToFailure<Unit>("dd")).Error)
            .Message
            .ShouldBe("dd");
        
        [Fact]
        public void ToFailure_WhenArgumentIsString_ReturnFailureResult() =>
            "error".ToFailure<Unit>()
                .IsFailure
                .ShouldBeTrue();
        
        [Fact]
        public void Map_OnSuccess_Return1() =>
            12.ToSuccess().Map(x => 1, y => 2).ShouldBe(1);
        
        [Fact]
        public async Task MapAsync_OnSuccess_Return1() =>
            (await 12.ToSuccessAsync().MapAsync(x => 1, y => 2)).ShouldBe(1);
        
        [Fact]
        public async Task MapAsync_OnSuccess_ReturnTaskWith1Inside() =>
            (await 12.ToSuccessAsync().MapAsync(x => Task.FromResult(1), y => Task.FromResult(2))).ShouldBe(1);
        
        [Fact]
        public void Map_OnFailure_Return2() =>
            ResultFactory.CreateFailure<Unit>("").Map(x => 1, y => 2).ShouldBe(2);
        
        [Fact]
        public async Task MapAsync_OnFailure_Return2() =>
            (await ResultFactory.CreateFailureAsync<Unit>("").MapAsync(x => 1, y => 2)).ShouldBe(2);
        
        [Fact]
        public async Task MapAsync_OnFailure_ReturnTaskWith2Inside() =>
            (await ResultFactory.CreateFailureAsync<Unit>("").MapAsync(x => Task.FromResult(1), y => Task.FromResult(2))).ShouldBe(2);

        [Fact]
        public void ToSuccessWhen_WhenConditionIsTrue_ReturnSuccess() =>
            "leszek walesa"
                .ToSuccessWhen(x => !string.IsNullOrWhiteSpace(x), "leszek jest pusty")
                .IsSuccess
                .ShouldBeTrue();
        
        [Fact]
        public void ToSuccessWhen_WhenConditionIsFalse_ReturnFailure() =>
            "leszek walesa"
                .ToSuccessWhen(string.IsNullOrWhiteSpace, "leszek jest pusty")
                .IsFailure
                .ShouldBeTrue();
        
        [Fact]
        public void ToFailureWhen_WhenConditionIsTrue_ReturnFailure() =>
            "leszek walesa"
                .ToFailureWhen(x => !string.IsNullOrWhiteSpace(x), "leszek jest pusty")
                .IsFailure
                .ShouldBeTrue();
        
        [Fact]
        public void ToFailureWhen_WhenConditionIsFalse_ReturnSuccess() =>
            "leszek walesa"
                .ToFailureWhen(string.IsNullOrWhiteSpace, "leszek jest pusty")
                .IsSuccess
                .ShouldBeTrue();

        [Fact]
        public void Flatten_WhenWeHaveAllSuccesses_ReturnSuccess()
        {
            var results = new[]
            {
                "d".ToSuccess(),
                "dd".ToSuccess(),
                "de".ToSuccess()
            };

            var result = results.Flatten();
            result.IsSuccess.ShouldBeTrue();
        }
        
        [Fact]
        public async Task FlattenAsync_WhenWeHaveAllSuccesses_ReturnSuccess()
        {
            var results = new[]
            {
                "d".ToSuccessAsync(),
                "dd".ToSuccessAsync(),
                "de".ToSuccessAsync()
            };

            var result = await results.FlattenAsync();
            result.IsSuccess.ShouldBeTrue();
        }
        
        [Fact]
        public void Flatten_WhenWeHaveSingleError_ReturnFailure()
        {
            var results = new[]
            {
                "d".ToSuccess(),
                "dd".ToFailure<string>(),
                "de".ToSuccess()
            };

            var result = results.Flatten();
            result.IsFailure.ShouldBeTrue();
            (result.Error as AggregateError).Errors.Count.ShouldBe(1);
        }
        
        [Fact]
        public async Task FlattenAsync_WhenWeHaveSingleError_ReturnFailure()
        {
            var results = new []
            {
                "d".ToSuccessAsync(),
                "dd".ToFailureAsync<string>(),
                "de".ToSuccessAsync()
            };

            var result = await results.FlattenAsync();
            result.IsFailure.ShouldBeTrue();
            (result.Error as AggregateError).Errors.Count.ShouldBe(1);
        }
        
        [Fact]
        public void Flatten_WhenWeHaveAllFailures_ReturnFailure()
        {
            var results = new[]
            {
                "d".ToFailure<string>(),
                "dd".ToFailure<string>(),
                "de".ToFailure<string>(),
            };

            var result = results.Flatten();
            result.IsFailure.ShouldBeTrue();
            (result.Error as AggregateError).Errors.Count.ShouldBe(3);
        }
        
        [Fact]
        public async Task FlattenAsync_WhenWeHaveAllFailures_ReturnFailure()
        {
            var results = new[]
            {
                "d".ToFailureAsync<string>(),
                "dd".ToFailureAsync<string>(),
                "de".ToFailureAsync<string>(),
            };

            var result = await results.FlattenAsync();
            result.IsFailure.ShouldBeTrue();
            (result.Error as AggregateError).Errors.Count.ShouldBe(3);
        }
        
        
        [Fact]
        public void ToSuccessWhen_WhenConditionIsTrueAndWePropagateIError_ReturnSuccess() =>
            "leszek walesa"
                .ToSuccessWhen(x => !string.IsNullOrWhiteSpace(x), new TestError("heheszki", "", "", 0))
                .IsSuccess
                .ShouldBeTrue();

        [Fact]
        public void ToSuccessWhen_WhenConditionIsFalseWePropagateIError_ReturnFailureWithPropagatedError()
        {
            var err = new TestError("heheszki", "", "", 0);
            var result = "leszek walesa"
                .ToSuccessWhen(string.IsNullOrWhiteSpace, err);
            
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldBe(err);
        }
        
        [Fact]
        public void ToFailureWhen_WhenConditionIsTrueWePropagateIError_ReturnFailureWithPropagatedError()
        {
            var err = new TestError("heheszki", "", "", 0);
            var result = "leszek walesa"
                .ToFailureWhen(x => !string.IsNullOrWhiteSpace(x), err);
        
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldBe(err);
        }

        [Fact]
        public void ToFailureWhen_WhenConditionIsFalseWePropagateIError_ReturnSuccess() =>
            "leszek walesa"
                .ToFailureWhen(string.IsNullOrWhiteSpace, new TestError("heheszki", "", "", 0))
                .IsSuccess
                .ShouldBeTrue();
    }
}