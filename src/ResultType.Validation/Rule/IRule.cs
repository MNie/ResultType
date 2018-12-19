namespace ResultType.Validation.Rule
{
    using Results;

    public interface IRule
    {
        Result<Unit> Apply();
    }
}
