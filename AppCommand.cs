namespace DialogCloser
{
  using System.Windows.Input;
  using System.Diagnostics;

  internal class AppCommand
  {
    private static RoutedUICommand exit;
    private static RoutedUICommand showDialog;

    static AppCommand()
    {
      InputGestureCollection inputs = null;

      // Initialize the exit command
      inputs = new InputGestureCollection();
      inputs.Add(new KeyGesture(Key.F4, ModifierKeys.Alt, "Alt+F4"));
      AppCommand.exit = new RoutedUICommand("Exit", "Exit", typeof(AppCommand), inputs);

      inputs = new InputGestureCollection();
      inputs.Add(new KeyGesture(Key.D, ModifierKeys.Control, "Ctrl+D"));
      AppCommand.showDialog = new RoutedUICommand("Show Dialog", "ShowDialog", typeof(AppCommand));
    }

    public static RoutedUICommand Exit
    {
      get { return AppCommand.exit; }
    }

    public static RoutedUICommand ShowDialog
    {
      get { return AppCommand.showDialog; }
    }
  }
}
