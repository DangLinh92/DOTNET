using BookMan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookMan.Controllers
{
    public class BookController : Controller
    {
        private readonly Service _service;

        public BookController(Service service)
        {
            _service = service;
        }

        public IActionResult Index(int page = 1)
        {
            var model = _service.Paging(page);
            ViewData["Pages"] = model.pages;
            ViewData["Page"] = model.page;
            return View(model.books);
        }

        public IActionResult Details(int id)
        {
            var b = _service.Get(id);
            if (b == null)
                return NotFound();
            return View(b);
        }

        public IActionResult Delete(int id)
        {
            var b = _service.Get(id);
            if (b == null)
            {
                return NotFound();
            }
            return View(b);
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            _service.Delete(book.Id);
            _service.SaveChange();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            return View(b);
        }

        [HttpPost]
        public IActionResult Edit(Book book, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _service.Upload(book, file);
                _service.Update(book);
                _service.SaveChange();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public IActionResult Create()
        {
            return View(_service.Create());
        }

        [HttpPost]
        public IActionResult Create(Book book, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _service.Upload(book, file);
                _service.Add(book);
                _service.SaveChange();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        public IActionResult Read(int id)
        {
            var b = _service.Get(id);
            if (b == null) return NotFound();
            if (!System.IO.File.Exists(_service.GetPathData(b.DataFile)))
            {
                return NotFound();
            }

            var (stream, type) = _service.Download(b);
            return File(stream, type, b.DataFile);
        }

        public IActionResult Search(string search)
        {
            return View("Index", _service.Get(search));
        }
    }
}