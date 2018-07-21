namespace ResultType.Validation.Tests.Rule
{
    using System;

    using ResultType.Validation.Operations;
    using ResultType.Validation.Rule;

    using Shouldly;

    using Xunit;

    public class RuleTests
    {
        [Fact]
        public void Rule_ifPredicateIsTrue_ReturnSuccessResult() =>
            new Rule(() => "name".StartsWith("n"), "name").Apply().IsSuccess.ShouldBeTrue();

        [Fact]
        public void Rule_ifEmptyRuleWasUsed_ReturnSuccessResult() =>
            Rule.CreateEmpty().Apply().IsSuccess.ShouldBeTrue();

        [Fact]
        public void Rule_ifPredicateIsFalseTrue_ReturnFailureResult()
        {
            var result = new Rule(() => "name".StartsWith("d"), "name").Apply();

            result.IsFailure.ShouldBeTrue();
            result.Error.Message.ShouldBe("name");
        }

        [Fact]
        public void Rule_ifAllPredicatesAreTrue_ReturnSuccessResult()
        {
            var rules = new[]
            {
                new Rule(() => "name".StartsWith("n"), "name"),
                new Rule(() => "dd".StartsWith("d"), "dd"),
                new Rule(() => "hh".StartsWith("h"), "hh")
            };
            var result = rules.Apply();

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact]
        public void Rule_ifSomePredicatesAreFalse_ReturnFailureResultWithValidMessage()
        {
            var rules = new[]
            {
                new Rule(() => "name".StartsWith("n"), "name"),
                new Rule(() => "dd".StartsWith("e"), "dd"),
                new Rule(() => "hh".StartsWith("a"), "hh"),
                new Rule(() => "hehe".StartsWith("h"), "hehe"),
            };
            var result = rules.Apply();

            result.IsFailure.ShouldBeTrue();
            result.Error.Message.ShouldBe($"dd{Environment.NewLine}hh");
        }

        [Fact]
        public void Rule_ifPredicatesAreEmpty_ReturnSuccessResult() => new IRule[0].Apply().IsSuccess.ShouldBeTrue();
    }
}
