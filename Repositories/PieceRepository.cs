using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repositories;

public class PieceRepository: IPieceRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PieceRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Piece> GetPieces()
    {
        return _context.Pieces.OrderBy(p => p.PieceId).ToList();
    }

    public Piece GetPiece(int PieceId)
    {
        return _context.Pieces.Where(p => p.PieceId == PieceId).FirstOrDefault();
    }

    public bool PieceExists(int PieceId)
    {
        return _context.Pieces.Any(p => p.PieceId == PieceId);
    }
    public Piece GetPieceById(int pieceId)
    {
        return _context.Pieces.FirstOrDefault(p => p.PieceId == pieceId);
    }

    public bool CreatePiece(Piece piece)
    {
        _context.Add(piece);
        return Save();
    }

    public bool UpdatePiece(Piece piece)
    {
        _context.Update(piece);
        return Save();
    }

    public bool DeletePiece(Piece piece)
    {
        _context.Remove(piece);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}