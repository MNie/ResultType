namespace ResultType.Tests.Results
{
    using System;

    using ResultType.Factories;
    using ResultType.Results;

    using Shouldly;

    using Xunit;

    public class SuccessResultTests
    {
        private static IResult<string> result;
        private const string ValidString = "validValue";
        public SuccessResultTests() => result = ResultFactory.CreateSuccess(ValidString);

        [Fact]
        public void Result_withValidResult_HasSuccessEqualToTrue() => (result is Success<string>).ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_HasFailureEqualToFalse() => (result is Failure<string>).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_HasPayloadEqualToValidValue() => (result as Success<string>).Payload.ShouldBe(ValidString);
    }
}
