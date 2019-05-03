﻿using System;
using System.Collections.Generic;
using System.Linq;
using NAPS2.Config.Experimental;
using NAPS2.WinForms;
using Ninject;

namespace NAPS2
{
    public class NinjectFormFactory : IFormFactory
    {
        private readonly IKernel kernel;

        public NinjectFormFactory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Create<T>() where T : FormBase
        {
            var form = kernel.Get<T>();
            form.FormFactory = kernel.Get<IFormFactory>();
            form.ConfigScopes = kernel.Get<ConfigScopes>();
            form.ConfigProvider = kernel.Get<ScopeSetConfigProvider<CommonConfig>>();
            return form;
        }
    }
}