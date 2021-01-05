namespace ResultType.Validation.Rule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Extensions;
    using Factories;
    using Results;
    using ResultType.Operations;

    public static class RuleBuilder
    {
        public static RuleBuilder<TType> Build<TType>(Func<TType> func)
        {
            try
            {
                return new RuleBuilder<TType>(func().ToSuccess());
            }
            catch (Exception e)
            {
                return new RuleBuilder<TType>(ResultFactory.CreateFailure<TType>(new BuildError($"Build of {typeof(TType).Name} failed with {e.Message}")));
            }
        }
    }
    
    public class RuleBuilder<TType>
    {
        private class Predicate
        {
            public readonly Func<bool> ToCheck;
            public readonly MemberInfo For;
            public readonly string CustomMessage = "Validation is incorrect";

            public Predicate(MemberInfo @for, Func<bool> toCheck)
            {
                For = @for;
                ToCheck = toCheck;
            }

            public Predicate(MemberInfo @for, Func<bool> toCheck, string customMessage)
                : this(@for, toCheck) =>
                CustomMessage = customMessage;
        }
        
        private readonly IResult<TType> _underInvestigation;
        private readonly IList<Predicate> _predicates = new List<Predicate>();

        internal RuleBuilder(IResult<TType> underInvestigation) => _underInvestigation = underInvestigation;

        private static MemberInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;

            var done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = ((LambdaExpression)expressionToCheck).Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)expressionToCheck;

                        if (memberExpression.Expression?.NodeType != ExpressionType.Parameter &&
                            memberExpression.Expression?.NodeType != ExpressionType.Convert)
                        {
                            throw new ArgumentException(
                                $"Expression '{lambdaExpression}' must resolve to top-level member and not any child object's fields/properties.",
                                nameof(lambdaExpression));
                        }

                        var member = memberExpression.Member;

                        return member;
                    default:
                        done = true;
                        break;
                }
            }
            throw new Exception("Invalid state.");
        }
        
        public RuleBuilder<TType> For<TMember>(Expression<Func<TType, TMember>> func, Predicate<TMember> predicate)
        {
            var memberInfo = FindProperty(func);
            _predicates.Add(
                new Predicate(
                    memberInfo, 
                () => 
                    _underInvestigation switch
                    {
                        Success<TType> s => predicate(GetValue<TMember>(s.Payload, memberInfo)),
                        _ => true
                    }
                )
            );
            return this;
        }
        
        public RuleBuilder<TType> For<TMember>(Expression<Func<TType, TMember>> func, Predicate<TMember> predicate, string customMessage)
        {
            var memberInfo = FindProperty(func);
            _predicates.Add(
                new Predicate(
                    memberInfo, 
                    () => 
                    _underInvestigation switch
                    {
                        Success<TType> s => predicate(GetValue<TMember>(s.Payload, memberInfo)),
                        _ => true
                    }, 
                    customMessage)
            );
            return this;
        }

        private static TMember GetValue<TMember>(TType investigated, MemberInfo info)
        {
            return info switch
            {
                FieldInfo fi => (TMember) fi.GetValue(investigated)!,
                PropertyInfo pi => (TMember) pi.GetValue(investigated)!,
                _ => throw new Exception("not supported type")
            };
        }

        public IResult<TType> Apply() =>
            _underInvestigation
                .Bind(investigated =>
                {
                    var result = _predicates
                        .Select(y => new {fieldName = y.For.Name, value = y.ToCheck(), msg = y.CustomMessage})
                        .Where(y => !y.value)
                        .ToList();
                    return result.Any()
                        ? ResultFactory.CreateFailure<TType>(new ValidationError(string.Join($"{Environment.NewLine}and ",
                            result.Select(y => $"{y.fieldName} should be {y.msg}."))))
                        : investigated.ToSuccess();
                });
    }
}