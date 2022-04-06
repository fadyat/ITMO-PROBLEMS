package ru.artyomfadeyev.javaserver.specifications.student;

import org.springframework.data.jpa.domain.Specification;
import org.springframework.lang.NonNull;
import ru.artyomfadeyev.javaserver.classes.Student;

import javax.persistence.criteria.*;

abstract public class AbstractSpecification implements Specification<Student> {
    @Override
    public Predicate toPredicate(
            @NonNull Root<Student> root, @NonNull CriteriaQuery<?> cq, @NonNull CriteriaBuilder cb) {
        return null;
    }
}
