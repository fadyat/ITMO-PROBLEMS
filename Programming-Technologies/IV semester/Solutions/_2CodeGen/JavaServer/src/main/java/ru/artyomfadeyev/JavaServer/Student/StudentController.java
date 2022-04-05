package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import ru.artyomfadeyev.JavaServer.Socials.Socials;

import java.util.List;

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
                                     @RequestParam(value = "socials", required = false) Socials socials) {
        List<Student> finalStudents = ids != null
                ? ids.stream()
                .map(studentService::getStudentById)
                .toList()
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
