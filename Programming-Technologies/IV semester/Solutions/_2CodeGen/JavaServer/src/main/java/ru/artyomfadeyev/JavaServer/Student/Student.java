package ru.artyomfadeyev.JavaServer.Student;

import ru.artyomfadeyev.JavaServer.Socials.Socials;

import javax.persistence.*;

@Entity
@Table
public class Student {
    @Id
    @SequenceGenerator(name = "student_sequence", sequenceName = "student_sequence", allocationSize = 1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "student_sequence")
    private Integer id;
    private String name;

    @Embedded
    private Socials socials;

    public Student(String name, Socials socials) {
        this.name = name;
        this.socials = socials;
    }

    public Student(Integer id, String name, Socials socials) {
        this.id = id;
        this.name = name;
        this.socials = socials;
    }

    public Student() {
    }

    public String getName() {
        return name;
    }

    public Integer getId() {
        return id;
    }

    public Socials getSocials() {
        return socials;
    }
}
