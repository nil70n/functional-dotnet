using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using ContosoUniversity.Data.Entities;
using Curryfy;
using LanguageExt;

namespace ContosoUniversity.Common.Services {
    public static class ReportService {
        public static Option<List<Student>> GenerateStudentGradeReport(List<Student> students, List<Instructor> instructors, int minGrade = 0, Department department = null, Instructor instructor = null) {
            // TODO modify this method to use an Option for the Student list
            // Make sure the ReportServiceTests run when you are finish to confirm your code works.
            // Students can be optionally filtered by minimum grade across enrollments, department they are enrolled in a course in, or an instructor they have. Order does not matter.
            
            // Build filters
            var filters = new Stack<Func<List<Student>, List<Student>>>();
            var minGradeFilter = minGrade != 0 ? FilterByGrade.Curry()(true) : null;
            if (minGradeFilter != null) {
                filters.Push(minGradeFilter(minGrade));
            }

            var departmentFilter = department != null ? FilterByDepartment.Curry()(department.Name) : null;
            if (departmentFilter != null) {
                filters.Push(departmentFilter);
            }

            if (instructor != null) {
                var courseAssignments = GetInstructorCourseAssignments(instructors, instructor);
                var instructorFilter = FilterByInstructor.Curry()(courseAssignments);
                if (instructorFilter != null) {
                    filters.Push(instructorFilter);
                }
            }

            return ApplyFilters(filters, students);
        }

        private static List<Student> ApplyFilters(Stack<Func<List<Student>, List<Student>>> filters, List<Student> students) {
            if (filters.Count == 0) return students;
            var current = filters.Pop();
            return ApplyFilters(filters, current(students));
        }

        private static readonly Func<bool, int, List<Student>, List<Student>> FilterByGrade = (minimum, grade, students) => students.Where(x => x.Enrollments.All(y => minimum ? y.Grade > grade : y.Grade < grade)).ToList();

        private static readonly Func<string, List<Student>, List<Student>> FilterByDepartment = (department, students) => students.Where(x => x.Enrollments.Any(y => y.Course.Department.Name == department)).ToList();

        private static readonly Func<List<Instructor>, Instructor, List<CourseAssignment>> GetInstructorCourseAssignments = (instructors, instructor) => instructors.Where(x => x.ID == instructor.ID).SelectMany(x => x.CourseAssignments).ToList();

        private static readonly Func<List<CourseAssignment>, List<Student>, List<Student>> FilterByInstructor = 
            (courseAssignments, students) => students.Where(
                x => x.Enrollments.Any(
                    y => courseAssignments.Select(z => z.CourseID).Contains(y.Course.ID)
                )
            ).ToList();
    }
}
