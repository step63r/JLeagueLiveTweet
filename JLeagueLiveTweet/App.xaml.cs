using MinatoProject.Apps.JLeagueLiveTweet.Content;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Views;
using MinatoProject.Apps.JLeagueLiveTweet.Views;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.IO;
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
        private static readonly string _settingsPath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\MinatoProject\Apps\JLeagueLiveTweet";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            if (!Directory.Exists(_settingsPath))
            {
                Directory.CreateDirectory(_settingsPath);
            }

            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TopPage>();
            containerRegistry.RegisterForNavigation<ClubsPage>();
            containerRegistry.RegisterForNavigation<PlayersPage>();
            containerRegistry.RegisterForNavigation<SettingsPage>();
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
