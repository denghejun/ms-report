using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    internal class ImageReportPrinter : MSReportPrinter
    {
        private ImageFormat _format;
        public ImageReportPrinter(ImageFormat format, LocalReport report) : base(report)
        {
            this._format = format;
        }

        protected override PrintSetting PrintSetting
        {
            get
            {
                return new PrintSetting()
                {
                    Format = "Image",
                    OutputType = this._format.ToString()
                };
            }
        }
    }
}
