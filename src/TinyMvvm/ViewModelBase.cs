using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TinyMvvm.IoC;
using TinyNavigationHelper;
using TinyPubSubLib;

namespace TinyMvvm
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private INavigationHelper _navigation;

        public ViewModelBase()
        {

        }

        public ViewModelBase(INavigationHelper navigation)
        {
            _navigation = navigation;
        }

        public async virtual Task Initialize()
        {

        }

        public async virtual Task OnAppearing()
        {

        }

        public async virtual Task OnDisappearing()
        {

        }

        public INavigationHelper Navigation
        {
            get
            {
                if (_navigation == null && Resolver.IsEnabled)
                {
                    return Resolver.Resolve<INavigationHelper>();
                }
                else if (_navigation != null)
                {
                    return _navigation;
                }

                throw new NullReferenceException("Please pass a INavigation implementation to the constructor");
            }
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsNotBusy");
            }
        }

        public bool IsNotBusy
        {
            get
            {
                return !IsBusy;
            }
        }

        private Dictionary<string, string> _subscriptions = new Dictionary<string, string>();

        public async Task PublishMessageAsync(string channel, string argument = null)
        {
            await TinyPubSub.PublishAsync(channel, argument);
        }

        public void SubscribeToMessageChannel(string channel, Action action)
        {
            var tag = TinyPubSub.Subscribe(channel, action);
            _subscriptions.Add(channel, tag);
        }

        public void SubscribeToMessageChannel(string channel, Action<string> action)
        {
            var tag = TinyPubSub.Subscribe(channel, action);
            _subscriptions.Add(channel, tag);
        }

        public void UnSubscribeFromMessageChannel(string channel)
        {
            var tag = _subscriptions[channel];
            TinyPubSub.Unsubscribe(tag);

            _subscriptions.Remove(channel);
        }

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
