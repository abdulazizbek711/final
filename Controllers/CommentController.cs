using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CommentController: Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CommentController(IReviewRepository reviewRepository, ICommentRepository commentRepository, DataContext context, IMapper mapper)
    {
        _reviewRepository = reviewRepository;
        _commentRepository = commentRepository;
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Comment>))]
    public IActionResult GetComments()
    {
        var comments = _commentRepository.GetComments();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(comments);
    }
    [HttpGet("{commentId}")]
    [ProducesResponseType(200, Type = typeof(Comment))]
    [ProducesResponseType(400)]
    public IActionResult GetComment(int commentId)
    {
        if (!_commentRepository.CommentExists(commentId))
            return NotFound();
        var comment = _mapper.Map<CommentDto>(_commentRepository.GetComment(commentId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(comment);
    }
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(CommentDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    [ProducesResponseType(500)]
    public IActionResult CreateComment(int reviewId, [FromBody] CommentDto commentCreate)
    {
        if (commentCreate == null)
        {
            return BadRequest(ModelState);
        }

        // Check if the specified reviewId exists
        var review = _reviewRepository.GetReview(reviewId);
        if (review == null)
        {
            ModelState.AddModelError("reviewId", "Review not found");
            return StatusCode(422, ModelState);
        }

        // Check if a comment with the same text already exists
        var existingComment = _commentRepository.GetComments()
            .FirstOrDefault(c => c.Text.Trim().ToUpper() == commentCreate.Text.TrimEnd().ToUpper());

        if (existingComment != null)
        {
            ModelState.AddModelError("", "Comment already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var commentMap = _mapper.Map<Comment>(commentCreate);

        // Set the ReviewId property based on the provided reviewId
        commentMap.ReviewId = reviewId;

        if (!_commentRepository.CreateComment(commentMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        // Return the created comment with a 201 status code
        var commentDto = _mapper.Map<CommentDto>(commentMap);
        return CreatedAtAction(nameof(GetComment), new { id = commentMap.CommentId }, commentDto);
    }
    [HttpPut("{commentId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]

    public IActionResult UpdateComment(int commentId, [FromBody] CommentDto updatedComment)
    {
        if (updatedComment == null)
            return BadRequest(ModelState);
        if (commentId != updatedComment.CommentId)
            return BadRequest(ModelState);
        if (!_commentRepository.CommentExists(commentId))
            return NotFound();
        if (!ModelState.IsValid)
            return BadRequest();
        var commentMap = _mapper.Map<Comment>(updatedComment);
        if (!_commentRepository.UpdateComment(commentMap))
        {
            ModelState.AddModelError("", "Something went wrong updating comment");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    [HttpDelete("{commentId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteComment(int commentId)
    {
        if (!_commentRepository.CommentExists(commentId))
            return NotFound();
        var commentToDelete = _commentRepository.GetComment(commentId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!_commentRepository.DeleteComment(commentToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting comment");
        }

        return NoContent();

    }




}