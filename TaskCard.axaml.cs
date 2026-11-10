using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TaskManagerApp;

public class TaskEventArgs : RoutedEventArgs
{
    public TaskItem Task { get; }

    public TaskEventArgs(RoutedEvent routedEvent, TaskItem task) : base(routedEvent)
    {
        Task = task;
    }
}

public partial class TaskCard : UserControl
{
    public static readonly RoutedEvent<TaskEventArgs> StatusChangedEvent =
        RoutedEvent.Register<TaskCard, TaskEventArgs>(nameof(StatusChanged), RoutingStrategies.Bubble);

    public static readonly RoutedEvent<TaskEventArgs> DeleteRequestedEvent =
        RoutedEvent.Register<TaskCard, TaskEventArgs>(nameof(DeleteRequested), RoutingStrategies.Bubble);

    public event EventHandler<TaskItem>? StatusChanged;
    public event EventHandler<TaskItem>? DeleteRequested;

    public TaskCard()
    {
        InitializeComponent();
    }

    private void OnToggleStatusClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is TaskItem task)
        {
            task.ToggleCompleted();
            StatusChanged?.Invoke(this, task);
            RaiseEvent(new TaskEventArgs(StatusChangedEvent, task));
        }
    }

    private void OnDeleteClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is TaskItem task)
        {
            DeleteRequested?.Invoke(this, task);
            RaiseEvent(new TaskEventArgs(DeleteRequestedEvent, task));
        }
    }
}
