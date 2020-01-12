using ATG_Notifier.Data.DataContexts;
using System;

namespace ATG_Notifier.Data.Services
{
    public partial class DataServiceBase : IDataService
    {
        private IDataSource dataSource;

        // Track whether Dispose has been called.
        private bool isDisposed = false;

        public DataServiceBase(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to take this object off the finalization queue
            // and prevent finalization code for this object from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.dataSource.Dispose();
                    this.dataSource = null!;

                    this.isDisposed = true;
                }         
            }
        }

        #endregion // IDisposable
    }
}
