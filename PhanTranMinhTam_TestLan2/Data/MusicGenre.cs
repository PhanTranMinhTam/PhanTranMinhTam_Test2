using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhanTranMinhTam_TestLan2.Data
{
    public class MusicGenre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenreId { get; set; }

        public string GenreName { get; set; }

        public ICollection<Music> Musics { get; set; }
    }
}
