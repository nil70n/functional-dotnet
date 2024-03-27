using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContosoUniversity.Data.Entities;

namespace ContosoUniversity.Common.Services {
    public static class CourseAssignmentService {
        public static List<Student> ReassignStudentsInCourse(List<Student> enrolledStudents, Course oldCourse, Course newCourse) {
            // TODO Refactor this method to use a yield return
            // Make sure the CourseAssignmentTests run to confirm your code works.

            var newStudents = new List<Student>();
            foreach (var enrolledStudent in enrolledStudents) {
                var notCurrentCourseEnrollment = enrolledStudent.Enrollments.Where(x => x.CourseID != oldCourse.ID).ToList();
                var currentEnrollments = enrolledStudent.Enrollments.Where(x => x.CourseID == oldCourse.ID).ToList();

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

                newStudents.Add(new Student() {
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

            return newStudents;
        }
    }
}
