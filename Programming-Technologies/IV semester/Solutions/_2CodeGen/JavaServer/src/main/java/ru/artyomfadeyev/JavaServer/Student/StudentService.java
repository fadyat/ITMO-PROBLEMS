package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public record StudentService(StudentRepository studentRepository) {
    @Autowired
    public StudentService {
    }

    public List<Student> getStudents() {
        return studentRepository.findAll();
    }

    public void addNewStudent(Student student) {
        Optional<Student> studentOptional = studentRepository.findStudentByTg(student.getTg());
        if (studentOptional.isPresent()) {
            throw new IllegalStateException("tg taken");
        }

        studentRepository.save(student);
    }

    public Student getStudentById(Integer id) {
        return studentRepository.getStudentById(id);
    }
}
