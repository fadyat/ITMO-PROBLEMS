package ru.artyomfadeyev.JavaServer.Specifications.StudentSpecifications;

import org.springframework.lang.NonNull;
import ru.artyomfadeyev.JavaServer.Classes.Student.Student;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Predicate;
import javax.persistence.criteria.Root;

public class ExistsSpecification extends AbstractSpecification {
    private final Integer studentId;

    public ExistsSpecification(Integer studentId) {
        this.studentId = studentId;
    }

    @Override
    public Predicate toPredicate(@NonNull Root<Student> root, @NonNull CriteriaQuery<?> cq, @NonNull CriteriaBuilder cb) {
        return cb.equal(
                root.get("id"),
                studentId
        );
    }
}
