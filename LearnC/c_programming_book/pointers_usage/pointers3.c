#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

// Function prototypes
int usingPointersToPointers();
int usingCharAddresses();
int usingIntAddresses();
int usingStringsInC();

int main(){
    usingPointersToPointers();

}

int usingCharAddresses(){
    char str[20] = "Hello, World!";
    char *p = str; // p points to the first character of str
    for (int i = 0; i < strlen(str); i++){
        printf("str[%d] = %c\n", i, *(p + i)); // Accessing characters using pointer arithmetic
        printf("str[%d] = %p\n", i, &str[i]); // Corrected: Use %p for pointer
    }
    return 0;
}

int usingIntAddresses(){
    int Arr[5] = {1, 2, 3, 4, 5};
    int *p = Arr; // p points to the first element of Arr
    for (int i = 0; i < 5; i++){
        printf("Arr[%d] = %d\n", i, *(p + i)); // Accessing elements using pointer arithmetic
        printf("Arr[%d] = %p\n", i, &Arr[i]); // Corrected: Use %p for pointer
    }
    return 0;
}

int usingStringsInC(){
    char str1[20] = "Hello";
    char str2[20] = "World";
    char *p1 = str1;
    char *p2 = str2;
    printf("String 1: %s\n", p1);
    printf("String 2: %s\n", p2);
    strcat(p1, p2); // Concatenating strings using pointer arithmetic
    printf("Concatenated String: %s\n", p1);
    return 0;
}

int usingPointersToPointers(){
    int x = 10;
    int *p = &x; // p points to x address
    int **pp = &p; // pp points to p (which points to x address)
    printf("Value of x: %d\n", x);
    printf("Value of x pointer: %d\n" ,*p); // Corrected: Use %p for pointer
    printf("Value of x using pointer to pointer: %d\n", **pp); // Corrected: Use %p for pointer
    return 0;
}