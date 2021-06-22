using BooksApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksApi.Controllers
{
    public class StatusController : ControllerBase
    {

        private readonly ILookupStatus _statusLookup;

        public StatusController(ILookupStatus statusLookup)
        {
            _statusLookup = statusLookup;
        }



        // GET /status 
        [HttpGet("status")]
        public ActionResult GetTheStatus()
        {

            // do the work... create the thing..
            GetStatusResponse response = _statusLookup.GetMyStatus();
            return Ok(response);
        }

        // GET /employees/3893893 (route parameters)
        [HttpGet("employees/{employeeId:int}", Name ="status-getemployees")]
        public ActionResult GetEmployees(int employeeId)
        {

            return Ok($"Getting you information about employee {employeeId}");
        }

        // GET /blogs/{year:int}/{month:int}/{day:int}
        [HttpGet("/blogs/{year:int}/{month:int}/{day:int}")]
        public ActionResult GetBlogPosts(int year, int month, int day)
        {
            return Ok($"Blogs for {year}/{month}/{day}");
        }

        [HttpGet("employees")]
        public ActionResult GetAllEmployees([FromQuery] int maxCount, [FromQuery] string role = "All")
        {
            if (maxCount < 0)
            {
                return BadRequest("Maxcount has to be > 0");
            }
            return Ok($"sending you {role} employees Count {maxCount}");
        }


        [HttpPost("employees")]
        public ActionResult Hire([FromBody] PostEmployeeRequest request)
        {
            // 1. Validate it.
            //    - if it isn't valid, return a 400 (perhaps with details details)
            // 2. Do whatever with it. Hire the person. Insert it into the database.
            // GET /employees/8398
            // Status code is 201 (Created)
            // Location: http://localhost:1337/employees/87398
            // give them a copy of what you created.
            // copypasta
            var response = new GetEmployeeResponse( // "Mapping"
                42,
                request.FirstName,
                request.LastName,
                request.Department,
                120000M
                );

            return CreatedAtRoute(
                "status-getemployees",
                new { employeeId = response.Id },
                response);
        }


    }


    //public class PostEmployeeRequest
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Department { get; set; }
    //}

    public record PostEmployeeRequest(string FirstName, string LastName, string Department);

    public record GetEmployeeResponse(
        int Id,
        string FirstName,
        string LastName,
        string Department,
        decimal StartingSalary);

    public class GetStatusResponse
    {
        public string Message { get; set; }
        public DateTime LastChecked { get; set; }
        public string CheckedBy { get; set; }
    }
}
