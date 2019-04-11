using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {       
        [Route("GetAll/")]
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            return new JsonResult("example");
        }
        //api/Example/GetByID?id=needId
        [Route("GetByID/")]
        [HttpGet]
        public async Task<JsonResult> Get(int id)
        {
            return new JsonResult("example");
        }

        [Route("PostNew/")]
        [HttpPost]
        public async void Post([FromBody] string value)
        {

        }
        //api/Example/PutInID?id=needId
        [Route("PutInID/")]
        [HttpPut]
        public async void Put(int id, [FromBody] string value)
        {
            

        }
        //api/Example/DeleteByID?id=needId
        [Route("DeleteByID/")]
        [HttpDelete]
        public async void Delete(int id)
        {

        }
    }
}
