using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PhanTranMinhTam_TestLan2.Data;
using PhanTranMinhTam_TestLan2.Models;
using PhanTranMinhTam_TestLan2.Reponsitory;

namespace PhanTranMinhTam_TestLan2.Services
{
    public interface IMusicServices
    {
        Task AddMusic(MusicDTO musicDto);
        Task<IEnumerable<Music>> GetAllMusicsAsync();
        Task<Music> GetMusicByIdAsync(int id);
        Task<Music> UpdateMusicAsync(int id, UpdateMusicDTO musicDTO);
        Task<bool> DeleteMusicAsync(int id);
    }
    public class MusicServices : IMusicServices
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public MusicServices(IMapper mapper, IRepositoryWrapper repositoryWrapper)
        {
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }
        //Xem tat ca danh sách 
        public async Task<IEnumerable<Music>> GetAllMusicsAsync()
        {
            // Lấy tất cả các quà từ repository
            IQueryable<Music> musics = _repositoryWrapper.Music.FindAll();
            return await musics.ToListAsync();
        }
        //Xem 1 danh sách
        public async Task<Music> GetMusicByIdAsync(int id)
        {
            // Lấy một nhạc theo ID từ repository
            Music? music = await _repositoryWrapper.Music.FindByCondition(g => g.MusicId == id).FirstOrDefaultAsync();
            return music;
        }
        //Add music
        public async Task AddMusic(MusicDTO musicDto)
        {
            Music music = _mapper.Map<Music>(musicDto); // Ánh xạ từ MusicDTO sang Music

            if (musicDto.Image != null && musicDto.Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(musicDto.Image.FileName); // Lấy tên tệp từ Image

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Đảm bảo thư mục tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Lưu ảnh vào thư mục
                using (FileStream fileStream = new(filePath, FileMode.Create))
                {
                    await musicDto.Image.CopyToAsync(fileStream);
                }

                music.Image = uniqueFileName; // Lưu tên tệp ảnh vào thuộc tính Image của Music
            }

            _repositoryWrapper.Music.Create(music);  // Lưu Music vào repository
            await _repositoryWrapper.SaveAsync();   // Lưu thay đổi vào cơ sở dữ liệu
        }
        //Update Music 
        public async Task<Music> UpdateMusicAsync(int id, UpdateMusicDTO musicDTO)
        {
            // Tìm kiếm Music bằng ID
            Music? existingMusic = await _repositoryWrapper.Music.FindByCondition(g => g.MusicId == id).FirstOrDefaultAsync();
            if (existingMusic == null)
            {
                return null;
            }

            // Ánh xạ các thuộc tính từ DTO sang đối tượng Music hiện có
            _mapper.Map(musicDTO, existingMusic);

            if (!string.IsNullOrEmpty(musicDTO.Image))
            {
                // Đường dẫn tới thư mục chứa hình ảnh
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                // Tạo tên file mới để đảm bảo tính duy nhất
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + musicDTO.Image;
                // Đường dẫn đầy đủ tới file mới
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Kiểm tra và tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Xóa hình ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(existingMusic.Image))
                {
                    string oldFilePath = Path.Combine(uploadsFolder, existingMusic.Image);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                // Lưu hình ảnh mới vào thư mục
                using (FileStream fileStream = new(filePath, FileMode.Create))
                {
                }

                // Cập nhật tên file hình ảnh mới vào đối tượng Music
                existingMusic.Image = uniqueFileName;
            }
            // Cập nhật đối tượng Music trong DbContext
            _repositoryWrapper.Music.Update(existingMusic);

            // Lưu các thay đổi vào cơ sở dữ liệu
            await _repositoryWrapper.SaveAsync();

            return existingMusic;
        }
        public async Task<bool> DeleteMusicAsync(int id)  // Triển khai phương thức xóa
        {
            Music? existingGift = await _repositoryWrapper.Music.FindByCondition(g => g.MusicId == id).FirstOrDefaultAsync();

            if (existingGift == null)
            {
                return false; // Không tìm thấy đối tượng để xóa
            }

            _repositoryWrapper.Music.Delete(existingGift);

            await _repositoryWrapper.SaveAsync();

            return true; // Xóa thành công
        }
    }
}
