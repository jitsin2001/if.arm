# Makefile
INCS = *.S

all: if

if : if.o input.o
	gcc -Os -ldl -lm -o $@ $+

if.o : main.S $(INCS)
	as -o $@ $<

lst:
	objdump -h -x -D -S if > lst.txt

clean:
	rm -vf if *.o
