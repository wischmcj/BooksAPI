using CalendarApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendarApi.Controllers
{
    public class HolidaysController : ControllerBase
    {
        private readonly HolidaysDataContext _context;

        public HolidaysController(HolidaysDataContext context)
        {
            _context = context;
        }

        [HttpGet("holidays")]
        public async Task<ActionResult> GetAllHolidays()
        {
            var upcomingHolidays = await _context.Holidays
                .Where(h => h.Date >= DateTime.Now)
                .ToListAsync();

            return Ok(new { data = upcomingHolidays });
        }
    }
}
