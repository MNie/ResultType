namespace ResultType.Validation.Rule
{
    using System;
    using Factories;
    using Results;

    public class Rule : IRule
    {
        private readonly Func<bool> _predicate;
        private readonly string _message;

        public Rule(Func<bool> pred, string msg)
        {
            _predicate = pred;
            _message = msg;
        }

        public static IRule CreateEmpty() => new Rule(() => true, string.Empty);

        public IResult<Unit> Apply() =>
            _predicate()
                ? ResultFactory.CreateSuccess()
                : ResultFactory.CreateFailure(_message);
    }
}
