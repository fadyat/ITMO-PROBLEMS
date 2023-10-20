package ru.artyomfadeyev.javaserver.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.artyomfadeyev.javaserver.classes.Student;
import ru.artyomfadeyev.javaserver.repositories.StudentRepository;

import java.util.ArrayList;
import java.util.List;

@Service
public record StudentService(StudentRepository studentRepository) {
    @Autowired
    public StudentService {
    }

    public List<Student> findAll(List<Integer> ids) {
        if (ids == null) {
            return studentRepository.findAll();
        }

        List<Student> students = new ArrayList<>();

        ids.forEach(id -> {
            var student = studentRepository.findStudentById(id);
            students.add(student);
        });

        return students;
    }

    public void save(Student student) {
        studentRepository.save(student);
    }

    public Student findById(Integer id) {
        return studentRepository.findStudentById(id);
    }
}
