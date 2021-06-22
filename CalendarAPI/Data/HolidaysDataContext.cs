using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApi.Data
{
    public class HolidaysDataContext : DbContext
    {
        public HolidaysDataContext(DbContextOptions<HolidaysDataContext> options): base(options)
        {

        }

        public DbSet<Holiday> Holidays { get; set; }
    }
}
