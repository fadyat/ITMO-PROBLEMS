using System.Collections.Generic;
using Isu.Classes;

namespace Isu.Interfaces
{
    public interface IIsuService
    {
        Group AddGroup(GroupName name);

        Student AddStudent(Group group, string name);

        Student GetStudent(uint id);

        Group GetGroup(GroupName groupName);

        Student FindStudent(string name);

        List<Student> FindStudents(GroupName groupName);

        List<Student> FindStudents(uint courseNumber);

        Group FindGroup(GroupName groupName);

        List<Group> FindGroups(uint courseNumber);

        Student ChangeStudentGroup(Student student, Group newGroup);
    }
}