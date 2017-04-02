using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using SubtitleMatcher.Client.Common;
using SubtitleMatcher.Client.SubsMatchProvidersModule.ViewModel;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition;

namespace SubtitleMatcher.Client.SubsMatchProvidersModule
{
    public class SubsMatchProvidersModule: IModule
    {
         private readonly IRegionViewRegistry _regionViewRegistry;
         private readonly IUnityContainer _container;

         public SubsMatchProvidersModule(IRegionViewRegistry regionViewRegistry, IUnityContainer container)
        {
            _regionViewRegistry = regionViewRegistry;
            _container = container;

        }


        #region IModule Members

        public void Initialize()
        {
            var subsMatchProvidersViewModel = new SubsMatchProvidersViewModel();
            _container.Resolve<ICompositionService>().SatisfyImportsOnce(subsMatchProvidersViewModel);
            subsMatchProvidersViewModel.InitSubsMatchProviders();
            _container.RegisterInstance<ISubsMatchProvidersViewModel>(subsMatchProvidersViewModel);
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.SUBMATCH_PROVIDERS_REGION, typeof(Views.SubsMatchProvidersView));
        }

        #endregion
    }
}
