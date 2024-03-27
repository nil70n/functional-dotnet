using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data.Entities;
using System;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Common;
using ContosoUniversity.Data.DbContexts;
using System.Collections.Generic;
using ContosoUniversity.Common.Services;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;

namespace ContosoUniversity.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IRepository<Instructor> _instructorRepo;
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<Department> _departmentRepo;
        private readonly IModelBindingHelperAdaptor _modelBindingHelperAdaptor;

        public ReportController(UnitOfWork<ApplicationContext> unitOfWork, IModelBindingHelperAdaptor modelBindingHelperAdaptor)
        {
            _instructorRepo = unitOfWork.InstructorRepository;
            _studentRepo = unitOfWork.StudentRepository;
            _departmentRepo = unitOfWork.DepartmentRepository;
            _modelBindingHelperAdaptor = modelBindingHelperAdaptor;
        }

        public async Task<IActionResult> RunGradeReport() {
            var students = _studentRepo.GetAll().ToList();
            var instructors = _instructorRepo.GetAll().ToList();

            var reportData = ReportService.GenerateStudentGradeReport(students, instructors, minGrade: 75);

            return View(reportData);
        }
    }
}
