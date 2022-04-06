package ru.artyomfadeyev.javaserver.specifications.student;

import org.springframework.data.jpa.domain.Specification;
import ru.artyomfadeyev.javaserver.classes.Student;

public class StudentSpecificationsBuilder {

    private Specification<Student> params;

    public StudentSpecificationsBuilder() {
        params = null;
    }

    public StudentSpecificationsBuilder and(AbstractSpecification studentSpecification) {
        params = params == null
                ? studentSpecification
                : params.and(studentSpecification);
        return this;
    }

    public StudentSpecificationsBuilder or(AbstractSpecification studentSpecification) {
        params = params == null
                ? studentSpecification
                : params.or(studentSpecification);
        return this;
    }

    public Specification<Student> build() {
        return params;
    }
}