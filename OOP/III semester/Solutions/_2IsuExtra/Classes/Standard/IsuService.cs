using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Exceptions;
using IsuExtra.Interfaces;

namespace IsuExtra.Classes.Standard
{
    public class IsuService : IIsuService
    {
        private Dictionary<Group, uint> _registeredGroups;
        private List<Student> _totalStudents;
        private uint _spareId;

        public IsuService()
        {
            _registeredGroups = new Dictionary<Group, uint>();
            _totalStudents = new List<Student>();
            _spareId = 100000;
        }

        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name);
            if (_registeredGroups.ContainsKey(newGroup))
                throw new IsuException("Group is already exists!");

            _registeredGroups = new Dictionary<Group, uint>(_registeredGroups)
            {
                { newGroup, 0 },
            };
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, _spareId++, group);
            if (!_registeredGroups.ContainsKey(group))
                throw new IsuException("Can't find group for student!");

            uint prevCapacity = _registeredGroups[group];
            if (prevCapacity >= group.MaxCapacity)
                throw new IsuException("Can't add student, full group!");

            _registeredGroups.Remove(group);
            _registeredGroups = new Dictionary<Group, uint>(_registeredGroups)
            {
                { group, ++prevCapacity },
            };
            _totalStudents = new List<Student>(_totalStudents)
            {
                newStudent,
            };
            return newStudent;
        }

        public Student GetStudent(uint id)
        {
            foreach (Student student in _totalStudents.Where(student => Equals(student.Id, id)))
                return student;

            throw new IsuException("Can't get student!");
        }

        public Student FindStudent(string name)
        {
            return _totalStudents.FirstOrDefault(student => Equals(student.Name, name));
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _totalStudents.Where(student => Equals(student.Group.Name, groupName)).ToList();
        }

        public List<Student> FindStudents(uint courseNumber)
        {
            return _totalStudents.Where(student => Equals(student.Group.Name.Course, courseNumber)).ToList();
        }

        public Group FindGroup(GroupName groupName)
        {
            return _registeredGroups.Keys.FirstOrDefault(group =>
                Equals(group.Name, groupName));
        }

        public List<Group> FindGroups(uint courseNumber)
        {
            return _registeredGroups.Keys.Where(group =>
                Equals(group.Name.Course, courseNumber)).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (!_totalStudents.Contains(student))
                throw new IsuException("This student doesn't exist!");

            if (!_registeredGroups.ContainsKey(student.Group))
                throw new IsuException("Previous group doesn't exist!");

            if (!_registeredGroups.ContainsKey(newGroup))
                throw new IsuException("New group doesn't exist!");

            if (Equals(student.Group, newGroup))
                throw new Exception("Student is already in this group!");

            if (_registeredGroups[newGroup] >= newGroup.MaxCapacity)
                throw new IsuException("Can't add student, to new full group!");

            _totalStudents.Remove(student);
            _totalStudents = new List<Student>(_totalStudents)
            {
                new Student(student.Name, student.Id, new Group(newGroup.Name)),
            };
            uint newCapacity = _registeredGroups[newGroup] + 1;
            uint prevCapacity = Math.Max(_registeredGroups[student.Group] - 1, 0);
            _registeredGroups.Remove(student.Group);
            _registeredGroups.Remove(newGroup);
            _registeredGroups = new Dictionary<Group, uint>(_registeredGroups)
            {
                { student.Group, prevCapacity },
                { newGroup, newCapacity },
            };
        }

        public override string ToString()
        {
            return _totalStudents.Aggregate(string.Empty, (current, student) =>
                current + (student.ToString() + '\n'));
        }
    }
}