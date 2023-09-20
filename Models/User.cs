namespace ReviewApp.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    
    public ICollection<Review> Reviews { get; set; } 
    public ICollection<Comment> Comments { get; set; } 
    public ICollection<Like> Likes { get; set; } 

    
    public ICollection<UserTag> UserTags { get; set; }
}