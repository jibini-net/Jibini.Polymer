# CMakeList.txt: CMake project for Jibini.Polymer.Shared, include source and
# define project specific logic here.
#
cmake_minimum_required(VERSION 3.8)

# Add source to this project's executable.
add_library(Jibini.Polymer.Shared STATIC "Jibini.Polymer.Shared.c"
    "Jibini.Polymer.Shared.h")

set_property(TARGET Jibini.Polymer.Shared PROPERTY C_STANDARD 17)
set_property(TARGET Jibini.Polymer.Shared PROPERTY CXX_STANDARD 20)
