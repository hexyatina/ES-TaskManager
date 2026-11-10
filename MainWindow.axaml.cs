using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;

namespace TaskManagerApp;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<TaskItem> _tasks = new();

    public MainWindow()
    {
        InitializeComponent();

        TasksListBox.ItemsSource = _tasks;
        _tasks.CollectionChanged += OnTasksCollectionChanged;

        // Підписка на події від компонентів TaskCard (через RoutedEvents)
        AddHandler(TaskCard.DeleteRequestedEvent, OnTaskCardDeleteRequested);
        AddHandler(TaskCard.StatusChangedEvent, OnTaskCardStatusChanged);

        // Початкові демонстраційні завдання відповідно до завдання
        _tasks.Add(new TaskItem { Title = "Прочитати розділ", IsCompleted = false });
        _tasks.Add(new TaskItem { Title = "Здати лабораторну", IsCompleted = false });

        UpdateActiveCounter();

        Loaded += OnWindowLoaded;
    }

    private async void OnWindowLoaded(object? sender, RoutedEventArgs e)
    {
        await Task.Delay(400);

        string[] args = Environment.GetCommandLineArgs();
        if (args.Contains("--demo-screenshots"))
        {
            // Крок 1: Початковий стан з двома завданнями
            CaptureScreenshot("screenshot.png");
            CaptureScreenshot("screenshot_step1_initial.png");

            await Task.Delay(300);

            // Крок 2: Зміна статусу першого завдання
            if (_tasks.Count > 0)
            {
                _tasks[0].ToggleCompleted();
            }
            await Task.Delay(300);
            CaptureScreenshot("screenshot_step2_completed.png");

            // Крок 3: Видалення другого завдання
            if (_tasks.Count > 1)
            {
                _tasks.RemoveAt(1);
            }
            await Task.Delay(300);
            CaptureScreenshot("screenshot_step3_deleted.png");

            Close();
        }
        else
        {
            // Зберегти скриншот початкового інтерфейсу для звіту
            CaptureScreenshot("screenshot.png");
        }
    }

    public void CaptureScreenshot(string filename)
    {
        try
        {
            int width = (int)Math.Max(Bounds.Width, 580);
            int height = (int)Math.Max(Bounds.Height, 620);
            using var rtb = new RenderTargetBitmap(new PixelSize(width, height), new Vector(96, 96));
            rtb.Render(this);

            string repoPath = Path.Combine("/home/artem/ES-TaskManager", filename);
            rtb.Save(repoPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Screenshot error: {ex.Message}");
        }
    }

    private void OnTaskCardDeleteRequested(object? sender, TaskEventArgs e)
    {
        _tasks.Remove(e.Task);
        ErrorMessageTextBlock.Text = string.Empty;
    }

    private void OnTaskCardStatusChanged(object? sender, TaskEventArgs e)
    {
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

        // Перевірка порожнього введення
        if (string.IsNullOrWhiteSpace(text))
        {
            ErrorMessageTextBlock.Text = "Помилка: назва завдання не може бути порожньою!";
            return;
        }

        ErrorMessageTextBlock.Text = string.Empty;

        var item = new TaskItem
        {
            Title = text,
            IsCompleted = false
        };

        _tasks.Add(item);
        NewTaskTextBox.Text = string.Empty;
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
        int total = _tasks.Count;
        int active = _tasks.Count(t => !t.IsCompleted);
        int completed = total - active;
        ActiveTasksCounterTextBlock.Text = $"Усього: {total} | Невиконаних: {active} | Виконаних: {completed}";
    }
}

