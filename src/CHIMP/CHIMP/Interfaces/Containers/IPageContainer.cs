using System.Windows.Controls;

namespace Chimp
{
    public interface IPageContainer
    {
        Page GetPage(string name);
    }
}
