using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Queue_Management_System.Services;
using Queue_Management_System.Models;
using System.Security.Claims;

namespace Queue_Management_System.Controllers
{
    public class QueueController : Controller
    {
        private readonly IQueueRepository _queueRepository;
        private ClaimsIdentity _identity;
        public QueueController(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }
        private int? GetServicePointId()
        {
            _identity = new ClaimsIdentity(User.Claims);
            var userServingPointId = _identity.HasClaim(claim => claim.Type == "ServicePointId")
               ? _identity.Claims.FirstOrDefault(claim => claim.Type == "ServicePointId").Value
               : null;
            return Convert.ToInt32(userServingPointId);
        }

        [HttpGet]
        public async Task<IActionResult> CheckinPage()
        {
            var services = await _queueRepository.GetServices();
            return View(services);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddCustomerToQueue(ServicePointVM servicePointId)
        {
            await _queueRepository.AddCustomerToQueue(servicePointId);
            return RedirectToAction(nameof(CheckinPage));
        }

        // GET: Queue/WaitingPage
        [HttpGet]
        public async Task<IActionResult> WaitingPage()
        {
            var calledCustomers = await _queueRepository.GetCalledCustomers();
            return View(calledCustomers);
        }

        // GET: Queue/ServicePoint
        [Authorize(Policy = "Service Provider"), HttpGet]
        public async Task<IActionResult> ServicePoint()
        {
            int? servicePointId = GetServicePointId();

            if (servicePointId != null)
            {
                var waitingCustomers = await _queueRepository.GetWaitingCustomers((int)servicePointId);
                QueueVM currentServingCustomerDetails = await _queueRepository.MyCurrentServingCustomer((int)servicePointId);
                var services = await _queueRepository.GetServices();

                QueueVMList queueList = new QueueVMList()
                {
                    WaitingCustomers = waitingCustomers,
                    MyCurrentServingCustomerDetails = currentServingCustomerDetails,
                    Services = services,
                };
                return View(queueList);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> GetNextNumber(int id) //outgoingCustomerId
        {
            int? servicePointId = GetServicePointId();

            if (servicePointId != null)
            {
                QueueVM incomingCustomerDetails = await _queueRepository.UpdateOutGoingAndIncomingCustomerStatus(id, (int)servicePointId);
                if (incomingCustomerDetails == null)
                {
                    return RedirectToAction(nameof(ServicePoint));
                }
                TempData["AlertMessage"] = $"Queue Id Number {incomingCustomerDetails.Id} Called successfully";
                return RedirectToAction(nameof(ServicePoint));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> RecallNumber()
        {
            int? servicePointId = GetServicePointId();

            if (servicePointId != null)
            {
                QueueVM currentlyCalledCustomerDetails = await _queueRepository.MyCurrentServingCustomer((int)servicePointId);
                if (currentlyCalledCustomerDetails == null)
                {
                    TempData["AlertMessage"] = $"Error encountered while Recalling Queue Id Number";
                    return RedirectToAction(nameof(ServicePoint));
                }
                TempData["AlertMessage"] = $"Queue Id Number {currentlyCalledCustomerDetails.Id} ReCalled successfully";
                return RedirectToAction(nameof(ServicePoint));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> MarkNumberASNoShow(int id) //outgoingCustomerId
        {
            int? servicePointId = GetServicePointId();

            if (servicePointId != null)
            {
                await _queueRepository.MarkNumberASNoShow(id,(int)servicePointId);
                TempData["AlertMessage"] = "Queue Id Number Marked as NoShow successfully";
                return RedirectToAction(nameof(ServicePoint));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> MarkNumberASFinished(int id) //outgoingCustomerId
        {
            int? servicePointId = GetServicePointId();

            if (servicePointId != null)
            {
                await _queueRepository.MarkNumberASFinished(id,(int)servicePointId);
                TempData["AlertMessage"] = "Queue Id Number Marked as Finished successfully";
                return RedirectToAction(nameof(ServicePoint));
            }
            return NotFound();
        }

        // POST: Queue/TransferNumber
        [HttpPost]
        public async Task<ActionResult> TransferNumber(QueueVMList room)
        {
            int? currentServicePointId = GetServicePointId();

            if (currentServicePointId != null)
            {
                int servicePointIdTranser = room.MyCurrentServingCustomerDetails.ServicePointId;

                await _queueRepository.TransferNumber((int)currentServicePointId, servicePointIdTranser);
                TempData["AlertMessage"] = "Queue Id Number Transfered successfully";
                return RedirectToAction(nameof(ServicePoint));
            }
            return NotFound();
        }
    }
}
