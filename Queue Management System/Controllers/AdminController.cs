using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queue_Management_System.Services;
using Queue_Management_System.Models;

namespace Queue_Management_System.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/ViewServiceProviders
        public async Task<ActionResult> ViewServiceProviders()
        {
            var serviceProviders = await _adminRepository.GetServiceProviders();
            return View(serviceProviders);
        }

        // GET: Admin/ViewServiceProviderDetails/5
        public async Task<ActionResult> ViewServiceProviderDetails(int id)
        {
            ServiceProviderVM serviceProviderDetails = await _adminRepository.GetServiceProviderDetails(id);
            if (serviceProviderDetails != null)            
                return View(serviceProviderDetails);            
            return NotFound();
        }

        // GET: Admin/CreateServiceProvider
        public async Task<ActionResult> CreateServiceProvider()
        {
            return View();
        }

        // POST: Admin/CreateServiceProvider
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateServiceProvider(ServiceProviderVM serviceProvider)
        {
            await _adminRepository.CreateServiceProvider(serviceProvider);
            return RedirectToAction(nameof(ViewServiceProviders));
        }

        // GET: Admin/EditServiceProvider/5
        public async Task<ActionResult> EditServiceProvider(int id)
        {
            ServiceProviderVM serviceProviderDetails = await _adminRepository.GetServiceProviderDetails(id);
            if (serviceProviderDetails != null)
            {
                return View(serviceProviderDetails);
            }
               return NotFound();
        }

        // POST: Admin/EditServiceProvider/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditServiceProvider(ServiceProviderVM serviceProvider)
        {
            await _adminRepository.UpdateServiceProvider(serviceProvider);
            return RedirectToAction(nameof(ViewServiceProviderDetails), new { id = serviceProvider.Id });
        }

        // POST: Admin/DeleteServiceProvider/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteServiceProvider(int id)
        {
            await _adminRepository.DeleteServiceProvider(id);
            return RedirectToAction(nameof(ViewServiceProviders));
        }

        // GET: Admin/ViewServicePoints
        public async Task<ActionResult> ViewServicePoints()
        {
            var servicePoints = await _adminRepository.GetServicePoints();
            if (servicePoints != null)
                return View(servicePoints);
            return NotFound();
        }

        // GET: Admin/ViewServicePointDetails/5
        public async Task<ActionResult> ViewServicePointDetails(int id)

        {
            ServicePointVM ServiceProviderDetails = await _adminRepository.GetServicePointDetails(id);
            if (ServiceProviderDetails != null)
                return View(ServiceProviderDetails);            
            return NotFound();
        }

        // GET: Admin/CreateServicePoint
        public async Task<ActionResult> CreateServicePoint()
        {
            return View();
        }

        // POST: Admin/CreateServicePoint
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateServicePoint(ServicePointVM servicePoint)
        {
            await _adminRepository.CreateServicePoint(servicePoint);
            return RedirectToAction(nameof(ViewServicePoints));
        }

        // GET: Admin/EditServicePoint/5
        public async Task<ActionResult> EditServicePoint(int id)
        {
            ServicePointVM servicePoint = await _adminRepository.GetServicePointDetails(id);
            if (servicePoint != null)
                return View(servicePoint);
            return NotFound();
        }

        // POST: Admin/EditServicePoint/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditServicePoint(ServicePointVM servicePoint)
        {
            await _adminRepository.UpdateServicePoint(servicePoint);
            return RedirectToAction(nameof(ViewServicePointDetails), new { id = servicePoint.Id });
        }

        // POST: Admin/DeleteServicePoint/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteServicePoint(int id)
        {
            await _adminRepository.DeleteServicePoint(id);
            return RedirectToAction(nameof(ViewServicePoints));
        }
        public IActionResult Reports()
        {
            return View();
        }
    }
}
