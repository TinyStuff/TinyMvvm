

namespace TinyMvvm;

/// <summary>
/// An <see cref="ICommand"/> implementation for TinyMvvm.
/// </summary>
public class TinyCommand : ICommand
{
    private readonly Action execute;
    private readonly Func<bool>? canExecute;

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    public TinyCommand(Action action, Func<bool>? canExexute = null)
    {
        execute = action;
    }

    /// <inheritdoc />
    public bool CanExecute(object? parameter)
    {
        if (canExecute != null)
        {
            return canExecute.Invoke();
        }

        return true;

    }

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        execute();
    }

    public bool CanExecute()
    {
        if (canExecute != null)
        {
            return canExecute.Invoke();
        }

        return true;

    }

    public void Execute()
    {
        execute();
    }
}

/// <summary>
/// An generic <see cref="ICommand"/> implementation for TinyMvvm.
/// </summary>
public class TinyCommand<T> : ICommand
{
    private readonly Action<T?> execute;
    private readonly Func<T?, bool>? canExecute;

    public TinyCommand(Action<T?> action, Func<T?, bool>? canExecute = null)
    {
        execute = action;
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    /// <inheritdoc />
    public bool CanExecute(object? parameter)
    {
        if (parameter is T castedParameter)
        {
            if (canExecute != null)
            {
                return this.canExecute.Invoke(castedParameter);
            }

            return true;
        }

        return false;
    }

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        execute((T?)parameter);
    }
}
