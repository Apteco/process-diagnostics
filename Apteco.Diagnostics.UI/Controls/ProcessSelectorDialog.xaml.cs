using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Apteco.Diagnostics.UI.Controls
{
  /// <summary>
  /// Interaction logic for ProcessSelectorDialog.xaml
  /// </summary>
  public partial class ProcessSelectorDialog : Window
  {

    public Process SelectedProcess { get; private set; }

    public ProcessSelectorDialog()
    {
      InitializeComponent();
      DataContext = new ProcessSelectorDialogViewModel();
      ((ProcessSelectorDialogViewModel)DataContext).DialogCompleted += OnDialogCompleted;
    }

    private void OnDialogCompleted(object sender, Utils.ProcessSelectorDialogResultEventArgs e)
    {
      if (Dispatcher.CheckAccess())
      {
        SelectedProcess = e.Process;
        DialogResult = e.DialogResult;
      }
      else
      {
        Dispatcher.BeginInvoke((Action)(() =>
        {
          SelectedProcess = e.Process;
          DialogResult = e.DialogResult;
        }));
      }
    }
  }
}
