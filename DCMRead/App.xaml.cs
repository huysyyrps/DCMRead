using System.ComponentModel;
using System.Windows;
using DCMRead.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;

namespace DCMRead
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return new MainView();    
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            
        }
    }
}
