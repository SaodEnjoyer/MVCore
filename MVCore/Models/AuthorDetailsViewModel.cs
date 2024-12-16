

namespace Model
{
    public class AuthorDetailsViewModel
    {
        public required string AuthorName { get; set; }        
        public required int AuthorId { get; set; }
        public required List<Song> Songs { get; set; }
    }
}
