using VS4Mac.GitHistory.Controllers.Base;

namespace VS4Mac.GitHistory.Views.Base
{
	public interface IView
	{
		void SetController(IController controller);
	}
}