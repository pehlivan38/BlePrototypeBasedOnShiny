using Microsoft.Extensions.DependencyInjection;
using Prism.Unity.Extensions;
using Shiny.Prism;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using BLEPrototype.Delegates;


namespace BLEPrototype.Startup
{
    public class BLEStartup : PrismStartup
    {

        public BLEStartup() : base(PrismContainerExtension.Current)
        {

        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            services.RegisterBleAdapterState<BleDelegates>();
            services.UseBleCentral();
        }
    }
}
