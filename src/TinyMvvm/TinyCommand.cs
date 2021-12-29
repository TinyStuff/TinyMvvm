namespace TinyMvvm;

/// <summary>
/// An <see cref="ICommand"/> implementation for TinyMvvm.
/// </summary>
public class TinyCommand : ICommand
{
    private readonly Action _action;

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    public TinyCommand(Action action)
    {
        _action = action;
    }

    /// <inheritdoc />
    public bool CanExecute(object parameter)
    {
        return true;
    }

    /// <inheritdoc />
    public void Execute(object parameter)
    {
        _action();
    }
}

/// <summary>
/// An generic <see cref="ICommand"/> implementation for TinyMvvm.
/// </summary>
public class TinyCommand<T> : ICommand
{
    private readonly Action<T> _action;

    public TinyCommand(Action<T> action)
    {
        _action = action;
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    /// <inheritdoc />
    public bool CanExecute(object parameter)
    {
        if (parameter is T)
            return true;

        return false;
    }

    /// <inheritdoc />
    public void Execute(object parameter)
    {
        _action((T)parameter);
    }
}
