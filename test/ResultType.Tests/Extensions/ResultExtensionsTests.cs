namespace ResultType.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using ResultType.Extensions;
    using ResultType.Factories;
    using ResultType.Results;
    using Shouldly;
    using Xunit;
    
    public class ResultExtensionsTests
    {
        [Fact]
        public void Success_ReturnSuccess() => (SuccessExtensions.Success() is Success<Unit>).ShouldBeTrue();
        [Fact]
        public void ToSuccess_ReturnSuccessResult() =>
            (12.ToSuccess() is Success<int>).ShouldBeTrue();

        [Fact]
        public async Task ToSuccessAsync_ReturnSuccessResult() =>
            (await 12.ToSuccessAsync() is Success<int>).ShouldBeTrue();
        
        [Fact]
        public async Task ToSuccessAsync_WhenArgumentIsTask_ReturnSuccessResult() =>
            (await Task.FromResult(12).ToSuccessAsync() is Success<int>).ShouldBeTrue();
        
        [Fact]
        public async Task ToFailureAsync_WhenArgumentIsExceptionWithoutMsg_ReturnFailureResult() =>
            (await Task.FromResult(new Exception("error")).ToFailureAsync<Unit>() as Failure<Unit>)
            .Error
            .Message
            .ShouldBe("error");
        
        [Fact]
        public async Task ToFailureAsync_ReturnSuccessResult() =>
            (await new Exception("error").ToFailureAsync<Unit>() as Failure<Unit>)
            .Error
            .Message
            .ShouldBe("error");
        
        [Fact]
        public void ToFailure_WhenArgumentIsException_ReturnFailureResult() =>
            (new Exception("error").ToFailure<Unit>() as Failure<Unit>)
            .Error
            .Message
            .ShouldBe("error");
        
        [Fact]
        public async Task ToFailureWithMsgAsync_ReturnSuccessResult() =>
            (await new Exception("error").ToFailureWithMsgAsync<Unit>("dd") as Failure<Unit>)
            .Error
            .Message
            .ShouldBe("dd");
        
        [Fact]
        public void ToFailureWithMsg_WhenArgumentIsException_ReturnFailureResult() =>
            (new Exception("error").ToFailureWithMsg<Unit>("dd") as Failure<Unit>)
            .Error
            .Message
            .ShouldBe("dd");
        
        [Fact]
        public void ToFailure_WhenArgumentIsString_ReturnFailureResult() =>
            ("error".ToFailure<Unit>()
                is Failure<Unit>)
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
            ("leszek walesa"
                .ToSuccessWhen(x => !string.IsNullOrWhiteSpace(x), "leszek jest pusty")
                 is Success<string>)
                .ShouldBeTrue();
        
        [Fact]
        public void ToSuccessWhen_WhenConditionIsFalse_ReturnFailure() =>
            ("leszek walesa"
                .ToSuccessWhen(string.IsNullOrWhiteSpace, "leszek jest pusty")
                is Failure<string>)
                .ShouldBeTrue();
        
        [Fact]
        public void ToFailureWhen_WhenConditionIsTrue_ReturnFailure() =>
            ("leszek walesa"
                .ToFailureWhen(x => !string.IsNullOrWhiteSpace(x), "leszek jest pusty")
                is Failure<string>)
                .ShouldBeTrue();
        
        [Fact]
        public void ToFailureWhen_WhenConditionIsFalse_ReturnSuccess() =>
            ("leszek walesa"
                .ToFailureWhen(string.IsNullOrWhiteSpace, "leszek jest pusty")
                 is Success<string>)
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
            (result is Success<IEnumerable<string>>).ShouldBeTrue();
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
            (result is Success<IEnumerable<string>>).ShouldBeTrue();
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
            (result is Failure<IEnumerable<string>>).ShouldBeTrue();
            ((result as Failure<IEnumerable<string>>).Error as AggregateError).Errors.Count.ShouldBe(1);
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
            (result is Failure<IEnumerable<string>>).ShouldBeTrue();
            ((result as Failure<IEnumerable<string>>).Error as AggregateError).Errors.Count.ShouldBe(1);
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
            (result is Failure<IEnumerable<string>>).ShouldBeTrue();
            ((result as Failure<IEnumerable<string>>).Error as AggregateError).Errors.Count.ShouldBe(3);
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
            (result is Failure<IEnumerable<string>>).ShouldBeTrue();
            ((result as Failure<IEnumerable<string>>).Error as AggregateError).Errors.Count.ShouldBe(3);
        }
        
        
        [Fact]
        public void ToSuccessWhen_WhenConditionIsTrueAndWePropagateIError_ReturnSuccess() =>
            ("leszek walesa"
                .ToSuccessWhen(x => !string.IsNullOrWhiteSpace(x), new TestError("heheszki", "", "", 0))
                 is Success<string>)
                .ShouldBeTrue();

        [Fact]
        public void ToSuccessWhen_WhenConditionIsFalseWePropagateIError_ReturnFailureWithPropagatedError()
        {
            var err = new TestError("heheszki", "", "", 0);
            var result = "leszek walesa"
                .ToSuccessWhen(string.IsNullOrWhiteSpace, err);
            
            (result is Failure<string>).ShouldBeTrue();
            (result as Failure<string>).Error.ShouldBe(err);
        }
        
        [Fact]
        public void ToFailureWhen_WhenConditionIsTrueWePropagateIError_ReturnFailureWithPropagatedError()
        {
            var err = new TestError("heheszki", "", "", 0);
            var result = "leszek walesa"
                .ToFailureWhen(x => !string.IsNullOrWhiteSpace(x), err);
        
            (result is Failure<string>).ShouldBeTrue();
            (result as Failure<string>).Error.ShouldBe(err);
        }

        [Fact]
        public void ToFailureWhen_WhenConditionIsFalseWePropagateIError_ReturnSuccess() =>
            ("leszek walesa"
                .ToFailureWhen(string.IsNullOrWhiteSpace, new TestError("heheszki", "", "", 0))
                 is Success<string>)
                .ShouldBeTrue();

        [Fact]
        public void IsSuccess_WhenResultIsSuccess_ReturnTrue() =>
            ResultFactory.CreateSuccess("on nie wiedzial").IsSuccess().ShouldBeTrue();
        
        [Fact]
        public void IsSuccess_WhenResultIsFailure_ReturnFalse() =>
            ResultFactory.CreateFailure("on nie wiedzial - exception").IsSuccess().ShouldBeFalse();
        
        [Fact]
        public void IsFailure_WhenResultIsSuccess_ReturnFalse() =>
            ResultFactory.CreateSuccess("on nie wiedzial").IsFailure().ShouldBeFalse();
        
        [Fact]
        public void IsFailure_WhenResultIsFailure_ReturnTrue() =>
            ResultFactory.CreateFailure("on nie wiedzial - exception").IsFailure().ShouldBeTrue();
    }
}