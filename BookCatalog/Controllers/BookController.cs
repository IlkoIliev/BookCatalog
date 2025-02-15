using BookCatalog.Data;
using BookCatalog.Models;
using BookCatalog.Models.BookModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.Controllers
{
    public class BookController : Controller
    {
        private readonly BookDbContext _dbContext;

        public BookController(BookDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var books = await _dbContext.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .ToListAsync();
            return View(books);
        }

        // 2. Форма за създаване на нова книга
        [HttpGet]
        public IActionResult Create()
        
        {
            ViewData["AuthorId"] = new SelectList(_dbContext.Authors, "AuthorId", "Name");
            ViewData["GenreId"] = new SelectList(_dbContext.Genres, "GenreId", "Name");
            return View();
        }

        // 3. Добавяне на нова книга
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var book = new Book
                {
                    Title = model.Title,
                    AuthorId = model.AuthorId,
                    GenreId = model.GenreId
                };

                if (model.ImageFile != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(ms);
                        book.Image = ms.ToArray();
                    }
                }

                _dbContext.Books.Add(book);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Authors = new SelectList(_dbContext.Authors, "AuthorId", "Name", model.AuthorId);
            ViewBag.Genres = new SelectList(_dbContext.Genres, "GenreId", "Name", model.GenreId);
            return View(model);
        }

        // 4. Форма за редактиране на книга
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null) return NotFound();

            var model = new EditBookViewModel
            {
                BookId = book.BookId,
                Title = book.Title,
                AuthorId = book.AuthorId,
                GenreId = book.GenreId,
                Image = book.Image
            };

            ViewBag.AuthorId = new SelectList(_dbContext.Authors, "AuthorId", "Name", book.AuthorId);
            ViewBag.GenreId = new SelectList(_dbContext.Genres, "GenreId", "Name", book.GenreId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                var book = await _dbContext.Books.FindAsync(model.BookId);
                if (book == null) return NotFound();

                // Обновяване на информацията за книгата
                book.Title = model.Title;
                book.AuthorId = model.AuthorId;
                book.GenreId = model.GenreId;

                // Ако е избрано ново изображение
                if (model.ImageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(memoryStream);
                        book.Image = memoryStream.ToArray(); // Записваме изображението като byte[]
                    }
                }

                // Записваме промените в базата данни
                _dbContext.Books.Update(book);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Authors"] = _dbContext.Authors.ToList();
            ViewData["Genres"] = _dbContext.Genres.ToList();
            return View(model);
        }
    }
}
