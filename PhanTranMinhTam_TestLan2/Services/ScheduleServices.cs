using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhanTranMinhTam_TestLan2.Data;
using PhanTranMinhTam_TestLan2.Models;
using PhanTranMinhTam_TestLan2.Reponsitory;

namespace PhanTranMinhTam_TestLan2.Services
{
    public interface IScheduleServices
    {
        Task AddOrUpdateScheduleAsync(ScheduleDTO scheduleDTO);
        Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
        Task<Schedule> GetScheduleByIdAsync(int id);
        Task<bool> DeleteScheduleAsync(int i);
    }
    public class ScheduleServices : IScheduleServices
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public ScheduleServices(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }
        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
        {
            // Lấy tất cả các quà từ repository
            IQueryable<Schedule> schedules = _repositoryWrapper.Schedule.FindAll();
            return await schedules.ToListAsync();
        }
        public async Task<Schedule> GetScheduleByIdAsync(int id)
        {
            // Lấy một nhạc theo ID từ repository
            Schedule? schedule = await _repositoryWrapper.Schedule.FindByCondition(g => g.ScheduleId == id).FirstOrDefaultAsync();
            return schedule;
        }
        public async Task AddOrUpdateScheduleAsync(ScheduleDTO scheduleDTO)
        {
            DateTime now = DateTime.Now;
            int currentDayOfWeek = (int)now.DayOfWeek;

            // Tìm lịch đã tồn tại
            Schedule? existingSchedule = await _repositoryWrapper.Schedule.FindByCondition(s =>
                s.MusicId == scheduleDTO.MusicId &&
                s.DayOfWeek == scheduleDTO.DayOfWeek &&
                s.StartDate <= now.Date &&
                s.EndDate >= now.Date
            ).FirstOrDefaultAsync();

            if (existingSchedule != null)
            {
                // Kiểm tra nếu thời gian hiện tại nằm trong khoảng thời gian của lịch đã tồn tại
                if (now.TimeOfDay >= existingSchedule.StartTime && now.TimeOfDay <= existingSchedule.EndTime)
                {
                    // Nếu lịch hiện tại đang được sử dụng, không cần thực hiện thay đổi
                    return;
                }
                else
                {
                    // Xử lý lịch mới nếu lịch đã tồn tại không phù hợp
                    DateTime nextWeekStart = now.AddDays(7 - (currentDayOfWeek - (int)scheduleDTO.DayOfWeek));
                    DateTime nextWeekEnd = nextWeekStart.AddDays(6);

                    Schedule newSchedule = new()
                    {
                        MusicId = scheduleDTO.MusicId,
                        DayOfWeek = scheduleDTO.DayOfWeek,
                        StartTime = scheduleDTO.StartTime,
                        EndTime = scheduleDTO.EndTime,
                        StartDate = nextWeekStart,
                        EndDate = nextWeekEnd
                    };

                    // Kiểm tra lịch trùng lặp
                    List<Schedule> conflictingSchedules = await _repositoryWrapper.Schedule.FindByCondition(s =>
                        s.DayOfWeek == newSchedule.DayOfWeek &&
                        s.StartDate <= newSchedule.EndDate &&
                        s.EndDate >= newSchedule.StartDate &&
                        (
                            (newSchedule.StartTime >= s.StartTime && newSchedule.StartTime < s.EndTime) ||
                            (newSchedule.EndTime > s.StartTime && newSchedule.EndTime <= s.EndTime) ||
                            (newSchedule.StartTime < s.StartTime && newSchedule.EndTime > s.EndTime)
                        )
                    ).ToListAsync();

                    if (conflictingSchedules.Any())
                    {
                        throw new InvalidOperationException("Lịch phát nhạc bị trùng lặp.");
                    }

                    _repositoryWrapper.Schedule.Create(newSchedule);
                    await _repositoryWrapper.SaveAsync();
                }
            }
            else
            {
                // Thêm lịch mới
                DateTime nextWeekStart = now.AddDays(7 - (currentDayOfWeek - (int)scheduleDTO.DayOfWeek));
                DateTime nextWeekEnd = nextWeekStart.AddDays(6);

                Schedule newSchedule = new()
                {
                    MusicId = scheduleDTO.MusicId,
                    DayOfWeek = scheduleDTO.DayOfWeek,
                    StartTime = scheduleDTO.StartTime,
                    EndTime = scheduleDTO.EndTime,
                    StartDate = nextWeekStart,
                    EndDate = nextWeekEnd
                };

                // Kiểm tra lịch trùng lặp
                List<Schedule> conflictingSchedules = await _repositoryWrapper.Schedule.FindByCondition(s =>
                    s.DayOfWeek == newSchedule.DayOfWeek &&
                    s.StartDate <= newSchedule.EndDate &&
                    s.EndDate >= newSchedule.StartDate &&
                    (
                        (newSchedule.StartTime >= s.StartTime && newSchedule.StartTime < s.EndTime) ||
                        (newSchedule.EndTime > s.StartTime && newSchedule.EndTime <= s.EndTime) ||
                        (newSchedule.StartTime < s.StartTime && newSchedule.EndTime > s.EndTime)
                    )
                ).ToListAsync();

                if (conflictingSchedules.Any())
                {
                    throw new InvalidOperationException("Lịch phát nhạc bị trùng lặp.");
                }

                _repositoryWrapper.Schedule.Create(newSchedule);
                await _repositoryWrapper.SaveAsync();
            }
        }
        public async Task<bool> DeleteScheduleAsync(int id)  // Triển khai phương thức xóa
        {
            Schedule? existingSchedule = await _repositoryWrapper.Schedule.FindByCondition(g => g.ScheduleId == id).FirstOrDefaultAsync();

            if (existingSchedule == null)
            {
                return false; // Không tìm thấy đối tượng để xóa
            }
            _repositoryWrapper.Schedule.Delete(existingSchedule);

            await _repositoryWrapper.SaveAsync();

            return true; // Xóa thành công
        }
    }
}
