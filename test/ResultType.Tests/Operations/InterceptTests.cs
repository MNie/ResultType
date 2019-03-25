namespace ResultType.Tests.Operations
{
    using ResultType.Extensions;
    using ResultType.Operations;
    using Shouldly;
    using Xunit;

    public class InterceptTests
    {
        [Fact]
        public void Intercept_DoingSomething_ReturnResultWithoutChangeAtTheEnd()
        {
            var input = "tt".ToSuccess();
            var counter = 0;
            
            var result = input.Intercept(() => counter++);
            
            result.ShouldBe(input);
            counter.ShouldBe(1);
        }
        
        [Fact]
        public void InterceptAsync_DoingSomething_ReturnResultWithoutChangeAtTheEnd()
        {
            var input = "tt".ToSuccessAsync();
            var counter = 0;
            
            var result = input.InterceptAsync(() => counter++);
            
            counter.ShouldBe(1);
        }
    }
}