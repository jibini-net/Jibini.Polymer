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

#include "source.h"

#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <assert.h>
#include <stdbool.h>
#include <fcntl.h>

#include "error.h"

/**
 * Linked series of lines of source with lines of arbitrary length.
 */
typedef struct source_buff_t
{
    char *line;
    struct source_buff_t *next;
} source_buff_t;

// Input source file being read, which should only be set once
FILE *static_file = NULL;

// Line zero of the stored source, or `NULL` if no source is loaded
source_buff_t *source_head = NULL;
// Location in chain to place the next element
source_buff_t **next_line = &source_head;
size_t lines = 0;
// Reference to the next character to provide to the scanner
char *line = NULL, *next_col = NULL;

void begin_file(FILE *file)
{
    assert(!static_file);
    static_file = file;
}

extern void shutdown();

// Tries to read the next line of source, with a maximum length
bool _read_line()
{
    char buffer[MAX_LINE_LEN + 2];
    // Needs to be nulled for length check
    buffer[MAX_LINE_LEN] = '\0';

    char *text = fgets(&buffer[0], sizeof(buffer), static_file);
    // Checks error and EOF state
    if (!text && !feof(static_file))
    {
        shutdown();
        DIE("Error reading from character stream");
    } else if (!text)
    {
        return false;
    }

    if (buffer[MAX_LINE_LEN] && buffer[MAX_LINE_LEN] != '\n')
    {
        write_message(stderr, "Exceeded the maximum line length");
        shutdown();
        exit(EXIT_FAILURE);
    }

    *next_line = (source_buff_t *)malloc(sizeof(source_buff_t));
    (*next_line)->line = strdup(buffer);
    (*next_line)->next = NULL;
    lines++;

    line = (*next_line)->line;
    next_col = &line[0];
    next_line = &(*next_line)->next;
    return true;
}

char read_next()
{
    // Attempt to read more at end of line
    if (!next_col || !(*next_col))
    {
        _read_line();
    }
    // Return next character or EOF, avoid leaving bounds
    return *next_col ? *(next_col++) : EOF;
}

void free_file()
{
    // Free entire chain of stored source code
    for (source_buff_t *line = source_head, *next; line; line = next)
    {
        free(line->line);

        next = line->next;
        free(line);
    }

    // Reset state
    source_head = NULL;
    next_line = NULL;
    next_col = NULL;

    static_file = NULL;
}

#define MIN(a, b) ((a) < (b) ? (a) : (b))
#define MAX(a, b) ((a) > (b) ? (a) : (b))

void write_message(FILE *file, char *message)
{
    write_message_at(file, message, lines - 1, next_col - line - 1);
}

void write_message_at(FILE *file, char *message, size_t line, size_t col)
{
    fprintf(file, "Line %lu, column %lu:\n", line + 1, col + 1);

    // Print out the line if it's loaded in memory
    source_buff_t *find = source_head;
    for (int i = 0; find && i < line; (find = find->next, i++));
    if (find)
    {
        fprintf(file, "%s", find->line);

        for (int i = 0; i < col; i++) fprintf(file, " ");
        fprintf(file, "^\n");
    }

    fprintf(file, "%s\n", message);
}
