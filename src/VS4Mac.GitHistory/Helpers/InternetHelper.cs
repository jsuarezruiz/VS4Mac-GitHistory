using System.Net.NetworkInformation;

namespace VS4Mac.GitHistory.Helpers
{
	public static class InternetHelper
	{
		public static bool CheckInternetConnection()
		{
			Ping ping = new Ping();

			PingReply pingStatus = ping.Send("www.microsoft.com", 1000);

			if (pingStatus.Status == IPStatus.Success)
			{
				return true;
			}

			return false;
		}
	}
}