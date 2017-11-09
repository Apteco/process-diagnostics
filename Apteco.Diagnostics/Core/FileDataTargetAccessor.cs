using Microsoft.Diagnostics.Runtime;

namespace Apteco.Diagnostics.Core
{
  public class FileDataTargetAccessor: IDataTargetAccessor
  {

    public string  FilePath { get; }

    public FileDataTargetAccessor(string filePath)
    {
      FilePath = filePath;
    }

    public DataTarget Access()
    {
      return DataTarget.LoadCrashDump(FilePath);
    }
  }
}
