using Microsoft.AspNetCore.Mvc;
using PhanTranMinhTam_TestLan2.Models;
using PhanTranMinhTam_TestLan2.Services;

namespace PhanTranMinhTam_TestLan2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly IMusicServices _musicServices;

        public MusicController(IMusicServices musicServices)
        {
            _musicServices = musicServices;

        }
        [HttpGet]
        public async Task<IActionResult> GetAllMusics()
        {
            IEnumerable<Data.Music> musics = await _musicServices.GetAllMusicsAsync();
            return Ok(musics);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicById(int id)
        {
            Data.Music music = await _musicServices.GetMusicByIdAsync(id);
            if (music == null)
            {
                return BadRequest();
            }
            return Ok(music);
        }
        [HttpPost]
        public async Task<IActionResult> CreateMusic([FromForm] MusicDTO musicDto)
        {
            if (ModelState.IsValid)
            {
                await _musicServices.AddMusic(musicDto);
                return Ok();
            }

            return BadRequest(ModelState);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMusic(int id, [FromBody] UpdateMusicDTO musicDto)
        {
            if (ModelState.IsValid)
            {
                Data.Music updatedGift = await _musicServices.UpdateMusicAsync(id, musicDto);
                if (updatedGift == null)
                {
                    return BadRequest();
                }

                return Ok(updatedGift);
            }

            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            bool result = await _musicServices.DeleteMusicAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(result);

        }
    }
}
