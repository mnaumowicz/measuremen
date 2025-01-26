#include <cjson/cJSON.h>
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <unistd.h>
#include <sys/ioctl.h>
#include <string.h>
#include <sys/stat.h>
#include <sys/types.h>

int read_temp(const char * const weather_data, int *temp)
{
    const cJSON *current = NULL;
    int status = 0;
    cJSON *weather_data_json = cJSON_Parse(weather_data);
    if (weather_data_json == NULL)
    {
        const char *error_ptr = cJSON_GetErrorPtr();
        if (error_ptr != NULL)
        {
            fprintf(stderr, "Error before: %s\n", error_ptr);
        }
        status = -1;
        goto end;
    }

    current = cJSON_GetObjectItemCaseSensitive(weather_data_json, "current");
    if (cJSON_IsString(current) && (current->valuestring != NULL))
    {
        printf("Checking weather_data \"%s\"\n", current->valuestring);
    }

        cJSON *temp_c = cJSON_GetObjectItemCaseSensitive(current, "temp_c");

        if (!cJSON_IsNumber(temp_c))
        {
            status = -1;
            goto end;
        }
//printf("%f\n",temp_c->valuedouble);
*temp = (int)temp_c->valuedouble;

end:
    cJSON_Delete(weather_data_json);
    return status;
}