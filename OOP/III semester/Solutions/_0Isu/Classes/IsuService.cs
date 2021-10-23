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
        private ImmutableDictionary<Group, uint> _groups;
        private ImmutableList<Student> _students;
        private uint _issuedStudentId;

        public IsuService()
        {
            _groups = ImmutableDictionary<Group, uint>.Empty;
            _students = ImmutableList<Student>.Empty;
            _issuedStudentId = 100000;
        }

        public IReadOnlyDictionary<Group, uint> Groups => _groups;
        protected IEnumerable<Student> Students => _students;

        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name);
            if (_groups.ContainsKey(newGroup))
                throw new IsuException("Group is already exists!");

            _groups = _groups.Add(newGroup, 0);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            if (!_groups.ContainsKey(group))
                throw new IsuException("Can't find group for student!");

            if (_groups[group] >= group.MaxCapacity)
                throw new IsuException("Can't add student, full group!");

            uint newCapacity = _groups[group] + 1;
            _groups = _groups.Remove(group);
            _groups = _groups.Add(group, newCapacity);
            var newStudent = new Student(name, _issuedStudentId++, group);
            _students = _students.Add(newStudent);
            return newStudent;
        }

        public Student GetStudent(uint id)
        {
            foreach (Student student in _students
                .Where(student => Equals(student.Id, id)))
                return student;

            throw new IsuException("Can't get student!");
        }

        public Student FindStudent(string name)
        {
            return _students
                .FirstOrDefault(student => Equals(student.Name, name));
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _students
                .Where(student => Equals(student.Group.Name, groupName))
                .ToList();
        }

        public List<Student> FindStudents(uint courseNumber)
        {
            return _students
                .Where(student => Equals(student.Group.Name.Course, courseNumber))
                .ToList();
        }

        public Group FindGroup(GroupName groupName)
        {
            return _groups.Keys
                .FirstOrDefault(group => Equals(group.Name, groupName));
        }

        public List<Group> FindGroups(uint courseNumber)
        {
            return _groups.Keys
                .Where(group => Equals(group.Name.Course, courseNumber))
                .ToList();
        }

        public void ChangeStudentGroup(ref Student student, Group newGroup)
        {
            ChangeStudentGroupCheck(student, newGroup);

            uint prevCapacity = _groups[student.Group] - 1;
            _groups = _groups.Remove(student.Group);
            _groups = _groups.Add(student.Group, prevCapacity);

            uint newCapacity = _groups[newGroup] + 1;
            _groups = _groups.Remove(newGroup);
            _groups = _groups.Add(newGroup, newCapacity);

            _students = _students.Remove(student);
            student = new Student(student, newGroup);
            _students = _students.Add(student);
        }

        public override string ToString()
        {
            return _students.Aggregate(string.Empty, (current, student) =>
                current + (student.ToString() + '\n'));
        }

        private void ChangeStudentGroupCheck(Student student, Group newGroup)
        {
            if (!_students.Contains(student))
                throw new IsuException("This student doesn't exist!");

            if (!_groups.ContainsKey(student.Group))
                throw new IsuException("Previous group doesn't exist!");

            if (!_groups.ContainsKey(newGroup))
                throw new IsuException("New group doesn't exist!");

            if (Equals(student.Group, newGroup))
                throw new Exception("Student is already in this group!");

            if (_groups[newGroup] >= newGroup.MaxCapacity)
                throw new IsuException("Can't add student, to new full group!");
        }
    }
}