using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReviewApp.Data;
using ReviewApp.Dto;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController: Controller
{
    private readonly IUserRepository _userRepository;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, DataContext context, IMapper mapper)
    {
        _userRepository = userRepository;
        _context = context;
        _mapper = mapper;
    }
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
    public IActionResult GetUsers()
    {
        var Users = _userRepository.GetUsers();
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(Users);
    }
    [HttpGet("{UserId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public IActionResult GetUser(int UserId)
    {
        if (!_userRepository.UserExists(UserId))
            return NotFound();
        var User = _mapper.Map<UserDto>(_userRepository.GetUser(UserId));
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        return Ok(User);
    }
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public IActionResult CreateUser([FromBody] UserDto userCreate)
    {
        if (userCreate == null)
            return BadRequest(ModelState);

        var user = _userRepository.GetUsers()
            .Where(c => c.UserName.Trim().ToUpper() == userCreate.UserName.TrimEnd().ToUpper())
            .FirstOrDefault();

        if (user != null)
        {
            ModelState.AddModelError("", "User already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userMap = _mapper.Map<User>(userCreate);

        if (!_userRepository.CreateUser(userMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }
    [HttpPut("{userId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]

    public IActionResult UpdateUser(int userId, [FromBody] UserDto updatedUser)
    {
        if (updatedUser == null)
            return BadRequest(ModelState);
        if (userId != updatedUser.UserId)
            return BadRequest(ModelState);
        if (!_userRepository.UserExists(userId))
            return NotFound();
        if (!ModelState.IsValid)
            return BadRequest();
        var userMap = _mapper.Map<User>(updatedUser);
        if (!_userRepository.UpdateUser(userMap))
        {
            ModelState.AddModelError("", "Something went wrong updating user");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }
    [HttpDelete("{userId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult DeleteUser(int userId)
    {
        if (!_userRepository.UserExists(userId))
            return NotFound();
        var userToDelete = _userRepository.GetUser(userId);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (!_userRepository.DeleteUser(userToDelete))
        {
            ModelState.AddModelError("", "Something went wrong deleting user");
        }

        return NoContent();

    }
    
}