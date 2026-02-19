#include <stdio.h>

int main(void) {
    int x = 42;
    int *p = &x;
    
    printf("Valor de x: %d\n", x);
    printf("Dirección de x: %p\n", (void *)&x);
    printf("Valor de p: %p\n", (void *)p);
    printf("Lo que apunta p: %d\n", *p);

    *p = 100;
    printf("x tras *p=100: %d\n", x);
    return 0;
}