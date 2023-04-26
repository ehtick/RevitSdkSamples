// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.


#include "pch.h"
#include "astocxgrid.h"
#include "unitsmanager.h"
// Dispatch interfaces referenced by this interface

/////////////////////////////////////////////////////////////////////////////
// CAstOCXGrid

IMPLEMENT_DYNCREATE(CAstOCXGrid, CWnd)

/////////////////////////////////////////////////////////////////////////////
// CAstOCXGrid properties

long CAstOCXGrid::GetColumnCount()
{
	long result;
	GetProperty(0x1, VT_I4, (void*)&result);
	return result;
}

void CAstOCXGrid::SetColumnCount(long propVal)
{
	SetProperty(0x1, VT_I4, propVal);
}

long CAstOCXGrid::GetRowCount()
{
	long result;
	GetProperty(0x2, VT_I4, (void*)&result);
	return result;
}

void CAstOCXGrid::SetRowCount(long propVal)
{
	SetProperty(0x2, VT_I4, propVal);
}

long CAstOCXGrid::GetFixedRowCount()
{
	long result;
	GetProperty(0x3, VT_I4, (void*)&result);
	return result;
}

void CAstOCXGrid::SetFixedRowCount(long propVal)
{
	SetProperty(0x3, VT_I4, propVal);
}

long CAstOCXGrid::GetFixedColumnCount()
{
	long result;
	GetProperty(0x4, VT_I4, (void*)&result);
	return result;
}

void CAstOCXGrid::SetFixedColumnCount(long propVal)
{
	SetProperty(0x4, VT_I4, propVal);
}

CUnitsManager CAstOCXGrid::GetUnits()
{
	LPDISPATCH pDispatch;
	GetProperty(0x5, VT_DISPATCH, (void*)&pDispatch);
	return CUnitsManager(pDispatch);
}

void CAstOCXGrid::SetUnits(LPDISPATCH propVal)
{
	SetProperty(0x5, VT_DISPATCH, propVal);
}

BOOL CAstOCXGrid::GetHeaderSort()
{
	BOOL result;
	GetProperty(0x6, VT_BOOL, (void*)&result);
	return result;
}

void CAstOCXGrid::SetHeaderSort(BOOL propVal)
{
	SetProperty(0x6, VT_BOOL, propVal);
}

BOOL CAstOCXGrid::GetListMode()
{
	BOOL result;
	GetProperty(0x7, VT_BOOL, (void*)&result);
	return result;
}

void CAstOCXGrid::SetListMode(BOOL propVal)
{
	SetProperty(0x7, VT_BOOL, propVal);
}

/////////////////////////////////////////////////////////////////////////////
// CAstOCXGrid operations

long CAstOCXGrid::GetRowHeight(long nRow)
{
	long result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nRow);
	return result;
}

void CAstOCXGrid::SetRowHeight(long nRow, long nNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x8, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nRow, nNewValue);
}

long CAstOCXGrid::GetColumnWidth(long nColumn)
{
	long result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x9, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nColumn);
	return result;
}

void CAstOCXGrid::SetColumnWidth(long nColumn, long nNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x9, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nColumn, nNewValue);
}

unsigned long CAstOCXGrid::GetCellBkColor(long nRow, long nCol)
{
	unsigned long result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetCellBkColor(long nRow, long nCol, unsigned long newValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0xa, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nRow, nCol, newValue);
}

unsigned long CAstOCXGrid::GetCellFgColor(long nRow, long nCol)
{
	unsigned long result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetCellFgColor(long nRow, long nCol, unsigned long newValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0xb, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nRow, nCol, newValue);
}

long CAstOCXGrid::GetColumnType(long nCol)
{
	long result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0xc, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nCol);
	return result;
}

void CAstOCXGrid::SetColumnType(long nCol, long nNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0xc, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nCol, nNewValue);
}

long CAstOCXGrid::GetColumnDataType(long nCol)
{
	long result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0xd, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nCol);
	return result;
}

void CAstOCXGrid::SetColumnDataType(long nCol, long nNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0xd, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nCol, nNewValue);
}

long CAstOCXGrid::GetColumnUnits(long nCol)
{
	long result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0xe, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nCol);
	return result;
}

void CAstOCXGrid::SetColumnUnits(long nCol, long nNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0xe, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nCol, nNewValue);
}

double CAstOCXGrid::GetItemDoubleValue(long nRow, long nCol)
{
	double result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0xf, DISPATCH_PROPERTYGET, VT_R8, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetItemDoubleValue(long nRow, long nCol, double newValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_R8;
	InvokeHelper(0xf, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nRow, nCol, newValue);
}

long CAstOCXGrid::GetItemIntegerValue(long nRow, long nCol)
{
	long result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x10, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetItemIntegerValue(long nRow, long nCol, long nNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0x10, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nRow, nCol, nNewValue);
}

CString CAstOCXGrid::GetItemTextValue(long nRow, long nCol)
{
	CString result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x11, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetItemTextValue(long nRow, long nCol, LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_BSTR;
	InvokeHelper(0x11, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 nRow, nCol, lpszNewValue);
}

long CAstOCXGrid::GetFocusCellRow()
{
	long result;
	InvokeHelper(0x12, DISPATCH_METHOD, VT_I4, (void*)&result, NULL);
	return result;
}

long CAstOCXGrid::GetFocusCellColumn()
{
	long result;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_I4, (void*)&result, NULL);
	return result;
}

void CAstOCXGrid::EnableCell(long nRow, long nCol, short bEnable)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I2;
	InvokeHelper(0x14, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nRow, nCol, bEnable);
}

long CAstOCXGrid::InsertColumn(BSTR* strHeading, long nFormat, long nCol, long nType, long dataType/* = 0*/, long unitType/* = 0*/)
{
	long result;
	static BYTE parms[] =
		VTS_PBSTR VTS_I4 VTS_I4 VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0x15, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		strHeading, nFormat, nCol, nType, dataType, unitType);
	return result;
}

long CAstOCXGrid::InsertRow(BSTR* strHeading, long nRow)
{
	long result;
	static BYTE parms[] =
		VTS_PBSTR VTS_I4;
	InvokeHelper(0x16, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		strHeading, nRow);
	return result;
}

BOOL CAstOCXGrid::DeleteColumn(long nCol)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x17, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		nCol);
	return result;
}

BOOL CAstOCXGrid::DeleteRow(long nRow)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x18, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		nRow);
	return result;
}

BOOL CAstOCXGrid::DeleteAllItems()
{
	BOOL result;
	InvokeHelper(0x19, DISPATCH_METHOD, VT_BOOL, (void*)&result, NULL);
	return result;
}

void CAstOCXGrid::AddStringToControl(BSTR* newValue)
{
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0x1a, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 newValue);
}

BOOL CAstOCXGrid::SortItems(long nCol, short bAscending)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I4 VTS_I2;
	InvokeHelper(0x1b, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		nCol, bAscending);
	return result;
}

void CAstOCXGrid::SetChecked(long nRow, long nCol, short nChecked)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I2;
	InvokeHelper(0x1c, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nRow, nCol, nChecked);
}

void CAstOCXGrid::AutoSizeRow(long nRow)
{
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x1d, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nRow);
}

void CAstOCXGrid::AutoSizeColumn(long nCol)
{
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x1e, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 nCol);
}

void CAstOCXGrid::AutoSizeRows()
{
	InvokeHelper(0x1f, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CAstOCXGrid::AutoSizeColumns()
{
	InvokeHelper(0x20, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CAstOCXGrid::AutoSize()
{
	InvokeHelper(0x21, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CAstOCXGrid::ExpandColumnsToFit()
{
	InvokeHelper(0x22, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CAstOCXGrid::ExpandRowsToFit()
{
	InvokeHelper(0x23, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CAstOCXGrid::ExpandToFit()
{
	InvokeHelper(0x24, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void CAstOCXGrid::AboutBox()
{
	InvokeHelper(0xfffffdd8, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}
void CAstOCXGrid::SetEditable(BOOL bEditable)
{
	static BYTE params[] = VTS_BOOL;
	InvokeHelper(0x25, DISPATCH_METHOD, VT_EMPTY, NULL, params, bEditable );
}

void CAstOCXGrid::RedrawCell(long nRow, long nCol)
{
	static BYTE params[] = VTS_I4 VTS_I4;
	InvokeHelper(0x26, DISPATCH_METHOD, VT_EMPTY, NULL, params, nRow, nCol);
}

unsigned long CAstOCXGrid::GetGridBkColor()
{
	unsigned long result;
	InvokeHelper(0x27, DISPATCH_PROPERTYGET, VT_UI4, (void*)&result, NULL);
	return result;
}
void CAstOCXGrid::SetGridBkColor(unsigned long newValue)
{
	static BYTE parms[] = VTS_UI4 ;
	InvokeHelper(0x27, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms, newValue);
}
void CAstOCXGrid::AddTableToComboCol(long nCol, LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_BSTR;
	InvokeHelper(0x28, DISPATCH_METHOD, VT_EMPTY, NULL, parms, nCol, lpszNewValue);
}

BOOL CAstOCXGrid::GetSortCombo()
{
	BOOL result;
	GetProperty(0x29, VT_BOOL, (void*)&result);
	return result;
}

void CAstOCXGrid::SetSortCombo(BOOL propVal)
{
	SetProperty(0x29, VT_BOOL, propVal);
}

void CAstOCXGrid::SetLongKey(long nRow, long nCol, long nNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_I4;
	InvokeHelper(0x2A, DISPATCH_METHOD, VT_EMPTY, NULL, parms, nRow, nCol, nNewValue);
}

long CAstOCXGrid::GetLongKey(long nRow, long nCol)
{
	BOOL result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x2B, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetStringKey(long nRow, long nCol,  LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_BSTR;
	InvokeHelper(0x2C, DISPATCH_METHOD, VT_EMPTY, NULL, parms, nRow, nCol, lpszNewValue);
}

BSTR CAstOCXGrid::GetStringKey(long nRow, long nCol)
{
	BSTR result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x2D, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetProfileName(long nRow, long nCol,  LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_I4 VTS_I4 VTS_BSTR;
	InvokeHelper(0x2E, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms, nRow, nCol, lpszNewValue);
}

CString CAstOCXGrid::GetProfileName(long nRow, long nCol)
{
	CString result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x2E, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetCellReadOnly( long nRow, long nCol, short bReadOnly )
{
	static BYTE params[] =
		VTS_I4 VTS_I4 VTS_I2;
	InvokeHelper(0x2F, DISPATCH_METHOD, VT_EMPTY, NULL, params, 
		nRow, nCol, bReadOnly);
}

long CAstOCXGrid::GetDataType(long nRow, long nCol)
{
	long result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x30, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetDataType(long nRow, long nCol, long nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I4 VTS_I4;
	InvokeHelper(0x30, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		nRow, nCol, nNewValue);
}

long CAstOCXGrid::GetUnits(long nRow, long nCol)
{
	long result;
	static BYTE parms[] =
		VTS_I4 VTS_I4;
	InvokeHelper(0x31, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, parms,
		nRow, nCol);
	return result;
}

void CAstOCXGrid::SetUnits(long nRow, long nCol, long nNewValue)
{
	static BYTE parms[] =
		VTS_I2 VTS_I4 VTS_I4;
	InvokeHelper(0x31, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		nRow, nCol, nNewValue);
}

void CAstOCXGrid::RedrawAllCells()
{
	InvokeHelper(0x32, DISPATCH_METHOD, VT_EMPTY, NULL, VTS_NONE);
}

void CAstOCXGrid::AppendProfileAcceptedClassGroup( LPCTSTR lpszNewValue )
{
	static BYTE parms[] = VTS_BSTR;
	InvokeHelper(0x33, DISPATCH_METHOD, VT_EMPTY, NULL, parms, lpszNewValue);
}
