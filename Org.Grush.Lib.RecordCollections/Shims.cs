namespace Org.Grush.Lib.RecordCollections;

#if NETSTANDARD2_0

/// <summary>
/// Shim for
/// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.codeanalysis.notnullwhenattribute?view=netstandard-2.1">NotNullWhenAttribute</see>
/// from the stdlib. Not worth bringing in the packaged version.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
internal sealed class NotNullWhenAttribute(bool returnValue) : Attribute
{
  public bool ReturnValue => returnValue;
}

#endif
