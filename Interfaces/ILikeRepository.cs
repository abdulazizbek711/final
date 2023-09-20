using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface ILikeRepository
{
    ICollection<Like> GetLikes();
    Like GetLike(int LikeId);
    bool LikeExists(int LikeId);
    bool CreateLike(Like like);
    bool UpdateLike(Like like);
    bool DeleteLike(Like like);

    bool Save();
}