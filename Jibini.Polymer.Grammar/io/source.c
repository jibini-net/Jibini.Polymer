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

#include "Jibini.Polymer.Grammar.h"
#include "error.h"

// Input source file being read, which should only be set once
FILE *static_file = NULL;

// Line zero of the stored source, or `NULL` if no source is loaded
source_buff_t *source_head = NULL;
// Current location in chain and place to add a new element
source_buff_t *line = NULL, **next_line = &source_head;
size_t lines = 0;
// Reference to the next character to provide to the scanner
char *next_col = NULL;

void begin_file(FILE *file)
{
    assert(!static_file);
    
    static_file = file;
}

// Tries to read the next line of source, with a maximum length
bool _read_line()
{
    assert(static_file);
    
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
        write_mesg(stderr, "Exceeded the maximum line length");
        shutdown();
        exit(EXIT_FAILURE);
    }

    *next_line = (source_buff_t *)malloc(sizeof(source_buff_t));
    (*next_line)->line = strdup(buffer);
    (*next_line)->index = lines;
    (*next_line)->next = NULL;

    line = *next_line;
    lines++;
    
    next_col = line->line;
    next_line = &(*next_line)->next;
    return true;
}

char read_next()
{
    assert(static_file);
    
    // Attempt to read more at end of line
    if (!next_col || !(*next_col))
    {
        _read_line();
    }
    // Return next character or EOF, avoid leaving bounds
    return (next_col && *next_col) ? *(next_col++) : EOF;
}

void free_file()
{
    assert(static_file);
    
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

void write_mesg_for(FILE *file, char *mesg, source_buff_t *line, size_t col)
{
    fprintf(file, "Line %lu, column %lu:\n", line->index + 1, col + 1);
    if (line)
    {
        fprintf(file, "%s", line->line);

        for (int i = 0; i < col; i++) fprintf(file, " ");
        fprintf(file, "^\n");
    }

    fprintf(file, "%s\n", mesg);
}

void write_mesg(FILE *file, char *mesg)
{
    assert(static_file);
    
    write_mesg_for(file, mesg, line, next_col - line->line - 1);
}

void write_mesg_at(FILE *file, char *mesg, size_t line, size_t col)
{
    assert(static_file);

    // Print out the line if it's loaded in memory
    source_buff_t *find = source_head;
    for (int i = 0; find && i < line; (find = find->next, i++));

    write_mesg_for(file, mesg, find, col);
}
