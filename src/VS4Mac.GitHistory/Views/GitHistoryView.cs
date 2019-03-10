using System;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using VS4Mac.GitHistory.Controllers;
using VS4Mac.GitHistory.Controllers.Base;
using VS4Mac.GitHistory.Controls;
using VS4Mac.GitHistory.Views.Base;
using Xwt;
using Xwt.Drawing;

namespace VS4Mac.GitHistory.Views
{
	public interface IGitHistoryView : IView
	{

	}

	public class GitHistoryView : AbstractXwtViewContent, IGitHistoryView
	{
		VBox _mainBox;
		GitHistoryWebView _gitHistoryWebView;
		Widget _xwtGitHistoryWebView;
		Label _errorLabel;

		GitHistoryController _controller;

		public GitHistoryView()
		{
			Init();
			BuildGui();
		}

		public override Widget Widget => _mainBox;

		public override bool IsViewOnly
		{
			get
			{
				return true;
			}
		}

		public override bool IsFile
		{
			get
			{
				return false;
			}
		}

		void Init()
		{
			_mainBox = new VBox();
			_gitHistoryWebView = new GitHistoryWebView();

			_errorLabel = new Label
			{
				Font = Font.SystemSansSerifFont.WithSize(18),
				TextColor = Styles.SecondaryTextColor,
				HorizontalPlacement = WidgetPlacement.Center,
				Visible = false
			};
		}

		void BuildGui()
		{
			ContentName = "Git History";

			_xwtGitHistoryWebView = Toolkit.CurrentEngine.WrapWidget(_gitHistoryWebView);
			_mainBox.PackStart(_xwtGitHistoryWebView, true);
			_mainBox.PackStart(_errorLabel, true);
		}

		public void SetController(IController controller)
		{
			_controller = (GitHistoryController)controller;

			LoadUrl();
		}

		void LoadUrl()
		{
			try
			{
				var url = _controller.GetUrl();

				if (!string.IsNullOrEmpty(url))
					_gitHistoryWebView.OpenUrl(url);
				else
					ShowError("Can not get the Git history of the file.");
			}
			catch(Exception ex)
			{
				LoggingService.LogError(ex.Message, ex);
				ShowError(ex.Message);
			}
		}

		void ShowError (string errorMessage)
		{
			_errorLabel.Text = errorMessage;
			_errorLabel.Visible = true;
			_xwtGitHistoryWebView.Visible = false;
		}
	}
}