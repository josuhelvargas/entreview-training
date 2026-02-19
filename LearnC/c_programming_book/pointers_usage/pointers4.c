#include <stdlib.h>
#include <stdio.h>
#include <string.h>

int main(){
    int value = 5;
    increment(&value); // al llamar la funcion se pasa la direccion de memoria
}

increment(int* value){ //en la imple,enmtacion obviamente se usa * para indicar el puntero
    *value = *value + 1;
}
