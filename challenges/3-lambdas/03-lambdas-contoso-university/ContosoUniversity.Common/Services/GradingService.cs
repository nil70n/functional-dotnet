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

        public static List<Enrollment> CurveGrades(List<Enrollment> currentEnrollments) {
            // Remember to maintain immutability and functional purity
            // TODO: complete this function
        }
    }
}
