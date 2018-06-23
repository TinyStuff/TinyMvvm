using System;
using System.ComponentModel;

namespace TinyMvvmSample.Views
{
    public abstract class ViewBase<T> : TinyMvvm.Forms.ViewBase<T> where T : INotifyPropertyChanged
    {
    }

    public abstract class ViewBase : TinyMvvm.Forms.ViewBase
    {
    }
}
