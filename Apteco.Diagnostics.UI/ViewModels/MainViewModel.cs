using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Apteco.Diagnostics.Core;
using Apteco.Diagnostics.Model;
using Apteco.Diagnostics.UI.Controls;
using Apteco.Diagnostics.UI.Utils;
using Microsoft.Diagnostics.Runtime;
using Microsoft.Win32;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Apteco.Diagnostics.UI.ViewModels
{
  public class MainViewModel: ViewModelBase
  {

    #region variables

    private ObservableCollection<ThreadViewModel> threadCollection;
    private ThreadViewModel selectedViewModel;
    private readonly BackgroundWorker loadThreadsWorker;
    private readonly BackgroundWorker exportStacktraceWorker;

    #endregion

    #region properties

    public ICommand LoadProccessesCommand { get; }
    public ICommand LoadFileCommand { get; }

    public DelegateCommand ExportStacktraceCommand { get; }

    public ObservableCollection<ThreadViewModel> ThreadCollection => threadCollection ?? (threadCollection = new ObservableCollection<ThreadViewModel>());

    public ThreadViewModel SelectedViewModel
    {
      get { return selectedViewModel; }
      set
      {
        if (selectedViewModel == value)
          return;
        selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));
        OnPropertyChanged(nameof(Stacktrace));
      }
    }

    public string Stacktrace => SelectedViewModel == null ? string.Empty : SelectedViewModel.Stacktrace;

    #endregion

    #region constructors

    public MainViewModel()
    {
      threadCollection = new ObservableCollection<ThreadViewModel>();
      LoadProccessesCommand = new DelegateCommand(LoadProcessesImpl);
      LoadFileCommand = new DelegateCommand(LoadFileImpl);
      ExportStacktraceCommand = new DelegateCommand(ExportStacktrace, CanExportStacktrace);

      loadThreadsWorker = new BackgroundWorker();
      loadThreadsWorker.DoWork += OnDoWork;
      loadThreadsWorker.RunWorkerCompleted += OnWorkCompleted;

      exportStacktraceWorker = new BackgroundWorker();
      exportStacktraceWorker.DoWork += ExportStacktraceWorkerOnDoWork;
    }
    
    #endregion

    #region private methods

    private void OnWorkCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
    {
      ThreadCollection.Clear();
      if (!(runWorkerCompletedEventArgs.Result is IEnumerable<DiagnosticThread> threadEnumerable))
        return;

      foreach (var thread in threadEnumerable)
      {
        ThreadCollection.Add(new ThreadViewModel(thread));
      }
      OnPropertyChanged(nameof(ThreadCollection));
      ExportStacktraceCommand.RaiseCanExecuteChanged();
    }

    private void OnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
    {
      try
      {
        doWorkEventArgs.Result = AppController.GetThreads();
      }
      catch (ClrDiagnosticsException e)
      {
        Console.WriteLine(e);
      }
    }

    private void LoadProcessesImpl()
    {
      var selectProcessDialog = new ProcessSelectorDialog();
      var success = selectProcessDialog.ShowDialog();
      if (!success.HasValue || !success.Value)
        return;

      AppController.Accessor = new ProcessDataTargetAccessor(selectProcessDialog.SelectedProcess);
      loadThreadsWorker.RunWorkerAsync();
    }

    private void LoadFileImpl()
    {
      var openFileDialog = new OpenFileDialog
      {
        Title = "Open Dump file",
        Filter = "DMP files|*.DMP",
        Multiselect = false
      };
      var success = openFileDialog.ShowDialog();
      if (!success.HasValue || !success.Value)
        return;

      AppController.Accessor = new FileDataTargetAccessor(openFileDialog.FileName);
      loadThreadsWorker.RunWorkerAsync();
    }

    private void ExportStacktrace()
    {
      if (AppController.Accessor == null)
        return;

      var saveFileDialog = new SaveFileDialog
      {
        Filter = "JSON files|*.json",
        AddExtension = true,
        DefaultExt = "json"
      };
      var success = saveFileDialog.ShowDialog();
      if (!success.HasValue || !success.Value)
        return;

      exportStacktraceWorker.RunWorkerAsync(saveFileDialog.FileName);
    }

    private bool CanExportStacktrace()
    {
      return AppController.Accessor != null;
    }

    private void ExportStacktraceWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
    {
      if (!(doWorkEventArgs.Argument is string fileName))
        return;

      ThreadCollection.Select(vm => vm.Thread).SaveThreads(fileName);
    }

    #endregion

  }
}
