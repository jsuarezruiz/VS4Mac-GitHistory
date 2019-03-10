using Foundation;
using WebKit;

namespace VS4Mac.GitHistory.Controls
{
	public class GitHistoryWebView : ExtendedWebView, IWKNavigationDelegate
	{
		public GitHistoryWebView()
		{
			NavigationDelegate = new GitHistoryNavigationDelegate();
		}

		public WKNavigation InitialNavigation { get; set; }

		public void OpenUrl(string uri)
		{
			var url = new NSUrl(uri);
			var request = new NSUrlRequest(url);
			InitialNavigation = LoadRequest(request);
		}
	}

	public class GitHistoryNavigationDelegate : WKNavigationDelegate
	{
		public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
		{
			var gitHistoryWebView = webView as GitHistoryWebView;

			if (navigation == gitHistoryWebView.InitialNavigation)
			{
				gitHistoryWebView.InitialNavigation = null;
			}
		}
	}
}