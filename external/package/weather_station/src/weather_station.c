#include <stdio.h>
#include "bmp280.h"
#include <unistd.h>
#include <math.h>

int init_bmp(struct bmp280_dev *bmp);
int read_sensor(struct bmp280_dev *bmp);
int init_gpio(int pin);
int change_gpio(unsigned char value);
int curl(int *temp);

int main(int argc, char* argv[])
{
    struct bmp280_dev bmp;
    int32_t i_temp, o_temp;

    init_gpio(967); 
    change_gpio(1);

   init_bmp(&bmp);
   while (1)
   {
	i_temp=roundf((float)read_sensor(&bmp)/100*10)/10;
	curl(&o_temp);
	printf("i_temp:%d o_temp:%d\n",i_temp, o_temp);
	if (o_temp<=-4 && i_temp<18)
		change_gpio(1);
	else
		change_gpio(0);
	sleep(10);
   }
   return 0;
}