using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargingStationBackend.SimulationCalculation;
using Xunit;

namespace ChargingStationBackendTests.SimulationCalculation
{
    public class Tests
    {
        [Fact]
        public void Testing()
        {
            new TempSim().SimulationRun();
        }
    }
}