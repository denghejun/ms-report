using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    internal class PDFReportPrinter : MSReportPrinter
    {
        public PDFReportPrinter(LocalReport report) : base(report)
        {
        }

        protected override PrintSetting PrintSetting
        {
            get
            {
                return new PrintSetting()
                {
                    Format = "PDF",
                    OutputType = "PDF"
                };
            }
        }
    }
}
