using az.Data;
using az.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace az.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly MainContext context;
        private readonly IConfiguration config;

        public TicketsController(MainContext context,IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
        // GET: api/<TicketsController>
        [HttpGet]
        public IEnumerable<Ticket> Get()
        {
            return context.Tickets.ToList();
        }

        // GET api/<TicketsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TicketsController>
        [HttpPost]
        public IActionResult Post([FromBody] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                bool isUploaded = Helper.UploadBlob(config, ticket).Result;
                if (isUploaded)
                {
                    return Ok("Ticket booking being processed");
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in API"); 
            }
            return BadRequest(ModelState);
        }

        // PUT api/<TicketsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TicketsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
