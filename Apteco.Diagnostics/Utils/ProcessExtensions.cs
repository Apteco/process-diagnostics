using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Apteco.Diagnostics.Utils
{
  public static class ProcessExtensions
  {

    public static bool IsWin64(this Process process)
    {
      if (Environment.OSVersion.Version.Major > 5 || Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
      {
        IntPtr processHandle;
        bool retVal;
        try
        {
          processHandle = Process.GetProcessById(process.Id).Handle;
        }
        catch
        {
          return false; // access is denied to the process
        }

        return IsWow64Process(processHandle, out retVal) && retVal;
      }

      return false; // not on 64-bit Windows
    }

    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

  }
}
