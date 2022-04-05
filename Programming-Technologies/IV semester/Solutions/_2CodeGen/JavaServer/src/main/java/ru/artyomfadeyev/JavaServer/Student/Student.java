package ru.artyomfadeyev.JavaServer.Student;

import javax.persistence.*;

@Entity
@Table
public class Student {
    @Id
    @SequenceGenerator(name = "student_sequence", sequenceName = "student_sequence", allocationSize = 1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "student_sequence")
    private Integer id;
    private String name;
    private String tg;

    public Student(String name, String tg) {
        this.name = name;
        this.tg = tg;
    }

    public Student(Integer id, String name, String tg) {
        this.id = id;
        this.name = name;
        this.tg = tg;
    }

    public Student() {
    }

    public String getName() {
        return name;
    }

    public Integer getId() {
        return id;
    }

    public String getTg() {
        return tg;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public void setTg(String tg) {
        this.tg = tg;
    }

    @Override
    public String toString() {
        return "Student{" +
                "id=" + id +
                ", name='" + name + '\'' +
                ", tg='" + tg + '\'' +
                '}';
    }
}
