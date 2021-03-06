\ core.fs - core words

\ ( "ccc<paren>" -- )
\ Compiler
\ skip everything up to the closing bracket on the same line
: (
    d=            \ ( ?  ? )
    $29 parse     \ ( ?  addr u )
    d-1 =d        \ ( ? )
; immediate


( -- )
\ make most current word compile only
: :c
    $F7FF widf
;

( -- )
\ make most current word inlinned
: inlined
    $FEFF widf
;

( -- )
\ make most current word immediate and compile only
: :ic
    $77FF widf
;

( -- wid ) ( C: x "<spaces>name" -- )
\ create a dictionary entry and register in current word list
: rword
    (create)      ( nfa )
    current+      ( wid )
;

\ inlinned assembly routines

( -- icell )
\ instruction cell size 
rword icell inlined
    ] 2 [
    _bxlr ,

( n -- n+icell )
\ add instruction stack cell size to n
rword icell+ inlined
    ] +2 [
    _bxlr ,
  
( n -- n-icell )
\ subtract instruction stack cell size from n
rword icell- inlined
    ] 2- [
    _bxlr ,

( n -- n*icell )
\ multiply n by instruction stack cell size 
rword icell* inlined
    ] *2 [
    _bxlr ,

( -- dcell )
\ push data stack cell size 
rword dcell inlined
    ] 4 [
    _bxlr ,

( n -- n+dcell )
\ add data stack cell size to n
rword dcell+ inlined
    ] +4 [
    _bxlr ,

( n -- n-dcell )
\ subtract data stack cell size from n
rword dcell- inlined
    ] 4- [
    _bxlr ,

( n -- n*dcell )
\ multiply n by data stack cell size 
rword dcell* inlined
    ] *4 [
    _bxlr ,

( C:"<spaces>name" -- 0 | nfa )
\ Dictionary
\ search dictionary for name, returns nfa if found or 0 if not found
: find
    pname findw
;

\ search dictionary for name, returns XT or 0
: 'f  ( "<spaces>name" -- XT XTflags )
    find
    nfa>xtf
;


\ search dictionary for name, returns XT
: '  ( "<spaces>name" -- XT )
    'f  ( XT XTflags )
    =d  ( XT )
;

( -- ) ( C: "<space>name" -- )
\ Compiler
\ what ' does in the interpreter mode, do in colon definitions
\ compiles xt as literal
: [']
    '
    #,
; :ic


( -- ) ( C: "<space>name" -- )
\ Compiler
\ what 'f does in the interpreter mode, do in colon definitions
\ and xt and flag are compiled as two literals
: ['f]
    'f
    d= d1
    #,
    \ compile literal of 'f push
    [ 'f d= d= d1 #, ]
    d=
    [ d0 #, d-1 =d ]
    xt,
    d0 #, d-1 =d
; :ic