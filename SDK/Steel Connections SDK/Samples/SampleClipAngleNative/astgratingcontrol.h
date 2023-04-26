#if !defined(AFX_ASTGRATINGCONTROL_H__3171B533_EE2B_4609_A8DD_E9ADD3AFAE37__INCLUDED_)
#define AFX_ASTGRATINGCONTROL_H__3171B533_EE2B_4609_A8DD_E9ADD3AFAE37__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++
// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.
/////////////////////////////////////////////////////////////////////////////
// CAstGratingControl wrapper class

class CAstGratingControl : public CWnd
{
protected:
	DECLARE_DYNCREATE(CAstGratingControl)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0xccddd738, 0x9cda, 0x4677, { 0x88, 0x85, 0x5, 0x7f, 0x2, 0x56, 0xf6, 0x1c } };
		return clsid;
	}
	virtual BOOL Create(LPCTSTR lpszClassName,
		LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect,
		CWnd* pParentWnd, UINT nID,
		CCreateContext* pContext = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID); }

    BOOL Create(LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect, CWnd* pParentWnd, UINT nID,
		CFile* pPersist = NULL, BOOL bStorage = FALSE,
		BSTR bstrLicKey = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID,
		pPersist, bStorage, bstrLicKey); }

// Attributes
public:
	CString GetCurrentSize();
	void SetCurrentSize(LPCTSTR);
	CString GetCurrentClass();
	void SetCurrentClass(LPCTSTR);
	long GetGratingType();
	void SetGratingType(long);
	long GetShowGratingTypeCombo();
	void SetShowGratingTypeCombo(long);
	long GetLabelLengthAll();
	void SetLabelLengthAll(long);
	BOOL GetEnabled();
	void SetEnabled(BOOL);
	long GetSummaryRepresentation();
	void SetSummaryRepresentation(long);
	BOOL GetAppearance();
	void SetAppearance(BOOL);
	long GetDropHeight();
	void SetDropHeight(long);
	long GetDropWidth();
	void SetDropWidth(long);
	BOOL GetSummaryDroppedDown();
	void SetSummaryDroppedDown(BOOL);
	CString GetGridCellDisplayName();
	void SetGridCellDisplayName(LPCTSTR);
	// property 'Enabled' not emitted because of invalid type

// Operations
public:
	long GetLabelDBKey(long ctrl);
	void SetLabelDBKey(long ctrl, long nNewValue);
	long GetLabelLength(long ctrl);
	void SetLabelLength(long ctrl, long nNewValue);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ASTGRATINGCONTROL_H__3171B533_EE2B_4609_A8DD_E9ADD3AFAE37__INCLUDED_)
