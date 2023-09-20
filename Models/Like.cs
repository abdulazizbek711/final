namespace ReviewApp.Models;

public class Like
{
    public int LikeId { get; set; }

    
    public int UserId { get; set; }
    public int ReviewId { get; set; }

    
    public User User { get; set; } 
    public Review Review { get; set; } 
}