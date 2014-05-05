using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hydrate.API.Models
{
    public class HistoryResponse
    {
        public Summary summary { get; set; }
        public List<History> water { get; set; }
    }
}
