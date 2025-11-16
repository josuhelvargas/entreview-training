🟦 MÓDULO 1 — Fundamentos de Spring & Spring Boot (2025)

(Con explicación interna, examen, respuestas y práctica)

🔥 1.1. ¿QUÉ ES SPRING FRAMEWORK HOY? (INTERNALS + EXPLICACIÓN)

Spring ya no es solo un framework:
es un ecosistema completo para construir aplicaciones Java empresariales con:

Inversión de Control (IoC)

Inyección de Dependencias (DI)

Aspect-Oriented Programming (AOP)

Abstracciones de datos, mensajería, web, seguridad, etc.

Soporte nativo para reactividad y programación funcional

Integración con contenedores (K8s), observabilidad, nubes, etc.

🔍 Internals clave: IoC / DI
🧠 ¿Qué es IoC en Spring?

IoC significa que Spring crea y controla los objetos, no tú.

Internamente:

Spring escanea tus clases (@ComponentScan)

Crea beans y sus dependencias

Guarda todo en un ApplicationContext (un contenedor gigante)

Cuando necesitas un objeto → Spring te lo inyecta (@Autowired, constructor)

🧪 ¿Qué pasa realmente dentro?

Cuando arranca la app:

AnnotationConfigServletWebServerApplicationContext
 → BeanFactory
   → Crear beans
   → Resolver dependencias
   → Aplicar proxies de AOP
   → Gestionar ciclo de vida


Spring genera objetos proxy para manejar:

seguridad,

transacciones,

logging,

validaciones,

eventos.

🌀 AOP (Aspect Oriented Programming)

Spring usa AOP para meter lógica transversal (cross-cutting) como:

logging,

manejo de transacciones,

seguridad,

medición de tiempos,

auditoría.

Internamente usa:

Proxy dinámico JDK

CGLIB para crear clases hijas en tiempo de ejecución.

🌱 Beans, Scopes, Profiles
Bean

Cualquier objeto administrado por Spring.

Scopes más usados:

singleton (default)

prototype

request

session

application

Profiles

Permiten activar configuración por ambiente:

spring.profiles.active=dev

🔥 1.2. SPRING BOOT 3.x / 2025 — INTERNALS Y ELEMENTOS IMPORTANTES

Spring Boot es una capa encima de Spring Framework que:

configura automáticamente,

da opinionated defaults,

tiene starters listos,

provee Actuator,

integra observabilidad.

⭐ Starter Dependencies (internals reales)

Son "pompacks" de dependencias curados.

Ejemplo:

spring-boot-starter-web:
  - spring-web
  - spring-webmvc
  - jackson
  - logging


Spring Boot usa su archivo spring.factories y AutoConfiguration para decidir qué inicializar.

⚙️ Auto-Configuration (Explicado a nivel interno)

Cuando tu app arranca:

@SpringBootApplication → incluye @EnableAutoConfiguration.

Spring carga cientos de configuraciones desde:

META-INF/spring/org.springframework.boot.autoconfigure.AutoConfiguration.imports


Cada clase usa condiciones como:

@ConditionalOnClass
@ConditionalOnMissingBean
@ConditionalOnProperty


Solo carga lo que tiene sentido para tu proyecto.

Ejemplo:
Si agregas spring-boot-starter-data-jpa, Spring detecta:

EntityManager

DataSource

Hibernate
→ y auto-configura todo.

📈 Observability con Micrometer

Spring Boot 3 integra:

métricas (CPU, heap, GC, HTTP requests)

trazas (OTel)

logs correlacionados

exportadores a Prometheus, Grafana, Datadog

Es el estándar enterprise en 2025.

🛠️ Spring Boot Actuator

Endpoints como:

/actuator/health

/actuator/metrics

/actuator/loggers

/actuator/prometheus

Ayudan para monitoreo, DevOps, métricas y debugging.

🔧 Configuración con application.yml y external config

Orden de prioridad de config:

Variables de entorno

Parámetros de línea

application.yml

Profiles específicos (application-dev.yml)

Config en Config Server

Spring Boot hace binding automático:

app:
  name: demo
  retries: 3

@ConfigurationProperties(prefix = "app")
public record AppConfig(String name, int retries) {}

🔥 1.3. PROYECTO BASE (HANDS-ON)
🟩 Crear un servicio REST
@RestController
@RequestMapping("/api/hello")
public class HelloController {

    @GetMapping
    public String sayHello() {
        return "Hola Spring Boot 2025!";
    }
}

🛑 Controladores y Excepciones Globales
@RestControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(Exception.class)
    public ResponseEntity<?> handleAny(Exception ex) {
        return ResponseEntity.status(500)
                .body(Map.of("error", ex.getMessage()));
    }
}

📝 Logging con SLF4J / Logback
@Slf4j
@RestController
public class DemoController {

    @GetMapping("/demo")
    public String demo() {
        log.info("Ejecutando endpoint demo");
        return "ok";
    }
}

🧼 DTOs y Validaciones (Jakarta Validation)
public record UserRequest(
   @NotBlank String name,
   @Email String email,
   @Min(18) int age
) {}

🧪 EXAMEN MÓDULO 1 (20 PREGUNTAS)

Preguntas tipo entrevista + teoría.

📝 PREGUNTAS DE OPCIÓN MÚLTIPLE
1. ¿Qué es IoC en Spring?

A. Que tú creas manualmente los objetos
B. Que Spring controla la creación y ciclo de vida de los objetos
C. Que los objetos se crean en la base de datos
D. Ninguna de las anteriores

Respuesta: B

2. ¿Qué es un Bean en Spring?

A. Una clase Java final
B. Un objeto administrado por el contenedor IoC
C. Una entidad JPA
D. Un método estático

Respuesta: B

3. ¿Qué anotación activa la autoconfiguración?

A. @EnableSpring
B. @SpringBootApplication
C. @Autowired
D. @EnableBeans

Respuesta: B

4. Spring Boot Actuator sirve para:

A. Compilar más rápido
B. Exponer diagnósticos de la app
C. Hacer consultas SQL
D. Crear entidades

Respuesta: B

5. Micrometer proporciona:

A. Logs solamente
B. Métricas, trazas y bindings para observabilidad
C. HTML dinámico
D. Persistencia

Respuesta: B

6. ¿Qué hace @RestController?

A. Configura logs
B. Marca una clase que manejará requests HTTP devolviendo JSON
C. Configura bean scopes
D. Crea proxies

Respuesta: B

7. ¿Qué hace @ControllerAdvice?

A. Configura caching
B. Maneja excepciones de forma global
C. Activa perfiles
D. Crea servicios

Respuesta: B

🧠 PREGUNTAS ABIERTAS (RESPUESTAS EXPLICADAS)
1. Explica IoC y DI con tus palabras.

Respuesta:
IoC significa que Spring controla la creación de objetos y su ciclo de vida.
DI significa que Spring inyecta las dependencias necesarias en un bean, generalmente por constructor.
En lugar de hacer new, Spring administra las relaciones entre objetos.

2. ¿Cómo funciona internamente la auto-configuración de Spring Boot?

Respuesta:
Spring Boot revisa las clases listadas en AutoConfiguration.imports.
Cada autoconfiguración se activa solo si cumple ciertas condiciones:

@ConditionalOnClass

@ConditionalOnProperty

@ConditionalOnMissingBean

Esto permite que Spring Boot genere configuraciones basadas en las dependencias presentes.

3. Explica qué es un Scope y da ejemplos.

Respuesta:
El scope define el ciclo de vida del bean.
Ejemplos:

Singleton: 1 instancia para toda la app.

Request: 1 instancia por request HTTP.

Prototype: una nueva instancia cada vez que se solicita.

4. ¿Qué diferencia hay entre application.yml y external config?

Respuesta:
application.yml vive dentro del proyecto.
La external config puede venir de:

variables de entorno

parámetros de línea

Config Server

archivos montados en contenedor
Y tiene mayor prioridad.

5. ¿Qué hace Spring Actuator y por qué es crítico en microservicios?

Respuesta:
Expone endpoints de salud, métricas y diagnósticos que permiten:

monitoreo,

alertas,

autoscaling,

readiness/liveness probes.

Sin Actuator Kubernetes no podría saber si tu microservicio está vivo.

🧪 PREGUNTAS PRÁCTICAS (CÓDIGO)
1. Crea un endpoint GET que reciba un name y devuelva “Hola {name}”.

Respuesta:

@GetMapping("/{name}")
public String saludo(@PathVariable String name){
    return "Hola " + name;
}

2. Crea un DTO con validaciones para crear un usuario.

Respuesta:

public record CreateUserDTO(
   @NotBlank String name,
   @Email String email,
   @Min(18) int age
) {}

3. Crea un controlador de excepciones global.

Respuesta:

@RestControllerAdvice
public class ApiErrorHandler {

    @ExceptionHandler(MethodArgumentNotValidException.class)
    public ResponseEntity<?> validationError(MethodArgumentNotValidException ex) {
        return ResponseEntity.badRequest().body(
                ex.getBindingResult().getFieldErrors().stream()
                   .map(e -> e.getField() + ": " + e.getDefaultMessage())
                   .toList()
        );
    }
}

🎯 MINI-PROYECTO (CIERRE DE MÓDULO)

Construir una API que:

Exponga /users para crear usuarios

Valide email, nombre y edad

Use un @RestControllerAdvice para errores

Use logging

Use perfiles (dev, prod)

Exponga /actuator/health

Use beans con diferentes scopes

Genere métricas con Micrometer

Si quieres, puedo armarte:

🔥 El examen en PDF
🔥 Flashcards tipo Anki
🔥 Código base en un proyecto Spring Boot real
🔥 Módulo 2 igual de completo

Dime qué sigue y lo armamos 😎💪