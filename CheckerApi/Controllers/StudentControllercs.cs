using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CheckerApi.Models;
using CheckerApi.Data;

namespace CheckerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentControllercs : ControllerBase
    {
        private readonly ApiContext _context;

        public StudentControllercs(ApiContext context)
        {
            _context=context;

        }
        [HttpPost]
        public JsonResult CreateEdit(Student student)
        {

            var studentsindb = _context.Students.Where(res => res.CourseName == student.CourseName && res.CourseYear == student.CourseYear && res.StudentId==student.StudentId).ToList();
                if(studentsindb.Count==0)
                {
                    _context.Students.Add(student);
                     _context.SaveChanges();
                return new JsonResult(Ok(student));
            }
            var a = _context.Students.SingleOrDefault(res => res.CourseName == student.CourseName && res.CourseYear == student.CourseYear && res.StudentId == student.StudentId);
                a = student;
            
            _context.SaveChanges();
            return new JsonResult(Ok(student));
        }

        [HttpGet]
        public JsonResult Get(string coursename,string courseyear)
        {
            var result = _context.Students.Where(res => res.CourseName==coursename && res.CourseYear ==courseyear).ToList();

            if (result == null)
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(result));
        }
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Students.Find(id);
            if( result == null)
            {
                return new JsonResult(NotFound());
            }
            _context.Students.Remove(result);
            _context.SaveChanges();
            return new JsonResult(NoContent());

        }
        [HttpGet()]
        public JsonResult GetAll()
        {
            return new JsonResult(Ok(_context.Students.ToList()));
        }
    }
}
