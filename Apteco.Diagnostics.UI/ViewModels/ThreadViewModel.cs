using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apteco.Diagnostics.Model;
using Apteco.Diagnostics.UI.Utils;

namespace Apteco.Diagnostics.UI.ViewModels
{
  public class ThreadViewModel : ViewModelBase
  {

    public string Name => Thread.Name;
    public string Stacktrace => Thread.Stacktrace;

    public DiagnosticThread Thread { get; }

    public ThreadViewModel(DiagnosticThread thread)
    {
      Thread = thread;
    }

  }
}
