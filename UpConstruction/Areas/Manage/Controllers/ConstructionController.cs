using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UpConstruction.DAL;
using UpConstruction.Helpers;
using UpConstruction.Models;

namespace UpConstruction.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ConstructionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ConstructionController(AppDbContext context,IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }
        public IActionResult Index()
        {
            var model = _context.Constructions.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Construction construction)
        {
            if (construction.ImageFile!=null)
            {
                if (construction.ImageFile.ContentType != "image/png" && construction.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File format must be image/png or image/jpeg");
                    
                }
                if (construction.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be max 2MB");
                    
                }
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Image is required!");
            }
           

            if (!ModelState.IsValid)
            {
                return View();
            }
            
            construction.Image = FileManager.Save(_env.WebRootPath,"uploads/constructions",construction.ImageFile);
           
            _context.Constructions.Add(construction);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
         public IActionResult Edit(int id)
        {
            var construc = _context.Constructions.FirstOrDefault(x => x.Id == id);

            if (construc == null)
            {
                return RedirectToAction("error", "dashboard");
            }

            return View(construc);
        }

        [HttpPost]
        public IActionResult Edit(Construction construction)
        {
            var existCons = _context.Constructions.FirstOrDefault(x => x.Id == construction.Id);
           

            if (construction == null)
            {
                return RedirectToAction("error", "dashboard");
            }

            if (construction.ImageFile != null)
            {
                if (construction.ImageFile.ContentType!="image/png" && construction.ImageFile.ContentType!="image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File type must be image/png or image/jpeg!");
                }
                if (construction.ImageFile.Length>2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be less than 2MB!");
                }
                if (!ModelState.IsValid)
                   return View();

                //var guid = Guid.NewGuid().ToString();
                //var newFileName = guid + construction.ImageFile.FileName;
                //var path = Path.Combine(_env.WebRootPath, "uploads/constructions", newFileName);

                //using (FileStream stream= new FileStream(path,FileMode.Create))
                //{
                //    construction.ImageFile.CopyTo(stream);
                //}

                //existCons.Image = newFileName;

                FileManager.Delete(_env.WebRootPath, "uploads/constructions", existCons.Image);
                existCons.Image = FileManager.Save(_env.WebRootPath, "uploads/constructions", construction.ImageFile);
                
            }

            existCons.Title = construction.Title;
            existCons.Desc = construction.Desc;
           


            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public IActionResult Delete(int id)
        {
            var construction = _context.Constructions.FirstOrDefault(x => x.Id == id);
            if (construction==null)
            {
                return RedirectToAction("error", "dashboard");
            }
            return View(construction);
        }
        [HttpPost]
        public IActionResult Delete(Construction construction)
        {
            var existConstruction = _context.Constructions.FirstOrDefault(x => x.Id == construction.Id);

            if (existConstruction == null)
            {
                return RedirectToAction("error", "dashboard");
            }

            FileManager.Delete(_env.WebRootPath, "uploads/constructions", existConstruction.Image);

            _context.Constructions.Remove(construction);
            _context.SaveChanges();
            return RedirectToAction("index");
        }





    }
}
