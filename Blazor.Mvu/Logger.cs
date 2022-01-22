namespace Blazor.Mvu;

using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

public class Logger
{
    readonly LoggerSettings _settings;

    public Logger(LoggerSettings settings) => _settings = settings;

    public void LogMessage<TState>(object msg,
        TState oldState,
        TState newState,
        [CallerMemberName] string memberName = "")
    {
        if (!_settings.DoLogMessages) return;

        Log(memberName, $"msg={msg}, oldState={oldState}, newState={newState}");
    }

    public void LogParameters(ParameterView parameters, [CallerMemberName] string memberName = "")
    {
        if (!_settings.DoLogParameters) return;

        var args = new List<string>();
        var iterator = parameters.GetEnumerator();
        while (iterator.MoveNext()) args.Add($"{iterator.Current.Name}={iterator.Current.Value}");

        Log(memberName, string.Join(", ", args));
    }

    public void LogRender(string msg)
    {
        if (!_settings.DoLogRender) return;

        Log("Render", msg);
    }

    public void Warning(string msg) => Log(msg);
    public Type Type { get; set; }
    void Log(string memberName, string msg) => Log($"{Name}.{memberName}: {msg}");

    void Log(string msg) => Console.WriteLine($"{Name}: {msg}");

    string Name => Type.ProperName();
}

public static class TypeExtensions
{
    public static string ProperName(this Type self)
    {
        var typeName = self.Name;
        var index = typeName.LastIndexOf('`');
        if (index >= 0) typeName = typeName.Substring(0, index);
        var typeArgs = self.IsGenericType ? $"<{toList(self.GenericTypeArguments)}>" : string.Empty;
        return $"{typeName}{typeArgs}";

        string toList(Type[] types) => string.Join(", ", types.Select(t => t.ProperName()));
    }
}