using Microsoft.AspNetCore.Mvc;
using StudentsRecordManagement.Models;
using StudentsRecordManagement.Services;

namespace StudentsRecordManagement.Controllers
{
    public class StudentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IStudentService _studentServices;
        public StudentController(IConfiguration configuration, IStudentService studentServices)
        {
            _configuration = configuration;
            _studentServices = studentServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentsList()
        {
            AllModels model = new AllModels();

            model.StudentList = _studentServices.GetStudentsList();

            return View(model);
        }

        [HttpPost]
        public IActionResult InsertUpdateStudent(Student formData)
        {
            AllModels model = new AllModels();

            formData.CreatedBy = 1;
            formData.CreateOn = DateTime.Now;

            string url = Request.Headers["Referer"].ToString();
            string result = String.Empty;

            if (formData.StudentId > 0)
            {
                result = _studentServices.UpdateStudent(formData);
            }
            else
            {
                result = _studentServices.InsertStudent(formData);
            }

            if (result == "Saved Successfully" || result == "Updated Successfully")
            {
                TempData["SuccessMsg"] = result;
                return Redirect(url);
            }
            else
            {
                TempData["ErrorMsg"] = result;
                return Redirect(url);
            }
        }

        [HttpPost]
        public JsonResult DeleteStudent(int StudentID)
        {
            string url = Request.Headers["Referer"].ToString();

            string result = _studentServices.DeleteStudent(StudentID);
            if (result == "Deleted Successfully")
            {
                return Json(new { message = "Deleted Successfully" });
            }
            else
            {
                return Json(new { message = "An Error occured! Please try again" });
            }
        }
    }
}
