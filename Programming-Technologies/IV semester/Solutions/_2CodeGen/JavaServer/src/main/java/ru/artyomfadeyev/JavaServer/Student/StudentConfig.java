package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import java.util.List;

@Configuration
public class StudentConfig {
    @Bean
    CommandLineRunner commandLineRunner(StudentRepository repository) {
        return args -> {
            Student a = new Student("Artyom", "@not_fadyat");
            Student s = new Student("Sergo", "@Dokep");
            repository.saveAll(List.of(a, s));
        };
    }
}
