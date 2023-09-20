using ReviewApp.Models;

namespace ReviewApp.Interfaces;

public interface ITagRepository
{
    ICollection<Tag> GetTags();
    Tag GetTag(int TagId);
    bool TagExists(int TagId);
    bool CreateTag(Tag tag);
    bool UpdateTag(Tag tag);
    bool DeleteTag(Tag tag);
        

    bool Save();
}