using System;

namespace WpfLikeAvaloniaNavigation;

public class NavigationEventArgs : EventArgs
{
    public object? Content { get; }
    public object? Parameter { get; }
    public Type? SourcePageType { get; }

    public NavigationEventArgs(object? content, object? parameter, Type? sourcePageType)
    {
        Content = content;
        Parameter = parameter;
        SourcePageType = sourcePageType;
    }
}

public class NavigatingCancelEventArgs : EventArgs
{
    public bool Cancel { get; set; }
    public Type? SourcePageType { get; }
    public object? Parameter { get; }

    public NavigatingCancelEventArgs(Type? sourcePageType, object? parameter)
    {
        SourcePageType = sourcePageType;
        Parameter = parameter;
    }
}