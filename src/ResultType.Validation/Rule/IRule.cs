namespace ResultType.Validation.Rule
{
    using Results;

    public interface IRule : IRule<Unit>
    {
    }

    public interface IRule<TValidate>
    {
        IResult<TValidate> Apply();
    }
}
