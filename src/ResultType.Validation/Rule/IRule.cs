namespace ResultType.Validation.Rule
{
    using Results;

    public interface IRule
    {
        IResult<Unit> Apply();
    }
}
