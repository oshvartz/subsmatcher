using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;

namespace SubtitlesMatcher.Infrastructure
{
    public class IoC
    {
        private static readonly IoC _instance = new IoC();
        public static IoC Instance { get { return _instance; } }
        private IoC()
        {
            _container = new UnityContainer();
        }

        private IUnityContainer _container;


        public IUnityContainer RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            return _container.RegisterType<TFrom, TTo>();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public IUnityContainer RegisterInstance<TInterface>(TInterface instance)
        {
            return _container.RegisterInstance<TInterface>(instance);
        }

        
    }
}
