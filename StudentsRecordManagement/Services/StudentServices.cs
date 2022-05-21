using Dapper;
using StudentsRecordManagement.Models;
using System.Data;
using System.Data.SqlClient;

namespace StudentsRecordManagement.Services
{
    public class StudentServices : IStudentService
    {
        public readonly IConfiguration _configuration;

        public string ConnectionString { get; }
        public string providerName { get; }

        public StudentServices(IConfiguration configuration)
        {
            _configuration = configuration;
            ConnectionString = _configuration.GetConnectionString("DBConnection");
            providerName = "System.Data.SqlClient";
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConnectionString);
            }
        }

        public string InsertStudent(Student model)
        {
            string result = string.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var students = dbConnection.Query<Student>("InsertStudentRecord", new { 
                        FullName = model.FullName, 
                        EmailAddress = model.EmailAddress, 
                        City = model.City, 
                        CreatedBy = 1
                    }, commandType: CommandType.StoredProcedure).ToList();

                    if (students != null && students.FirstOrDefault().Response == "Saved Successfully")
                    {
                        result =  "Saved Successfully";
                    }
                    dbConnection.Close();
                    return result;
                }               
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return errorMsg;               
            }
        }

        public List<Student> GetStudentsList()
        {
            List<Student> result = new List<Student>();
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    result = dbConnection.Query<Student>("GetStudentsList", null, commandType: CommandType.StoredProcedure).ToList();
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return result;
            }
        }

        public string DeleteStudent(int StudentId)
        {
            string result = string.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var students = dbConnection.Query<Student>("DeleteStudentRecord", new { StudentId = StudentId }, commandType: CommandType.StoredProcedure).ToList();

                    if (students != null && students.FirstOrDefault().Response == "Deleted Successfully")
                    {
                        result = "Deleted Successfully";
                    }
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return errorMsg;
            }
        }

        public string UpdateStudent(Student model)
        {
            string result = string.Empty;
            try
            {
                using (IDbConnection dbConnection = Connection)
                {
                    dbConnection.Open();
                    var students = dbConnection.Query<Student>("UpdateStudentRecord", new
                    {
                        StudentId = model.StudentId,
                        FullName = model.FullName,
                        EmailAddress = model.EmailAddress,
                        City = model.City                       
                    }, commandType: CommandType.StoredProcedure).ToList();

                    if (students != null && students.FirstOrDefault().Response == "Updated Successfully")
                    {
                        result = "Updated Successfully";
                    }
                    dbConnection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return errorMsg;
            }
        }
    }
}
