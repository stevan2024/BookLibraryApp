using Books.Models;
using Microsoft.EntityFrameworkCore;

namespace Books.Database
{
    public class BooksDataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BooksDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<BooksDBContext>>()))
            {
                // Look for any books.
                if (context.Books.Any())
                {
                    return;   // Data was already seeded
                }

                context.Books.AddRange(
                    new Book
                    {
                        ISBN = 9780671612979,
                        Title = "Ben Hogan's Five Lessons: The Modern Fundamentals of Golf",
                        Authors = "Ben Hogan; Herbert Warren Wind",
                        Publisher = "Touchstone"

                    },
                    new Book
                    {
                        ISBN = 9781401309619,
                        Title = "The Match: The Day the Game of Golf Changed Forever",
                        Authors = "Mark Frost",
                        Publisher = "Hachette Books"

                    }

                    );

                context.SaveChanges();
            }
        }
    }
}
