using System;
using System.Windows.Input;

namespace CardEditor.ViewModels
{

    public class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        private Func<object, bool> canMoveEffectUp;
        private ICommand moveEffectUp;

        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public RelayCommand(Func<object, bool> canMoveEffectUp, ICommand moveEffectUp)
        {
            this.canMoveEffectUp = canMoveEffectUp;
            this.moveEffectUp = moveEffectUp;
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

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
