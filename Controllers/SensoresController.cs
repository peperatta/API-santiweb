using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using APISensores.Model;

namespace APISensores.Controllers
{
    [Route("[controller]")]
    public class SensoresController : Controller
    {
        private readonly string _connectionString = "Server=10.22.29.151;Port=3306;Database=PlantCy;Uid=root;password=capEllos#777;";

        [HttpGet]
        public ActionResult<IEnumerable<Check>> GetChecks()
        {
            var checks = new List<Check>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM `check`", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            checks.Add(new Check
                            {
                                ID_Check = reader.GetInt32("ID_Check"),
                                ID_Planta = reader.GetInt32("ID_Planta"),
                                ID_Sensor = reader.GetInt32("ID_Sensor"),
                                Medicion = reader.GetFloat("Medicion"),
                                Accion_Requerida = reader.GetBoolean("Accion_Requerida"),
                                Fecha_Check = reader.GetDateTime("Fecha_Check")
                            });
                        }
                    }
                }
            }

            return Ok(checks);
        }

        [HttpPost]
        public IActionResult AddCheck([FromBody] Check newCheck)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("INSERT INTO `check` (ID_Planta, ID_Sensor, Medicion, Accion_Requerida, Fecha_Check) VALUES (@PLANTA, @SENSOR, @MEDICION, @ACCION, NOW())", connection))
                {
                    command.Parameters.AddWithValue("@PLANTA", newCheck.ID_Planta);
                    command.Parameters.AddWithValue("@SENSOR", newCheck.ID_Sensor);
                    command.Parameters.AddWithValue("@MEDICION", newCheck.Medicion);
                    command.Parameters.AddWithValue("@ACCION", newCheck.Accion_Requerida);

                    command.ExecuteNonQuery();
                }
            }

            return CreatedAtAction(nameof(GetChecks), new { id = newCheck.ID_Check }, newCheck);
        }

        [HttpPost("update")]
        public IActionResult UpdateCheck([FromBody] Check updatedCheck)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("UPDATE `check` SET Medicion = @MEDICION, Accion_Requerida = @ACCION WHERE ID_Check = @IDCHECK", connection))
                {
                    command.Parameters.AddWithValue("@MEDICION", updatedCheck.Medicion);
                    command.Parameters.AddWithValue("@ACCION", updatedCheck.Accion_Requerida);
                    command.Parameters.AddWithValue("@IDCHECK", updatedCheck.ID_Check);

                    command.ExecuteNonQuery();
                }
            }

            return NoContent();
        }
    }
}
