namespace ResultType.Tests.Extensions
{
    using System;
    using System.Threading.Tasks;
    using ResultType.Extensions;
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
        public async Task ToFailureAsync_WithoutMsg_ReturnSuccessResult() =>
            ((Error) (await (new Exception("error").ToFailureAsync<Unit>())).Error)
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
        public void ToFailure_WhenArgumentIsExceptionButMessageIsNotProvided_ReturnFailureResult() =>
            new Exception("error").ToFailure<Unit>()
                .Error
                .Message
                .ShouldBe("error");
    }
}