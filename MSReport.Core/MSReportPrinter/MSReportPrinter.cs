using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    internal abstract class MSReportPrinter : BaseDispose
    {
        protected LocalReport _msReport;

        public MSReportPrinter(LocalReport report)
        {
            this._msReport = report;
        }

        protected abstract PrintSetting PrintSetting { get; }

        public virtual byte[] Print(PageMargin? margin = null, short copies = 1)
        {
            this.PrintSetting.Margin = margin;
            return this._msReport.Render(this.PrintSetting?.Format, this.PrintSetting.DeviceInfo);
        }

        protected override void DisposeManagedSource()
        {
            if (this._msReport != null)
            {
                this._msReport.Dispose();
            }
        }
    }
}
