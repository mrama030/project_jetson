﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Object_Classes;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller to support requests from the Web Server.
    /// Use "api/datarequest" ar the URL.
    /// </summary>
    [Route("api/[controller]")]
    public class DataRequestController : Controller
    {
        // Authentication will be considered later in development.

        /// <summary>
        /// Returns all statistics between a given Date/Time interval. Uses 24 hour clock. 
        /// Called constantly for real-time graph and occasionally for on-demand requests.
        /// </summary>
        /// <param name="timeInterval"></param>
        /// <returns>HTTP BAD REQUEST or HTTP OK with JSON-serialized Data Message containing query results.</returns>
        [HttpGet]
        public IActionResult Get ([FromBody] TimeInterval timeInterval)
        {
            // Validate provided TimeInterval from Web Server.
            if (timeInterval.isValidInterval() == false)
                return BadRequest( new JsonResult("Invalid TimeInterval object provided.") );

            // Obtain database context.
            StatisticsDatabaseContext context = HttpContext.RequestServices.GetService(typeof(WebAPI.Models.StatisticsDatabaseContext)) as StatisticsDatabaseContext;
            
            // Obtain DataMessage using special method of the database context class.
            DataMessage responseMessage = context.getStatsFromInterval(timeInterval);

            // Serialize DataMessage to JSON format.
            JsonResult jsonResponseMessage = new JsonResult(responseMessage);

            // Return HTTP OK and JSON-serialized DataMessage containing all requested query results. 
            return Ok(jsonResponseMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="averagesOfDayRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromBody] AveragesOfDayRequest averagesOfDayRequest)
        {
            // Validate the date which the hourly averages are being requested for from Front-End API client.
            if (averagesOfDayRequest.hasValidDay() == false)
                return BadRequest(new JsonResult("Invalid date provided in request object."));

            // Obtain database context.
            StatisticsDatabaseContext context = HttpContext.RequestServices.GetService(typeof(WebAPI.Models.StatisticsDatabaseContext)) as StatisticsDatabaseContext;

            // Obtain AveragesOfDayResponse using special method of the database context class.
            AveragesOfDayResponse responseMessage = context.getHourlyAveragesForDay(averagesOfDayRequest);

            // Serialize DataMessage to JSON format.
            JsonResult jsonResponseMessage = new JsonResult(responseMessage);

            // Return HTTP OK and JSON-serialized Response object.
            return Ok(jsonResponseMessage);
        }

        [Route("api/[controller]/dbtest")]
        [HttpGet]
        public string Get()
        {
            // Ensure the Default Connection String matches your SQL Local Server. Look at appsettings.json

            StatisticsDatabaseContext context = HttpContext.RequestServices.GetService(typeof(WebAPI.Models.StatisticsDatabaseContext)) as StatisticsDatabaseContext;



            return ("test");
        }
    }
}