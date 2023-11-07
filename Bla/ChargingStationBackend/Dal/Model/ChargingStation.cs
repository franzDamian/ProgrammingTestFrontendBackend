using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Model
{
    // basic model for charging station for different charging powers
    public class ChargingStation
    {
        public int Id { get; set; }
        public int ChargingPower { get; set; }
    }
}
