﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SadovodBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        string connectionString = @"Server=tcp:sadovodhelperexampledbserver.database.windows.net,1433;Initial Catalog=Steads;Persist Security Info=False;User ID=Hikirangi;Password=Satana666;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        //пример get запроса из базы данных
        [Route("DatabaseGetByGardenerID")]
        [HttpGet]
        public async Task<JsonResult> DatabaseGetByGardenerID(int id)
        {
            var str = new List<string>();
            //происходит запрос с id, в базе данных ищутся все строки с id садовода == id, который пришел в запросе
            string sqlExpression = $"SELECT * FROM Steads WHERE GardenerID = {id}";
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
                        object Id = reader.GetValue(0);
                        object Stead = reader.GetValue(1);
                        object GardnerID = reader.GetValue(2);
                        str.Add($"{Id},{Stead},{GardnerID}");
                    }
                }
                return new JsonResult(str);
            }
        }

        //пример delete запроса(удаляется вся информация о данном садоводе из базы данных)
        [Route("DatabaseDeleteByGardenerID")]
        [HttpDelete]
        public async void DatabaseDeleteByGardenerID(int id)
        {
            //происходит поиск строк в базе данных, у которых id садовода == id, который пришел в запросе, затем такие строки удаляются
            string sqlExpression = $"DELETE  FROM Steads WHERE GardenerID={id}";
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

        //пример post запроса(добавляется грядка по id садовода)
        [Route("DatabasePostStead")]
        [HttpPost]
        public async void DatabasePostStead(int id, [FromBody] string value)
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
                SqlParameter gardenerID = new SqlParameter("@GardenerID", id);
                // добавляем параметр к команде
                command.Parameters.Add(gardenerID);
                command.ExecuteNonQuery();
            }
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