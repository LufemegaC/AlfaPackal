using FellowOakDicom;

namespace InterfazBasica_DCStore.Models.Dicom
{
    //
    // Resumen:
    //     Setup helper methods for initializing library.
    internal static class SetupPruebas
    {
        private static IServiceProviderHost _serviceProviderHost;

        internal static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProviderHost == null)
                {
                    new DicomSetupBuilder().Build();
                }

                return _serviceProviderHost.GetServiceProvider();
            }
            private set
            {
                _serviceProviderHost = new DefaultServiceProviderHost(value);
            }
        }

    //    var setupBuilder = new DicomSetupBuilder();
    //    setupBuilder.RegisterServices(
    //        services => { services.AddSingleton<IDicomOrchestrator, DicomOrchestrator>(); 
    //    });
    //// Configura opciones específicas de DICOM.
    //setupBuilder.SkipValidation().Build();


        public static void SetupDI(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static void SetupDI(IServiceProviderHost serviceProviderHost)
        {
            _serviceProviderHost = serviceProviderHost;
        }
    }
}
