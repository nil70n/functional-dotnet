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
            // TODO using composition and currying, complete this report.
            // Students can be optionally filtered by minimum grade across enrollments, department they are enrolled in a course in, or an instructor they have. Order does not matter.
        }
    }
}
