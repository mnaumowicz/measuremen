CC=gcc
CFLAGS=-I.

all: *.c
	$(CC) -Os -lcjson -lm -lcurl -Wall  *.c -o weather_station


clean:
	rm -f a.out weather_station
	rm -f *.o
