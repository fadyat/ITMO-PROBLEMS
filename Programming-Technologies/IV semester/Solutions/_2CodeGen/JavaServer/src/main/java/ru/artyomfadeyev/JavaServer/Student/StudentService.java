package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.stereotype.Component;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class StudentService {
    public List<Student> getStudents() {
        return List.of(
                new Student(1, "Artyom", "@not_fadyat")
        );
    }
}
