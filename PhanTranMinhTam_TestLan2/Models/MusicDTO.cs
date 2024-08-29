namespace PhanTranMinhTam_TestLan2.Models
{
    public class MusicDTO
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public IFormFile? Image { get; set; }
        public string Compose { get; set; }
        public string Format { get; set; }
        public int GenreId { get; set; }
    }
}
