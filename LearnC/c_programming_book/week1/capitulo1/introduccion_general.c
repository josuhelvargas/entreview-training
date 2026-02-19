#include <stdio.h>   // para printf, scanf, fprintf
#include <stdlib.h>  // para exit

// Prototipos de las funciones de conversión
float temp_to_celsius(float fahrenheit_temperature);
float temp_to_fahrenheit(float celsius_temperature);
void using_get_chars();


int main(void) {
    using_get_chars();
    return 0;
}

// Convierte Fahrenheit a Celsius
float temp_to_celsius(float fahrenheit_temperature) {
    return (5.0f / 9.0f) * (fahrenheit_temperature - 32.0f);
}

// Convierte Celsius a Fahrenheit
float temp_to_fahrenheit(float celsius_temperature) {
    return (celsius_temperature * 9.0f / 5.0f) + 32.0f;
}



int using_thermometeres(){
    float height = 1.66f;
    int age = 34;
    char name[] = "Josue";
    int converterType = 0;
    float temperature = 0.0f;
    float result = 0.0f;

    // Imprimimos los datos iniciales
    printf("Mi nombre es %s,\nmi edad es %i,\ny mi altura es %.2f m\n\n",
           name, age, height);

    // Preguntamos al usuario qué conversión quiere
    printf("Selecciona el tipo de conversión:\n"
           "  1) Fahrenheit → Celsius\n"
           "  2) Celsius    → Fahrenheit\n"
           "Ingresa 1 o 2: ");
    if (scanf("%i", &converterType) != 1) {
        fprintf(stderr, "Error al leer la opción.\n");
        exit(EXIT_FAILURE);
    }
    if (converterType != 1 && converterType != 2) {
        printf("Opción inválida. Solo se permiten 1 o 2.\n");
        return 0;
    }

    // Leemos la temperatura a convertir
    printf("Ingresa el valor de la temperatura: ");
    if (scanf("%f", &temperature) != 1) {
        fprintf(stderr, "Error al leer la temperatura.\n");
        exit(EXIT_FAILURE);
    }

    // Realizamos la conversión según la opción
    if (converterType == 1) {
        result = temp_to_celsius(temperature);
    } else {
        result = temp_to_fahrenheit(temperature);
    }

    // Mostramos el resultado
    printf("El resultado de la conversión es: %.2f\n", result);
    return 0;
}


void using_get_chars(){
    //EOF: constante (normalmente -1) que indica “fin de archivo” o un error de lectura.
    printf("Inicia declaracion de int c para ingresar characters");
    int c;
    while ( (c = getchar()) !=  EOF){
        putchar(c);
    }//
}