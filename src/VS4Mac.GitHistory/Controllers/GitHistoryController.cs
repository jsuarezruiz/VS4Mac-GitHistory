using System;
using MonoDevelop.Projects;
using VS4Mac.GitHistory.Controllers.Base;
using VS4Mac.GitHistory.Helpers;
using VS4Mac.GitHistory.Views;

namespace VS4Mac.GitHistory.Controllers
{
	public class GitHistoryController : IController
	{
		readonly IGitHistoryView _view;
		readonly ProjectFile _projectFile;

		public GitHistoryController(IGitHistoryView view, ProjectFile projectFile)
		{
			_view = view;
			_projectFile = projectFile;
			view.SetController(this);
		}

		public string GetUrl()
		{
			if (!InternetHelper.CheckInternetConnection())
				throw new Exception("Can not access the Git history without a internet connection.");

			if(_projectFile != null)
				return UrlHelper.GetUrl(_projectFile);

			return string.Empty;
		}
	}
}