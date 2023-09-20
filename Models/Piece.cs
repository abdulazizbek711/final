namespace ReviewApp.Models;

public class Piece
{
    public int PieceId { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }

    
    public ICollection<Review> Reviews { get; set; }
}