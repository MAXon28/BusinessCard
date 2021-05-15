using Microsoft.AspNetCore.Mvc;

namespace BusinessCard.Controllers
{
    public class BusinessCardController : Controller
    {
        [HttpGet]
        public IActionResult Card()
        {
            //var fullName = new FullName { FirstName = "Максим", SecondName = "Безуглый" };
            return View();
        }
    }
}