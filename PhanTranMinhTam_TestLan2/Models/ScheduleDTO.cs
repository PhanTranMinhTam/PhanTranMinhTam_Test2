namespace PhanTranMinhTam_TestLan2.Models
{
    public class ScheduleDTO
    {
        public int MusicId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
