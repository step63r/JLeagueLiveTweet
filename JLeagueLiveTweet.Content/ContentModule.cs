using MinatoProject.Apps.JLeagueLiveTweet.Content.ViewModels;
using MinatoProject.Apps.JLeagueLiveTweet.Content.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MinatoProject.Apps.JLeagueLiveTweet.Content
{
    public class ContentModule : IModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionMan = containerProvider.Resolve<IRegionManager>();
            _ = regionMan.RegisterViewWithRegion("ContentRegion", typeof(TopPage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<InputPinCodeDialogView, InputPinCodeDialogViewViewModel>();
        }
    }
}