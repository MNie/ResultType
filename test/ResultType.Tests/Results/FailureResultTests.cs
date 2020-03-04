namespace ResultType.Tests.Results
{
    using System;
    using System.Threading.Tasks;

    using ResultType.Factories;
    using ResultType.Operations;
    using ResultType.Results;

    using Shouldly;

    using Xunit;

    internal class FailureError : IError
    {
        public FailureError(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        }

        public string Message { get; }
        public string MemberName { get; }
        public string FilePath { get; }
        public int Line { get; }
        public Exception Exception { get; }
        
    }

    public class FailureResultTests
    {
        private static IResult<Unit> result;
        private const string ErrorMsg = "error msg";
        public FailureResultTests() => result = ResultFactory.CreateFailure<Unit>(ErrorMsg);

        [Fact]
        public void Result_withValidResult_HasNotToBeSuccess() => (result is Success<Unit>).ShouldBeFalse();

        [Fact]
        public void Result_withValidResult_HasToBeFailure() => (result is Failure<Unit>).ShouldBeTrue();

        [Fact]
        public void Result_withValidResult_HasErrorMsgEqualToErrorMsg() => (result as Failure<Unit>).Error.Message.ShouldBe(ErrorMsg);
        
        [Fact]
        public void Deconstruct_ReturnError()
        {
            if (result is Failure<Unit> (var payload))
                payload.Message.ShouldBe(ErrorMsg);
            else
                throw new Exception($"{nameof(result)} should be a {nameof(Failure<Unit>)} but was not.");
        }
    }
}
