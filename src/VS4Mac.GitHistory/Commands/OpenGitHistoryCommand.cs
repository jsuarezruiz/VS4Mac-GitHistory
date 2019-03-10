using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.VersionControl;
using VS4Mac.GitHistory.Controllers;
using VS4Mac.GitHistory.Views;

namespace VS4Mac.GitHistory.Commands
{
	public class OpenGitHistoryCommand : CommandHandler
	{
		protected override void Run()
		{
			var projectFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;
			var gitHistoryView = new GitHistoryView();
			var skiaSharpFiddleController = new GitHistoryController(gitHistoryView, projectFile);
			IdeApp.Workbench.OpenDocument(gitHistoryView, true);
		}

		protected override void Update(CommandInfo info)
		{
			base.Update(info);

			if (VersionControlService.IsGloballyDisabled)
			{
				info.Enabled = false;
				return;
			}

			var projectFile = IdeApp.ProjectOperations.CurrentSelectedItem as ProjectFile;

			info.Enabled = projectFile != null;
		}
	}
}