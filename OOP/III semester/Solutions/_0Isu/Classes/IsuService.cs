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
        private ImmutableList<Student> _students;
        private ImmutableList<Group> _groups;
        private uint _issuedStudentId;

        public IsuService()
        {
            _groups = ImmutableList<Group>.Empty;
            _students = ImmutableList<Student>.Empty;
            _issuedStudentId = 100000;
        }

        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name,  0);
            if (_groups.Contains(newGroup))
                throw new IsuException("Group is already exists!");

            _groups = _groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            group = GetGroup(group.Name);
            if (group.Capacity >= group.MaxCapacity)
                throw new IsuException("Can't add student, full group!");

            group = new Group(group, group.Capacity + 1);
            UpdateGroup(group);

            var newStudent = new Student(name, _issuedStudentId++, group.Name);
            _students = _students.Add(newStudent);
            return newStudent;
        }

        public Student GetStudent(uint id)
        {
            foreach (Student student in _students
                .Where(student => Equals(student.Id, id)))
            {
                return student;
            }

            throw new IsuException("Can't get student!");
        }

        public Group GetGroup(GroupName groupName)
        {
            foreach (Group group in _groups
                .Where(group => Equals(group.Name, groupName)))
            {
                return group;
            }

            throw new IsuException("Can't get group!");
        }

        public Student FindStudent(string name)
        {
            return _students
                .FirstOrDefault(student => Equals(student.Name, name));
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            return _students
                .Where(student => Equals(student.GroupName, groupName))
                .ToList();
        }

        public List<Student> FindStudents(uint courseNumber)
        {
            return _students
                .Where(student => Equals(student.GroupName.Course, courseNumber))
                .ToList();
        }

        public Group FindGroup(GroupName groupName)
        {
            return _groups
                .FirstOrDefault(group => Equals(group.Name, groupName));
        }

        public List<Group> FindGroups(uint courseNumber)
        {
            return _groups
                .Where(group => Equals(group.Name.Course, courseNumber))
                .ToList();
        }

        public Student ChangeStudentGroup(Student student, Group newGroup)
        {
            Group prevGroup = GetGroup(student.GroupName);
            newGroup = GetGroup(newGroup.Name);
            student = GetStudent(student.Id);

            if (Equals(prevGroup, newGroup))
                throw new Exception("Student is already in this group!");

            if (newGroup.Capacity >= newGroup.MaxCapacity)
                throw new IsuException("Can't add student, to new full group!");

            prevGroup = new Group(prevGroup, prevGroup.Capacity - 1);
            UpdateGroup(prevGroup);

            newGroup = new Group(newGroup, newGroup.Capacity + 1);
            UpdateGroup(newGroup);

            student = new Student(student, newGroup.Name);
            UpdateStudent(student);

            return student;
        }

        public void UpdateStudent(Student student)
        {
            Student prevStatus = GetStudent(student.Id);
            _students = _students.Remove(prevStatus);
            _students = _students.Add(student);
        }

        public void UpdateGroup(Group group)
        {
            Group prevStatus = GetGroup(group.Name);
            _groups = _groups.Remove(prevStatus);
            _groups = _groups.Add(group);
        }
    }
}