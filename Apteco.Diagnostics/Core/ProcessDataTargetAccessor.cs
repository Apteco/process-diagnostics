using System.Diagnostics;
using Microsoft.Diagnostics.Runtime;

namespace Apteco.Diagnostics.Core
{
  public class ProcessDataTargetAccessor: IDataTargetAccessor
  {

    public Process Process { get; }

    public ProcessDataTargetAccessor(Process process)
    {
      Process = process;
    }

    public DataTarget Access()
    {
      return DataTarget.AttachToProcess(Process.Id, 5000, AttachFlag.Passive);
    }
  }
}
