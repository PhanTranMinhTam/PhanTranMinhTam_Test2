using Microsoft.EntityFrameworkCore.Storage;
using PhanTranMinhTam_TestLan2.Data;

namespace PhanTranMinhTam_TestLan2.Reponsitory
{
    public interface IMusicRepository : IRepositoryBase<Music> { }
    public interface IMusicGenerRepository : IRepositoryBase<MusicGenre> { }
    public interface IScheduleRepository : IRepositoryBase<Schedule> { }
    public interface IRepositoryWrapper
    {
        IMusicRepository Music { get; }
        IMusicGenerRepository MusicGener { get; }
        IScheduleRepository Schedule { get; }
        void Save();
        Task SaveAsync();
        IDbContextTransaction Transaction();
    }
    public class MusicRepository : ReponsitoryBase<Music>, IMusicRepository
    {
        public MusicRepository(MyDbContext context) : base(context) { }
    }
    public class MusicGenerRepository : ReponsitoryBase<MusicGenre>, IMusicGenerRepository
    {
        public MusicGenerRepository(MyDbContext context) : base(context) { }
    }
    public class ScheduleRepository : ReponsitoryBase<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(MyDbContext context) : base(context) { }
    }
    public class ReponsitoryWapper : IRepositoryWrapper
    {
        private IMusicRepository music;
        private IMusicGenerRepository musicgener;
        private IScheduleRepository schedule;
        private readonly MyDbContext context;

        public ReponsitoryWapper(MyDbContext context)
        {
            this.context = context;
        }
        public IMusicRepository Music => music ??= new MusicRepository(context);
        public IMusicGenerRepository MusicGener => musicgener ??= new MusicGenerRepository(context);
        public IScheduleRepository Schedule => schedule ??= new ScheduleRepository(context);
        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public IDbContextTransaction Transaction()
        {
            return context.Database.BeginTransaction();
        }
    }
}
