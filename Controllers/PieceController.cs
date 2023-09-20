using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PieceController : Controller
{
    private readonly IPieceRepository _pieceRepository;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PieceController(IPieceRepository pieceRepository, DataContext context, IMapper mapper)
    {
        _pieceRepository = pieceRepository;
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Piece>))]
    public IActionResult GetComments()
    {
        var pieces = _pieceRepository.GetPieces();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(pieces);
    }
    [HttpGet("{pieceId}")]
    [ProducesResponseType(200, Type = typeof(Piece))]
    [ProducesResponseType(400)]
    public IActionResult GetPiece(int pieceId)
    {
        if (!_pieceRepository.PieceExists(pieceId))
            return NotFound();
        var comment = _mapper.Map<PieceDto>(_pieceRepository.GetPiece(pieceId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(comment);
    }
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreatePiece([FromQuery] int PieceId, [FromBody] PieceDto pieceCreate) 
    {
        if (pieceCreate == null)
            return BadRequest(ModelState);

        // Check if the review name already exists
        var piece = _pieceRepository.GetPieces()
            .Where(c => c.Name.Trim().ToUpper() == pieceCreate.Name.TrimEnd().ToUpper())
            .FirstOrDefault();
        if (piece != null)
        {
            ModelState.AddModelError("", "Piece already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if the Piece with the provided PieceId exists
        var Piece = _pieceRepository.GetPieceById(pieceCreate.PieceId);
        if (Piece == null)
        {
            ModelState.AddModelError("PieceId", "Piece not found");
            return BadRequest(ModelState);
        }

        var pieceMap = _mapper.Map<Piece>(pieceCreate);

        

        if (!_pieceRepository.CreatePiece(pieceMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Created");
    }
    [HttpPut("{pieceId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]

    public IActionResult UpdatePiece(int pieceId, [FromBody] PieceDto updatedPiece)
    {
        if (updatedPiece == null)
            return BadRequest(ModelState);
        if (pieceId != updatedPiece.PieceId)
            return BadRequest(ModelState);
        if (!_pieceRepository.PieceExists(pieceId))
            return NotFound();
        if (!ModelState.IsValid)
            return BadRequest();
        var pieceMap = _mapper.Map<Piece>(updatedPiece);
        if (!_pieceRepository.UpdatePiece(pieceMap))
        {
            ModelState.AddModelError("", "Something went wrong updating piece");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    [HttpDelete("{pieceId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeletePiece(int pieceId)
    {
        if (!_pieceRepository.PieceExists(pieceId))
            return NotFound();
        var pieceToDelete = _pieceRepository.GetPiece(pieceId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!_pieceRepository.DeletePiece(pieceToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting piece");
        }

        return NoContent();

    }
}