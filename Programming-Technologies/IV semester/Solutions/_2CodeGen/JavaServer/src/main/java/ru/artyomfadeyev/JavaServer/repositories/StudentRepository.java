package ru.artyomfadeyev.JavaServer.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.JpaSpecificationExecutor;
import org.springframework.stereotype.Repository;
import ru.artyomfadeyev.JavaServer.classes.Student;

@Repository
public interface StudentRepository
        extends JpaRepository<Student, Integer>, JpaSpecificationExecutor<Student> {
}
