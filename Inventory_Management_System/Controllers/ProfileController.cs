using Inventory_Management_System.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Inventory_Management_System.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepo;


        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepo = profileRepository;
        }




          public async Task <IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var items =  await _profileRepo.GetOwnItems(userId);

            return View(items);
        }





    }
}
