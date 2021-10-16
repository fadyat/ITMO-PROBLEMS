using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Isu.Exceptions;
using Isu.Interfaces;

namespace Isu.Classes
{
    public class IsuService : IIsuService
    {
        private ImmutableDictionary<Group, uint> _registeredGroups;
        private ImmutableList<Student> _totalStudents;
        private uint _sparedStudentId;

        public IsuService()
        {
            _registeredGroups = ImmutableDictionary<Group, uint>.Empty;
            _totalStudents = ImmutableList<Student>.Empty;
            _sparedStudentId = 100000;
        }

        protected IReadOnlyDictionary<Group, uint> RegisteredGroups => _registeredGroups;
        protected IEnumerable<Student> TotalStudents => _totalStudents;

        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name);
            if (_registeredGroups.ContainsKey(newGroup))
                throw new IsuException("Group is already exists!");

            _registeredGroups = _registeredGroups.Add(newGroup, 0);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (!_registeredGroups.ContainsKey(group))
                throw new IsuException("Can't find group for student!");

            if (_registeredGroups[group] >= group.MaxCapacity)
                throw new IsuException("Can't add student, full group!");

            var newStudent = new Student(name, _sparedStudentId++, group);
            uint newCapacity = _registeredGroups[group] + 1;
            _registeredGroups = _registeredGroups.Remove(group);
            _registeredGroups = _registeredGroups.Add(group, newCapacity);
            _totalStudents = _totalStudents.Add(newStudent);
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
            return _totalStudents.FirstOrDefault(student =>
                Equals(student.Name, name));
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _totalStudents.Where(student =>
                Equals(student.Group.Name, groupName)).ToList();
        }

        public List<Student> FindStudents(uint courseNumber)
        {
            return _totalStudents.Where(student =>
                Equals(student.Group.Name.Course, courseNumber)).ToList();
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
            ChangeStudentGroupCheck(student, newGroup);
            _totalStudents = _totalStudents.Remove(student);
            _totalStudents = _totalStudents.Add(new Student(student, newGroup));
            uint newCapacity = _registeredGroups[newGroup] + 1;
            uint prevCapacity = _registeredGroups[student.Group] - 1;
            _registeredGroups = _registeredGroups.Remove(student.Group);
            _registeredGroups = _registeredGroups.Add(student.Group, prevCapacity);
            _registeredGroups = _registeredGroups.Remove(newGroup);
            _registeredGroups = _registeredGroups.Add(newGroup, newCapacity);
        }

        public override string ToString()
        {
            return _totalStudents.Aggregate(string.Empty, (current, student) =>
                current + (student.ToString() + '\n'));
        }

        private void ChangeStudentGroupCheck(Student student, Group newGroup)
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
        }
    }
}