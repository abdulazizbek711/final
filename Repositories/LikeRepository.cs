using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repositories;

public class LikeRepository: ILikeRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public LikeRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Like> GetLikes()
    {
        return _context.Likes.OrderBy(l => l.LikeId).ToList();
    }

    public Like GetLike(int LikeId)
    {
        return _context.Likes.Where(l => l.LikeId == LikeId).FirstOrDefault();
    }

    public bool LikeExists(int LikeId)
    {
        return _context.Likes.Any(l => l.LikeId == LikeId);
    }

    public bool CreateLike(Like like)
    {
        _context.Add(like);
        return Save();
    }

    public bool UpdateLike(Like like)
    {
        _context.Update(like);
        return Save();
    }

    public bool DeleteLike(Like like)
    {
        _context.Remove(like);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}