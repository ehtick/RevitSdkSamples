// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.

#include "pch.h"
#include "astunitcontrol.h"
#include "font.h"
#include "unitsmanager.h"
// Dispatch interfaces referenced by this interface

/////////////////////////////////////////////////////////////////////////////
// CAstUnitControl

IMPLEMENT_DYNCREATE(CAstUnitControl, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CAstUnitControl properties

unsigned long CAstUnitControl::GetBackColor()
{
	unsigned long result;
	GetProperty(0x1, VT_I4, (void*)&result);
	return result;
}

void CAstUnitControl::SetBackColor(unsigned long propVal)
{
	SetProperty(0x1, VT_I4, propVal);
}

short CAstUnitControl::GetEditType()
{
	short result;
	GetProperty(0x2, VT_I2, (void*)&result);
	return result;
}

void CAstUnitControl::SetEditType(short propVal)
{
	SetProperty(0x2, VT_I2, propVal);
}

COleFont CAstUnitControl::GetFont()
{
	LPDISPATCH pDispatch;
	GetProperty(DISPID_FONT, VT_DISPATCH, (void*)&pDispatch);
	return COleFont(pDispatch);
}

void CAstUnitControl::SetFont(LPDISPATCH propVal)
{
	SetProperty(DISPID_FONT, VT_DISPATCH, propVal);
}

double CAstUnitControl::GetDoubleValue()
{
	double result;
	GetProperty(0x3, VT_R8, (void*)&result);
	return result;
}

void CAstUnitControl::SetDoubleValue(double propVal)
{
	SetProperty(0x3, VT_R8, propVal);
}

CString CAstUnitControl::GetText()
{
	CString result;
	GetProperty(0x4, VT_BSTR, (void*)&result);
	return result;
}

void CAstUnitControl::SetText(LPCTSTR propVal)
{
	SetProperty(0x4, VT_BSTR, propVal);
}

CUnitsManager CAstUnitControl::GetUnits()
{
	LPDISPATCH pDispatch;
	GetProperty(0x5, VT_DISPATCH, (void*)&pDispatch);
	return CUnitsManager(pDispatch);
}

void CAstUnitControl::SetUnits(LPDISPATCH propVal)
{
	SetProperty(0x5, VT_DISPATCH, propVal);
}

long CAstUnitControl::GetLabelDbKey()
{
	long result;
	GetProperty(0x6, VT_I4, (void*)&result);
	return result;
}

void CAstUnitControl::SetLabelDbKey(long propVal)
{
	SetProperty(0x6, VT_I4, propVal);
}

long CAstUnitControl::GetLabelLength()
{
	long result;
	GetProperty(0x7, VT_I4, (void*)&result);
	return result;
}

void CAstUnitControl::SetLabelLength(long propVal)
{
	SetProperty(0x7, VT_I4, propVal);
}

BOOL CAstUnitControl::GetEnabled()
{
	BOOL result;
	GetProperty(DISPID_ENABLED, VT_BOOL, (void*)&result);
	return result;
}

void CAstUnitControl::SetEnabled(BOOL propVal)
{
	SetProperty(DISPID_ENABLED, VT_BOOL, propVal);
}

long CAstUnitControl::GetIntegerValue()
{
	long result;
	GetProperty(0x8, VT_I4, (void*)&result);
	return result;
}

void CAstUnitControl::SetIntegerValue(long propVal)
{
	SetProperty(0x8, VT_I4, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CAstUnitControl operations
