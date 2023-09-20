namespace ReviewApp.Models;

public class UserTag
{
    public int UserId { get; set; }
    public int TagId { get; set; }

    
    public User User { get; set; } 
    public Tag Tag { get; set; } 
}