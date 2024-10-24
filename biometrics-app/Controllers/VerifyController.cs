using biometrics_app.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace biometrics_app.Controllers
{
    public class VerifyController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IFingerprintService _fingerprintservice;
        public VerifyController(IUserRepository userRepository, IFingerprintService fingerprintService)
        {
            _userRepository = userRepository;
            _fingerprintservice = fingerprintService;
        }
        
    }
}
