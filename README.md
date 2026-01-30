# WpfLikeAvaloniaNavigation
Simple WPF-like navigation for Avalonia

## Usage

MainWindow.axaml
```xaml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:nav="clr-namespace:WpfLikeAvaloniaNavigation;assembly=WpfLikeAvaloniaNavigation"
        mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
        x:Class="AvaloniaNavigation.MainWindow"
        Title="AvaloniaNavigation">
    <nav:Frame 
        x:Name="MainFrame"/>
</Window>
```
MainWindow.axaml.cs
```csharp
using Avalonia.Controls;

namespace AvaloniaNavigation;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate<MainPage>();
        // or like WPF
        // MainFrame.Navigate(new MainPage());
    }
}
```
MainPage.axaml
```xaml
<nav:Page xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:nav="clr-namespace:WpfLikeAvaloniaNavigation;assembly=WpfLikeAvaloniaNavigation"
             mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
             x:Class="AvaloniaNavigation.MainPage">
    <StackPanel>
        <TextBlock 
            Text="MainPage"/>
        <Button 
            x:Name="SecondPageButton" 
            Content="Go to SecondPage" 
            Click="SecondPageButton_OnClick"/>
    </StackPanel>
</nav:Page>
```
MainPage.axaml.cs
```csharp
using Avalonia.Interactivity;
using WpfLikeAvaloniaNavigation;

namespace AvaloniaNavigation;

public partial class MainPage : Page
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void SecondPageButton_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(new SecondPage());
    }
}
```
SecondPage.axaml
```xaml
<nav:Page xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:nav="clr-namespace:WpfLikeAvaloniaNavigation;assembly=WpfLikeAvaloniaNavigation"
             mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
             x:Class="AvaloniaNavigation.SecondPage">
    <StackPanel>
        <TextBlock 
            Text="SecondPage"/>
        <Button 
            x:Name="BackButton" 
            Content="Back" 
            Click="BackButton_OnClick"/>
    </StackPanel>
</nav:Page>
```
SecondPage.axaml.cs
```csharp
using Avalonia.Interactivity;
using WpfLikeAvaloniaNavigation;

namespace AvaloniaNavigation;

public partial class SecondPage : Page
{
    public SecondPage()
    {
        InitializeComponent();
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        NavigationService?.GoBack();
    }
}
```