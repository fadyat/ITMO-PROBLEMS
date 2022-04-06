package ru.artyomfadeyev.JavaServer.services;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.artyomfadeyev.JavaServer.classes.Socials;
import ru.artyomfadeyev.JavaServer.classes.Student;
import ru.artyomfadeyev.JavaServer.repositories.StudentRepository;
import ru.artyomfadeyev.JavaServer.specifications.studentSpecifications.ExistsSpecification;
import ru.artyomfadeyev.JavaServer.specifications.studentSpecifications.SocialsSpecification;
import ru.artyomfadeyev.JavaServer.specifications.studentSpecifications.StudentSpecificationsBuilder;
import ru.artyomfadeyev.JavaServer.specifications.studentSpecifications.TgSpecification;

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
