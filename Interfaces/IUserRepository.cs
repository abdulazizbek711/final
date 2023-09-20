using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User? GetUser(int UserId);
    ICollection<Review> GetReviewsByUser(int UserId);
    ICollection<Comment> GetCommentsByUser(int UserId);
    ICollection<Like> GetLikesByUser(int LikeId);
    bool UserExists(int UserId);
    bool CreateUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(User user);

    bool Save();

}