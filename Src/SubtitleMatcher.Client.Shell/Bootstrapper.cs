using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.UnityExtensions;
using System.Windows;
using Microsoft.Practices.Composite.Modularity;
using System.ComponentModel.Composition;
using SubtitlesMatcher.Common.Interfaces;
using SubtitlesMatcher.Server;
using SubtitlesMatcher.Common.Parser;
using SubCenterSubtitlesMatcher.Parser;

namespace SubtitleMatcher.Client.Shell
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterInstance<ICompositionService>(App.CompositionContainer);
            Container.RegisterInstance<ISubtitlesMatcherMgr>(new SubtitlesMatcherMgr());
            Container.RegisterInstance<IMediaFileNameParser>(new MediaFileNameParser());

        }
        protected override DependencyObject CreateShell()
        {
            Shell shell = new Shell();
            shell.Show();
            return shell;
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            ModuleCatalog catalog = new ModuleCatalog()
            .AddModule(typeof(SubtitleMatcher.Client.FileSelectionModules.FileSelectionModule))
            .AddModule(typeof(SubtitleMatcher.Client.SubsMatchProvidersModule.SubsMatchProvidersModule))
            .AddModule(typeof(SubtitleMatcher.Client.ActionsModule.ActionsModule));
            return catalog;
        }


    }
}
