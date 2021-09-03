using MinatoProject.Apps.JLeagueLiveTweet.Content;
using MinatoProject.Apps.JLeagueLiveTweet.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace MinatoProject.Apps.JLeagueLiveTweet
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            // モジュールの登録
            _ = moduleCatalog.AddModule<ContentModule>();
        }
    }
}
