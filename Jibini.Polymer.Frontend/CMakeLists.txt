# CMakeList.txt: CMake project for Jibini.Polymer.Frontend, include source and
# define project specific logic here.
#
cmake_minimum_required(VERSION 3.8)

# Add source to this project's executable.
add_executable(Jibini.Polymer.Frontend "Jibini.Polymer.Frontend.c"
    "Jibini.Polymer.Frontend.h")

target_link_libraries(Jibini.Polymer.Frontend Jibini.Polymer.Shared)

set_property(TARGET Jibini.Polymer.Frontend PROPERTY CXX_STANDARD 20)
