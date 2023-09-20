namespace ReviewApp.Dto;

public class ReviewDto
{
    public int ReviewId { get; set; }
    public string ReviewName { get; set;}
    public string Title { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public int UserId { get; set; }
    public int PieceId { get; set; }
}