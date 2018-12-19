namespace ResultType.Tests.Results
{
    using System;

    using ResultType.Factories;
    using ResultType.Results;

    using Shouldly;

    using Xunit;

    public class SuccessResultTests
    {
        private static Result<string> result;
        private const string ValidString = "validValue";
        public SuccessResultTests() => result = ResultFactory.CreateSuccess(ValidString);

        [Fact]
        public void Result_withValidResult_HasSuccessEqualToTrue() => result.IsSuccess.ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_HasFailureEqualToFalse() => result.IsFailure.ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_HasEmptyError() => Assert.Throws<InvalidOperationException>(() => result.Error);

        [Fact]
        public void Result_withValidResult_HasPayloadEqualToValidValue() => result.Payload.ShouldBe(ValidString);

        [Fact]
        public void Result_withValidResult_WhenComparingToValidValueResult_ReturnTrue() => (result == ResultFactory.CreateSuccess(ValidString)).ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_WhenComparingToInvalidResult_ReturnFalse() => (result == ResultFactory.CreateSuccess("invalidString")).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_WhenComparingToFailureResult_ReturnFalse() => (result == ResultFactory.CreateFailure<string>(new TestError("error msg", "member", "path", 1))).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_WhenComparingIfIsNotEquaToValidValueResult_ReturnFalse() => (result != ResultFactory.CreateSuccess(ValidString)).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_WhenComparingIfIsNotEquaToInvalidResult_ReturnTrue() => (result != ResultFactory.CreateSuccess("invalidString")).ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_WhenComparingIfIsNotEquaToFailureResult_ReturnTrue() => (result != ResultFactory.CreateFailure<string>(new TestError("error msg", "member", "path", 1))).ShouldBeTrue();
    }
}
