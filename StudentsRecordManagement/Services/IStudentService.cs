using StudentsRecordManagement.Models;

namespace StudentsRecordManagement.Services
{
    public interface IStudentService
    {
        string InsertStudent(Student model);
        public List<Student> GetStudentsList();
        public string DeleteStudent(int StudentId);
        public string UpdateStudent(Student model);
    }
}
