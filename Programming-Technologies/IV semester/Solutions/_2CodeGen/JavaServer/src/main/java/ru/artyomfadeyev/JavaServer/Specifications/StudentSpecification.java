package ru.artyomfadeyev.JavaServer.Specifications;

import org.springframework.data.jpa.domain.Specification;
import org.springframework.lang.NonNull;
import ru.artyomfadeyev.JavaServer.Socials.Socials;
import ru.artyomfadeyev.JavaServer.Student.Student;

import javax.persistence.criteria.CriteriaBuilder;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Predicate;
import javax.persistence.criteria.Root;

public class StudentSpecification implements Specification<Student> {
    private final SearchCriteria criteria;

    public StudentSpecification(SearchCriteria criteria) {
        this.criteria = criteria;
    }

    @Override
    public Predicate toPredicate(@NonNull Root<Student> root, @NonNull CriteriaQuery<?> cq, @NonNull CriteriaBuilder cb) {

        if (criteria.getOperation().equalsIgnoreCase(">")) {
            return cb.greaterThanOrEqualTo(
                    root.get(criteria.getKey()),
                    criteria.getValue().toString()
            );
        } else if (criteria.getOperation().equalsIgnoreCase("<")) {
            return cb.lessThanOrEqualTo(
                    root.get(criteria.getKey()),
                    criteria.getValue().toString()
            );
        } else if (criteria.getOperation().equalsIgnoreCase("=")) {
            if (root.get(criteria.getKey()).getJavaType() == String.class) {
                return cb.like(
                        root.get(criteria.getKey()),
                        "%" + criteria.getValue() + "%"
                );
            } else {
                return cb.equal(
                        root.get(criteria.getKey()),
                        criteria.getValue()
                );
            }
        }
        return null;
    }

    public static Specification<Student> getById(Integer id) {
        return (root, cq, cb) -> (
                cb.equal(
                        root.get("id"),
                        id
                )
        );
    }

    public static Specification<Student> getTg(String tg) {
        return (root, cq, cb) -> (
                cb.equal(
                        root.get("socials").get("tg"),
                        tg
                )
        );
    }
}
