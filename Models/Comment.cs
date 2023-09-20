namespace ReviewApp.Models;

public class Comment
{
    public int CommentId { get; set; }
    public string Text { get; set; }

    
    public int UserId { get; set; }
    public int ReviewId { get; set; }

    
    public User User { get; set; } 
    public Review Review { get; set; } 
}