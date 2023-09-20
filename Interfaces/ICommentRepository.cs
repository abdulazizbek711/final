using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface ICommentRepository
{
    ICollection<Comment> GetComments();
    Comment GetComment(int CommentId);
    bool CommentExists(int CommentId);
    bool CreateComment(Comment comment);
    bool UpdateComment(Comment comment);
    bool DeleteComment(Comment comment);
        

    bool Save();
}