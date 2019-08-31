using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using FibonacciTestGui.Annotations;

namespace FibonacciTestGui
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly FibonacciModel _fibonacciModel;
        private CancellationTokenSource _cancelToken;
        public ViewModel()
        {
            _fibonacciModel = new FibonacciModel();
            _cancelToken = new CancellationTokenSource();
            _cancelToken.Token.Register(UpdateResult);
            _fibonacciModel.PropertyChanged += FibonacciModelOnPropertyChanged;
            PropertyChanged += OnPropertyChanged;
        }
       
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(CurrentFibonacciIndex)))
            {
                if (_cancelToken.IsCancellationRequested)
                {
                    _cancelToken.Dispose();
                    _cancelToken = new CancellationTokenSource();
                    _cancelToken.Token.Register(UpdateResult);
                }
                _cancelToken.CancelAfter(TimeSpan.FromSeconds(1));
            }
        }

        private void UpdateResult()
        {
            _fibonacciModel.QueryFibonacciResult(_currentFibonacciIndex);
        }
        
        private void FibonacciModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals(nameof(CurrentFibonacciResult)))
            {
                CurrentFibonacciResult = _fibonacciModel.CurrentFibonacciResult.ToString();
            }
        }

        private string _currentFibonacciResult = "0";
        private long _currentFibonacciIndex = 0;

        public long CurrentFibonacciIndex
        {
            get => _currentFibonacciIndex;
            set
            {
                if (value == _currentFibonacciIndex) return;
                _currentFibonacciIndex = value;
                OnPropertyChanged();
            }
        }

        public string CurrentFibonacciResult
        {
            get => _currentFibonacciResult;
            set
            {
                if (value == _currentFibonacciResult) return;
                _currentFibonacciResult = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}