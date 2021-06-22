using CalendarApi.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarAPI.Data
{
    public class HolidayDataContext: DbContext
    {
        public HolidayDataContext(DbContextOptions<HolidayDataContext> options) : base(options)
        {



        }



        public DbSet<Holiday> Holidays { get; set; }
    }
}
