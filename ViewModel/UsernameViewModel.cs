namespace DialogCloser.ViewModel
{
  using DialogCloser.ViewModel.Base;

  /// <summary>
  /// Viewmodel class that manages input states for user who has a first and a last name.
  /// </summary>
  public class UsernameViewModel : ViewModelBase
  {
    #region fields
    private DialogViewModelBase mOpenCloseView;
    #endregion fields

    #region constructor
    /// <summary>
    /// Standard Constructor
    /// </summary>
    public UsernameViewModel()
      : base()
    {
    }

    /// <summary>
    /// Copy constructor
    /// </summary>
    public UsernameViewModel(UsernameViewModel copyThis)
      : this()
    {
      if (copyThis == null) return;

      this.mOpenCloseView = new DialogViewModelBase(copyThis.mOpenCloseView);
    }

    #endregion fields

    #region properties
    /// <summary>
    /// Get property to expose elements necessary to evaluate user input
    /// when the user completes his input (eg.: clicks OK in a dialog).
    /// </summary>
    public DialogViewModelBase OpenCloseView
    {
      get
      {
        return this.mOpenCloseView;
      }
      
      private set
      {
        if (this.mOpenCloseView != value)
        {
          this.mOpenCloseView = value;

          this.NotifyPropertyChanged(() => this.OpenCloseView);
        }
      }
    }
    #endregion properties

    #region methods
    /// <summary>
    /// Initilize input states such that user can input information
    /// with a view based GUI (eg.: dialog)
    /// </summary>
    public void InitDialogInputData()
    {
      this.OpenCloseView = new Base.DialogViewModelBase();

      // Attach delegate method to validate user input on OK
      // Not setting this means that user input is never validated and view will always close on OK
      //if (this.mOpenCloseView != null)
      //  this.mOpenCloseView.EvaluateInputData = this.ValidateData;
    }

    /// <summary>
    /// Delegate method that is call whenever a user OK'es or Cancels the view that is bound to <seealso cref="OpenCloseView"/>
    /// </summary>
    /// <param name="listMsgs"></param>
    /// <returns></returns>
    //private bool ValidateData(out List<Msg> listMsgs)
    //{
    //  listMsgs = new List<Msg>();

    //  const int MinStrLen = 5;

    //  if (this.FirstName.Trim().Length < MinStrLen)
    //    listMsgs.Add(new Msg(string.Format("First name must contain at least {0} letters.", MinStrLen), Msg.MsgCategory.Error));

    //  if (this.LastName.Trim().Length < MinStrLen)
    //    listMsgs.Add(new Msg(string.Format("Last name must contain at least {0} letters.", MinStrLen), Msg.MsgCategory.Warning));

    //  return !(listMsgs.Count > 0);
    //}
    #endregion methods
  }
}
