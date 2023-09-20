using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TagController: Controller
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public TagController(ITagRepository tagRepository, IMapper mapper, DataContext context)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
        _context = context;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
    public IActionResult GetTags()
    {
        var tags = _tagRepository.GetTags();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(tags);
    }
    [HttpGet("{tagId}")]
    [ProducesResponseType(200, Type = typeof(Tag))]
    [ProducesResponseType(400)]
    public IActionResult GetTag(int TagId)
    {
        if (!_tagRepository.TagExists(TagId))
            return NotFound();
        var tag = _mapper.Map<TagDto>(_tagRepository.GetTag(TagId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(tag);
    }
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreatePiece([FromQuery] int tagId, [FromBody] TagDto tagCreate) 
    {
        if (tagCreate == null)
            return BadRequest(ModelState);

        // Check if the review name already exists
        var tag = _tagRepository.GetTags()
            .Where(c => c.Name.Trim().ToUpper() == tagCreate.Name.TrimEnd().ToUpper())
            .FirstOrDefault();
        if (tag != null)
        {
            ModelState.AddModelError("", "Tag already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Check if the Piece with the provided PieceId exists
       

        var tagMap = _mapper.Map<Tag>(tagCreate);

        

        if (!_tagRepository.CreateTag(tagMap))
        {
            ModelState.AddModelError("", "Something went wrong while saving");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully Created");
    }
    [HttpPut("{tagId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]

    public IActionResult UpdateTag(int tagId, [FromBody] TagDto updatedTag)
    {
        if (updatedTag == null)
            return BadRequest(ModelState);
        if (tagId != updatedTag.TagId)
            return BadRequest(ModelState);
        if (!_tagRepository.TagExists(tagId))
            return NotFound();
        if (!ModelState.IsValid)
            return BadRequest();
        var tagMap = _mapper.Map<Tag>(updatedTag);
        if (!_tagRepository.UpdateTag(tagMap))
        {
            ModelState.AddModelError("", "Something went wrong updating tag");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    [HttpDelete("{tagId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteTag(int tagId)
    {
        if (!_tagRepository.TagExists(tagId))
            return NotFound();
        var tagToDelete = _tagRepository.GetTag(tagId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!_tagRepository.DeleteTag(tagToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting tag");
        }

        return NoContent();

    }




}
