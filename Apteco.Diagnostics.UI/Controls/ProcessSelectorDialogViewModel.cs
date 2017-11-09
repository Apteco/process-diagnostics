using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Apteco.Diagnostics.UI.Utils;

namespace Apteco.Diagnostics.UI.Controls
{
  public class ProcessSelectorDialogViewModel : ViewModelBase
  {

    public class ProcessViewModel
    {
      
      public Process Process { get; }

      public int ProcessId => Process.Id;

      public string ProcessName => Process.ProcessName;

      public ProcessViewModel(Process process)
      {
        Process = process;
      }

      public override bool Equals(object obj)
      {
        var otherProcess = obj as ProcessViewModel;
        return otherProcess?.ProcessId == ProcessId;
      }

      public override int GetHashCode()
      {
        return ProcessId.GetHashCode();
      }
    }

    #region events

    public event EventHandler<ProcessSelectorDialogResultEventArgs> DialogCompleted;

    #endregion

    #region variables

    private ProcessViewModel selectedProcessViewModel;
    private ObservableCollection<ProcessViewModel> processCollection;
    private readonly BackgroundWorker loadProcessesWorker;

    #endregion

    #region properties

    public ProcessViewModel SelectedProcess
    {
      get { return selectedProcessViewModel; }
      set
      {
        if (selectedProcessViewModel != null && !selectedProcessViewModel.Equals(value))
          return;
        selectedProcessViewModel = value;
        OnPropertyChanged(nameof(SelectedProcess));
        AcceptCommand.RaiseCanExecuteChanged();
      }
    }

    public ObservableCollection<ProcessViewModel> ProcessCollection
    {
      get { return processCollection; }
      set
      {
        if (ProcessCollection == value)
          return;
        processCollection = value;
        OnPropertyChanged(nameof(ProcessCollection));
      }
    }

    public ICommand RefreshCommand { get; }
    public DelegateCommand AcceptCommand { get; }
    public ICommand CancelCommand { get; }

    #endregion

    #region constructor

    public ProcessSelectorDialogViewModel()
    {
      loadProcessesWorker = new BackgroundWorker();
      loadProcessesWorker.DoWork += OnDoWork;
      loadProcessesWorker.RunWorkerCompleted += OnRunWorkerCompleted;
      RefreshCommand = new DelegateCommand(LoadProcesses);
      AcceptCommand = new DelegateCommand(Accept, CanAccept);
      CancelCommand = new DelegateCommand(Cancel);
      LoadProcesses();
    }

    #endregion

    #region private methods
    
    private void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
    {
      ProcessCollection = new ObservableCollection<ProcessViewModel>((IEnumerable<ProcessViewModel>) runWorkerCompletedEventArgs.Result);
    }

    private void OnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
    {
      doWorkEventArgs.Result = Process.GetProcesses().Where(p => p.ProcessName.Contains("CLMClient") || p.ProcessName.Contains("Discoverer")).Select(p => new ProcessViewModel(p));
    }

    private void LoadProcesses()
    {
      loadProcessesWorker.RunWorkerAsync();
    }

    private void Accept()
    {
      OnDialogResult(true, SelectedProcess.Process);
    }

    private bool CanAccept()
    {
      return SelectedProcess != null;
    }

    private void Cancel()
    {
      OnDialogResult(false, null);
    }

    #endregion

    #region event handlers

    private void OnDialogResult(bool? result, Process process)
    {
      DialogCompleted?.Invoke(this, new ProcessSelectorDialogResultEventArgs(result, process));
    }

    #endregion
  }
}
