# CMakeList.txt: CMake project for Jibini.Polymer.Grammar, include source and
# define project specific logic here.
#
cmake_minimum_required(VERSION 3.8)

# Included from the CMake docs for finding Flex.
# Find it here: https://cmake.org/cmake/help/latest/module/FindFLEX.html
#
find_package(FLEX 2.6.4)
FLEX_TARGET(Lexer "lexer/polymer.l" "lexer/lexer.c"
    DEFINES_FILE "lexer/defines.h")

message(STATUS "Found LLVM ${LLVM_PACKAGE_VERSION}")
message(STATUS "Using LLVMConfig.cmake in: ${LLVM_DIR}")

# Included from the CMake docs for finding Bison.
# Find it here: https://cmake.org/cmake/help/latest/module/FindBISON.html
#
find_package(BISON 3.8.2)
BISON_TARGET(Parser "parser/polymer.y" "parser/parser.c"
    DEFINES_FILE "parser/defines.h")

ADD_FLEX_BISON_DEPENDENCY(Lexer Parser)

# Add source to this project's executable.
add_executable(Jibini.Polymer.Grammar "Jibini.Polymer.Grammar.c"
    "Jibini.Polymer.Grammar.h"
    ${BISON_MyParser_OUTPUTS}
    ${FLEX_MyScanner_OUTPUTS})

target_link_libraries(Jibini.Polymer.Grammar Jibini.Polymer.Shared)

set_property(TARGET Jibini.Polymer.Grammar PROPERTY CXX_STANDARD 20)
