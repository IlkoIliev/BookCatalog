namespace BookCatalog.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public ICollection<BookTag> BookTags { get; set; }
    }
}
