using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerWatchAgent.Mirroring
{
    public class MirroringData
    {
        public string DatabaseName { get; set; }
        public DateTime Timestamp { get; set; }
        public int MirroringRole { get; set; }
        public int MirroringState { get; set; }
        public string WitnessStatus { get; set; }
    }
}
