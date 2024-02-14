using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]

    [ApiController]

    public class ValuesController : ControllerBase
    {
        string CONNECTIONSTRING = "Host=localhost;Port = 5432;Database = Lesson;User Id = postgres;Password = root;";

        [HttpGet]

        public List<Student> GetAllWithDupper()
        {
            string sql = "SELECT * FROM students";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                connection.Open();

                return connection.Query<Student>(sql).ToList();
            }
        }

        [HttpPost]
        public StudentDTO InsertWithDupper(StudentDTO viewStudent)
        {
            var sql = "insert into students (full_name,grade,avg_score) values (@fullName,@grade,@avgScore)";
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                connection.Execute(sql, new StudentDTO
                {
                    fullName = viewStudent.fullName,
                    grade = viewStudent.grade,
                    avgScore = viewStudent.avgScore,
                });

                return viewStudent;
            }
        }

        [HttpPut]
        public StudentDTO PutWithDapper(StudentDTO viewStudent,int id)
        {
            var sql = $"update students set full_name = @fullname,grade = @grade,avg_score = @avgScore where id = {id} ";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                connection.Execute(sql, new StudentDTO
                {
                    fullName = viewStudent.fullName,
                    grade = viewStudent.grade,
                    avgScore = viewStudent.avgScore,
                });

                return viewStudent;
            }

        }

        [HttpDelete]

        public string deleteWithDapper(int id)
        {
            var sql = $"delete from students where id = @id ";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {

                connection.Execute(sql,new {id});
                return "Deleted";
            }
        }

        [HttpPatch]
        public string updateStudentFullaNameWithDapper(int id,string fullName)
        {
            var sql = $"update students set full_name = @fullName where id = @id";

            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {

                connection.Execute(sql, new {fullName, id });
                return "Updated";
            }
        }


        [HttpGet]
        public List<Student> Get()
        {
            List<Student> students = new List<Student>();
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand("select * from students", connection);

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        students.Add(new Student()
                        {
                            Id = reader.GetInt32(0),
                            full_name = reader.GetString(1),
                            grade = reader.GetInt32(2),
                            avgScore = reader.GetDouble(3)
                        });
                    }

                }
                catch { }
                return students;
            }
        }



        [HttpPost]
        public string Post(string flName, int grade, double avg)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                connection.Open();
                try
                {
                    using NpgsqlCommand cmd = new NpgsqlCommand($"insert into students(full_name,grade,avg_score) values('{flName}',{grade},{avg});", connection);
                    var reader = cmd.ExecuteNonQuery();
                    return "All right";
                }
                catch
                {
                    return "Something wrong";
                }

            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public StudentDTO Put(StudentDTO student,int id)
        {
            try
            {

                using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();

                    using NpgsqlCommand cmd = new NpgsqlCommand($"update from students set fulla_name = '{student.fullName}',grade = {student.grade}, avg_score = {student.avgScore});");
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
               
            }
            return student;

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        public string Delete(int id)
        {
            try
            {

                using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();

                    using NpgsqlCommand cmd = new NpgsqlCommand($"delete from students where id = {id};", connection);
                    cmd.ExecuteNonQuery();
                    return "All right";
                }
            }
            catch
            {
                return " Not found";
            }
        }
        [HttpPatch]
        public string UpdateStudentsFullName(int id,string fullaName)
        {
            try
            {

                using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
                {
                    connection.Open();

                    using NpgsqlCommand cmd = new NpgsqlCommand($"update students set full_name = '{fullaName}' where id = {id};", connection);
                    cmd.ExecuteNonQuery();
                    return "updated";
                }
            }
            catch
            {
                return " Not found";
            }
        }
    }
}
