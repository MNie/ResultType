namespace ResultType.Validation.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Factories;
    using Results;
    using Rule;

    public static class RuleExtensions
    {
        public static Result<Unit> Apply(this IEnumerable<IRule> rules)
        {
            var notApplied = rules
                .Select(x => x.Apply())
                .Where(x => x.IsFailure)
                .ToList();
            return notApplied.Any() 
                ? ResultFactory.CreateFailure(FormatMessages(notApplied)) 
                : ResultFactory.CreateSuccess();
        }

        private static string FormatMessages(IEnumerable<Result<Unit>> results)
            => string.Join(Environment.NewLine, results.Select(x => x.Error.Message));
    }
}
