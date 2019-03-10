using System;
using System.IO;
using System.Linq;
using MonoDevelop.Core;
using MonoDevelop.Projects;
using VS4Mac.GitHistory.Models;

namespace VS4Mac.GitHistory.Helpers
{
	/// <summary>
	/// Based on ShowInGitHub addin by Lluis Sanchez.
	/// More information: https://github.com/slluis/ShowInGithub
	/// </summary>
	public static class UrlHelper
	{
		internal static string GetUrl(ProjectFile projectFile)
		{
			string result = string.Empty;

			try
			{
				var gitDir = GetGitDir(projectFile.FilePath);

				if (gitDir == null)
					return null;

				var rootDir = Path.GetDirectoryName(gitDir);

				var gitModulesFile = Path.Combine(rootDir, ".gitmodules");
				GitConfigSection submoduleSection = null;
				if (File.Exists(gitModulesFile))
				{
					var modulesConfig = new GitConfigFile();
					modulesConfig.LoadFile(gitModulesFile);
					foreach (var section in modulesConfig.Sections)
					{
						//Checking if file is inside submodule folder
						if (section.Type == "submodule" &&
							section.GetValue("path") != null &&
							Path.GetFullPath(projectFile.FilePath.FileName).StartsWith(Path.Combine(rootDir, Path.Combine(section.GetValue("path").Split('/'))), StringComparison.Ordinal))
						{
							gitDir = Path.Combine(gitDir, "modules", Path.Combine(section.GetValue("path").Split('/')));
							submoduleSection = section;
							break;
						}
					}
				}

				var file = new GitConfigFile();
				file.LoadFile(Path.Combine(gitDir, "config"));

				string head = File.ReadAllText(Path.Combine(gitDir, "HEAD")).Trim();
				string branch;
				string remote = null;
				if (head.StartsWith("ref: refs/heads/", StringComparison.CurrentCulture))
				{
					branch = head.Substring(16);
					var sec = file.Sections.FirstOrDefault(s => s.Type == "branch" && s.Name == branch);
					if (sec != null)
						remote = sec.GetValue("remote");
				}
				else
				{
					branch = head;
				}
				if (string.IsNullOrEmpty(remote))
					remote = "origin";
				var rref = file.Sections.FirstOrDefault(s => s.Type == "remote" && s.Name == remote);
				if (rref == null)
					return null;

				var url = rref.GetValue("url");
				if (url.EndsWith(".git", StringComparison.CurrentCulture))
					url = url.Substring(0, url.Length - 4);

				string host;

				int k = url.IndexOfAny(new[] { ':', '@' });
				if (k != -1 && url[k] == '@')
				{
					k++;
					int i = url.IndexOf(':', k);
					if (i != -1)
						host = url.Substring(k, i - k);
					else
						return null;
				}
				else
				{
					if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
						host = uri.Host;
					else
						return null;
				}

				int j = url.IndexOf(host, StringComparison.CurrentCulture);
				var repo = url.Substring(j + host.Length + 1);

				var fileRootDir = submoduleSection == null ? rootDir : Path.Combine(rootDir, Path.Combine(submoduleSection.GetValue("path").Split('/')));
				string subdir = projectFile.FilePath.ToRelative(fileRootDir);
				subdir = subdir.Replace('\\', '/');

				result = "https://" + host + "/" + repo + "/blob/" + branch + "/" + subdir;

				result = result.Replace("github.com", "github.githistory.xyz");
			}
			catch(Exception ex)
			{
				LoggingService.LogError(ex.Message, ex);
			}

			return result;
		}

		internal static string GetGitDir(string subdir)
		{
			var r = Path.GetPathRoot(subdir);
			while (!string.IsNullOrEmpty(subdir) && subdir != r)
			{
				var gd = Path.Combine(subdir, ".git");
				if (Directory.Exists(gd))
					return gd;
				subdir = Path.GetDirectoryName(subdir);
			}
			return null;
		}
	}
}