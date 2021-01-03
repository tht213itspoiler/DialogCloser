namespace DialogCloser
{
  using System;
  using System.Windows;
  using System.Windows.Input;

  using View;
  using ViewModel;
  using DialogCloser.Service;
  using DialogCloser.Service.WindowViewModelMapping;

  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private bool mRequestClose = false;
    private MainWindow win = null;

    /// <summary>
    /// Initialize the RoutedCommand binding between MainWindow View and ViewModel
    /// </summary>
    /// <param name="win"></param>
    public void InitMainWindowCommandBinding(Window win)
    {
      win.CommandBindings.Add(new CommandBinding(AppCommand.Exit,
      (s, e) =>
      {
        e.Handled = true;

        ((AppViewModel)win.DataContext).ExitExecuted();
      }));

      win.CommandBindings.Add(new CommandBinding(AppCommand.ShowDialog,
      (s, e) =>
      {
        ((AppViewModel)win.DataContext).ShowUserNameDialog();

        e.Handled = true;
      }));
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
      // Configure service locator
      ServiceLocator.RegisterSingleton<IDialogService, DialogService>();
      ServiceLocator.RegisterSingleton<IWindowViewModelMappings, WindowViewModelMappings>();

      AppViewModel tVM = new AppViewModel();  // Construct ViewModel and MainWindow
      this.win = new MainWindow();

      tVM.OnSessionEnding = this.OnSessionEnding;

      this.win.Closing += this.OnClosing;

      // When the ViewModel asks to be closed, it closes the window via attached behaviour.
      // We use this event to shut down the remaining parts of the application
      tVM.RequestClose += delegate
      {
        // Make sure close down event is processed only once
        if (this.mRequestClose == false)
        {
          this.mRequestClose = true;

          // Save session data and close application
          this.OnClosed(this.win.DataContext as ViewModel.AppViewModel, this.win);
        }
      };

      this.win.DataContext = tVM; // Attach ViewModel to DataContext of ViewModel
      this.InitMainWindowCommandBinding(this.win);  // Initialize RoutedCommand bindings
      this.win.Show(); // SHOW ME DA WINDOW!
    }

    /// <summary>
    /// This executes when Windows is 'forced' to shut down the application.
    /// We can either tell Windows to wait (cancel App shutdown) or get out of its way.
    /// 
    /// See http://msdn.microsoft.com/en-us/library/system.windows.application.sessionending.aspx for details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
    {
      e.Cancel = this.OnSessionEnding();
    }

    /// <summary>
    /// Determine whether Application MainWindow is ready to close down or
    /// whether close down should be cancelled - and cancel it.
    /// 
    /// This event is typically raised when the user clicks the window close button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
            ((AppViewModel)win.DataContext).ShowUserNameDialog();
            e.Cancel = this.OnSessionEnding();
    }

    /// <summary>
    /// Evaluate whether application is ready to shutdown and print a corresponding message if not.
    /// </summary>
    /// <returns></returns>
    private bool OnSessionEnding()
    {
      ViewModel.AppViewModel tVM = this.MainWindow.DataContext as ViewModel.AppViewModel;

      if (tVM != null)
      {
        if (tVM.IsReadyToClose == false)
        {
                    //MessageBox.Show("Application is not ready to exit.\n" +
                    //                "Hint: Check the checkbox in the MainWindow before exiting the application.",
                    //                "Cannot exit application", MessageBoxButton.YesNoCancel);

                    return !tVM.IsReadyToClose; // Cancel close down request if ViewModel is not ready, yet
        }

        //tVM.OnRequestClose(false);

        return !tVM.IsReadyToClose; // Cancel close down request if ViewModel is not ready, yet
      }

      return true;
    }

    /// <summary>
    /// Execute closing function and persist session data here.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnClosed(ViewModel.AppViewModel appVM, Window win)
    {
      try
      {
        Console.WriteLine("Closing down apllication.");
      }
      catch (System.Exception exp)
      {
        MessageBox.Show(exp.ToString(), "Error in OnClosed method", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }
}
