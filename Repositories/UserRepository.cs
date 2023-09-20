using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Repositories;

namespace ReviewApp.Repositories;

public class UserRepository: IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public ICollection<User> GetUsers()
    {
        return _context.Users.ToList();
    }

    public User? GetUser(int UserId)
    {
        return _context.Users.Where(user => user.UserId == UserId).Include(e => e.Reviews).FirstOrDefault();
    }

    public ICollection<Review> GetReviewsByUser(int UserId)
    {
        return _context.Reviews.Where(u => u.UserId == UserId).ToList();
    }

    public ICollection<Comment> GetCommentsByUser(int UserId)
    {
        return _context.Comments.Where(c => c.UserId == UserId).ToList();
    }

    public ICollection<Like> GetLikesByUser(int LikeId)
    {
        return _context.Likes.Where(l => l.LikeId == LikeId).ToList();
    }


    public bool UserExists(int UserId)
    {
        return _context.Users.Any(u => u.UserId == UserId);
    }

    public bool CreateUser(User user)
    {
        _context.Add(user);
        return Save();
    }

    public bool UpdateUser(User user)
    {
        _context.Update(user);
        return Save();
    }

    public bool DeleteUser(User user)
    {
        _context.Remove(user);
        return Save();
    }


    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}