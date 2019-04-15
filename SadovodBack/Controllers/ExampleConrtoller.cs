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
using SadovodClasses;

namespace WebApplication3.Controllers
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        [Route("ExampleGetJSON/")]
        [HttpGet]
        public async Task<JsonResult> ExampleGetJSON()
        {
            var user = new User()
            {
                Id = "404",
                Email = "chernikov@gmail.com",
                UserName = "rollinx",
                Name = "Andrey",
                FirstName = "Andrey",
                MiddleName = "Alexandrovich",
                LastName = "Chernikov",
                Gender = "M"
            };
            var user1 = new User()
            {
                Id = "4",
                Email = "che",
                UserName = "r",
                Name = "An",
                FirstName = "Ay",
                MiddleName = "Aledh",
                LastName = "Cherov",
                Gender = "F"
            };
            var users = new List<User> { user, user1 };
            var jsonUsers = JsonConvert.SerializeObject(users);
            return new JsonResult(jsonUsers);
        }
        [Route("ExamplePostJSON/")]
        [HttpPost]
        public async void ExamplePutJSON(int id, [FromBody] string value)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(value);
        }
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
