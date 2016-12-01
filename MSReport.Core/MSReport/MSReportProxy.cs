using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    /// <summary>
    /// The MSReport Factory (TReport must be designed without state stored.)
    /// </summary>
    /// <typeparam name="TReport">Report Type</typeparam>
    public static class MSReportProxy<TReport> where TReport : MSReportBase
    {
        private static TReport _report = null;
        public static TReport Report
        {
            get
            {
                return _report ?? (_report = (TReport)Activator.CreateInstance(typeof(TReport)));
            }
        }
    }
}
