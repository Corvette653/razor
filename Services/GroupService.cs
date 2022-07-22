using razor.Models;

namespace razor.Services;

public static class GroupService
{
    static List<Group> Groups { get; } = new List<Group>();
    static int nextId = 1;
    public static Group? Get(int id)
    {
        return Groups.FirstOrDefault(g => g.Id == id, null);
    }
    public static Group Add(Group newGroup)
    {
        newGroup.Id = nextId;
        nextId++;
        Groups.Add(newGroup);
        return newGroup;
    }
}