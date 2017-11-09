using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using Apteco.Diagnostics.UI.Annotations;

namespace Apteco.Diagnostics.UI.Utils
{
  public class ViewModelBase: INotifyPropertyChanged
  {

    #region events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region protected methods

    protected void RunOnUIThread(Action method)
    {
      if (Dispatcher.CurrentDispatcher.CheckAccess())
        method();
      else
        Dispatcher.CurrentDispatcher.BeginInvoke(method);
    }

    

    #endregion

    #region event handlers

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion


  }
}
