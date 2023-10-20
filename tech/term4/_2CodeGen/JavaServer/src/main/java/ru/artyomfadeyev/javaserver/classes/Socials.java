package ru.artyomfadeyev.javaserver.classes;

import javax.persistence.Embeddable;

@Embeddable
public class Socials {
    private String tg;
    private String ig;
    private String vk;

    public Socials(String tg, String ig, String vk) {
        this.tg = tg;
        this.ig = ig;
        this.vk = vk;
    }

    public Socials() {

    }

    public String getTg() {
        return tg;
    }

    public void setTg(String tg) {
        this.tg = tg;
    }

    public String getIg() {
        return ig;
    }

    public void setIg(String ig) {
        this.ig = ig;
    }

    public String getVk() {
        return vk;
    }

    public void setVk(String vk) {
        this.vk = vk;
    }
}
