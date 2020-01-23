namespace ResultType.Validation.Rule
{
    using Results;

    public class ValidationError : Error
    {
        public ValidationError(string msg, string memberName = "", string filePath = "", int line = 0) : base(msg, memberName, filePath, line)
        {
        }
    }
    
    public class BuildError : Error
    {
        public BuildError(string msg, string memberName = "", string filePath = "", int line = 0) : base(msg, memberName, filePath, line)
        {
        }
    }
}