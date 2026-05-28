// Polyfill for IsExternalInit to support 'init' accessors and 'record' types when targeting net48.
// This type is defined in System.Runtime.dll on .NET 5+ but must be declared explicitly for older targets.
// See https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init

#if NETFRAMEWORK
namespace System.Runtime.CompilerServices
{
	internal class IsExternalInit { }
}
#endif
