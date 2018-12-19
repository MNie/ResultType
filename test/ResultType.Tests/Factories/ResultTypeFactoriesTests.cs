namespace ResultType.Tests.Factories
{
    using System.Threading.Tasks;

    using ResultType.Factories;

    using Shouldly;

    using Xunit;

    using static ResultType.Factories.ResultFactory;
    public class ResultTypeFactoriesTests
    {
        [Fact]
        public void CreateSuccessWithType_CreatesSuccess()
        {
            var r = CreateSuccess("s");

            r.IsSuccess.ShouldBeTrue();
            r.Payload.ShouldBe("s");
        }

        [Fact]
        public async Task CreateSuccessAsyncWithType_CreatesSuccess()
        {
            var r = await CreateSuccessAsync("s");

            r.IsSuccess.ShouldBeTrue();
            r.Payload.ShouldBe("s");
        }

        [Fact]
        public void CreateSuccess_CreatesSuccess() => CreateSuccess().IsSuccess.ShouldBeTrue();

        [Fact]
        public async Task CreateSuccessAsync_CreatesSuccess() => (await CreateSuccessAsync()).IsSuccess.ShouldBeTrue();

        [Fact]
        public void CreateFailureAsyncWithType_CreatesFailure()
        {
            var r = CreateFailure<string>("s");

            r.IsFailure.ShouldBeTrue();
            r.Error.Message.ShouldBe("s");
        }

        [Fact]
        public void CreateFailureAsyncWithTypeWithError_CreatesFailure()
        {
            var r = CreateFailure<string>(new TestError("s", "member", "path", 0));

            r.IsFailure.ShouldBeTrue();
            r.Error.Message.ShouldBe("s");
        }

        [Fact]
        public void CreateFailure_CreatesFailure()
        {
            var result = CreateFailure("s");
            
            result.IsFailure.ShouldBeTrue();
            result.Error.Message.ShouldBe("s");
            result.Error.MemberName.ShouldBe("CreateFailure_CreatesFailure");
            result.Error.FilePath.ShouldEndWith("ResultType\\test\\ResultType.Tests\\Factories\\ResultTypeFactoriesTests.cs");
            result.Error.Line.ShouldBe(59);
        }

        [Fact]
        public void CreateFailureWithError_CreatesFailure() => CreateFailure(new TestError("s", "member", "path", 0)).IsFailure.ShouldBeTrue();

        [Fact]
        public async Task CreateFailureAsync_CreatesFailure() => (await CreateFailureAsync("s")).IsFailure.ShouldBeTrue();

        [Fact]
        public async Task CreateFailureAsyncWithError_CreatesFailure() => (await CreateFailureAsync(new TestError("s", "member", "path", 0))).IsFailure.ShouldBeTrue();
    }
}
