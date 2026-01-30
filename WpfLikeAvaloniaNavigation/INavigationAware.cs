namespace WpfLikeAvaloniaNavigation;

/// <summary>
/// Интерфейс для страниц, которые хотят получать уведомления о навигации
/// </summary>
public interface INavigationAware
{
    void OnNavigatedTo(object? parameter);
    void OnNavigatedFrom();
}