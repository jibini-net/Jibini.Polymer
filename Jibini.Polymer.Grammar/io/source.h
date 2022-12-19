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

#define MAX_LINE_LEN 2048

/**
 * Begins reading the stream of characters, preparing to provide characters.
 * 
 * Not thread safe.
 * 
 * @param file Source for input stream of characters.
 */
void begin_file(FILE *file);

/**
 * Returns the next character from the input stream, reading more if the current
 * buffer is exhausted.
 * 
 * Not thread safe.
 */
char read_next();

/**
 * Writes a message surrounded by and pointing to a particular position in the
 * source code. Uses the current position of the scanner.
 * 
 * Not thread safe.
 * 
 *  @param file File to which source lines and message are written.
 *  @param message Helpful message related to the area in input source.
 */
void write_message(FILE *file, char *message);

/**
 * Writes a message surrounded by and pointing to a particular position in the
 * source code. May result in more data being read, if the requested position
 * has not yet been reached.
 * 
 * Not thread safe.
 * 
 *  @param file File to which source lines and message are written.
 *  @param message Helpful message related to the area in input source.
 *  @param line Line number (zero-indexed) at which the error occurred.
 */
void write_message_at(FILE *file, char *message, size_t line, size_t col);

/**
 * Resets the static state and releases the allocated memory for stored lines.
 * 
 * Not thread safe.
 */
void free_file();
