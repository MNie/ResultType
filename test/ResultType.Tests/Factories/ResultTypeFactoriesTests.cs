namespace ResultType.Tests.Factories
{
    using System.Threading.Tasks;
    using ResultType.Results;
    using Shouldly;

    using Xunit;

    using static ResultType.Factories.ResultFactory;
    public class ResultTypeFactoriesTests
    {
        [Fact]
        public void CreateSuccessWithType_CreatesSuccess()
        {
            var r = CreateSuccess("s");

            (r is Success<string>).ShouldBeTrue();
            (r as Success<string>).Payload.ShouldBe("s");
        }

        [Fact]
        public async Task CreateSuccessAsyncWithType_CreatesSuccess()
        {
            var r = await CreateSuccessAsync("s");

            (r is Success<string>).ShouldBeTrue();
            (r as Success<string>).Payload.ShouldBe("s");
        }

        [Fact]
        public void CreateSuccess_CreatesSuccess() => (CreateSuccess() is Success<Unit>).ShouldBeTrue();

        [Fact]
        public async Task CreateSuccessAsync_CreatesSuccess() => (await CreateSuccessAsync() is Success<Unit>).ShouldBeTrue();

        [Fact]
        public void CreateFailureAsyncWithType_CreatesFailure()
        {
            var r = CreateFailure<string>("s");

            (r is Failure<string>).ShouldBeTrue();
            (r as Failure<string>).Error.Message.ShouldBe("s");
        }

        [Fact]
        public void CreateFailureAsyncWithTypeWithError_CreatesFailure()
        {
            var r = CreateFailure<string>(new TestError("s", "member", "path", 0));

            (r is Failure<string>).ShouldBeTrue();
            (r as Failure<string>).Error.Message.ShouldBe("s");
        }

        [Fact]
        public void CreateFailure_CreatesFailure()
        {
            var result = CreateFailure("s");
            
            (result is Failure<Unit>).ShouldBeTrue();
            (result as Failure<Unit>).Error.Message.ShouldBe("s");
            (result as Failure<Unit>).Error.MemberName.ShouldBe("CreateFailure_CreatesFailure");
            (result as Failure<Unit>).Error.FilePath.ShouldEndWith("ResultTypeFactoriesTests.cs");
            (result as Failure<Unit>).Error.Line.ShouldBe(57);
        }

        [Fact]
        public void CreateFailureWithError_CreatesFailure() => (CreateFailure(new TestError("s", "member", "path", 0)) is Failure<Unit>).ShouldBeTrue();

        [Fact]
        public async Task CreateFailureAsync_CreatesFailure() => (await CreateFailureAsync("s") is Failure<Unit>).ShouldBeTrue();

        [Fact]
        public async Task CreateFailureAsyncWithError_CreatesFailure() => (await CreateFailureAsync(new TestError("s", "member", "path", 0)) is Failure<Unit>).ShouldBeTrue();
    }
}
