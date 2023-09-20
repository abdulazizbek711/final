namespace ReviewApp.Models;

public class UserActions
{
    public int UserActionsId { get; set; }

    
    public int ReviewId { get; set; }
    public int CommentId { get; set; }
    public int UserId { get; set; }

    
    public Review Review { get; set; } 
    public Comment Comment { get; set; } 
    public User User { get; set; } 
}