using Microsoft.EntityFrameworkCore;

namespace ChargePointAPI.Controllers
{
    public class ChargerContext : DbContext
    {
        public ChargerContext(DbContextOptions options) : base(options)
        {
        }

        public object Chargers { get; internal set; }
    }
}