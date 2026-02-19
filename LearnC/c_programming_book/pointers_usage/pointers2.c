#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

int main(){
    float Pi = 3.14;
    float *f = &Pi;


    printf("Pi = %f\n", Pi);
    printf("Pi = %p\n", f); 
    return 0;
}
