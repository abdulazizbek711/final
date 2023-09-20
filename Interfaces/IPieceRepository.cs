using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface IPieceRepository
{
    ICollection<Piece> GetPieces();
    Piece GetPiece(int PieceId);
    bool PieceExists(int PieceId);
    Piece GetPieceById(int PieceId);
    bool CreatePiece(Piece piece);
    bool UpdatePiece(Piece piece);
    bool DeletePiece(Piece piece);

    bool Save();

}