using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    public class MSReportDataSource
    {
        public MSReportDataSource()
        {

        }

        public MSReportDataSource(string name, IEnumerable value)
        {
            this.Name = name;
            this.Value = value;
        }

        public string Name { get; set; }

        public IEnumerable Value { get; set; }

        public static MSReportDataSource Empty
        {
            get
            {
                return new MSReportDataSource()
                {
                    Name = nameof(MSReportDataSource),
                    Value = new List<MSReportDataSource>() { new MSReportDataSource() }
                };
            }
        }
    }
}
