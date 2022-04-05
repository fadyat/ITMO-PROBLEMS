package ru.artyomfadeyev.JavaServer.Student;

import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import ru.artyomfadeyev.JavaServer.Socials.Socials;

import java.util.List;

@Configuration
public class StudentConfig {
    @Bean
    CommandLineRunner commandLineRunner(StudentRepository repository) {
        return args -> {
            Student a = new Student("Artyom", new Socials("@not_fadyat", "@itsfadyat", "@mrfadeyev"));
            Student s = new Student("Sergo", new Socials("@Dokep", null, null));
            repository.saveAll(List.of(a, s));
        };
    }
}
