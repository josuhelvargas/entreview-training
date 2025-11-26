# EXERCISES — Módulo 2 (pom.xml en profundidad)


sdk list 
sdk use java 17.0.10-tem
sdk use java 21-tem
sdk use java 25-tem

## 1) Calienta motores
1. Ejecuta `mvn -v` y verifica tu versión de Maven y Java.
2. Compila el proyecto con `mvn clean compile`. // genera target/classes   PERO NO JAR (para ello se requiere el package)
3. Genera el paquete con `mvn -Pdev package` y revisa `target/`.

**Objetivo:** Reconocer el ciclo `clean/compile/test/package` y el efecto de perfiles. 
//mvn clean compile
//mvn -Pdev package
//mvn -Pqa package
//mvn -Pprod package

## 2) Perfiles por entorno
1. Empaqueta con `-Pdev`, `-Pqa`, `-Pprod`.  
2. Ejecuta el JAR resultante (en `prod` se crea un fat JAR con Shade). //java -jar target/app-sample-1.0.0.jar 
3. Observa en la consola cómo cambia `app.env` y `app.message`.

**Objetivo:** Entender `profiles` y filtrado de recursos.

## 3) dependencyManagement vs dependencies
1. Añade una dependencia que exista en BOM (por ejemplo `org.junit.jupiter:junit-jupiter-params` en `test`).  
//si ves se agrega la dependencias de junit.jupiter-params
2. No pongas `<version>` y confirma que la versión se resuelve desde el BOM del `dependencyManagement`. -> ejecutar mvn dependency:tree ( se muestra al fina del output que org.junit.jupiter:junit-jupiter-params:jar:5.10.2:test)
3. Cambia la versión del BOM y repite.

**Objetivo:** Centralizar versiones via BOM.

## 4) Plugins y fases
1. Cambia `maven-compiler-plugin` para compilar con otra `release` (por ejemplo 17) y observa el fallo si tu JDK no coincide.
2. Añade ejecución de `maven-jar-plugin` en `package` con un `Classifier` distinto.
3. Ejecuta `mvn help:describe -Dplugin=<pluginId>` para inspeccionar documentación.

**Objetivo:** Comprender configuración y ejecución de plugins por fase.

## 5) Resource filtering avanzado
1. Añade nueva propiedad `${app.region}` con valores distintos por perfil.
2. Añade uso de `${project.version}` en `config.properties` y muéstralo en la app.
3. Crea otro recurso filtrado (por ejemplo `banner.txt`) y cárgalo desde `App.java`.

**Objetivo:** Plantillas de recursos por entorno.

## 6) Árbol de dependencias
1. Ejecuta `mvn dependency:tree -Pprod -Dverbose`.
2. Forzar un conflicto de versión (añade guava con otra versión en `dependencies`) y observa la “nearest definition”.

**Objetivo:** Diagnosticar conflictos y cómo Maven elige versiones.

## 7) Empaquetado “fat JAR”
1. Cambia la configuración de Shade para renombrar el artefacto a `company-app-${project.version}-${app.env}`. 2. Añade exclusiones en Shade para evitar incluir recursos no deseados.

**Objetivo:** Controlar empaquetado avanzado para despliegue.