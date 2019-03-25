# ResultType & ResultType.Validation

* NuGet Status

    |   | ResultType | ResultType.Validation |
    |---|---|---|
    | nuget | [![NuGet](https://buildstats.info/nuget/MNie.ResultType?includePreReleases=true)](https://www.nuget.org/packages/MNie.ResultType) | [![NuGet](https://buildstats.info/nuget/MNie.ResultType.Validation?includePreReleases=true)](https://www.nuget.org/packages/MNie.ResultType.Validation) |


* Build Status
    [![Build Status](https://travis-ci.org/MNie/ResultType.svg?branch=master)](https://travis-ci.org/MNie/ResultType)

# ResultType
ResultType implementation in C#

Could be downloaded from NuGet:
```Install-Package MNie.ResultType```

Simple usage:

* without value
```csharp
var result = ResultFactory.CreateSuccess();

var error = ResultFactory.CreateFailure<Unit>();
```

* with value
```csharp
var result = ResultFactory.CreateSuccess(true);

var error = ResultFactory.CreateFailure<bool>("error message");
```

* chaining results
```csharp
var result = ResultFactory.CreateSuccess()
  .Bind(fromPreviousSuccess => ResultFactory.CreateSuccess(true));
```

```csharp
var result = ResultFactory.CreateSuccess()
  .Bind(() => ResultFactory.CreateSuccess(true));
```

```csharp
var result = ResultFactory.CreateSuccess()
  .Bind((fromPreviousSuccess, fromPreviousFailure) => ResultFactory.CreateSuccess(true));
```

* mapping result

```csharp
[HttpGet]
public IActionResult SomeAction() =>
    "test".ToSuccess().Map(Ok, BadRequest);
```

`Bind` function accepts `Func<TPrevious, TOutput>` or `Func<TOuput>` it depends on you if you want to based on the previous value. There is also an async implementation of `Bind` with `Async` postfix.
There are also async functions which in fact are boxing result into a Task.
```csharp
ResultFactory.CreateSuccessAsync
ResultFactory.CreateFailureAsync
```

# ResultType.Validation

[ResultType.Validation](https://www.nuget.org/packages/MNie.ResultType.Validation/) package provides a simple Rule class to defines Rules which should apply to some objects and as a result returns ResultType.
Could be downloaded from NuGet:
```Install-Package MNie.ResultType.Validation```

example usage looks like this:
```csharp
var rules = new[]
{
    new Rule(() => "name".StartsWith("n"), "name"),
    new Rule(() => "dd".StartsWith("e"), "dd"),
    new Rule(() => "hh".StartsWith("a"), "hh"),
    new Rule(() => "hehe".StartsWith("h"), "hehe"),
};
var result = rules.Apply();
```
