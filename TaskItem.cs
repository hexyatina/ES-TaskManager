using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media;

namespace TaskManagerApp;

public class TaskItem : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private bool _isCompleted;

    public string Title
    {
        get => _title;
        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            if (_isCompleted != value)
            {
                _isCompleted = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StatusText));
                OnPropertyChanged(nameof(StatusColor));
                OnPropertyChanged(nameof(StatusButtonText));
                OnPropertyChanged(nameof(StatusActionBrush));
            }
        }
    }

    public string StatusText => IsCompleted ? "Виконано" : "Не виконано";
    public string StatusButtonText => IsCompleted ? "Повернути" : "Виконано";
    public IBrush StatusColor => IsCompleted ? new SolidColorBrush(Color.Parse("#2E7D32")) : new SolidColorBrush(Color.Parse("#E65100"));
    public IBrush StatusActionBrush => IsCompleted ? new SolidColorBrush(Color.Parse("#757575")) : new SolidColorBrush(Color.Parse("#2E7D32"));

    public void ToggleCompleted()
    {
        IsCompleted = !IsCompleted;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
