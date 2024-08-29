using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhanTranMinhTam_TestLan2.Data
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        public int MusicId { get; set; }
        public Music Music { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DayOfWeek DayOfWeek { get; set; } // Thứ trong tuần (e.g., Monday, Tuesday)

        public DateTime StartDate { get; set; } // Ngày bắt đầu áp dụng lịch

        public DateTime EndDate { get; set; } // Ngày kết thúc áp dụng lịch
    }
}
