using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace FPTBookStore.Controllers
{
    public class BooksController : Controller
    {
        private readonly FPTBookContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BooksController(FPTBookContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Books
        [Route("admin/Books")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Index(int pg=1)
        {
            List<Book> books = await _context.Books.ToListAsync();
            const int pageSize = 5;
            if (pg < 1)
                pg = 1;
            int recsCount = books.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = books.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Genres = await _context.Genre.ToListAsync();
            ViewBag.Pager = pager;
            return View("Index",data);
        }
        [Route("Shop")]
        public async Task<IActionResult> Shop(int pg = 1)
        {
            List<Book> books = await _context.Books.ToListAsync();
            const int pageSize = 9;
            if (pg < 1)
                pg = 1;
            int recsCount = books.Count();
            var pager = new Pager(recsCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = books.Skip(recSkip).Take(pager.PageSize).ToList();
            ViewBag.Genres = await _context.Genre.ToListAsync();
            ViewBag.Pager = pager;
            return View("Shop",data);
        }
        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
           
            return View(book);
        }
        public async Task<IActionResult> Single(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Genres = await _context.Genre.ToListAsync();
            return View("Single",book);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Genres = await _context.Genre.ToListAsync();
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( AddedBook model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);
                Book book = new Book
                {
                    BookName = model.BookName,
                    BookDescription = model.BookDescription,
                    BookAuthor = model.BookAuthor,
                    BookPrice = model.BookPrice,
                    BookImage = uniqueFileName,
                    BookSells = 0,
                    BookStock = 0,
                    GenreId = model.GenreId
                };
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            AddedBook editBook = new AddedBook
            {
                BookName = book.BookName,
                BookAuthor = book.BookAuthor,
                BookId = book.BookId,
                BookPrice = book.BookPrice,
                BookDescription=book.BookDescription,
                GenreId=book.GenreId,
                BookSells=book.BookSells,
                BookStock = book.BookStock,
            };
            ViewBag.Genres = await _context.Genre.ToListAsync();
            return View(editBook);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AddedBook model)
        {

            if (ModelState.IsValid)
            {
              
                try
                {
                    var book = await _context.Books.FindAsync(model.BookId);
                    if (book == null)
                    {
                        return NotFound();
                    }
                    book.BookName = model.BookName;
                    book.BookDescription = model.BookDescription;
                    book.BookAuthor = model.BookAuthor;
                    book.BookPrice = model.BookPrice;
                    book.GenreId = model.GenreId;
                    book.BookStock = model.BookStock;
                    book.BookSells = model.BookSells;
                    if (model.BookImage != null)
                    {
                        string uniqueFileName = UploadedFile(model);
                        book.BookImage = uniqueFileName;
                    }
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(model.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'FPTBookContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return _context.Books.Any(e => e.BookId == id);
        }
        private string UploadedFile(AddedBook model)
        {
            string uniqueFileName = null;

            if (model.BookImage != null)
            {
                string path = webHostEnvironment.WebRootPath;
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.BookImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.BookImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
