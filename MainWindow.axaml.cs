using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Input;

namespace TaskManagerApp;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<TaskItem> _tasks = new();

    public MainWindow()
    {
        InitializeComponent();

        TasksListBox.ItemsSource = _tasks;
        _tasks.CollectionChanged += OnTasksCollectionChanged;
        
        UpdateActiveCounter();
    }

    private void OnAddTaskClicked(object? sender, RoutedEventArgs e)
    {
        AddTask();
    }

    private void OnTaskTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddTask();
        }
    }

    private void AddTask()
    {
        string text = NewTaskTextBox.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        var item = new TaskItem
        {
            Title = text,
            IsCompleted = false
        };

        _tasks.Add(item);
        NewTaskTextBox.Text = string.Empty;
    }

    private void OnDeleteTaskClicked(object? sender, RoutedEventArgs e)
    {
        if (TasksListBox.SelectedItem is TaskItem selectedTask)
        {
            _tasks.Remove(selectedTask);
        }
    }

    private void OnTasksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (TaskItem item in e.NewItems)
            {
                item.PropertyChanged += OnTaskItemPropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (TaskItem item in e.OldItems)
            {
                item.PropertyChanged -= OnTaskItemPropertyChanged;
            }
        }

        UpdateActiveCounter();
    }

    private void OnTaskItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TaskItem.IsCompleted))
        {
            UpdateActiveCounter();
        }
    }

    private void UpdateActiveCounter()
    {
        int count = _tasks.Count(t => !t.IsCompleted);
        ActiveTasksCounterTextBlock.Text = $"Невиконаних завдань: {count}";
    }
}