using ContosoUniversity.Data.Entities;
using ContosoUniversity.Tests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Common;
using ContosoUniversity.Common.Services;
using ContosoUniversity.Data.DbContexts;
using ContosoUniversity.Web;
using ContosoUniversity.Web.Controllers;

namespace ContosoUniversity.Test.ServiceTests {
    public class GradingServiceTests {
        private readonly ITestOutputHelper output;
        private readonly Mock<IRepository<Course>> mockCourseRepo;
        private readonly Mock<IRepository<Department>> mockDepartmentRepo;
        private readonly Mock<IPersonRepository<Student>> mockStudentRepo;
        private readonly Mock<IModelBindingHelperAdaptor> mockModelBindingHelperAdaptor;
        private readonly CoursesController sut;

        public GradingServiceTests(ITestOutputHelper output) {
            this.output = output;
            mockCourseRepo = Courses().AsMockRepository();
            mockDepartmentRepo = Departments().AsMockRepository();
            mockStudentRepo = Students().AsMockPersonRepository();
            mockModelBindingHelperAdaptor = new Mock<IModelBindingHelperAdaptor>();

            var mockUnitOfWork = new Mock<UnitOfWork<ApplicationContext>>();
            mockUnitOfWork.Setup(c => c.DepartmentRepository).Returns(mockDepartmentRepo.Object);
            mockUnitOfWork.Setup(c => c.StudentRepository).Returns(mockStudentRepo.Object);
            mockUnitOfWork.Setup(c => c.CourseRepository).Returns(mockCourseRepo.Object);

            sut = new CoursesController(mockUnitOfWork.Object, mockModelBindingHelperAdaptor.Object);
        }

        [Fact]
        public async Task CurveGradesTest() {
            var curvedGrades = GradingService.CurveGrades(Enrollments());
            
            var bottomGrade = curvedGrades.OrderBy(x => x.Grade).First();
            var topGrade = curvedGrades.OrderBy(x => x.Grade).Last();

            Assert.Equal(100, topGrade.Grade);
            Assert.Equal(1, topGrade.StudentID);
            
            Assert.Equal(78, bottomGrade.Grade);
            Assert.Equal(4, bottomGrade.StudentID);
        }

        private List<Enrollment> Enrollments() {
            return new List<Enrollment>
            {
                new Enrollment { ID = 1, CourseID = 1, Grade = 83, StudentID = 1 },
                new Enrollment { ID = 2, CourseID = 1, Grade = 81, StudentID = 2 },
                new Enrollment { ID = 3, CourseID = 1, Grade = 75, StudentID = 3 },
                new Enrollment { ID = 4, CourseID = 1, Grade = 61, StudentID = 4 },
            };
        }

        private List<Student> Students() {
            return new List<Student>
            {
                new Student { ID = 1, FirstMidName = "John Billy", LastName = "Johnson", Enrollments = new List<Enrollment>() { new Enrollment() { ID = 1, CourseID = 2 } } },
                new Student { ID = 2, FirstMidName = "Jane Marie", LastName = "Smith", Enrollments = new List<Enrollment>() { new Enrollment() { ID = 1, CourseID = 2 } } },
                new Student { ID = 3, FirstMidName = "Kandice Lewis", LastName = "Makelroy", Enrollments = new List<Enrollment>() { new Enrollment() { ID = 1, CourseID = 2 } } },
                new Student { ID = 4, FirstMidName = "Jason Williams", LastName = "Bower", Enrollments = new List<Enrollment>() { new Enrollment() { ID = 1, CourseID = 4 } } },
            };
        }

        private List<Department> Departments() {
            return new List<Department>
            {
                    new Department { ID = 1, Name = "English",     Budget = 350000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 1 },
                    new Department { ID = 2, Name = "Mathematics", Budget = 100000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 2 },
                    new Department { ID = 3, Name = "Engineering", Budget = 350000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 3 },
                    new Department { ID = 4, Name = "Economics",   Budget = 100000, AddedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow, StartDate = DateTime.Parse("2007-09-01"), InstructorID  = 4 }
            };
        }

        private List<Course> Courses() {
            return new List<Course>
            {
                new Course { ID = 1, CourseNumber = 1050, Title = "Chemistry", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "Engineering").ID },
                new Course { ID = 2, CourseNumber = 4022, Title = "Microeconomics", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "Economics").ID },
                new Course { ID = 3, CourseNumber = 4041, Title = "Macroeconomics", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "Economics").ID },
                new Course { ID = 4, CourseNumber = 1045, Title = "Calculus", Credits = 4, DepartmentID = Departments().Single( s => s.Name == "Mathematics").ID },
                new Course { ID = 5, CourseNumber = 3141, Title = "Trigonometry", Credits = 4, DepartmentID = Departments().Single( s => s.Name == "Mathematics").ID },
                new Course { ID = 6, CourseNumber = 2021, Title = "Composition", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "English").ID },
                new Course { ID = 7, CourseNumber = 2042, Title = "Literature", Credits = 4, DepartmentID = Departments().Single( s => s.Name == "English").ID }
            };
        }
    }
}
