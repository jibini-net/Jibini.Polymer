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

#include "pincludes.h"

void *tok__Prog__a(void *_top_levels)
{
    return NULL;
}

void *tok__Prog__b()
{
    return NULL;
}

void *tok__TopLevels__a(void *_top_levels, void *_top_level)
{
    return NULL;
}

void *tok__TopLevels__b(void *_top_levels)
{
    return NULL;
}

void *tok__TopLevel__a(void *_type_decl)
{
    return NULL;
}

void *tok__TopLevel__b(void *_declaration)
{
    return NULL;
}

void *tok__TopLevel__c(void *_function)
{
   return NULL;
}

void *tok__Vars__a(void *_vars, void *_var)
{
    return NULL;
}

void *tok__Vars__b(void *_var)
{
    return NULL;
}

void *tok__Var__a(void *_ident, void *_type)
{
    free(_ident);
}

void *tok__Var__b(void *_ident, void *_type)
{
    free(_ident);
}
