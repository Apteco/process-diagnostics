using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Apteco.Diagnostics.UI.Utils
{
  public class DelegateCommand : ICommand
  {
    private readonly Func<bool> canExecute;
    private readonly Action execute;

    public event EventHandler CanExecuteChanged;

    public DelegateCommand(Action execute)
      : this(execute, null)
    {
    }

    public DelegateCommand(Action execute,
      Func<bool> canExecute)
    {
      this.execute = execute;
      this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
      if (canExecute == null)
        return true;
      return canExecute();
    }

    public void Execute(object parameter)
    {
      execute();
    }

    public void RaiseCanExecuteChanged()
    {
      CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
  }
}
