using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bit0.CrunchLog.Plugin
{
    public interface ICrunchPlugin
    {
        void Register(IServiceCollection services);
    }
}
