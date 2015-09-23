using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using ImageComparison.ViewModels;

namespace ImageComparison.Commands
{
    class OpenCompareResultCommand: ICommand
    {
        public OpenCompareResultCommand(ImageComparisonViewModel viewModel)
        {
            _ViewModel = viewModel;
        }

        private ImageComparisonViewModel _ViewModel;

        #region ICommand Members

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _ViewModel.CanOpen;
        }

        public void Execute(object parameter)
        {
            _ViewModel.Open();
        }

        #endregion
    }
}
