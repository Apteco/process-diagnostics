using System.Collections.Generic;
using System.IO;
using Apteco.Diagnostics.Model;
using Microsoft.Diagnostics.Runtime;
using Newtonsoft.Json;

namespace Apteco.Diagnostics.Utils
{
  public static class DataTargetExtensions
  {

    public static IEnumerable<DiagnosticThread> LoadThreads(this DataTarget dataTarget)
    {
      var threads = new List<DiagnosticThread>();

      if (dataTarget.ClrVersions.Count == 0)
        return threads;

      var runtime = dataTarget.ClrVersions[0].CreateRuntime();
      foreach (var runtimeThread in runtime.Threads)
      {
        if (!runtimeThread.IsAlive)
          continue;

        if (runtimeThread.StackTrace.Count == 0)
          continue;

        var diagnosticThread = new DiagnosticThread {Id = runtimeThread.ManagedThreadId};

        var currentException = runtimeThread.CurrentException;
        if (currentException != null)
        {
          var diagnosticException = new DiagnosticException
          {
            Address = currentException.Address,
            HResult = currentException.HResult,
            Message = currentException.Message
          };
          diagnosticThread.Exception = diagnosticException;
        }
        var stack = new List<string>();
        foreach (var frame in runtimeThread.StackTrace)
        {
          if (frame.DisplayString.Contains("UNKNOWN") || frame.DisplayString.Contains("Frame"))
            continue;

          stack.Add(frame.DisplayString);
        }
        diagnosticThread.Stack = stack.ToArray();
        threads.Add(diagnosticThread);
      }
      return threads;
    }

  }
}
