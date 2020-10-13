using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Model
{
    abstract class RecordsSource
    {
        public abstract IEnumerable<string> GetRecords();
        public abstract void UpdateRecords(List<string> record);
    }
}
