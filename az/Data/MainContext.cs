using az.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace az.Data
{
    public class MainContext: DbContext
    {
        public MainContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
