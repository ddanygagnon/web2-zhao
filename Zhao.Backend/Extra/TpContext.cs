using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zhao.Backend.Models;

namespace Zhao.Backend.Extra
{
    public class TpContext : DbContext
    {
        public TpContext(DbContextOptions<TpContext> options) : base(options)
        {
        }

        public DbSet<Evaluer> Evaluations { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
