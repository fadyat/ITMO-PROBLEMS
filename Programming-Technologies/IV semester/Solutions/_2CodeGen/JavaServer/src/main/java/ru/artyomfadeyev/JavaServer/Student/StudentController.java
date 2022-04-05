package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Objects;

@RestController
@RequestMapping("api/student")
public class StudentController {
    private final StudentService studentService;

    @Autowired
    public StudentController(StudentService studentService) {
        this.studentService = studentService;
    }

    @GetMapping
    @ResponseBody
    public List<Student> getStudents(@RequestParam(value = "id", required = false) List<Integer> ids) {
        if (ids == null) {
            return studentService.getStudents();
        }

        return ids.stream()
                .map(studentService::getStudentById)
                .filter(Objects::nonNull)
                .toList();
    }

    @PostMapping
    public void registerNewStudent(@RequestBody Student student) {
        this.studentService.addNewStudent(student);
    }
}
