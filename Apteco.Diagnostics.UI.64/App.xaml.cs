﻿using System.Runtime.CompilerServices;
using System.Windows;
using Apteco.Diagnostics.UI.Views;

namespace Apteco.Diagnostics.UI64
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      var mainView = new MainView();
      mainView.Show();
    }
  }
}
