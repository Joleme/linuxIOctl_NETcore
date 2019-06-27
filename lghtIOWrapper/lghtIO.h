#pragma once
#include <linux/ioctl.h>
//enum to describe possible colors of each traffic light
typedef enum lghtColor_t { Red, Ylw, Grn, NoneColor = 987 }lghtColor;

/*structure to align request from user and answer back*/
typedef struct
{
	int idx;
	lghtColor clrT;
} lghtIO_arg_t;

/*declaring types of request*/
#define getLghtByIdx _IOWR('q', 1, lghtColor *)
#define resetLghtByIdx _IOWR('q', 2, lghtColor *)
#define setLghtByIdx _IOW('q', 3, lghtIO_arg_t *)

/*converting color enum to string*/
static char* enumToColor(lghtColor targetColor)
{
	switch (targetColor)
	{
	case Grn:
		return (char*)"Green";
	case Ylw:
		return (char*)"Yellow";
	case Red:
		return (char*)"Red";
	case NoneColor:
		return (char*)"NoneColor";
	default:
		return (char*)"WrongCode";
	}
}