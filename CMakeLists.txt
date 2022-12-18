# CMakeList.txt: Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required(VERSION 3.8)

project("Jibini.Polymer")

# Include sub-projects in a correct topological order.
add_subdirectory("Jibini.Polymer.Shared")
add_subdirectory("Jibini.Polymer.Grammar")
add_subdirectory("Jibini.Polymer.LlvmBackend")
add_subdirectory("Jibini.Polymer.Frontend")