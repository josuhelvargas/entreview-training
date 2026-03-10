#Antes de Cmake se utilizaba make el caul era una form aexplicita de desribir elrpcioeso de compilacion de progrmaas c y c++ 
#Pero era depdendiente del sistema ( cambiaba el contenido dependiendo del sistema)

#Cmake es un build system generator.
#Ejemplos de lo que puede generar:

# Sistema	   Genera
# Linux	        Makefiles
# Linux	        Ninja
# Mac	        Xcode  project
# Windows	    Visual Studio solution


# CMakeLists.txt
#       ↓
# CMake Parser
#       ↓
# Build Graph (targets + dependencies)
#       ↓
# Generator
#       ↓
# Makefile / Ninja / VisualStudio
#       ↓
# Compiler (gcc / clang / msvc)

############################################
#   Este Archivo CMakeLists Define:        #
############################################       
    # proyecto
    # compilador
    # targets
    # dependencias
    # flags
    # includes
    # librerías

cmake_minimum_required(VERSION 3.20)

project(Calculator)

add_executable(
    calculator
    calculator.cpp
)