#if !defined(AFX_ASTCHECKBOXCONTROL_H__3DE7A33E_C3ED_419E_B57F_46CA43CD67B9__INCLUDED_)
#define AFX_ASTCHECKBOXCONTROL_H__3DE7A33E_C3ED_419E_B57F_46CA43CD67B9__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.
/////////////////////////////////////////////////////////////////////////////
// CAstCheckBoxControl wrapper class

class CAstCheckBoxControl : public CWnd
{
protected:
	DECLARE_DYNCREATE(CAstCheckBoxControl)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x17d55625, 0x30d6, 0x421f, { 0x8a, 0x3f, 0x85, 0xc6, 0x15, 0xff, 0xa9, 0x3c } };
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
	long GetCaptionKey();
	void SetCaptionKey(long);
	BOOL GetValue();
	void SetValue(BOOL);
	BOOL GetEnabled();
	void SetEnabled(BOOL);

// Operations
public:
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ASTCHECKBOXCONTROL_H__3DE7A33E_C3ED_419E_B57F_46CA43CD67B9__INCLUDED_)
