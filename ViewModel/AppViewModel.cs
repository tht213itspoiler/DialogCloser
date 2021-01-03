namespace DialogCloser.ViewModel
{
  using System;
  using System.Diagnostics.Contracts;
  using System.Windows;

  using DialogCloser.Service;
  using DialogCloser.View;

  public class AppViewModel : DialogCloser.ViewModel.Base.ViewModelBase
  {
    #region fields
    private readonly IDialogService dialogService;

    private bool mShutDownInProgress;
    private bool? mDialogCloseResult;

    private bool mIsReadyToClose;

    private UsernameViewModel mTestDialogViewModel;
    #endregion fields

    #region constructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public AppViewModel()
     : this(ServiceLocator.Resolve<IDialogService>())
    {
      this.InitObject();
    }

		/// <summary>
    /// Initializes a new instance of the <see cref="AppViewModel"/> class from a service reference.
		/// </summary>
		/// <param name="dialogService">The dialog service.</param>
    public AppViewModel(IDialogService dialogService)
		{
			Contract.Requires(dialogService != null);

			this.dialogService = dialogService;
    
      this.InitObject();
      this.DisplayName = "Demo Application for Start-up/Shutdown and Dialog usage in WPF";
    }
    #endregion constructor

    #region event
    /// <summary>
    /// Raised when this workspace is shutting down.
    /// </summary>
    public event EventHandler RequestClose;
    #endregion event

    #region properties
    /// <summary>
    /// This can be used to close the attached view via ViewModel
    /// 
    /// Source: http://stackoverflow.com/questions/501886/wpf-mvvm-newbie-how-should-the-viewmodel-close-the-form
    /// </summary>
    public bool? WindowCloseResult
    {
      get
      {
        return this.mDialogCloseResult;
      }

      set
      {
        if (this.mDialogCloseResult != value)
        {
          this.mDialogCloseResult = value;
          this.NotifyPropertyChanged(() => this.WindowCloseResult);
        }
      }
    }

    /// <summary>
    /// Get/set property to determine whether application is ready to close
    /// (the setter is public here to bind it to a checkbox - in a normal
    /// application that setter would more likely be private and be set via
    /// a corresponding method call that manages/overrides the properties' value).
    /// </summary>
    public bool IsReadyToClose
    {
      get
      {
        return this.mIsReadyToClose;
      }

      set
      {
        if (this.mIsReadyToClose != value)
        {
          this.mIsReadyToClose = value;
          this.NotifyPropertyChanged(() => this.IsReadyToClose);
        }
      }
    }

    /// <summary>
    /// Get/set delegate method to evaluate whether application can shut down or not.
    /// </summary>
    public Func<bool> OnSessionEnding { get; set; }
    #endregion properties

    #region methods
    /// <summary>
    /// Method to be executed when user closes the application.
    /// This method calls the <seealso cref="OnSessionEnding"/> delegate methode (if set)
    /// to evaluate whether session can actually end or not.
    /// 
    /// The method fires the  <seealso cref="RequestClose"/> event to signal outside objects
    /// that close application should occur now.
    /// </summary>
    public void ExitExecuted()
    {
            //ShowUserNameDialog();
      try
      {
        if (this.mShutDownInProgress == false)
        {
          this.mShutDownInProgress = true;

          if (this.OnSessionEnding != null)
          {
            if (this.OnSessionEnding() == true)
            {
              this.mShutDownInProgress = false;
              return;
            }
          }

          this.WindowCloseResult = true;              // Close the MainWindow and tell outside world
                                                     // that we are closed down.
          EventHandler handler = this.RequestClose;

          if (handler != null)
            handler(this, EventArgs.Empty);
        }
      }
      catch (Exception exp)
      {
        System.Console.WriteLine("Exception occurred in OnRequestClose\n{0}", exp.ToString());
        this.mShutDownInProgress = false;
      }
    }

    /// <summary>
    /// Initialize ibject states upon construction
    /// </summary>
    private void InitObject()
    {
      this.OnSessionEnding = null;
      this.mDialogCloseResult = null;
      this.mIsReadyToClose = false;
      this.mShutDownInProgress = false;
      this.mTestDialogViewModel = new UsernameViewModel();
    }

    /// <summary>
    /// Show a dialog that is closed via corresponding viewmodel and attached behaviour.
    /// </summary>
    public void ShowUserNameDialog()
    {
      UsernameViewModel dlgVM = null;

      try
      {
        dlgVM = new UsernameViewModel(this.mTestDialogViewModel);

        // It is important to either:
        // 1> Use the InitDialogInputData methode here or
        // 2> Reset the WindowCloseResult=null property
        // because otherwise ShowDialog will not work twice
        // (Symptom: The dialog is closed immeditialy by the attached behaviour)
        dlgVM.InitDialogInputData();

        // Showing the dialog, alternative 1.
        // Showing a specified dialog. This doesn't require any form of mapping using 
        // IWindowViewModelMappings.
        dialogService.ShowDialog(this, dlgVM);

        // Copy input if user OK'ed it. This could also be done by a method, equality operator, or copy constructor
        if (dlgVM.OpenCloseView.WindowCloseResult == true)
        {
          Console.WriteLine("Dialog was OK'ed.");
                    //this.mTestDialogViewModel.FirstName = dlgVM.FirstName;
                    //this.mTestDialogViewModel.LastName = dlgVM.LastName;

                    IsReadyToClose = true;
        }
        else
                {
                    IsReadyToClose = false;
                    Console.WriteLine("Dialog was Cancel'ed.");

                }

                // Showing the dialog, alternative 2.
                // Showing a dialog without specifying the type. This require some form of mapping using 
                // IWindowViewModelMappings.
                //dialogService.ShowDialog(this, personDialogViewModel);
            }
      catch (Exception exc)
      {
        MessageBox.Show(exc.ToString());
      }
    }
    #endregion methods
  }
}
