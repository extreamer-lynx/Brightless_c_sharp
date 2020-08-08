// brightness_dll.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include <iostream>

using namespace std;
//#include "brightness.h"
#include "gammaramp.h"

extern "C" __declspec(dllexport)const char * GetPowerStatus()
{
	SYSTEM_POWER_STATUS sps;
	if (GetSystemPowerStatus(&sps))
	{
		switch (sps.ACLineStatus) // �������� ������ �������
		{
		case 0:
			return "Power-off";
		case 1:
			return "Power on";
		case 255:
		default:
			return "�������";
			break;
		}
	}
	return "Unknown";
}

extern "C" __declspec(dllexport)const char * GetBatteryFlag()
{
	SYSTEM_POWER_STATUS sps;
	GetSystemPowerStatus(&sps);
	switch (sps.BatteryFlag) // ������ ������
	{
	case 1:
		return "�������";
	case 2:
		return "�������";
	case 4:
		return "���������";
	case 8:
		return "�����������";
	case 128:
		return "������� �������";
	case 255:
	default:
		return "��������";
	}
}
extern "C" __declspec(dllexport) int GetBatteryPercent()
{
	SYSTEM_POWER_STATUS sps;
	GetSystemPowerStatus(&sps);
	return (int)sps.BatteryLifePercent;
}
extern "C" __declspec(dllexport) int GetBatteryTime()
{
	SYSTEM_POWER_STATUS sps;
	GetSystemPowerStatus(&sps);
	return sps.BatteryLifeTime;
}
extern "C" __declspec(dllexport) int GetFullBatteryTime()
{
	SYSTEM_POWER_STATUS sps;
	GetSystemPowerStatus(&sps);
	return sps.BatteryFullLifeTime;
}

 extern "C" __declspec(dllexport) void SetBrightness(int Brightness)
{
	CGammaRamp GammaRamp;
	GammaRamp.SetBrightness(NULL, Brightness);
}

