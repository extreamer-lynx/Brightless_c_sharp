#pragma once
#include <windows.h>
#include "gammaramp.h"


int WINAPI WinMain(
	HINSTANCE hInstance,      // handle to current instance
	HINSTANCE hPrevInstance,  // handle to previous instance
	LPSTR lpCmdLine,          // command line
	int nCmdShow              // show state
)
{
	//Example for changing the brightness with CGammaRamp class:
	//Be aware that this exmaple may not work properly in all
	//Video cards.

	CGammaRamp GammaRamp;

	//Make the screen darker:
	GammaRamp.SetBrightness(NULL, 64);

	//Wait 3 seconds:
	Sleep(3000);

	//Return back to normal:
	GammaRamp.SetBrightness(NULL, 128);

	return 0;
}