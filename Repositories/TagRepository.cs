using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repositories;

public class TagRepository: ITagRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public TagRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ICollection<Tag> GetTags()
    {
        return _context.Tags.OrderBy(t => t.TagId).ToList();
    }

    public Tag GetTag(int TagId)
    {
        return _context.Tags.Where(t => t.TagId == TagId).FirstOrDefault();
    }

    public bool TagExists(int TagId)
    {
        return _context.Tags.Any(t => t.TagId == TagId);
    }

    public bool CreateTag(Tag tag)
    {
        _context.Add(tag);
        return Save();
    }

    public bool UpdateTag(Tag tag)
    {
        _context.Update(tag);
        return Save();
    }

    public bool DeleteTag(Tag tag)
    {
        _context.Remove(tag);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}