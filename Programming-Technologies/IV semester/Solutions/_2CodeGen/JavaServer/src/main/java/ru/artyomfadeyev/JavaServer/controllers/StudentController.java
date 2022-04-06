package ru.artyomfadeyev.JavaServer.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import ru.artyomfadeyev.JavaServer.classes.Socials;
import ru.artyomfadeyev.JavaServer.classes.Student;
import ru.artyomfadeyev.JavaServer.services.StudentService;

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
                                     @ModelAttribute Socials socials) {
        return studentService.findAll(ids, socials);
    }

    @GetMapping("{id}")
    public List<Student> getStudent(@PathVariable Integer id) {
        return studentService.findById(id);
    }

    @PostMapping
    public void saveStudent(@RequestBody Student student) {
        this.studentService.save(student);
    }
}
