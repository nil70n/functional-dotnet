using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Data.Entities
{
    public class Enrollment : BaseEntity
    {
        public int CourseID { get; set; }
        public int StudentID { get; set; }

        /// <summary>
        /// On a 100 point scale
        /// </summary>
        [DisplayFormat(NullDisplayText = "No Grade")]
        public int? Grade { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
