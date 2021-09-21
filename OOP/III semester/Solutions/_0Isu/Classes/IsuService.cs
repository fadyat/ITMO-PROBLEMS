using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Exceptions;

namespace Isu.Classes
{
    public class IsuService : IIsuService
    {
        private readonly HashSet<Group> _registeredGroups;
        private readonly List<Student> _totalStudents;
        private uint _spareId;

        public IsuService()
        {
            _registeredGroups = new HashSet<Group>();
            _totalStudents = new List<Student>();
            _spareId = 100000;
        }

        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name);
            if (_registeredGroups.Contains(newGroup))
                throw new IsuException("Group is already exists!");

            _registeredGroups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, _spareId++) { Group = group };
            if (!_registeredGroups.Contains(group))
                throw new IsuException("Can't find group for student!");

            if (group.Capacity >= group.MaxCapacity)
                throw new IsuException("Can't add student, full group!");

            group.Capacity++;
            _totalStudents.Add(newStudent);
            return newStudent;
        }

        public Student GetStudent(uint id)
        {
            foreach (Student student in _totalStudents.Where(student => student.Id == id))
                return student;

            throw new IsuException("Can't get student!");
        }

        public Student FindStudent(string name)
        {
            return _totalStudents.FirstOrDefault(student => student.Name == name);
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _totalStudents.Where(student => student.Group.Name == groupName).ToList();
        }

        public List<Student> FindStudents(uint courseNumber)
        {
            return _totalStudents.Where(student => student.Group.Name.Course == courseNumber).ToList();
        }

        public Group FindGroup(GroupName groupName)
        {
            return _registeredGroups.FirstOrDefault(group => group.Name == groupName);
        }

        public List<Group> FindGroups(uint courseNumber)
        {
            return _registeredGroups.Where(group => group.Name.Course == courseNumber).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student.Group == newGroup)
                throw new Exception("Student is already in this group!");

            if (!_registeredGroups.Contains(student.Group))
                throw new IsuException($"Last group doesn't exist!");

            student.Group.Capacity--;

            if (!_registeredGroups.Contains(newGroup))
                throw new IsuException($"New group doesn't exist!");

            if (newGroup.Capacity >= newGroup.MaxCapacity)
                throw new IsuException("Can't add student, to new full group!");

            student.Group = newGroup;
            newGroup.Capacity++;
        }

        public override string ToString()
        {
            return _totalStudents.Aggregate(string.Empty, (current, student) =>
                current + (student.ToString() + '\n'));
        }
    }
}