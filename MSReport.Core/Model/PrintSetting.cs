using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    internal class PrintSetting
    {
        public static readonly string DEFAULT_DEVICE_INFO_FORMAT =
        @"<DeviceInfo>
                <OutputFormat>{0}</OutputFormat>
                <PageWidth>8.5in</PageWidth>
                <PageHeight>11in</PageHeight>
                <MarginTop>{1}in</MarginTop>
                <MarginLeft>{2}in</MarginLeft>
                <MarginRight>{3}in</MarginRight>
                <MarginBottom>{4}in</MarginBottom>
            </DeviceInfo>";

        public string Format { get; set; }
        public string OutputType { get; set; }
        public PageMargin? Margin { get; set; }
        public string DeviceInfo
        {
            get
            {
                var pageMargin = this.Margin ?? PageMargin.Default;
                return string.Format(DEFAULT_DEVICE_INFO_FORMAT, OutputType, pageMargin.Top, pageMargin.Left, pageMargin.Right, pageMargin.Bottom);
            }
        }
    }
}
