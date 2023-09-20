using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LikeController: Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public LikeController(IReviewRepository reviewRepository, ILikeRepository likeRepository, DataContext context, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _likeRepository = likeRepository;
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Like>))]
    public IActionResult GetLikes()
    {
        var likes = _likeRepository.GetLikes();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(likes);
    }
    [HttpGet("{likeId}")]
    [ProducesResponseType(200, Type = typeof(Like))]
    [ProducesResponseType(400)]
    public IActionResult GetLike(int likeId)
    {
        if (!_likeRepository.LikeExists(likeId))
            return NotFound();
        var like = _mapper.Map<LikeDto>(_likeRepository.GetLike(likeId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(like);
    }
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreatePiece([FromBody] LikeDto likeCreate) 
    {
        if (likeCreate == null)
            return BadRequest(ModelState);

        // Check if the like already exists based on some criteria (e.g., UserId and ReviewId)
        var existingLike = _likeRepository.GetLikes()
            .FirstOrDefault(c => c.UserId == likeCreate.UserId && c.ReviewId == likeCreate.ReviewId);

        if (existingLike != null)
        {
            ModelState.AddModelError("", "Like already exists");
            return StatusCode(422, ModelState);
        }

        // Check if the Review with the provided ReviewId exists
        var review = _reviewRepository.GetReview(likeCreate.ReviewId);
        if (review == null)
        {
            ModelState.AddModelError("", "Review not found");
            return StatusCode(404, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var likeMap = _mapper.Map<Like>(likeCreate);

        if (!_likeRepository.CreateLike(likeMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Created");
    }
    [HttpPut("{likeId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]

    public IActionResult UpdateLike(int likeId, [FromBody] LikeDto updatedLike)
    {
        if (updatedLike == null)
            return BadRequest(ModelState);
        if (likeId != updatedLike.LikeId)
            return BadRequest(ModelState);
        if (!_reviewRepository.ReviewExists(likeId))
            return NotFound();
        if (!ModelState.IsValid)
            return BadRequest();
        var likeMap = _mapper.Map<Like>(updatedLike);
        if (!_likeRepository.UpdateLike(likeMap))
        {
            ModelState.AddModelError("", "Something went wrong updating like");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    [HttpDelete("{likeId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteLike(int likeId)
    {
        if (!_likeRepository.LikeExists(likeId))
            return NotFound();
        var likeToDelete = _likeRepository.GetLike(likeId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!_likeRepository.DeleteLike(likeToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting like");
        }

        return NoContent();

    }


}