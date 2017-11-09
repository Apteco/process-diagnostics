using System;
using System.Collections.Generic;
using Apteco.Diagnostics.Core;
using Apteco.Diagnostics.Model;
using Apteco.Diagnostics.Utils;

namespace Apteco.Diagnostics.UI
{
  public static class AppController
  {

    #region properties

    public static IDataTargetAccessor Accessor { get; set; }

    #endregion

    #region constructors
    
    #endregion

    #region public methods

    public static IEnumerable<DiagnosticThread> GetThreads()
    {
      if (Accessor == null)
        throw new Exception("DataTargetAccessor has not yet been initialised");

      using (var dataTarget = Accessor.Access())
      {
        return dataTarget.LoadThreads();
      }
    }

    #endregion

  }
}
