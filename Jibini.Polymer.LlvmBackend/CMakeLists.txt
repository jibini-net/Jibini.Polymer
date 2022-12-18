## Copyright (c) 2022 Zach Goethel
## 
## Permission is hereby granted, free of charge, to any person obtaining a copy
## of this software and associated documentation files (the "Software"), to deal
## in the Software without restriction, including without limitation the rights
## to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
## copies of the Software, and to permit persons to whom the Software is furnished
## to do so, subject to the following conditions:
## 
## The above copyright notice and this permission notice shall be included in 
## all copies or substantial portions of the Software.
## 
## THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
## IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
## FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
## AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
## LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
## FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
## IN THE SOFTWARE.

# CMakeList.txt: CMake project for Jibini.Polymer.Llvm, include source and
# define project specific logic here.
#
cmake_minimum_required(VERSION 3.8)

# Add source to this project's library.
add_executable(Jibini.Polymer.LlvmBackend "Jibini.Polymer.LlvmBackend.cpp"
    "Jibini.Polymer.LlvmBackend.h")

target_link_libraries(Jibini.Polymer.LlvmBackend Jibini.Polymer.Shared)

set_property(TARGET Jibini.Polymer.LlvmBackend PROPERTY C_STANDARD 17)
set_property(TARGET Jibini.Polymer.LlvmBackend PROPERTY CXX_STANDARD 20)

# Included from the LLVM documentation for CMake projects.
# Find it here: https://llvm.org/docs/CMake.html#embedding-llvm-in-your-project
#
find_package(LLVM REQUIRED CONFIG)

message(STATUS "Found LLVM ${LLVM_PACKAGE_VERSION}")
message(STATUS "Using LLVMConfig.cmake in: ${LLVM_DIR}")

include_directories(${LLVM_INCLUDE_DIRS})
separate_arguments(LLVM_DEFINITIONS_LIST NATIVE_COMMAND ${LLVM_DEFINITIONS})
add_definitions(${LLVM_DEFINITIONS_LIST})

# Find the libraries that correspond to the LLVM components
# that we wish to use
llvm_map_components_to_libnames(llvm_libs support core irreader)

# Link against LLVM libraries
target_link_libraries(Jibini.Polymer.LlvmBackend ${llvm_libs})
