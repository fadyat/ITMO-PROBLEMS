package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.jpa.domain.Specification;
import org.springframework.web.bind.annotation.*;
import ru.artyomfadeyev.JavaServer.Socials.Socials;
import ru.artyomfadeyev.JavaServer.Specifications.SearchCriteria;
import ru.artyomfadeyev.JavaServer.Specifications.StudentSpecification;
import ru.artyomfadeyev.JavaServer.Specifications.StudentSpecificationsBuilder;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("api/student")
public class StudentController {
    private final StudentService studentService;

    @Autowired
    public StudentController(StudentService studentService) {
        this.studentService = studentService;
    }

    @GetMapping
    public List<Student> getStudents(@RequestParam(value = "id", required = false) List<Integer> ids,


                                     // mistake !!!!



                                     @RequestParam(value = "socials", required = false) String tg) {
// https://www.baeldung.com/rest-api-search-language-spring-data-specifications
        StudentSpecificationsBuilder builder = new StudentSpecificationsBuilder();
        Boolean used = false;
        if (ids != null) {
            for (Integer id : ids) {
                builder.with("id", "=", id, true);
            }
            used = true;
        }
        System.out.println(tg);
        if (tg != null) {
            builder.with("socials.tg", "=", tg, false);
            used = true;
        }

        Specification<Student> specification = builder.build();
        List<Student> finalStudents = used
                ? studentService.studentRepository().findAll(specification)
                : studentService.getStudents();

        return finalStudents;
    }

    @GetMapping("{id}")
    public Student getStudent(@PathVariable Integer id) {
        return studentService.getStudentById(id);
    }

    @PostMapping
    public void registerNewStudent(@RequestBody Student student) {
        this.studentService.addNewStudent(student);
    }
}
