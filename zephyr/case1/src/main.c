/*
 * Copyright (c) 2012-2014 Wind River Systems, Inc.
 * Copyright (c) 2021 Nordic Semiconductor ASA
 *
 * SPDX-License-Identifier: Apache-2.0
 */

#include <errno.h>
#include <math.h>
#include <string.h>

#include <zephyr/device.h>
#include <zephyr/devicetree.h>
#include <zephyr/drivers/sensor.h>
#include <sys/time.h>
#include <unistd.h>
#include <zephyr/kernel.h>
/*
 * Get a device structure from a devicetree node with compatible
 * "bosch,bme280". (If there are multiple, just pick one.)
 */
static const struct device *get_bme280_device(void)
{
	const struct device *dev = DEVICE_DT_GET_ANY(bosch_bme280);

	if (dev == NULL) {
		/* No such node, or the node does not have status "okay". */
		printk("\nError: no device found.\n");
		return NULL;
	}

	if (!device_is_ready(dev)) {
		printk("\nError: Device \"%s\" is not ready; "
		       "check the driver initialization logs for errors.\n",
		       dev->name);
		return NULL;
	}

	printk("Found device \"%s\", getting sensor data\n", dev->name);
	return dev;
}

int main(void)
{
	const struct device *dev = get_bme280_device();
	double t;
	double t_rounded;
	int no = 0, device_id = 1234;
	struct timespec tv;
	char *str;

	tv.tv_sec = 1712241917;
	tv.tv_nsec = 0;
	clock_settime(CLOCK_REALTIME, &tv);

	if (dev == NULL) {
		return 0;
	}
	printk("no;date;device id;data;status\n");
	while (1) {
		struct sensor_value temp, press, humidity;
		struct timeval tv;
		int res = gettimeofday(&tv, NULL);
		time_t now = time(NULL);
		struct tm tm;
		localtime_r(&now, &tm);

		if (res < 0) {
			printk("Error in gettimeofday(): %d\n", errno);
			return 0;
		}
		sensor_sample_fetch(dev);
		sensor_channel_get(dev, SENSOR_CHAN_AMBIENT_TEMP, &temp);
		sensor_channel_get(dev, SENSOR_CHAN_PRESS, &press);
		sensor_channel_get(dev, SENSOR_CHAN_HUMIDITY, &humidity);
		t = sensor_value_to_double(&temp);
		t_rounded = ROUND_UP(t * 1000.0, 1) / 1000.0;

		str = asctime(&tm);
		str[strlen(str) - 1] = 0;
		printk("%d;%s;%d;%d;", no, str, device_id, temp.val1 + 1);
		if ((temp.val1 + 1) > 38)
			printk("too high\n");
		else if ((temp.val1 + 1) < 25)
			printk("too low\n");
		else
			printk("normal\n");
		k_sleep(K_MSEC(1000));
		no++;
	}
}
