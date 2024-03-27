namespace ContosoUniversity.Data.DTO
{
    public class EnrollmentDTO
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public int? Grade { get; set; }
    }
}
