using Microsoft.Extensions.DependencyInjection;
using Prism.Unity.Extensions;
using Shiny.Prism;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using BLEPrototype.Delegates;
using Prism.Ioc;
//using PotentialX.Data.Infrastructure;

namespace BLEPrototype.Startup
{
    public class BLEStartup : ShinyStartup
    {

        public override void ConfigureServices(IServiceCollection services)
        {
            services.UseBleCentral<BleCentralDelegate>();
            //services.RegisterModule<DataModule>();
        }

        public override IServiceProvider CreateServiceProvider(IServiceCollection services)
            => PrismContainerExtension.Current.CreateServiceProvider(services);
    }
}
