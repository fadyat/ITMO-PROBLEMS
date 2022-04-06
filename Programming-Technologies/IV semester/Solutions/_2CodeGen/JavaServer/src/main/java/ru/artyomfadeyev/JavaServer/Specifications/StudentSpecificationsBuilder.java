package ru.artyomfadeyev.JavaServer.Specifications;

import org.springframework.data.jpa.domain.Specification;
import ru.artyomfadeyev.JavaServer.Student.Student;

import java.util.ArrayList;
import java.util.List;

public class StudentSpecificationsBuilder {

    private final List<SearchCriteria> params;

    public StudentSpecificationsBuilder() {
        params = new ArrayList<>();
    }

    public StudentSpecificationsBuilder with(String key, String operation, Object value, Boolean isOr) {
        params.add(new SearchCriteria(key, operation, value, isOr));
        return this;
    }

    public Specification<Student> build() {
        if (params.size() == 0) {
            return null;
        }

        List<StudentSpecification> specs = params.stream()
                .map(StudentSpecification::new)
                .toList();

        Specification<Student> result = specs.get(0);

        for (int i = 1; i < params.size(); i++) {
            result = params.get(i)
                    .isOrPredicate()
                    ? Specification.where(result)
                    .or(specs.get(i))
                    : Specification.where(result)
                    .and(specs.get(i));
        }
        return result;
    }
}