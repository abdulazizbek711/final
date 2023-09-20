namespace ReviewApp.Models;

public class Tag
{
    public int TagId { get; set; }
    public string Name { get; set; }

  
    public ICollection<ReviewTag> ReviewTags { get; set; } 
    public ICollection<UserTag> UserTags { get; set; }
}