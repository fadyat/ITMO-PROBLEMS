package ru.artyomfadeyev.JavaServer.Specifications.StudentSpecifications;

import org.springframework.data.jpa.domain.Specification;
import ru.artyomfadeyev.JavaServer.Student.Student;

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


//    public Specification<Student> build() {
//        if (params.size() == 0) {
//            return null;
//        }
//
//        List<StudentSpecification> specs = params.stream()
//                .map(StudentSpecification::new)
//                .toList();
//
//        Specification<Student> result = specs.get(0);
//
//        for (int i = 1; i < params.size(); i++) {
//            result = params.get(i)
//                    .isOrPredicate()
//                    ? Specification.where(result)
//                    .or(specs.get(i))
//                    : Specification.where(result)
//                    .and(specs.get(i));
//        }
//        return result;
//    }
}