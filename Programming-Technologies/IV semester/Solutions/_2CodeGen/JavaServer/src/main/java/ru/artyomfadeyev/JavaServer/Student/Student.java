package ru.artyomfadeyev.JavaServer.Student;

public class Student {
    private final String name;
    private final Integer id;
    private final String tg;

    public Student(Integer id, String name, String telegramLink) {
        this.id = id;
        this.name = name;
        this.tg = telegramLink;
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
}
