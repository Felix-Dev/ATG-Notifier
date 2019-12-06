using ATG_Notifier.Data.DataContexts;
using System;

namespace ATG_Notifier.Data.Services
{
    public partial class DataServiceBase : IDataService
    {
        private readonly IDataSource dataSource;

        // Track whether Dispose has been called.
        private bool disposed = false;

        public DataServiceBase(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        #region Dispose

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
            if (!this.disposed && disposing)
            {
                this.dataSource.Dispose();
                this.disposed = true;
            }
        }

        #endregion // Dispose
    }
}
