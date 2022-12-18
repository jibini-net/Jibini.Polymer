# CMakeList.txt: CMake project for Jibini.Polymer.Grammar, include source and
# define project specific logic here.
#
cmake_minimum_required(VERSION 3.8)

# Included from the CMake docs for finding Flex.
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

# Find it here: https://cmake.org/cmake/help/latest/module/FindFLEX.html
#
file(MAKE_DIRECTORY "${CMAKE_CURRENT_BINARY_DIR}/lexer/")
find_package(FLEX 2.6.4)
FLEX_TARGET(Lexer "lexer/polymer.l" "${CMAKE_CURRENT_BINARY_DIR}/lexer/lexer.c"
    DEFINES_FILE "${CMAKE_CURRENT_BINARY_DIR}/lexer/lexer.h")

# Included from the CMake docs for finding Bison.
# Find it here: https://cmake.org/cmake/help/latest/module/FindBISON.html
#
file(MAKE_DIRECTORY "${CMAKE_CURRENT_BINARY_DIR}/parser/")
find_package(BISON 3.8.2)
BISON_TARGET(Parser "parser/polymer.y" "${CMAKE_CURRENT_BINARY_DIR}/parser/parser.c"
    DEFINES_FILE "${CMAKE_CURRENT_BINARY_DIR}/parser/parser.h")

ADD_FLEX_BISON_DEPENDENCY(Lexer Parser)
include_directories(${CMAKE_CURRENT_BINARY_DIR} ${CMAKE_CURRENT_SOURCE_DIR})

# Add source to this project's executable.
add_executable(Jibini.Polymer.Grammar "Jibini.Polymer.Grammar.c"
    "Jibini.Polymer.Grammar.h"
    ${FLEX_Lexer_OUTPUTS}
    ${BISON_Parser_OUTPUTS})

target_link_libraries(Jibini.Polymer.Grammar Jibini.Polymer.Shared)

set_property(TARGET Jibini.Polymer.Grammar PROPERTY C_STANDARD 17)
set_property(TARGET Jibini.Polymer.Grammar PROPERTY CXX_STANDARD 20)
