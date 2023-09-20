using ReviewApp.Data;
using ReviewApp.Models;
using ReviewApp.Interfaces;

namespace ReviewApp.Repositories;

public class ReviewRepository: IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }
  
    public ICollection<Review> GetReviews()
    {
        return _context.Reviews.OrderBy(r => r.UserId).ToList();
    }

    public Review GetReview(int reviewId)
    {
        return _context.Reviews.Where(r => r.ReviewId == reviewId).FirstOrDefault();
    }

    public bool ReviewExists(int reviewId)
    {
        return _context.Reviews.Any(r => r.ReviewId == reviewId);
    }

    public bool CreateReview(Review review)
    {
        _context.Add(review);
        return Save();
    }

    public bool UpdateReview(Review review)
    {
        _context.Update(review);
        return Save();
    }

    public bool DeleteReview(Review review)
    {
        _context.Remove(review);
        return Save();
    }


    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}