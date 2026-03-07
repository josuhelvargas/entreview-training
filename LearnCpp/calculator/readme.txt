Fase 1 — Fundamentos reales de programa C++

Proyecto 1: Mini CLI Toolkit
Un ejecutable de consola con módulos pequeños.


Funciones:
sumar/restar/multiplicar/dividir
manipulación de strings
parseo de argumentos
logger simple
manejo básico de errores


Qué aprenderás:
estructura include/, src/, app/
.h vs .cpp
CMake
translation units
compilación y linking
clases simples
std::string, std::vector
separación en módulos


Este proyecto parece sencillo, pero está perfecto para aprender a organizar código en C++ como proyecto real.


include/domain/calculator.h
include/domain/expression_parser.h
include/domain/calculator_error.h

include/app/calculator_controller.h

include/ui/main_window.h




src/domain/calculator.cpp
src/domain/expression_parser.cpp
src/domain/calculator_error.cpp

src/app/calculator_controller.cpp

src/ui/main.cpp
src/ui/main_window.cpp


