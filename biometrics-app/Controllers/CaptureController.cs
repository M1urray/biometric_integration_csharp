using Microsoft.AspNetCore.Mvc;
using DPUruNet;
using biometrics_app.Interfaces;
namespace biometrics_app.Controllers
{
    [Route("api/fingerprint")]
    [ApiController]
    public class CaptureController : Controller
    {
        private readonly IUserRepository _userRepository;
        // ReSharper disable once IdentifierTypo
        private readonly IFingerprintService _fingerprintservice;

        // ReSharper disable once ConvertToPrimaryConstructor
        public CaptureController(IUserRepository userRepository, IFingerprintService fingerprintService)
        {
            _userRepository = userRepository;
            _fingerprintservice = fingerprintService;
        }
        [HttpPost("capture")]
        public async Task<IActionResult> CaptureFingerprint(int userId){
            var fingerprint = await _fingerprintservice.CaptureFingerprintAsync(userId);
            var isSaved = await _userRepository.SaveFingerprintDataAsync(userId, fingerprint);
            if(isSaved){
                return Ok("Fingerprint captured successfully!");
            }
            return BadRequest("Fingerprint Not Saved");
        }
    }
}
