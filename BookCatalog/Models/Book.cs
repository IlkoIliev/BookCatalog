namespace BookCatalog.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public byte[] Image { get; set; }
        public ICollection<BookTag> BookTags { get; set; }
    }
}
