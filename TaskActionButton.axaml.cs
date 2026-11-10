using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace TaskManagerApp;

public partial class TaskActionButton : UserControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<TaskActionButton, string>(nameof(Text), "Дія");

    public static readonly StyledProperty<IBrush?> ButtonBackgroundProperty =
        AvaloniaProperty.Register<TaskActionButton, IBrush?>(nameof(ButtonBackground), Brushes.Gray);

    public static readonly StyledProperty<IBrush?> ButtonForegroundProperty =
        AvaloniaProperty.Register<TaskActionButton, IBrush?>(nameof(ButtonForeground), Brushes.White);

    public string Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public IBrush? ButtonBackground
    {
        get => GetValue(ButtonBackgroundProperty);
        set => SetValue(ButtonBackgroundProperty, value);
    }

    public IBrush? ButtonForeground
    {
        get => GetValue(ButtonForegroundProperty);
        set => SetValue(ButtonForegroundProperty, value);
    }

    public event EventHandler<RoutedEventArgs>? ActionClicked;

    public TaskActionButton()
    {
        InitializeComponent();
    }

    private void OnButtonClicked(object? sender, RoutedEventArgs e)
    {
        ActionClicked?.Invoke(this, e);
    }
}
