package ru.artyomfadeyev.javaserver.config;

import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import ru.artyomfadeyev.javaserver.classes.Socials;
import ru.artyomfadeyev.javaserver.classes.Student;
import ru.artyomfadeyev.javaserver.repositories.StudentRepository;

import java.util.List;

@Configuration
public class StudentConfig {
    @Bean
    CommandLineRunner commandLineRunner(StudentRepository repository) {
        return args -> {
            Student a = new Student("Artyom", new Socials("@not_fadyat", "@itsfadyat", "@mrfadeyev"));
            Student s = new Student("Sergo", new Socials("@Dokep", null, null));
            Student l = new Student("Alyona", new Socials("@alyona.f", "@lol", "@me"));
            repository.saveAll(List.of(a, s, l));
        };
    }
}
