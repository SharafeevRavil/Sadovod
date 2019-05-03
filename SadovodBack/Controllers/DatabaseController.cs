using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SadovodClasses;
using SadovodBack.Models;

namespace SadovodBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DatabaseController : Controller
    {
        string connectionString = @"Server=tcp:sadovodhelperexampledbserver.database.windows.net,1433;Initial Catalog=Steads;Persist Security Info=False;User ID=Hikirangi;Password=Satana666;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //пример get запроса из базы данных
        [Route("DatabaseGetByGardenerID")]
        [HttpGet]
        public async Task<JsonResult> DatabaseGetByGardenerID()
        {
            var str = new List<DatabaseStead>();
            //происходит запрос с id, в базе данных ищутся все строки с id садовода == id, который пришел в запросе
            string sqlExpression = $"SELECT * FROM Steads WHERE GardenerID = {User.Identity.Name}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    //в этом месте присвоения можно создавать экземпляры классов с десериализацией или же просто доставать строку, затем добавлять в какой-то IEnumerable и возвращать его
                    while (reader.Read()) // построчно считываем данные
                    {
                        var stead = new ConvertClass();
                        stead.Id =(int) reader.GetValue(0);
                        stead.Stead = (string) reader.GetValue(1);
                        stead.GardenerID = (int) reader.GetValue(2);
                        var convertedStead = new DatabaseStead();
                        convertedStead.Id = stead.Id;
                        convertedStead.Stead = JsonConvert.DeserializeObject<Stead>(stead.Stead);
                        convertedStead.GardenerID = stead.GardenerID;
                        str.Add(convertedStead);
                    }
                }
                
            }
            return Json(str);
            //var l = JsonConvert.SerializeObject(str);
            //var k = l.Substring(1, l.Length);
            //return new JsonResult(JsonConvert.SerializeObject(str));
        }

        //пример delete запроса(удаляется вся информация о данном садоводе из базы данных)
        [Route("DatabaseDeleteByGardenerID")]
        [HttpDelete]
        public async void DatabaseDeleteByGardenerID()
        {
            //происходит поиск строк в базе данных, у которых id садовода == id, который пришел в запросе, затем такие строки удаляются
            string sqlExpression = $"DELETE  FROM Steads WHERE GardenerID={User.Identity.Name}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }
        //пример delete запроса(удаляется грядка по глобальному id)
        [Route("DatabaseDeleteStead")]
        [HttpDelete]
        public async void DatabaseDeleteStead(int id)
        {
            //после get запроса у нас будут все данные по грядкам садовода
            //если пользователь захочет удалить какую-то грядку, то она удалится по своему уникальному id
            string sqlExpression = $"DELETE  FROM Steads WHERE id={id}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }
        [Route("DatabaseDeleteGardenBed")]
        [HttpDelete]
        public async void DatabaseDeleteGardenBed(int steadId, int posInList)
        {
            var str = new List<DatabaseStead>();
            //происходит запрос с id, в базе данных ищутся все строки с id садовода == id, который пришел в запросе
            string sqlExpression = $"SELECT * FROM Steads WHERE id={steadId}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    //в этом месте присвоения можно создавать экземпляры классов с десериализацией или же просто доставать строку, затем добавлять в какой-то IEnumerable и возвращать его
                    while (reader.Read()) // построчно считываем данные
                    {
                        var stead = new ConvertClass();
                        stead.Id = (int)reader.GetValue(0);
                        stead.Stead = (string)reader.GetValue(1);
                        stead.GardenerID = (int)reader.GetValue(2);
                        var convertedStead = new DatabaseStead();
                        convertedStead.Id = stead.Id;
                        convertedStead.Stead = JsonConvert.DeserializeObject<Stead>(stead.Stead);
                        convertedStead.GardenerID = stead.GardenerID;
                        str.Add(convertedStead);
                    }
                }
            }
            var newList = new List<GardenBed>();
            var beds = str.FirstOrDefault().Stead.GardenBeds;
            for (var i=0;i<beds.Count;i++)
            {
                if (i!=posInList)
                {
                    newList.Add(beds[i]);
                }
            }
            var steadd = str.FirstOrDefault();
            steadd.Stead = new Stead(newList, steadd.Stead.Name);
            DatabaseUpdateStead(steadId, JsonConvert.SerializeObject(steadd.Stead));
            

        }
        //пример post запроса(добавляется грядка по id садовода)
        [Route("DatabasePostStead")]
        [HttpPost]
        public ActionResult DatabasePostStead([FromBody] string value)
        {
            string sqlExpression = "INSERT INTO Steads (Stead, GardenerID) VALUES (@value, @GardenerID)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // создаем параметр для информации о грядке
                SqlParameter steadInfo = new SqlParameter("@value", value);
                // добавляем параметр к команде
                command.Parameters.Add(steadInfo);
                // создаем параметр для id садовода
                SqlParameter gardenerID = new SqlParameter("@GardenerID", User.Identity.Name);
                // добавляем параметр к команде
                command.Parameters.Add(gardenerID);
                command.ExecuteNonQuery();
            }
            string newSqlExpression = $"SELECT * FROM Steads WHERE Stead = '{value}' AND GardenerID = {User.Identity.Name}";
            var str = new List<int>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(newSqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read()) // если есть данные
                {
                    var id = (int)reader["id"];
                    str.Add(id);
                }
            }
            return Ok(str.LastOrDefault());
        }
        //пример put запроса(изменяется информация о конкретной грядке садовода)
        [Route("DatabaseUpdateStead")]
        [HttpPut]
        public async void DatabaseUpdateStead(int id, [FromBody] string newSteadInfo)
        {
            //находится строка с данным уникальным id, затем в ней значение ячейке Stead заменяется на новую информацию 
            string sqlExpression = $"UPDATE Steads Set Stead = '{newSteadInfo}' WHERE id = {id}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }
    }
}
