using System;

namespace Cdh.Toolkit.Services
{
    public abstract class LightService : IService
    {
        #region IService implementation

        void IService.Start()
        {
            isRunning = true;
        }

        void IService.Stop()
        {
            isRunning = false;
        }

        bool isRunning = false;

        bool IService.IsRunning {
            get { return isRunning; }
        }

        #endregion

        protected LightService() { }
    }
}

