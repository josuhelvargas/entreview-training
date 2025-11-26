# Módulo 2 — `pom.xml` en profundidad

Este repositorio contiene:

- **pom.xml** con `groupId`, `artifactId`, `version`, `packaging`, `dependencies`, `dependencyManagement`, `build`, `plugins` y **perfiles** (`dev`, `qa`, `prod`).
- **Ejercicios** paso a paso en `EXERCISES.md` y soluciones en `SOLUTIONS.md`.
- **App de ejemplo** (`App.java`) que lee una propiedad filtrada por perfil.
- **Tests** (`AppTest.java`) con Surefire/JUnit.
- **Recursos** con filtrado de propiedades (`config.properties`).

## Comandos clave

Compilar con Java 21 (por defecto):  
```bash
mvn -v
mvn clean compile
```

Ejecutar tests:  
```bash
mvn test
```

Empaquetar (JAR normal):  
```bash
mvn -Pdev package
mvn -Pqa package
mvn -Pprod package
```

Crear **fat JAR** con Shade (perfil `prod`):  
```bash
mvn -Pprod clean package
java -jar target/app-sample-1.0.0-prod.jar
```

Mostrar árbol de dependencias:  
```bash
mvn -Pdev -Dverbose dependency:tree
```

Ver propiedades efectivas (útil para depurar perfiles/props):  
```bash
mvn help:effective-pom -Pdev
```