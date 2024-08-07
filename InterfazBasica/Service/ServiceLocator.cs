﻿namespace InterfazBasica_DCStore.Service
{
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
