package ru.artyomfadeyev.javaserver.classes;

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

    public Student() {
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setSocials(Socials socials) {
        this.socials = socials;
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
