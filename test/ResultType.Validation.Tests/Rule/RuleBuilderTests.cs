namespace ResultType.Validation.Tests.Rule
{
    using System;
    using Results;
    using Shouldly;
    using Validation.Rule;
    using Xunit;

    public class RuleBuilderTests
    {
        public class TestObjectWithFields
        {
            public readonly string FieldA;
            public readonly int FieldB;

            public TestObjectWithFields(string fieldA, int fieldB)
            {
                FieldA = fieldA;
                FieldB = fieldB;
            }
        }
        
        public class TestObjectWithProperties
        {
            public string FieldA { get; }
            public int FieldB { get; }

            public TestObjectWithProperties(string fieldA, int fieldB)
            {
                FieldA = fieldA;
                FieldB = fieldB;
            }
        }

        [Fact]
        public void For_WhenPredicateIsTrue_ReturnResultWithObject()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithFields("", 5))
                .For(x => x.FieldA, string.IsNullOrEmpty)
                .Apply();
                
            (validatedObject is Success<TestObjectWithFields>).ShouldBeTrue();
            (validatedObject as Success<TestObjectWithFields>).Payload.FieldA.ShouldBe("");
        }

        [Fact]
        public void For_WhenOneOfThePredicatesIsFalse_ReturnFailure()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithFields("", 5))
                .For(x => x.FieldA, string.IsNullOrEmpty)
                .For(x => x.FieldB, x => x > 10, "greater than 10")
                .Apply();
                
            (validatedObject as Failure<TestObjectWithFields>).Error.Message.ShouldBe("FieldB should be greater than 10.");
        }

        [Fact]
        public void For_WhenThereIsNoPredicate_ReturnResultWithObject()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithFields("", 5))
                .Apply();
                
            (validatedObject is Success<TestObjectWithFields>).ShouldBeTrue();
            (validatedObject as Success<TestObjectWithFields>).Payload.FieldA.ShouldBe("");
        }

        [Fact]
        public void For_WhenPredicatesOnTheSameFieldFails_ReturnFailureWithInformationAboutBoth()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithFields("", 5))
                .For(x => x.FieldB, x => x > 10, "greater than 10")
                .For(x => x.FieldB, x => x < 2, "less than 2")
                .Apply();
                
            (validatedObject as Failure<TestObjectWithFields>).Error.Message.ShouldBe($"FieldB should be greater than 10.{Environment.NewLine}and FieldB should be less than 2.");
        }

        [Fact]
        public void For_WhenPredicateIsTrue_AndObjectHasProperties_ReturnResultWithObject()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithProperties("", 5))
                .For(x => x.FieldA, string.IsNullOrEmpty)
                .Apply();
                
            (validatedObject is Success<TestObjectWithProperties>).ShouldBeTrue();
            (validatedObject as Success<TestObjectWithProperties>).Payload.FieldA.ShouldBe("");
        }

        [Fact]
        public void For_WhenOneOfThePredicatesIsFalse_AndObjectHasProperties_ReturnFailure()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithProperties("", 5))
                .For(x => x.FieldA, string.IsNullOrEmpty)
                .For(x => x.FieldB, x => x > 10, "greater than 10")
                .Apply();
                
            (validatedObject as Failure<TestObjectWithProperties>).Error.Message.ShouldBe("FieldB should be greater than 10.");
        }

        [Fact]
        public void For_WhenThereIsNoPredicate_AndObjectHasProperties_ReturnResultWithObject()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithProperties("", 5))
                .Apply();
                
            (validatedObject is Success<TestObjectWithProperties>).ShouldBeTrue();
            (validatedObject as Success<TestObjectWithProperties>).Payload.FieldA.ShouldBe("");
        }

        [Fact]
        public void For_WhenPredicatesOnTheSameFieldFails_AndObjectHasProperties_ReturnFailureWithInformationAboutBoth()
        {
            var validatedObject = RuleBuilder
                .Build(() => new TestObjectWithProperties("", 5))
                .For(x => x.FieldB, x => x > 10, "greater than 10")
                .For(x => x.FieldB, x => x < 2, "less than 2")
                .Apply();
                
            (validatedObject as Failure<TestObjectWithProperties>).Error.Message.ShouldBe($"FieldB should be greater than 10.{Environment.NewLine}and FieldB should be less than 2.");
        }
        
        [Fact]
        public void For_WhenBuildThrowAnException_ReturnFailure()
        {
            var validatedObject = RuleBuilder
                .Build<TestObjectWithFields>(() => throw new Exception())
                .For(x => x.FieldB, x => x > 10, "greater than 10")
                .For(x => x.FieldB, x => x < 2, "less than 2")
                .Apply();
                
            ((validatedObject as Failure<TestObjectWithFields>).Error as BuildError).Message.ShouldStartWith("Build of TestObjectWithFields failed with");
        }
        
        [Fact]
        public void For_WhenBuildThrowAnException_AndObjectHasProperties_ReturnFailure()
        {
            var validatedObject = RuleBuilder
                .Build<TestObjectWithProperties>(() => throw new Exception())
                .For(x => x.FieldB, x => x > 10, "greater than 10")
                .For(x => x.FieldB, x => x < 2, "less than 2")
                .Apply();
                
            ((validatedObject as Failure<TestObjectWithProperties>).Error as BuildError).Message.ShouldStartWith("Build of TestObjectWithProperties failed with");
        }
        
        [Fact]
        public void For_WhenForWasDefinedForNotADirectChild_ThrowsException() =>
            Assert.Throws<ArgumentException>(() => RuleBuilder
                .Build<TestObjectWithProperties>(() => throw new Exception())
                .For(x => x.FieldA.Length, x => x > 10, "greater than 10")
                .Apply());
    }
}