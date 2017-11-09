using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apteco.Diagnostics.UI.Utils
{
  public class ProcessSelectorDialogResultEventArgs: EventArgs
  {

    public bool? DialogResult { get; }

    public Process Process { get; }

    public ProcessSelectorDialogResultEventArgs(bool? dialogResult, Process process)
    {
      DialogResult = dialogResult;
      Process = process;
    }

  }
}
