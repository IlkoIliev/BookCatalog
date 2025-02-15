using System.ComponentModel.DataAnnotations;

namespace BookCatalog.Models.BookModels
{
    public class CreateBookViewModel
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }

        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; } // За качване на файла
    }
}
