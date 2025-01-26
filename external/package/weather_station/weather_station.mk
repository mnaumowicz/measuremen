WEATHER_STATION_VERSION = 1.0
WEATHER_STATION_SITE = $(BR2_EXTERNAL_EXT_PATH)/package/weather_station/src
WEATHER_STATION_SITE_METHOD = local

define WEATHER_STATION_BUILD_CMDS
	$(MAKE) CC="$(TARGET_CC)" LD="$(TARGET_LD)" -C $(@D)
endef

define WEATHER_STATION_INSTALL_TARGET_CMDS
	$(INSTALL) -D -m 0755 $(@D)/weather_station $(TARGET_DIR)/usr/bin
endef

$(eval $(generic-package))