using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using Microsoft.Practices.Unity;
using SubtitleMatcher.Client.Common;
using SubtitleMatcher.Client.ActionsModules.Views;
using SubtitleMatcher.Client.ActionsModules.ViewModels;

namespace SubtitleMatcher.Client.ActionsModule
{
    public class ActionsModule : IModule
    {
         private readonly IRegionViewRegistry _regionViewRegistry;
        private IUnityContainer _container;

        public ActionsModule(IRegionViewRegistry regionViewRegistry, IUnityContainer container)
        {
            _regionViewRegistry = regionViewRegistry;
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {

            _container.RegisterType<ActionsViewModel, ActionsViewModel>(new ContainerControlledLifetimeManager());
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.ACTIONS_REGION, typeof(ActionsView));
        }

        #endregion
    }
}
