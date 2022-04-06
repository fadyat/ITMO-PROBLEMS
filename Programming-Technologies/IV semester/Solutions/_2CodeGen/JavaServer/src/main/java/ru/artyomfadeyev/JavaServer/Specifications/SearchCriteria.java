package ru.artyomfadeyev.JavaServer.Specifications;

public class SearchCriteria {
    private final String key;
    private final String operation;
    private final Object value;
    private final Boolean isOr;

    public SearchCriteria(String key, String operation, Object value, Boolean isOr) {
        this.key = key;
        this.operation = operation;
        this.value = value;
        this.isOr = isOr;
    }

    public String getKey() {
        return key;
    }

    public String getOperation() {
        return operation;
    }

    public Object getValue() {
        return value;
    }

    public Boolean isOrPredicate() {
        return isOr;
    }
}
