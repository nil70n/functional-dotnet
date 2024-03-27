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
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace ContosoUniversity.Test.ServiceTests {
    public class ReportServiceTests {
        private readonly ITestOutputHelper output;
        private readonly Mock<IRepository<Course>> mockCourseRepo;
        private readonly Mock<IRepository<Department>> mockDepartmentRepo;
        private readonly Mock<IModelBindingHelperAdaptor> mockModelBindingHelperAdaptor;
        private readonly CoursesController sut;

        public ReportServiceTests(ITestOutputHelper output) {
            this.output = output;
            mockCourseRepo = Courses().AsMockRepository();
            mockDepartmentRepo = Departments().AsMockRepository();
            mockModelBindingHelperAdaptor = new Mock<IModelBindingHelperAdaptor>();

            var mockUnitOfWork = new Mock<UnitOfWork<ApplicationContext>>();
            mockUnitOfWork.Setup(c => c.DepartmentRepository).Returns(mockDepartmentRepo.Object);
            mockUnitOfWork.Setup(c => c.CourseRepository).Returns(mockCourseRepo.Object);

            sut = new CoursesController(mockUnitOfWork.Object, mockModelBindingHelperAdaptor.Object);
        }

        [Fact]
        public void StudentsWithInstructor_Report_Test() {
            var students = ReportService.GenerateStudentGradeReport(Students(), Instructors(), instructor: Instructors().Last());

            // This is not the best way to do testing with Option's, but it will suffice for now.
            Assert.True(students.IsSome);
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 1) != null));
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 2) != null));
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 3) != null));
        }

        [Fact]
        public void StudentsWithMinGrade_Report_Test() {
            var students = ReportService.GenerateStudentGradeReport(Students(), Instructors(), minGrade: 80);
            
            // This is not the best way to do testing with Option's, but it will suffice for now.
            Assert.True(students.IsSome);
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 3) != null));
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 4) != null));
        }

        [Fact]
        public void StudentsInDepartment_Report_Test() {
            var students = ReportService.GenerateStudentGradeReport(Students(), Instructors(), department: Departments().Last());
            
            // This is not the best way to do testing with Option's, but it will suffice for now.
            Assert.True(students.IsSome);
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 1) != null));
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 2) != null));
            Assert.True(students.Exists(s => s.FirstOrDefault(x => x.ID == 3) != null));
        }

        private List<Student> Students() {
            return new List<Student>
            {
                new Student { ID = 1, FirstMidName = "John Billy", LastName = "Johnson", Enrollments = new List<Enrollment>() {
                    new Enrollment() { ID = 1, CourseID = 2, Course = Courses().First(x => x.ID == 2), Grade = 75 }
                } },
                new Student { ID = 2, FirstMidName = "Jane Marie", LastName = "Smith", Enrollments = new List<Enrollment>() {
                    new Enrollment() { ID = 1, CourseID = 2, Course = Courses().First(x => x.ID == 2), Grade = 54 }
                } },
                new Student { ID = 3, FirstMidName = "Kandice Lewis", LastName = "Makelroy", Enrollments = new List<Enrollment>() {
                    new Enrollment() { ID = 1, CourseID = 2, Course = Courses().First(x => x.ID == 2), Grade = 100 }
                } },
                new Student { ID = 4, FirstMidName = "Jason Williams", LastName = "Bower", Enrollments = new List<Enrollment>() {
                    new Enrollment() { ID = 1, CourseID = 4, Course = Courses().First(x => x.ID == 4), Grade = 89 }
                } },
            };
        }

        private List<Instructor> Instructors() {
            return new List<Instructor>
            {
                new Instructor { ID = 1, FirstMidName = "Jacob Willis", LastName = "Hopkins", CourseAssignments = new List<CourseAssignment>() {
                    new CourseAssignment() { CourseID = 1, InstructorID = 1 },
                    new CourseAssignment() { CourseID = 5, InstructorID = 1 },
                    new CourseAssignment() { CourseID = 6, InstructorID = 1 }
                } },
                new Instructor { ID = 2, FirstMidName = "Mary Tilly", LastName = "Hopkins", CourseAssignments = new List<CourseAssignment>() {
                    new CourseAssignment() { CourseID = 2, InstructorID = 2 },
                    new CourseAssignment() { CourseID = 3, InstructorID = 2 }
                } },
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
                new Course {
                    ID = 1, CourseNumber = 1050, Title = "Chemistry", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "Engineering").ID, Department = Departments().Single( s => s.Name == "Engineering")
                },
                new Course {
                    ID = 2, CourseNumber = 4022, Title = "Microeconomics", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "Economics").ID, Department = Departments().Single( s => s.Name == "Economics")
                },
                new Course {
                    ID = 3, CourseNumber = 4041, Title = "Macroeconomics", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "Economics").ID, Department = Departments().Single( s => s.Name == "Economics")
                },
                new Course {
                    ID = 4, CourseNumber = 1045, Title = "Calculus", Credits = 4, DepartmentID = Departments().Single( s => s.Name == "Mathematics").ID, Department = Departments().Single( s => s.Name == "Mathematics")
                },
                new Course {
                    ID = 5, CourseNumber = 3141, Title = "Trigonometry", Credits = 4, DepartmentID = Departments().Single( s => s.Name == "Mathematics").ID, Department = Departments().Single( s => s.Name == "Mathematics")
                },
                new Course {
                    ID = 6, CourseNumber = 2021, Title = "Composition", Credits = 3, DepartmentID = Departments().Single( s => s.Name == "English").ID, Department = Departments().Single( s => s.Name == "English")
                },
                new Course {
                    ID = 7, CourseNumber = 2042, Title = "Literature", Credits = 4, DepartmentID = Departments().Single( s => s.Name == "English").ID, Department = Departments().Single( s => s.Name == "English")
                }
            };
        }
    }
}
