package ru.artyomfadeyev.JavaServer.Classes.Student;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.artyomfadeyev.JavaServer.Classes.Socials.Socials;
import ru.artyomfadeyev.JavaServer.Specifications.StudentSpecifications.ExistsSpecification;
import ru.artyomfadeyev.JavaServer.Specifications.StudentSpecifications.SocialsSpecification;
import ru.artyomfadeyev.JavaServer.Specifications.StudentSpecifications.StudentSpecificationsBuilder;
import ru.artyomfadeyev.JavaServer.Specifications.StudentSpecifications.TgSpecification;

import java.util.List;

@Service
public record StudentService(StudentRepository studentRepository) {
    @Autowired
    public StudentService {
    }

    public List<Student> findAll(List<Integer> ids, Socials socials) {
        StudentSpecificationsBuilder builder = new StudentSpecificationsBuilder()
                .and(new SocialsSpecification(socials));

        if (ids != null) {
            ids.stream()
                    .map(ExistsSpecification::new)
                    .forEach(builder::or);
        }

        return studentRepository.findAll(builder.build());
    }

    public void save(Student student) {
        StudentSpecificationsBuilder builder = new StudentSpecificationsBuilder()
                .and(new TgSpecification(student.getSocials().getTg()));
        List<Student> foundStudent = studentRepository.findAll(builder.build());
        if (foundStudent.isEmpty()) {
            studentRepository.save(student);
        }
        else {
            throw new IllegalStateException("Student w/ tg exists!");
        }
    }

    public List<Student> findById(Integer id) {
        StudentSpecificationsBuilder builder = new StudentSpecificationsBuilder()
                .and(new ExistsSpecification(id));

        return studentRepository.findAll(builder.build());
    }
}
