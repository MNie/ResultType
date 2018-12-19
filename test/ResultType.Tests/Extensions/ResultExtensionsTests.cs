namespace ResultType.Tests.Extensions
{
    using System;
    using System.Threading.Tasks;
    using ResultType.Extensions;
    using ResultType.Factories;
    using ResultType.Results;
    using Shouldly;
    using Xunit;

    public class ResultExtensionsTests
    {
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
    }
}