using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    internal abstract class BaseDispose : IDisposable
    {
        bool disposed = false;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseDispose()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                this.DisposeManagedSource();
            }

            this.DisposeUnManagedSource();

            disposed = true;
        }

        protected virtual void DisposeManagedSource()
        {
        }

        protected virtual void DisposeUnManagedSource()
        {
        }
    }
}
