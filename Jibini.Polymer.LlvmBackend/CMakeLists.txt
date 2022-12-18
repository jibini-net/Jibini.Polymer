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
