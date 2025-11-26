# SOLUTIONS — Guía rápida

## 2) Perfiles por entorno
- Comandos:
  - `mvn -Pdev package`
  - `mvn -Pqa package`
  - `mvn -Pprod clean package`
- Resultados esperados: `config.properties` se filtra con `app.env` y `app.message` distintos.
- `prod` genera `target/app-sample-1.0.0-prod.jar` ejecutable.

## 3) dependencyManagement vs dependencies
- En `pom.xml`:
```xml
<dependency>
  <groupId>org.junit.jupiter</groupId>
  <artifactId>junit-jupiter-params</artifactId>
  <scope>test</scope>
</dependency>
```
- Maven toma la versión desde `junit-bom` del `dependencyManagement`.

## 4) Plugins y fases
- Cambiar release:
```xml
<properties>
  <maven.compiler.release>17</maven.compiler.release>
</properties>
```
- Ver doc de plugin:
```
mvn help:describe -Dplugin=org.apache.maven.plugins:maven-compiler-plugin -Ddetail
```

## 5) Resource filtering avanzado
- Añadir en `properties` y perfiles:
```xml
<properties>
  <app.region>mx</app.region>
</properties>
```
- En `profiles`:
```xml
<profile>
  <id>qa</id>
  <properties>
    <app.region>us</app.region>
  </properties>
</profile>
```
- En `config.properties`:
```
app.region=${app.region}
app.version=${project.version}
```

## 6) Árbol de dependencias
- Añadir conflicto:
```xml
<dependency>
  <groupId>com.google.guava</groupId>
  <artifactId>guava</artifactId>
  <version>32.0.0-jre</version>
</dependency>
```
- Ejecutar `mvn dependency:tree -Dverbose` y observar cuál versión queda.

## 7) Empaquetado “fat JAR”
- Cambiar `finalName` de Shade:
```xml
<finalName>company-app-${project.version}-${app.env}</finalName>
```
- Añadir exclusiones dentro de `<filters>` si es necesario.