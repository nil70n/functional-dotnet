using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContosoUniversity.Data.Entities;
using LanguageExt;

namespace ContosoUniversity.Common.Services {
    public static class CourseAssignmentService {
        public static Either<string, List<Student>> ReassignStudentsInCourse(List<Student> enrolledStudents, Course currentCourse, Course newCourse) {
            // TODO Refactor this function to use Either for error handling.
            // Make sure the CourseAssignmentServiceTests run to confirm that your code works.
            if (currentCourse.DepartmentID != newCourse.DepartmentID) {
                return "You cannot reassign students to courses in different departments.";
            }

            var newStudentEnrollments = new List<Student>();
            var errors = new StringBuilder();
            foreach (var student in enrolledStudents) {
                (string error, Student s) = ReassignStudentInCourse(student, currentCourse, newCourse);
                if (error.IsNull()) {
                    newStudentEnrollments.Add(s);
                }
                else {
                    errors.AppendLine(error);
                }
            }

            if (errors.Length > 0) {
                return errors.ToString();
            }

            return newStudentEnrollments;
        }

        private static (string, Student) ReassignStudentInCourse(Student enrolledStudent, Course currentCourse, Course newCourse) {
            // TODO Refactor this function to use Either for error handling.
            // Make sure the CourseAssignmentServiceTests run to confirm that your code works.
            if (enrolledStudent.Enrollments.Count == 0) {
                return ($"{enrolledStudent.FullName} has no current enrollments. Could not reassign.", new Student());
            }

            var notCurrentCourseEnrollment = enrolledStudent.Enrollments.Where(x => x.CourseID != currentCourse.ID).ToList();
            var currentEnrollments = enrolledStudent.Enrollments.Where(x => x.CourseID == currentCourse.ID).ToList();

            if (currentEnrollments.Count == 0) {
                return ($"{enrolledStudent.FullName} is not currently enrolled in {currentCourse.Title}. Could not reassign.", new Student());
            }

            var enrollmentsList = new List<Enrollment>();
            enrollmentsList.AddRange(notCurrentCourseEnrollment);
            enrollmentsList.AddRange(
                currentEnrollments.Select(x => new Enrollment() {
                    ID = x.ID,
                    AddedDate = x.AddedDate,
                    ModifiedDate = x.ModifiedDate,
                    RowVersion = x.RowVersion,
                    Student = x.Student,
                    Course = newCourse,
                    CourseID = newCourse.ID,
                    Grade = null,
                    StudentID = x.StudentID
                })
            ); 
            
            return ("", new Student() {
                AddedDate = enrolledStudent.AddedDate,
                EnrollmentDate = enrolledStudent.EnrollmentDate,
                FirstMidName = enrolledStudent.FirstMidName,
                ID = enrolledStudent.ID,
                LastName = enrolledStudent.LastName,
                ModifiedDate = enrolledStudent.ModifiedDate,
                RowVersion = enrolledStudent.RowVersion,
                Enrollments = enrollmentsList
            });
        }
    }
}
