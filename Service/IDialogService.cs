﻿namespace DialogCloser.Service
{
  using System.Collections.ObjectModel;
  using System.Diagnostics.Contracts;
  using System.Windows;

  /// <summary>
	/// Interface responsible for abstracting ViewModels from Views.
	/// </summary>
	public interface IDialogService
	{
		/// <summary>
		/// Gets the registered views.
		/// </summary>
		ReadOnlyCollection<FrameworkElement> Views { get; }


		/// <summary>
		/// Registers a View.
		/// </summary>
		/// <param name="view">The registered View.</param>
		void Register(FrameworkElement view);


		/// <summary>
		/// Unregisters a View.
		/// </summary>
		/// <param name="view">The unregistered View.</param>
		void Unregister(FrameworkElement view);


		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <remarks>
		/// The dialog used to represent the ViewModel is retrieved from the registered mappings.
		/// </remarks>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <param name="viewModel">The ViewModel of the new dialog.</param>
		/// <returns>
		/// A nullable value of type bool that signifies how a window was closed by the user.
		/// </returns>
		bool? ShowDialog(object ownerViewModel, object viewModel);


		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <param name="ownerViewModel">
		/// A ViewModel that represents the owner window of the dialog.
		/// </param>
		/// <param name="viewModel">The ViewModel of the new dialog.</param>
		/// <typeparam name="T">The type of the dialog to show.</typeparam>
		/// <returns>
		/// A nullable value of type bool that signifies how a window was closed by the user.
		/// </returns>
		bool? ShowDialog<T>(object ownerViewModel, object viewModel) where T : Window;
  }
}
