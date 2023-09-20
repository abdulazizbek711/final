using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;
using ReviewApp.Dto;



namespace ReviewApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReviewController : Controller
{
    private readonly IPieceRepository _pieceRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserRepository _userRepository;

    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ReviewController(IPieceRepository pieceRepository, IReviewRepository reviewRepository,IUserRepository userRepository,  DataContext context, IMapper mapper)
    {
        _pieceRepository = pieceRepository;
        _reviewRepository = reviewRepository;
        _userRepository = userRepository;

        _context = context;
        _mapper = mapper;
    } 
   

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    public IActionResult GetReviews()
    {
        var reviews = _reviewRepository.GetReviews();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(reviews);
    }
    [HttpGet("{reviewId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetReview(int reviewId)
    {
        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();
        var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(reviewId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(review);
    }
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateReview([FromQuery] int UserId, [FromBody] ReviewDto reviewCreate) 
    {
        if (reviewCreate == null)
            return BadRequest(ModelState);

        // Check if the review name already exists
        var review = _reviewRepository.GetReviews()
            .Where(c => c.ReviewName.Trim().ToUpper() == reviewCreate.ReviewName.TrimEnd().ToUpper())
            .FirstOrDefault();
        if (review != null)
        {
            ModelState.AddModelError("", "Review already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if the Piece with the provided PieceId exists
        var piece = _pieceRepository.GetPieceById(reviewCreate.PieceId);
        if (piece == null)
        {
            ModelState.AddModelError("PieceId", "Piece not found");
            return BadRequest(ModelState);
        }

        var reviewMap = _mapper.Map<Review>(reviewCreate);

        reviewMap.User = _userRepository.GetUser(UserId);
        reviewMap.PieceId = piece.PieceId; // Set the PieceId

        if (!_reviewRepository.CreateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Created");
    }
    [HttpPut("{reviewId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]

    public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
    {
        if (updatedReview == null)
            return BadRequest(ModelState);
        if (reviewId != updatedReview.UserId)
            return BadRequest(ModelState);
        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();
        if (!ModelState.IsValid)
            return BadRequest();
        var reviewMap = _mapper.Map<Review>(updatedReview);
        if (!_reviewRepository.UpdateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong updating review");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    [HttpDelete("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteReview(int reviewId)
    {
        if (!_reviewRepository.ReviewExists(reviewId))
            return NotFound();
        var reviewToDelete = _reviewRepository.GetReview(reviewId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!_reviewRepository.DeleteReview(reviewToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting review");
        }

        return NoContent();

    }




    
}
