using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SadovodClasses;

namespace SadovodBack.Controllers
{
    public class Garedener
    {
        public string Id { get; set; }
        public List<Stead> Stead { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        //JSON Examples
        #region
        [Route("ExampleGetJSON/")]
        [HttpGet]
        public async Task<JsonResult> ExampleGetJSON()
        {
            var user = new User()
            {
                ID = 0        
            };
            var users = new List<User> {user};
            var jsonUsers = JsonConvert.SerializeObject(users);
            return new JsonResult(jsonUsers);
        }
        [Route("ExamplePostJSON/")]
        [HttpPost]
        public async void ExamplePutJSON(int id, [FromBody] string value)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(value);
        }
        #endregion
        //DB Actions
        #region
        
        #endregion
        //Simple Actions
        #region
        [Route("GetAll/")]
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            
            return new JsonResult("hellowqsad");
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
        #endregion
    }
}
