namespace ResultType.Validation.Rule
{
    using ResultType.Results;

    public interface IRule
    {
        Result<Unit> Apply();
    }
}
