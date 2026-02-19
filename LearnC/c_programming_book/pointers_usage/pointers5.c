#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>

void create_array(int **arr, size_t len) {
    
}

int main(void) {
    int *data = NULL;
    create_array(&data, 4);
    for (size_t i = 0; i < 4; i++) {
        data[i] = (int)(i + 1) * 10;
        printf("%d ", data[i]);
    }
    printf("\n");
    free(data);
    return 0;
}