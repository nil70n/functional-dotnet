using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContosoUniversity.Data.Entities;

namespace ContosoUniversity.Common.Services {
    public static class GradingService {
        // Curve the course grades on a highest-point curve.
        // Take the highest grade, get the difference between that and 100
        // and add those points to all grades
        static int GetCurveValue(List<Enrollment> enrollments) => 100 - enrollments.Max(x => x.Grade ?? 0);

        static Enrollment CurveGrade(Enrollment enrollment, int curveValue) => new Enrollment() {
            AddedDate = enrollment.AddedDate,
            Course = enrollment.Course,
            CourseID = enrollment.CourseID,
            ID = enrollment.ID,
            ModifiedDate = enrollment.ModifiedDate,
            RowVersion = enrollment.RowVersion,
            Student = enrollment.Student,
            StudentID = enrollment.StudentID,
            Grade = enrollment.Grade.HasValue ? enrollment.Grade + curveValue : 0
        };

        public static List<Enrollment> CurveGrades(List<Enrollment> currentEnrollments) {
            // Remember to maintain immutability and functional purity
            var curve = GetCurveValue(currentEnrollments);
            return currentEnrollments.Select(x => CurveGrade(x, curve)).ToList();
        }
    }
}
