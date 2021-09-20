using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Exceptions;

namespace Isu.Classes
{
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups;
        private uint _spareId;
        public IsuService() { (_groups, _spareId) = (new List<Group>(), 100000); }
        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name);
            if (_groups.Contains(newGroup))
                throw new IsuException("Group is already exists!");

            _groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, _spareId++) { Group = group };
            int groupIndex = _groups.IndexOf(group);
            if (groupIndex < 0)
                throw new IsuException("Can't find group for student!");

            if (_groups[groupIndex].Capacity >= _groups[groupIndex].MaxCapacity)
                throw new IsuException("Can't add student, full group!");

            _groups[groupIndex].StudentList.Add(newStudent);
            _groups[groupIndex].Capacity++;
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            foreach (Student student in _groups.SelectMany(grp => grp.StudentList.Where(student => student.Id == id)))
                return student;

            throw new IsuException("Can't get student!");
        }

        public Student FindStudent(string name)
        {
            return _groups.SelectMany(grp => grp.StudentList.Where(student => student.Name == name)).FirstOrDefault();
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return (from grp in _groups where grp.Name == groupName select grp.StudentList).FirstOrDefault();
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var tmp = new List<Student>();
            foreach (Group grp in _groups.Where(grp => grp.Name.Course == courseNumber))
                tmp.AddRange(grp.StudentList);

            return tmp;
        }

        public Group FindGroup(GroupName groupName)
        {
            return _groups.FirstOrDefault(grp => grp.Name == groupName);
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Where(grp => grp.Name.Course == courseNumber).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student.Group == newGroup)
                throw new Exception("Student is already in this group!");

            int groupIndex = _groups.IndexOf(student.Group);
            if (groupIndex < 0)
                throw new IsuException($"Last group doesn't exist!");

            _groups[groupIndex].StudentList.Remove(student);
            _groups[groupIndex].Capacity--;

            groupIndex = _groups.IndexOf(newGroup);
            if (groupIndex < 0)
                throw new IsuException($"New group doesn't exist!");

            if (_groups[groupIndex].Capacity >= _groups[groupIndex].MaxCapacity)
                throw new IsuException("Can't add student, to new full group!");

            student.Group = newGroup;
            _groups[groupIndex].StudentList.Add(student);
            _groups[groupIndex].Capacity++;
        }

        public override string ToString()
        {
            return _groups.Aggregate(string.Empty, (current1, group) =>
                    group.StudentList.Select(student =>
                    student.ToString()).Aggregate(current1, (current, tmp) =>
                    current + tmp + "\n"));
        }
    }
}