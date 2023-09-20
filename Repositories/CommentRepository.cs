using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repositories;

public class CommentRepository: ICommentRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CommentRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Comment> GetComments()
    {
        return _context.Comments.OrderBy(c => c.CommentId).ToList();
    }

    public Comment GetComment(int CommentId)
    {
        return _context.Comments.Where(c => c.CommentId == CommentId).FirstOrDefault();
    }

    public bool CommentExists(int CommentId)
    {
        return _context.Comments.Any(c => c.CommentId == CommentId);
    }

    public bool CreateComment(Comment comment)
    {
        _context.Add(comment);
        return Save();
    }

    public bool UpdateComment(Comment comment)
    {
        _context.Update(comment);
        return Save();
    }

    public bool DeleteComment(Comment comment)
    {
        _context.Remove(comment);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}