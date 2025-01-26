#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <unistd.h>
#include <sys/ioctl.h>
#include <sys/stat.h>
#include <sys/types.h>
#define BUFFER_MAX 255
int init_gpio(int pin)
{
	char buffer[BUFFER_MAX];
	ssize_t bytes_written;
	int fd;
	fd = open("/sys/class/gpio/export", O_WRONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open export for writing!\n");
		return(-1);
	}
	
	bytes_written = snprintf(buffer, BUFFER_MAX, "%d", pin);
	write(fd, buffer, bytes_written);
	close(fd);
	
	fd = open("/sys/class/gpio/gpio967/direction", O_WRONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open direction for writing!\n");
		return(-1);
	}

	bytes_written = snprintf(buffer, BUFFER_MAX, "out");
	write(fd, buffer, bytes_written);
	close(fd);
	return 0;
}

int change_gpio(unsigned char value)
{
	char buffer[BUFFER_MAX];
	ssize_t bytes_written;
	int fd;
	fd = open("/sys/class/gpio/gpio967/value", O_WRONLY);
	if (-1 == fd) {
		fprintf(stderr, "Failed to open value for writing!\n");
		return(-1);
	}
	if (value==0)	
	bytes_written = snprintf(buffer, BUFFER_MAX, "0");
	else
	bytes_written = snprintf(buffer, BUFFER_MAX, "1");
	write(fd, buffer, bytes_written);	
	sleep(1);	
	/*bytes_written = snprintf(buffer, BUFFER_MAX, "0");
	write(fd, buffer, bytes_written);*/
	close(fd);
	return 0;
}