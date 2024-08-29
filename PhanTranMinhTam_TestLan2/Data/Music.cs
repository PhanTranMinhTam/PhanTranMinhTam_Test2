using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhanTranMinhTam_TestLan2.Data
{
    public class Music
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MusicId { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Image { get; set; }
        public string Compose { get; set; }
        public string Format { get; set; }
        public int GenreId { get; set; }

        [ForeignKey("GenreId")]
        public MusicGenre MusicGenre { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
