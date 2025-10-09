namespace PayEase_CaseStudy.DTOs
{
    public class LeaveWithEmployeeDTO
    {
        public int LeaveId { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
    }
}
