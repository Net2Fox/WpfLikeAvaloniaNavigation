using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WpfLikeAvaloniaNavigation;

public class NavigationService : INotifyPropertyChanged
{
    private readonly Stack<NavigationEntry> _backStack = new();
    private readonly Stack<NavigationEntry> _forwardStack = new();
    private Frame? _frame;
    private object? _currentContent;

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<NavigatingCancelEventArgs>? Navigating;
    public event EventHandler<NavigationEventArgs>? Navigated;

    public object? CurrentContent => _currentContent;
    public bool CanGoBack => _backStack.Count > 0;
    public bool CanGoForward => _forwardStack.Count > 0;

    internal void SetFrame(Frame frame)
    {
        _frame = frame;
    }

    /// <summary>
    /// Навигация к странице по типу
    /// </summary>
    public bool Navigate(Type pageType, object? parameter = null)
    {
        return NavigateInternal(pageType, parameter, clearForward: true);
    }

    /// <summary>
    /// Навигация к странице (generic версия)
    /// </summary>
    public bool Navigate<TPage>(object? parameter = null) where TPage : class
    {
        return Navigate(typeof(TPage), parameter);
    }

    /// <summary>
    /// Навигация к уже созданному экземпляру
    /// </summary>
    public bool Navigate(object content, object? parameter = null)
    {
        return NavigateToInstance(content, parameter, clearForward: true);
    }

    /// <summary>
    /// Вернуться назад
    /// </summary>
    public bool GoBack()
    {
        if (!CanGoBack) return false;

        var entry = _backStack.Pop();
        
        // Сохраняем текущую страницу в forward stack
        if (_currentContent != null)
        {
            _forwardStack.Push(new NavigationEntry(_currentContent.GetType(), null, _currentContent));
        }

        NavigateToInstance(entry.Instance ?? CreateInstance(entry.PageType), entry.Parameter, clearForward: false);
        
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
        
        return true;
    }

    /// <summary>
    /// Перейти вперёд
    /// </summary>
    public bool GoForward()
    {
        if (!CanGoForward) return false;

        var entry = _forwardStack.Pop();
        
        // Сохраняем текущую страницу в back stack
        if (_currentContent != null)
        {
            _backStack.Push(new NavigationEntry(_currentContent.GetType(), null, _currentContent));
        }

        NavigateToInstance(entry.Instance ?? CreateInstance(entry.PageType), entry.Parameter, clearForward: false);
        
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
        
        return true;
    }

    /// <summary>
    /// Очистить историю навигации
    /// </summary>
    public void ClearHistory()
    {
        _backStack.Clear();
        _forwardStack.Clear();
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));
    }

    private bool NavigateInternal(Type pageType, object? parameter, bool clearForward)
    {
        var content = CreateInstance(pageType);
        return NavigateToInstance(content, parameter, clearForward);
    }

    private bool NavigateToInstance(object content, object? parameter, bool clearForward)
    {
        // Событие Navigating с возможностью отмены
        var navigatingArgs = new NavigatingCancelEventArgs(content.GetType(), parameter);
        Navigating?.Invoke(this, navigatingArgs);
        
        if (navigatingArgs.Cancel) return false;

        // Уведомляем текущую страницу о том, что уходим
        if (_currentContent is INavigationAware currentAware)
        {
            currentAware.OnNavigatedFrom();
        }

        // Сохраняем текущую страницу в back stack
        if (_currentContent != null && clearForward)
        {
            _backStack.Push(new NavigationEntry(_currentContent.GetType(), null, _currentContent));
        }

        // Очищаем forward stack при новой навигации
        if (clearForward)
        {
            _forwardStack.Clear();
        }

        // Устанавливаем NavigationService для страницы
        if (content is Page page)
        {
            page.NavigationService = this;
        }

        _currentContent = content;
        _frame?.SetContent(content);

        // Уведомляем новую страницу
        if (content is INavigationAware newAware)
        {
            newAware.OnNavigatedTo(parameter);
        }

        // Событие Navigated
        Navigated?.Invoke(this, new NavigationEventArgs(content, parameter, content.GetType()));
        
        OnPropertyChanged(nameof(CurrentContent));
        OnPropertyChanged(nameof(CanGoBack));
        OnPropertyChanged(nameof(CanGoForward));

        return true;
    }

    private static object CreateInstance(Type type)
    {
        return Activator.CreateInstance(type) 
               ?? throw new InvalidOperationException($"Cannot create instance of {type.FullName}");
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private record NavigationEntry(Type PageType, object? Parameter, object? Instance = null);
}