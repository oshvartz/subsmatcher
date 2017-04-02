using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Composite.Regions;
using SubtitleMatcher.Client.Common;
using Microsoft.Practices.Unity;
using SubtitleMatcher.Client.FileSelectionModules.ViewModels;
using Microsoft.Practices.Composite.Events;

namespace SubtitleMatcher.Client.FileSelectionModules
{
    public class FileSelectionModule : IModule
    {
        private readonly IRegionViewRegistry _regionViewRegistry;
        private IUnityContainer _container;

        public FileSelectionModule( IRegionViewRegistry regionViewRegistry,IUnityContainer container)
        {
            
            _regionViewRegistry = regionViewRegistry;
            _container = container;
        }

        #region IModule Members

        public void Initialize()
        {

            _container.RegisterType <IFileSelectionViewModel,FileSelectionViewModel>(new ContainerControlledLifetimeManager());
            _regionViewRegistry.RegisterViewWithRegion(RegionNames.FILE_SELECTION_REGION, typeof(Views.FileSelectionView));
        }

        #endregion
    }
}
