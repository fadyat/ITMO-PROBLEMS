package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.artyomfadeyev.JavaServer.Specifications.StudentSpecification;

import java.lang.module.FindException;
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
        Optional<Student> studentOptional = studentRepository.findStudentByTg(student.getSocials().getTg());
        if (studentOptional.isPresent()) {
            throw new IllegalStateException("Tg is already taken!");
        }

        studentRepository.save(student);
    }

    public Student getStudentById(Integer id) {
        return studentRepository.findAll(StudentSpecification.getById(id)).get(0);
    }
}
