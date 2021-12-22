namespace TinyMvvm;

internal class WeakEventManager
{
    private Dictionary<string, List<(WeakReference Reference, MethodInfo Info)>> eventHandlers = new();


    public void AddEventHandler<TEventArgs>(string eventName, EventHandler<TEventArgs> value)
        where TEventArgs : EventArgs
    {
        BuildEventHandler(eventName, value.Target, value.GetMethodInfo());
    }

    public void AddEventHandler(string eventName, EventHandler value)
    {
        BuildEventHandler(eventName, value.Target, value.GetMethodInfo());
    }

    private void BuildEventHandler(string eventName, object handlerTarget, MethodInfo methodInfo)
    {
        if (!eventHandlers.TryGetValue(eventName, out var target))
        {
            target = new List<(WeakReference Reference, MethodInfo Info)>();
            eventHandlers.Add(eventName, target);
        }

        target.Add((new WeakReference(handlerTarget), methodInfo));
    }

    public void HandleEvent(object sender, object args, string eventName)
    {
        var toRaise = new List<(object Object, MethodInfo Info)>();

        if (eventHandlers.TryGetValue(eventName, out var targets))
        {
            foreach (var target in targets.ToList())
            {
                var obj = target.Reference.Target;

                if (obj == null)
                {
                    targets.Remove(target);
                }
                else
                {
                    toRaise.Add((obj, target.Info));
                }
            }
        }

        foreach (var target in toRaise)
        {
            target.Info.Invoke(target.Object, new[] { sender, args });
        }
    }

    public void RemoveEventHandler<TEventArgs>(string eventName, EventHandler<TEventArgs> value)
    where TEventArgs : EventArgs
    {
        RemoveEventHandlerImpl(eventName, value.Target, value.GetMethodInfo());
    }

    public void RemoveEventHandler(string eventName, EventHandler value)
    {
        RemoveEventHandlerImpl(eventName, value.Target, value.GetMethodInfo());
    }

    private void RemoveEventHandlerImpl(string eventName, object handlerTarget, MemberInfo methodInfo)
    {
        if (eventHandlers.TryGetValue(eventName, out var targets))
        {
            var targetsToRemove = targets.Where(t => t.Reference.Target == handlerTarget &&
                t.Info.Name == methodInfo.Name).ToList();

            foreach (var target in targetsToRemove)
            {
                targets.Remove(target);
            }
        }
    }
}

