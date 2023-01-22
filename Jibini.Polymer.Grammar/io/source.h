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
 * Linked series of lines of source with lines of arbitrary length.
 */
typedef struct source_buff_t
{
    /**
     * Source content for this line of code.
     */
    char *line;

    /**
     * Remember the line number (zero-indexed).
    */
    size_t index;

    /**
     * Next stored line of source, if any.
     */
    struct source_buff_t *next;
} source_buff_t;

/**
 * Begins reading the stream of characters, preparing to provide characters.
 * 
 * Not thread safe.
 * 
 *  @param file Source for input stream of characters.
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
 * Provides access to the line currently being parsed.
 */
source_buff_t *curr_line();

/**
 * Provides access to the index of the current character being scanned.
 */
size_t curr_col();

/**
 * Writes a message surrounded by and pointing to a particular position in the
 * source code. Uses the provided source line.
 * 
 *  @param file File to which source lines and message are written.
 *  @param mesg Helpful message related to the area in input source.
 *  @param line Reference to stored source content for the line.
 *  @param col Column number (zero-index) at which the error occurred.
 */
void write_mesg_for(FILE *file, char *mesg, source_buff_t *line, size_t col);

/**
 * Writes a message surrounded by and pointing to a particular position in the
 * source code. Uses the current position of the scanner.
 * 
 * Not thread safe.
 * 
 *  @param file File to which source lines and message are written.
 *  @param message Helpful message related to the area in input source.
 */
void write_mesg(FILE *file, char *mesg);

/**
 * Writes a message surrounded by and pointing to a particular position in the
 * source code. May result in more data being read, if the requested position
 * has not yet been reached.
 * 
 * Not thread safe.
 * 
 *  @param file File to which source lines and message are written.
 *  @param mesg Helpful message related to the area in input source.
 *  @param line Line number (zero-indexed) at which the error occurred.
 *  @param col Column number (zero-index) at which the error occurred.
 */
void write_mesg_at(FILE *file, char *mesg, size_t line, size_t col);

/**
 * Resets the static state and releases the allocated memory for stored lines.
 * 
 * Not thread safe.
 */
void free_file();
