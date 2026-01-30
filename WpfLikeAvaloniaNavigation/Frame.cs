using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace WpfLikeAvaloniaNavigation;

public class Frame : ContentControl
{
    public static readonly DirectProperty<Frame, NavigationService> NavigationServiceProperty =
        AvaloniaProperty.RegisterDirect<Frame, NavigationService>(
            nameof(NavigationService),
            o => o.NavigationService);

    private NavigationService _navigationService;

    public NavigationService NavigationService
    {
        get => _navigationService;
        private set => SetAndRaise(NavigationServiceProperty, ref _navigationService, value);
    }

    public Frame()
    {
        _navigationService = new NavigationService();
        _navigationService.SetFrame(this);
        
        // Растягиваем контент
        HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
        VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
    }

    internal void SetContent(object? content)
    {
        Content = content;
    }

    /// <summary>
    /// Навигация к странице по типу
    /// </summary>
    public bool Navigate(Type pageType, object? parameter = null)
    {
        return NavigationService.Navigate(pageType, parameter);
    }
    
    /// <summary>
    /// Навигация к уже созданному экземпляру
    /// </summary>
    public bool Navigate(object content, object? parameter = null)
    {
        return NavigationService.Navigate(content, parameter);
    }

    /// <summary>
    /// Навигация к странице (generic)
    /// </summary>
    public bool Navigate<TPage>(object? parameter = null) where TPage : class
    {
        return NavigationService.Navigate<TPage>(parameter);
    }
}