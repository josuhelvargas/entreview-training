
//Para poder eficientar la carga de clases en java a traves del classloade, es necesario especificar los paqutes o directorio apar alimitar la busqueda genracion de beans y su psoterior carga con sprinbgboot al spring context.

@SpringBootApplication(scanBasePackages = {"backend.java.javaversionsEvolution"})
public class LearningVirtualMachineOne {
  


}


// Estrategias de optimización:java// ❌ MAL - Escanea todo
// @SpringBootApplication

// // ✓ MEJOR - Limita el scan
// @SpringBootApplication(scanBasePackages="com.myapp.core")

// // ✓ EXCELENTE - Configuración explícita
@Configuration @Import({SecurityConfig.class,DatabaseConfig.class,ServiceConfig.class
})

public class AppConfig {
}

// // ✓ Spring Boot 3 - Usa AOT (Ahead-of-Time)
// // Genera configuración en compile time
// mvn spring-boot:process-aot





// Otras optimizaciones:
// Lazy Initialization:
// properties#
// application.properties
// spring.main.lazy-initialization=true




// Exclude Auto-configurations no usadas:
// java 
// @SpringBootApplication(exclude={DataSourceAutoConfiguration.class,HibernateJpaAutoConfiguration.class})
// IndexedComponent para
// scan más rápido:
// xml<!--pom.xml--><dependency><groupId>org.springframework</groupId><artifactId>spring-context-indexer</artifactId><optional>true</optional></dependency>





// Profile-specific beans:
// java 
@Profile("!test")
@Component
public class ExpensiveBean {
}
//Medición:bash#

// Ver qué
// se está
// cargando java-verbose:class-jar myapp.jar
// #
// Tiempo de
// startup detallado java-Dspring.profiles.active=dev\-Ddebug=true\-jar myapp.
// jar







/// 2.0: No es lo mismo hacer el calssloading ( 1 vez por clase) , que el numero de isntancias creadas con spring en el applicationcontext.
/// 
/// explica esto en detalle: 2. (5 pts) Análisis de código - ¿Cuántas veces se
/// carga la clase UserService y por qué?
// java

// @Configuration
public class AppConfig {
  @Bean
  public UserService userService() {
    return new UserService();
  }
}

@Service
public class UserService {
  public void process() {
    System.out.println("Processing...");
  }
}

@RestController
public class UserController {
  @Autowired
  private UserService userService;

  private static final UserService STATIC_SERVICE = new UserService();}

//   En ese
//   código hay dos“UserService”
//   distintos en juego:

//   El UserService
//   que crea
//   Spring como

//   bean (y que inyecta con @Autowired)

// El UserService que tú creas manualmente con new en el campo STATIC_SERVICE

// Y además hay que separar dos conceptos: carga de clase (class loading) vs creación de instancias (new / beans).






// 1) ¿Cuántas veces se carga la clase UserService?

// Una sola vez por ClassLoader.

// La JVM carga la clase UserService (bytecode + metadatos) cuando se necesita por primera vez.

// En una app Spring Boot típica, eso ocurre cuando:

// Spring escanea @Service y/o

// se ejecuta new UserService() (tu static) y/o

// se llama al método @Bean userService() de AppConfig

// Pero aunque la “uses” en varios lados, la clase no se recarga cada vez. La carga es única por ClassLoader.

// Si el examen usa “cargar” en sentido estricto JVM ⇒ respuesta: 1 vez.





// 2) Lo que sí pasa varias veces: instancias creadas

// Aquí se crean múltiples objetos UserService.

// A) Bean de Spring creado por @Bean
// @Configuration
// public class AppConfig {
//     @Bean
//     public UserService userService() {
//         return new UserService();
//     }
// }


// Por default, un @Bean es singleton dentro del ApplicationContext.

// Eso significa: Spring ejecuta ese método una sola vez y guarda el objeto como userService.

// ✅ Instancias por aquí: 1

// B) Bean por @Service (posible segunda instancia)
// @Service
// public class UserService { ... }


// Con esto, Spring también intenta registrar un bean llamado algo como userService.

// ¿Entonces Spring crea otra instancia? Depende:

// Si dejas ambas cosas (@Service y @Bean) con el mismo nombre, Spring termina con colisión de definición de bean.

// En muchas configuraciones modernas de Spring Boot, esto falla o requiere permitir overriding.

// Si se permite overriding, una definición pisa a la otra y se queda solo una como bean.

// Si cambias el nombre del @Bean o lo marcas @Primary, etc., podrías terminar con dos beans diferentes (y entonces dos instancias), pero tu ejemplo no muestra eso.

// 📌 En un examen normalmente se asume que Spring registraría 1 bean (no dos) o que habría conflicto.

// ✅ Como “instancia Spring” normalmente: 1
// ⚠️ Pero el código tal cual es un “code smell” porque define el mismo bean por dos vías.

// C) Instancia estática manual (STATIC_SERVICE)
// private static final UserService STATIC_SERVICE = new UserService();


// Esto es clave:

// No la crea Spring

// La crea la JVM cuando se inicializa UserController (fase de class initialization del controlador).

// Ocurre una sola vez por ClassLoader.

// ✅ Instancias por aquí: 1







// 3) Entonces… ¿cuántas “veces se carga” y cuántas instancias hay?
// ✅ Carga de clase (JVM)

// UserService se carga 1 vez por ClassLoader.

// ✅ Instancias creadas (objetos new UserService())

// 1 instancia como bean de Spring (por @Bean o por @Service, pero no deberían coexistir bien)

// 1 instancia manual estática: STATIC_SERVICE

// ➡️ Total típico de instancias: 2

// 4) ¿Por qué @Autowired no usa el STATIC_SERVICE?

// Porque Spring solo puede inyectar cosas que estén en su contenedor (ApplicationContext).

// @Autowired private UserService userService; ⇒ inyecta el bean administrado por Spring

// STATIC_SERVICE ⇒ es un objeto “sueltito” (not managed) creado por new

// Esto provoca un problema clásico:

// Tendrás dos instancias distintas de UserService viviendo en la app.

// Si UserService tuviera estado, caches, conexiones, etc., se vuelve inconsistente.

// 5) Respuesta tipo examen (directa)

// La clase UserService se carga 1 vez (por el ClassLoader) porque la JVM no recarga clases repetidamente.

// Pero se crean 2 instancias de UserService: una por Spring (bean) y otra por el static final new UserService() en el controller (fuera del contenedor).

// Si quieres, te explico también qué pasaría en Spring Boot si no se permite bean overriding (normalmente obtienes error por tener @Service y @Bean duplicados).





















//3.0
//Arregla este issue: 
// java.lang.ClassNotFoundException:
// com.myapp.service.PaymentService$$EnhancerBySpringCGLIB$$a1b2c3d4
// 1) ¿Qué es PaymentService$$EnhancerBySpringCGLIB$$a1b2c3d4?

// Es una subclase generada en runtime por Spring usando CGLIB para poder interceptar llamadas:

// @Transactional

// @Cacheable

// @Async

// @Secured / AOP en general

// Ejemplo mental:

// Tu clase real: PaymentService

// Spring genera algo como: class PaymentService$$EnhancerBySpringCGLIB$$... extends PaymentService

// Esa clase proxy “envuelve” métodos para meter lógica transversal (transacciones, cache, etc.)

// Normal en cualquier app Spring… pero esa clase no existe en tu JAR/WAR, se crea en memoria.

// 2) ¿Por qué aparece ClassNotFoundException entonces?

// Porque algo está intentando cargar esa clase proxy por nombre, como si fuera una clase “real”.

// Eso casi siempre pasa por 1 de estas causas:

// Causa A (muy común): spring-boot-devtools activo en producción

// devtools habilita restart classloader para hot reload (dos ClassLoaders: base + restart).

// En prod, eso es veneno cuando:

// un objeto proxy creado por un classloader A

// termina siendo usado/serializado/guardado y luego leído por classloader B

// o el classloader A se “descarta” y la clase generada ya no está disponible

// Resultado: la app intenta resolver PaymentService$$EnhancerBySpringCGLIB$$... y no encuentra esa clase.

// 📌 Por eso el “modelo” dice “⚠️ NUNCA en producción”.