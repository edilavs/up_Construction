using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpConstruction.DAL;

namespace UpConstruction.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Constructions.ToList();
            return View(model);
        }
    }
}
