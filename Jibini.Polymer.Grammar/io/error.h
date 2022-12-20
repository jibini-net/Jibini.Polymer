// Copyright (c) 2022 Zach Goethel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

#pragma once

#include <stdlib.h>
#include <stdio.h>

/**
 * Prints the current error message according to the error state, then exits.
 * 
 * @param message Error message to print if the syscall was not successful.
 */
#define DIE_SYS(message) (perror(message), exit(EXIT_FAILURE))

/**
 * Prints the provided error message to standard error, then exits.
 * 
 * @param message Error message to print if the call was not successful.
 */
#define DIE(message) (fprintf(stderr, "%s\n", message), exit(EXIT_FAILURE))

/**
 * Substitutes the result of the operation with a default value if the operation
 * was not completed successfully.
 * 
 * @param message Error message to print if the syscall was not successful.
 */
#define REPLACE(default) ({ result = default; })

/**
 * Attempts the syscall, evaluating an action if the result indicates error.
 * 
 *  @param T Resulting type of the syscall operation, passed through the macro.
 *  @param operation Syscall statement to attempt and check the result.
 *  @param on_fail Expression which will be lazily evaluated in the case of an
 *      error. This can call error handling or exit the entire process.
 */
#define TRY(T, operation, on_fail) {\
    T result = operation;\
    result == -1 ? (on_fail, result) : result;\
}
