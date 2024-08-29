using PhanTranMinhTam_TestLan2.Data;
using PhanTranMinhTam_TestLan2.Models;

namespace PhanTranMinhTam_TestLan2.Mappings
{
    public class MappingMusic : AutoMapper.Profile
    {
        public MappingMusic()
        {
            CreateMap<MusicDTO, Music>();
            CreateMap<Music, MusicDTO>();
            CreateMap<UpdateMusicDTO, Music>();
            CreateMap<Music, UpdateMusicDTO>();
        }
    }
}
