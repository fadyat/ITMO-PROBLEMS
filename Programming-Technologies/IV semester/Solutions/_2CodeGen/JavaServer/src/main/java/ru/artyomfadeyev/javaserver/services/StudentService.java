package ru.artyomfadeyev.javaserver.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.artyomfadeyev.javaserver.classes.Socials;
import ru.artyomfadeyev.javaserver.classes.Student;
import ru.artyomfadeyev.javaserver.repositories.StudentRepository;
import ru.artyomfadeyev.javaserver.specifications.student.ExistsSpecification;
import ru.artyomfadeyev.javaserver.specifications.student.SocialsSpecification;
import ru.artyomfadeyev.javaserver.specifications.student.StudentSpecificationsBuilder;
import ru.artyomfadeyev.javaserver.specifications.student.TgSpecification;

import java.util.List;

@Service
public record StudentService(StudentRepository studentRepository) {
    @Autowired
    public StudentService {
    }

    public List<Student> findAll(List<Integer> ids, Socials socials) {
        StudentSpecificationsBuilder builder = new StudentSpecificationsBuilder();

        if (ids != null) {
            ids.stream()
                    .map(ExistsSpecification::new)
                    .forEach(builder::or);
        }

        builder.and(new SocialsSpecification(socials));

        return studentRepository.findAll(builder.build());
    }

    public void save(Student student) {
        StudentSpecificationsBuilder builder = new StudentSpecificationsBuilder()
                .and(new TgSpecification(student.getSocials().getTg()));

        List<Student> foundStudent = studentRepository.findAll(builder.build());
        if (foundStudent.isEmpty()) {
            studentRepository.save(student);
        } else {
            throw new IllegalStateException("Student w/ tg exists!");
        }
    }

    public List<Student> findById(Integer id) {
        StudentSpecificationsBuilder builder = new StudentSpecificationsBuilder()
                .and(new ExistsSpecification(id));

        return studentRepository.findAll(builder.build());
    }
}