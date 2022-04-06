package ru.artyomfadeyev.javaserver.specifications.student;

import org.springframework.lang.NonNull;
import ru.artyomfadeyev.javaserver.classes.Student;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Predicate;
import javax.persistence.criteria.Root;

public class TgSpecification extends AbstractSpecification {
    private final String studentTg;

    public TgSpecification(String studentTg) {
        this.studentTg = studentTg;
    }

    @Override
    public Predicate toPredicate(@NonNull Root<Student> root, @NonNull CriteriaQuery<?> cq, @NonNull CriteriaBuilder cb) {
        return cb.equal(
                root.get("socials")
                        .get("tg"),
                studentTg
        );
    }
}
