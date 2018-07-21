namespace ResultType.Tests.Results
{
    using System;

    using ResultType.Factories;
    using ResultType.Results;

    using Shouldly;

    using Xunit;

    public class FailureResultTests
    {
        private static Result<Unit> result;
        private static string errorMsg = "error msg";
        public FailureResultTests() => result = ResultFactory.CreateFailure(errorMsg);

        [Fact]
        public void Result_withValidResult_HasSuccessEqualToFalse() => result.IsSuccess.ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_HasFailureEqualToTrue() => result.IsFailure.ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_WhenGettingPayload_throwsInvalidOperationException() =>
            Assert.Throws<InvalidOperationException>(() => result.Payload);

        [Fact]
        public void Result_withValidResult_HasErrorMsgEqualToErrorMsg() => result.Error.Message.ShouldBe(errorMsg);

        [Fact]
        public void Result_withValidResult_WhenComparingToValidValueResult_ReturnFalse() => (result == ResultFactory.CreateSuccess()).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_WhenComparingToInvalidFailureResult_ReturnFalse() => (result == ResultFactory.CreateFailure("invalidString")).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_WhenComparingToFailureResult_ReturnTrue() => (result == ResultFactory.CreateFailure(errorMsg)).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_WhenComparingIfIsNotEqualToValidValueResult_ReturnTrue() => (result != ResultFactory.CreateSuccess()).ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_WhenComparingIfIsNotEqualToInvalidFailureResult_ReturnTrue() => (result != ResultFactory.CreateFailure("invalidString")).ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_WhenComparingIfIsNotEqualToFailureResult_ReturnFalse() => (result != ResultFactory.CreateFailure(errorMsg)).ShouldBeTrue();
    }
}
