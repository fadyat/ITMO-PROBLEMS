package ru.artyomfadeyev.JavaServer.Specifications.StudentSpecifications;

import org.springframework.lang.NonNull;
import ru.artyomfadeyev.JavaServer.Socials.Socials;
import ru.artyomfadeyev.JavaServer.Student.Student;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Predicate;
import javax.persistence.criteria.Root;

public class SocialsSpecification extends AbstractSpecification {
    private final Socials studentSocials;

    public SocialsSpecification(Socials studentSocials) {
        this.studentSocials = studentSocials;
    }

    @Override
    public Predicate toPredicate(
            @NonNull Root<Student> root, @NonNull CriteriaQuery<?> cq, @NonNull CriteriaBuilder cb) {
        return studentSocials.attributesAssigned()
                ? cb.and(
                cb.equal(
                        root.get("socials"),
                        studentSocials
                ))
                : null;
    }
}
