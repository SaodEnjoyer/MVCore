using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using MVCore.Models;

public class MusicController : Controller
{
    private readonly MusicContext _context;

    public MusicController(MusicContext context)
    {
        _context = context;
    }

    [HttpGet("author/details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var author = await _context.Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorDetailsViewModel
            {
                AuthorName = a.Name,
                AuthorId = a.Id,
                Songs = _context.Songs.Where(s => s.AuthorId == id).ToList()
            })
            .FirstOrDefaultAsync();

        if (author == null)
        {
            return NotFound(new { Message = "Автор не найден" });
        }

        return View("Author/Details", author);
    }

    [HttpGet("authors")]
    public async Task<IActionResult> GetAllAuthors()
    {
        var authors = await _context.Authors.ToListAsync();
        if (authors == null || !authors.Any())
            return NotFound(new { Message = "Авторы не найдены." });

        return View("AuthorsList", authors);
    }

    [HttpGet("author/{id}")]
    public async Task<IActionResult> GetAuthorById(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null) return NotFound(new { Message = $"Автор с ID {id} не найден." });

        return View("AuthorDetails", author);
    }

    [HttpPost("author")]
    public async Task<IActionResult> CreateAuthor([FromBody] Author author)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
    }

    [HttpPut("author/{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author updatedAuthor)
    {
        if (id != updatedAuthor.Id) return BadRequest(new { Message = "ID не совпадает." });

        _context.Entry(updatedAuthor).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Authors.Any(a => a.Id == id))
                return NotFound(new { Message = $"Автор с ID {id} не найден." });
            throw;
        }

        return Ok(updatedAuthor);
    }

    [HttpDelete("author/{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null) return NotFound(new { Message = $"Автор с ID {id} не найден." });

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("author/{authorId}/songs")]
    public async Task<IActionResult> GetSongsByAuthor(int authorId)
    {
        var songs = await _context.Songs.Where(s => s.AuthorId == authorId).ToListAsync();
        if (!songs.Any())
        {
            return NotFound(new { Message = "Нет песен у этого автора." });
        }

        return View("SongsList", songs);
    }
}
