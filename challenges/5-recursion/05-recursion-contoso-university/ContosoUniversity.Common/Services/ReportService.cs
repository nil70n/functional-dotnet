using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using ContosoUniversity.Data.Entities;
using Curryfy;

namespace ContosoUniversity.Common.Services {
    public static class ReportService {
        public static List<Student> GenerateStudentGradeReport(List<Student> students, List<Instructor> instructors, int minGrade = 0, Department department = null, Instructor instructor = null) {
            // TODO modify this method to use recursion instead of the for loop.
            // Students can be optionally filtered by minimum grade across enrollments, department they are enrolled in a course in, or an instructor they have. Order does not matter.

            // Build filters
            var filters = new List<Func<List<Student>, List<Student>>>();
            var minGradeFilter = minGrade != 0 ? FilterByGrade.Curry()(true) : null;
            if (minGradeFilter != null) {
                filters.Add(minGradeFilter(minGrade));
            }

            var departmentFilter = department != null ? FilterByDepartment.Curry()(department.Name) : null;
            if (departmentFilter != null) {
                filters.Add(departmentFilter);
            }

            if (instructor != null) {
                var courseAssignments = GetInstructorCourseAssignments(instructors, instructor);
                var instructorFilter = FilterByInstructor.Curry()(courseAssignments);
                if (instructorFilter != null) {
                    filters.Add(instructorFilter);
                }
            }

            // This is not functional, but the next lesson will introduce what is needed here to make this functional.
            var cache = new List<Student>();
            foreach (var filter in filters) {
                cache = filter(students);
            }

            return cache;
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
