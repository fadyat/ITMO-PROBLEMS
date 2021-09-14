using System;
using System.Collections.Generic;
using Isu.Tools;

namespace Isu.Services
{
    /*
     isuService
     |
     |_ List<group> + sparedId
         |
         |_ group
            |
            |_ groupName  +  List<student>
               |             |
               |             |_ student
               |
               |_ tag + groupNumber + courseNumber
    */
    public class IsuService : IIsuService
    {
        private readonly List<Group> _groups;
        private uint _spareId;
        public IsuService() { (_groups, _spareId) = (new List<Group>(), 100000); }
        public Group AddGroup(GroupName name)
        {
            var newGroup = new Group(name);
            if (_groups.Contains(newGroup))
                throw new GroupException("Group is already exists!");

            _groups.Add(newGroup);
            return newGroup;
        }

        public Student AddStudent(Group group, string name)
        {
            var newStudent = new Student(name, _spareId++);
            int groupIndex = _groups.IndexOf(group);
            if (groupIndex < 0)
                throw new GroupException("Can't find group for student!");

            if (_groups[groupIndex].Capacity >= _groups[groupIndex].MaxCapacity)
                throw new GroupException("Can't add student, group is full!");

            _groups[groupIndex].StudentList.Add(newStudent);
            _groups[groupIndex].Capacity++;
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            foreach (Group grp in _groups)
            {
                foreach (Student student in grp.StudentList)
                {
                    if (student.Id == id) return student;
                }
            }

            throw new StudentException("Can't get student!");
        }

        public Student FindStudent(string name)
        {
            foreach (Group grp in _groups)
            {
                foreach (Student student in grp.StudentList)
                {
                    if (student.Name == name) return student;
                }
            }

            return null;
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            foreach (Group grp in _groups)
            {
                if (grp.Name == groupName)
                {
                    return grp.StudentList;
                }
            }

            return null;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            var tmp = new List<Student>();
            foreach (Group grp in _groups)
            {
                if (grp.Name.Course == courseNumber)
                {
                    foreach (Student std in grp.StudentList)
                        tmp.Add(std);
                }
            }

            return tmp;
        }

        public Group FindGroup(GroupName groupName)
        {
            foreach (Group grp in _groups)
            {
                if (grp.Name == groupName)
                    return grp;
            }

            return null;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            var tmp = new List<Group>();
            foreach (Group grp in _groups)
            {
                if (grp.Name.Course == courseNumber)
                {
                    tmp.Add(grp);
                }
            }

            return tmp;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            bool existingStudent = false;
            foreach (Group grp in _groups)
            {
                foreach (Student stud in grp.StudentList)
                {
                    if (stud.Id == student.Id && stud.Name == student.Name)
                    {
                        grp.StudentList.Remove(student);
                        grp.Capacity--;
                        existingStudent = true;
                        break;
                    }
                }

                if (existingStudent) break;
            }

            if (!existingStudent)
                throw new StudentException("Student doesn't exist!");

            int groupIndex = _groups.IndexOf(newGroup);
            if (groupIndex < 0)
                throw new GroupException($"Group doesn't exist!");

            if (_groups[groupIndex].Capacity >= _groups[groupIndex].MaxCapacity)
                throw new GroupException("Can't add student, group is full!");

            _groups[groupIndex].StudentList.Add(student);
            _groups[groupIndex].Capacity++;
        }

        public void GroupInfo(Group group) // my method
        {
            int groupIndex = _groups.IndexOf(group);
            if (groupIndex < 0)
                throw new GroupException($"Can't find group: {group.Name}! ");
            Console.WriteLine($"{_groups[groupIndex].Name}, {_groups[groupIndex].Capacity}");
            foreach (Student student in _groups[groupIndex].StudentList)
                Console.WriteLine(student);
        }
    }
}