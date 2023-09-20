namespace ReviewApp.Models;

public class Review
{
    public int ReviewId { get; set; }
    public string ReviewName { get; set;}
    public string Title { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }

    public User User { get; set; } 
    public int UserId { get; set; }
    public Piece Piece { get; set; }
    public int PieceId { get; set; }
   
   
    public ICollection<Comment> Comments { get; set; } 
    public ICollection<Like> Likes { get; set; } 
    public ICollection<ReviewTag> ReviewTags { get; set; } 
   
}