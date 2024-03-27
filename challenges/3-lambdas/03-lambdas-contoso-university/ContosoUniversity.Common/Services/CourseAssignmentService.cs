using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContosoUniversity.Data.Entities;

namespace ContosoUniversity.Common.Services {
    public static class CourseAssignmentService {
        public static List<Student> ReassignStudentsInCourse(List<Student> enrolledStudents, int courseId, int newCourseId, Course newCourse) {
            // --- Maintain Immutability ---
            var newStudentEnrollments = new List<Student>();
            foreach (var enrolledStudent in enrolledStudents) {
                var notCurrentCourseEnrollment = enrolledStudent.Enrollments.Where(x => x.CourseID != courseId).ToList();
                var currentEnrollments = enrolledStudent.Enrollments.Where(x => x.CourseID == courseId).ToList();

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
                        CourseID = newCourseId,
                        Grade = null,
                        StudentID = x.StudentID
                    })
                );

                newStudentEnrollments.Add(new Student() {
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

            return newStudentEnrollments;
        }
    }
}
