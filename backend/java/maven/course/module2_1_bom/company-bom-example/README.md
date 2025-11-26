# Ejemplo práctico — BOM corporativo y microservicios

Este ejemplo muestra cómo una empresa define un **BOM propio** (`company-bom`) y dos microservicios
(`orders-service` y `payments-service`) que **usan las mismas versiones** de librerías sin repetirlas.

Estructura:

- `company-bom/`
  - `pom.xml` — BOM corporativo con versiones de:
    - Spring Boot starter web y actuator
    - Logback
    - Lombok
    - JUnit Jupiter
- `orders-service/`
  - `pom.xml` que **importa el BOM** y declara dependencias **sin versión**
  - `OrdersServiceApplication` con endpoint `/health`
- `payments-service/`
  - `pom.xml` que **importa el BOM** y declara dependencias **sin versión**
  - `PaymentsServiceApplication` con endpoint `/health`

## Cómo usarlo

1. Instalar el BOM en tu repo local de Maven:
   ```bash
   cd company-bom
   mvn clean install
   ```

2. Compilar y probar `orders-service`:
   ```bash
   cd ../orders-service
   mvn clean test
   mvn spring-boot:run
   ```
   Visita: `http://localhost:8080/health` → `orders-service OK`

3. Compilar y probar `payments-service`:
   ```bash
   cd ../payments-service
   mvn clean test
   mvn spring-boot:run
   ```
   Visita: `http://localhost:8080/health` (o cambia el puerto) → `payments-service OK`

## Ejercicio sugerido

1. Cambia `<spring.boot.version>` en `company-bom/pom.xml` a otra versión compatible.
2. Ejecuta de nuevo:
   ```bash
   cd company-bom
   mvn clean install

   cd ../orders-service
   mvn dependency:tree | grep spring-boot-starter-web

   cd ../payments-service
   mvn dependency:tree | grep spring-boot-starter-web
   ```
3. Verifica que **ambos servicios** usan la nueva versión automáticamente.