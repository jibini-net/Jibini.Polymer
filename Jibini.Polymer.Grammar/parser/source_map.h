// Copyright (c) 2022-2023 Zach Goethel
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

#include "io/source.h"

/**
 * Marks the association between a character range in source and a parsed token
 * or flow control structure.
 */
typedef struct source_tag_t
{
    /**
     * Line in memory where the element occurs.
    */
    source_buff_t *line;

    /**
     * Pointer to first character of source element in the line.
     */
    char *start;

    /**
     * Number of characters consumed by the tagged portion.
     */
    size_t length;

    /**
     * Identifier for the type of source element.
     */
    char *name_tag;

    /**
     * Unique number linking multiple tagged portions to one parent.
     */
    unsigned long uid;

    /**
     * Next stored tagged range of source, if any.
     */
    struct source_tag_t *next;
} source_tag_t;

