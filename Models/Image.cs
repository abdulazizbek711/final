namespace ReviewApp.Models;

public class Image
{
    public int ImageId { get; set; }
    public string Url { get; set; }

    
    public int ReviewId { get; set; }

    
    public Review Review { get; set; }
}