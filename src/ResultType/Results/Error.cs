﻿namespace ResultType.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public interface IError
    {
        string Message { get; }
        string MemberName { get; }
        string FilePath { get; }
        int Line { get; }
    }

    public class Error : IError
    {
        public string Message { get; }
        public string MemberName { get; }
        public string FilePath { get; }
        public int Line { get; }

        public Error(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
        {
            Message = msg;
            MemberName = memberName;
            FilePath = filePath;
            Line = line;
        }
    }
    
    public class AggregateError : Error
    {
        public IReadOnlyCollection<IError> Errors { get; }

        public AggregateError(IReadOnlyCollection<IError> err, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int line = 0)
            : base("Aggregate error", memberName, filePath, line)
        {
            Errors = err;
        }
    }
}