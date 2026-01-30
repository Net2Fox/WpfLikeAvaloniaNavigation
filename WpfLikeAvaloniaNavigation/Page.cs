using Avalonia.Controls;

namespace WpfLikeAvaloniaNavigation;

public class Page : UserControl, INavigationAware
{
    public NavigationService? NavigationService { get; internal set; }

    /// <summary>
    /// Вызывается при переходе на эту страницу
    /// </summary>
    public virtual void OnNavigatedTo(object? parameter)
    {
    }

    /// <summary>
    /// Вызывается при уходе с этой страницы
    /// </summary>
    public virtual void OnNavigatedFrom()
    {
    }
}