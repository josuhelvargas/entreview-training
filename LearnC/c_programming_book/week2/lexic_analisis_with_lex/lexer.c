#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <ctype.h>


//Escribe una función get_token() que lea carácter a carácter de un buffer y devuelva un enum Token { TOK_ID, TOK_INT, TOK_PLUS, … } junto con el lexema.
typedef enum {
    TOK_ID,
    TOK_INT,
    TOK_PLUS,
    TOK_MINUS,
    TOK_MULTIPLY,
    TOK_DIVIDE,
    TOK_ASSIGN,
    TOK_SEMICOLON,
    TOK_LPAREN,
    TOK_RPAREN,
    TOK_UNKNOWN,
    TOK_EOF
} Token;

typedef struct {
    Token token;
    char lexeme[100];
} TokenInfo;


TokenInfo get_token(const char **buffer);
void skip_whitespace(const char **buffer);
void print_token(TokenInfo token_info);

// Función principal
int main() {
    const char *buffer = "int a = 5 + 3;"; // Cadena de entrada
    TokenInfo token_info;

    while (1) {
        token_info = get_token(&buffer);
        print_token(token_info);
        if (token_info.token == TOK_EOF) {
            break;
        }
    }

    return 0;
}
// Función para obtener el siguiente token del buffer
TokenInfo get_token(const char **buffer) {
    TokenInfo token_info;
    skip_whitespace(buffer);

    if (**buffer == '\0') {
        token_info.token = TOK_EOF;
        return token_info;
    }

    if (isalpha(**buffer)) { // Identificador
        int i = 0;
        while (isalnum(**buffer) && i < sizeof(token_info.lexeme) - 1) {
            token_info.lexeme[i++] = **buffer;
            (*buffer)++;
        }
        token_info.lexeme[i] = '\0';
        token_info.token = TOK_ID;
    } 
    
    else if (isdigit(**buffer)) { // Número entero
        int i = 0;
        while (isdigit(**buffer) && i < sizeof(token_info.lexeme) - 1) {
            token_info.lexeme[i++] = **buffer;
            (*buffer)++;
        }
        token_info.lexeme[i] = '\0';
        token_info.token = TOK_INT;
    } 
    
    else {
        switch (**buffer) {
            case '+':
                token_info.token = TOK_PLUS;
                break;
            case '-':
                token_info.token = TOK_MINUS;
                break;
            case '*':
                token_info.token = TOK_MULTIPLY;
                break;
            case '/':
                token_info.token = TOK_DIVIDE;
                break;
            case '=':
                token_info.token = TOK_ASSIGN;
                break;
            case ';':
                token_info.token = TOK_SEMICOLON;
                break;
            case '(':
                token_info.token = TOK_LPAREN;
                break;
            case ')':
                token_info.token = TOK_RPAREN;
                break;
            default:
                token_info.token = TOK_UNKNOWN;
                break;
        }
        snprintf(token_info.lexeme, sizeof(token_info.lexeme), "%c", **buffer);
        (*buffer)++;
    }

    return token_info;
}

// Función para omitir espacios en blanco
void skip_whitespace(const char **buffer) {
    while (isspace(**buffer)) {
        (*buffer)++;
    }
}
// Función para imprimir el token y su lexema
void print_token(TokenInfo token_info) {
    switch (token_info.token) {
        case TOK_ID:
            printf("Token: ID, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_INT:
            printf("Token: INT, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_PLUS:
            printf("Token: PLUS, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_MINUS:
            printf("Token: MINUS, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_MULTIPLY:
            printf("Token: MULTIPLY, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_DIVIDE:
            printf("Token: DIVIDE, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_ASSIGN:
            printf("Token: ASSIGN, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_SEMICOLON:
            printf("Token: SEMICOLON, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_LPAREN:
            printf("Token: LPAREN, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_RPAREN:
            printf("Token: RPAREN, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_UNKNOWN:
            printf("Token: UNKNOWN, Lexeme: %s\n", token_info.lexeme);
            break;
        case TOK_EOF:
            printf("Token: EOF\n");
            break;
    }
}
// Compilar con: gcc -o lexer lexer.c
// Ejecutar con: ./lexer
// El programa imprimirá los tokens y sus lexemas a medida que los analiza.
// Puedes modificar la cadena de entrada en la función main para probar diferentes casos.       
