using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CardEditor.ViewModels
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Func<object, Task> _execute;

        public AsyncRelayCommand(Predicate<object> canExecute, Func<object, Task> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await _execute(parameter);
        }
    }
}
