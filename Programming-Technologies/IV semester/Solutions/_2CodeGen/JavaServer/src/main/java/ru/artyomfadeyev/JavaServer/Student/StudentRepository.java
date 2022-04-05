package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface StudentRepository extends JpaRepository<Student, Integer> {
    @Query("SELECT s FROM Student s WHERE s.tg = ?1")
    Optional<Student> findStudentByTg(String tg);

    @Query("SELECT s FROM Student s WHERE s.id = ?1")
    Student getStudentById(Integer id);
}
