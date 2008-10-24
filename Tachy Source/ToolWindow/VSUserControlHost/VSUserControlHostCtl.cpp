//Copyright (c) Microsoft Corporation.  All rights reserved.

// VSUserControlHostCtl.cpp : Implementation of CVSUserControlHostCtl
#include "stdafx.h"
#include "VSUserControlHost.h"
#include "VSUserControlHostCtl.h"

//Import DTE:
#import "libid:80cc9f66-e7d8-4ddd-85b6-d9e6cd0e93e2" version("7.0") lcid("0") raw_interfaces_only named_guids

// CVSUserControlHostCtl
LRESULT CVSUserControlHostCtl::OnSize(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM lParam, BOOL& /*bHandled*/)
{
	WORD wLength = LOWORD(lParam);
	WORD wHeight = HIWORD(lParam);
	::MoveWindow((HWND)m_hWndForm, 0, 0, wLength, wHeight, TRUE);
	return 0;
}

void CVSUserControlHostCtl::Reset()
{
	m_pHost = NULL;
	m_pDefaultDomain = NULL;
	m_pObjHandle = NULL;
	m_varUnwrappedObject.Clear();
	m_hWndForm = 0;
}

HPALETTE CreateDIBPalette (LPBITMAPINFO lpbmi, LPINT lpiNumColors) 
{ 
	LPBITMAPINFOHEADER  lpbi;
	LPLOGPALETTE     lpPal;
	HANDLE           hLogPal;
	HPALETTE         hPal = NULL;
	int              i;

	lpbi = (LPBITMAPINFOHEADER)lpbmi;
	if (lpbi->biBitCount <= 8)
		*lpiNumColors = (1 << lpbi->biBitCount);
	else
		*lpiNumColors = 0;  // No palette needed for 24 BPP DIB

	if (lpbi->biClrUsed > 0)
		*lpiNumColors = lpbi->biClrUsed;  // Use biClrUsed

	if (*lpiNumColors)
	{
		hLogPal = GlobalAlloc (GHND, sizeof (LOGPALETTE) + sizeof (PALETTEENTRY) * (*lpiNumColors));
		lpPal = (LPLOGPALETTE) GlobalLock (hLogPal);
		lpPal->palVersion    = 0x300;
		lpPal->palNumEntries = *lpiNumColors;

		for (i = 0;  i < *lpiNumColors;  i++)
		{
			lpPal->palPalEntry[i].peRed   = lpbmi->bmiColors[i].rgbRed;
			lpPal->palPalEntry[i].peGreen = lpbmi->bmiColors[i].rgbGreen;
			lpPal->palPalEntry[i].peBlue  = lpbmi->bmiColors[i].rgbBlue;
			lpPal->palPalEntry[i].peFlags = 0;
		}
		hPal = CreatePalette (lpPal);
		GlobalUnlock (hLogPal);
		GlobalFree   (hLogPal);
	}
	return hPal;
} 

HBITMAP LoadResourceBitmap(HINSTANCE hInstance, TCHAR *pszString, HPALETTE FAR* lphPalette)
{
	HGLOBAL hGlobal;
	HBITMAP hBitmapFinal = NULL;
	LPBITMAPINFOHEADER  lpbi;
	HDC hdc;
	int iNumColors;
	HRSRC hRsrc = FindResource(hInstance, pszString, RT_BITMAP);
	if (hRsrc)
	{
		hGlobal = LoadResource(hInstance, hRsrc);
		lpbi = (LPBITMAPINFOHEADER)LockResource(hGlobal);

		hdc = GetDC(NULL);
		*lphPalette =  CreateDIBPalette ((LPBITMAPINFO)lpbi, &iNumColors);
		if (*lphPalette)
		{
			SelectPalette(hdc,*lphPalette,FALSE);
			RealizePalette(hdc);
		}

		hBitmapFinal = CreateDIBitmap(hdc, (LPBITMAPINFOHEADER)lpbi, (LONG)CBM_INIT, (LPSTR)lpbi + lpbi->biSize + iNumColors * sizeof(RGBQUAD), (LPBITMAPINFO)lpbi, DIB_RGB_COLORS );

		ReleaseDC(NULL,hdc);
		UnlockResource(hGlobal);
		FreeResource(hGlobal);
	}
	return (hBitmapFinal);
} 

HRESULT CVSUserControlHostCtl::HostUserControl2(IUnknown *pToolWindow, BSTR SatelliteDLL, BSTR Assembly, BSTR Class, int ResourceID, [out,retval] IUnknown** ppControlObject)
{
	CComQIPtr<EnvDTE::Window> pWindow(pToolWindow);

	RECT rc;
	CComPtr<IWin32Window> pIWin32Window;
	HRESULT hr = m_pDefaultDomain->CreateInstance(Assembly, Class, &m_pObjHandle);
	if(FAILED(hr) || (!m_pObjHandle))
	{
		hr = m_pDefaultDomain->CreateInstanceFrom(Assembly, Class, &m_pObjHandle); 
		if(FAILED(hr) || (!m_pObjHandle))
		{
			hr = E_FAIL;
			Reset();
			return hr;
		}
	}
	hr = m_pObjHandle->Unwrap(&m_varUnwrappedObject);
	if((m_varUnwrappedObject.vt != VT_DISPATCH) && (m_varUnwrappedObject.vt != VT_UNKNOWN) || (!m_varUnwrappedObject.punkVal))
	{
		hr = E_FAIL;
		Reset();
		return hr;
	}
	hr = m_varUnwrappedObject.pdispVal->QueryInterface(IID_IUnknown, (LPVOID*)ppControlObject);
	hr = m_varUnwrappedObject.pdispVal->QueryInterface(IID_IWin32Window, (LPVOID*)&pIWin32Window);
	if(FAILED(hr))
	{
		Reset();
		return hr;
	}
	hr = pIWin32Window->get_Handle(&m_hWndForm);
	if(FAILED(hr) || (!m_hWndForm))
	{
		Reset();
		return hr;
	}
	::SetParent((HWND)m_hWndForm, m_hWnd);
	::GetWindowRect(m_hWnd, &rc);
	::MoveWindow((HWND)m_hWndForm, 0, 0, rc.right-rc.left, rc.bottom-rc.top, TRUE);
	::ShowWindow((HWND)m_hWndForm, SW_SHOW);

	if(pWindow.p)
	{
		USES_CONVERSION;
		CComVariant varPic;
		CComPtr<IPictureDisp> pPictureDisp;
		PICTDESC pd;
		pd.cbSizeofstruct=sizeof(PICTDESC);
		pd.picType=PICTYPE_BITMAP;
		HMODULE hModule = LoadLibrary(W2T(SatelliteDLL));
		pd.bmp.hbitmap = LoadResourceBitmap(hModule, MAKEINTRESOURCE(ResourceID), &pd.bmp.hpal);
		OleCreatePictureIndirect(&pd, IID_IPictureDisp, FALSE, (LPVOID*)&pPictureDisp);
		pPictureDisp->QueryInterface(IID_IUnknown, (LPVOID*)&varPic.punkVal);
		varPic.vt = VT_UNKNOWN;
		pWindow->SetTabPicture(varPic);
		FreeLibrary(hModule);
	}

	return hr;
}

HRESULT CVSUserControlHostCtl::HostUserControl(BSTR Assembly, BSTR Class, IDispatch **ppControlObject)
{
	RECT rc;
	CComPtr<IWin32Window> pIWin32Window;
	HRESULT hr = m_pDefaultDomain->CreateInstance(Assembly, Class, &m_pObjHandle);
	if(FAILED(hr) || (!m_pObjHandle))
	{
		hr = m_pDefaultDomain->CreateInstanceFrom(Assembly, Class, &m_pObjHandle); 
		if(FAILED(hr) || (!m_pObjHandle))
		{
			hr = E_FAIL;
			Reset();
			return hr;
		}
	}
	hr = m_pObjHandle->Unwrap(&m_varUnwrappedObject);
	if((m_varUnwrappedObject.vt != VT_DISPATCH) && (m_varUnwrappedObject.vt != VT_UNKNOWN) || (!m_varUnwrappedObject.punkVal))
	{
		hr = E_FAIL;
		Reset();
		return hr;
	}
	hr = m_varUnwrappedObject.pdispVal->QueryInterface(IID_IDispatch, (LPVOID*)ppControlObject);
	hr = m_varUnwrappedObject.pdispVal->QueryInterface(IID_IWin32Window, (LPVOID*)&pIWin32Window);
	if(FAILED(hr))
	{
		Reset();
		return hr;
	}
	hr = pIWin32Window->get_Handle(&m_hWndForm);
	if(FAILED(hr) || (!m_hWndForm))
	{
		Reset();
		return hr;
	}
	::SetParent((HWND)m_hWndForm, m_hWnd);
	::GetWindowRect(m_hWnd, &rc);
	::MoveWindow((HWND)m_hWndForm, 0, 0, rc.right-rc.left, rc.bottom-rc.top, TRUE);
	::ShowWindow((HWND)m_hWndForm, SW_SHOW);
	return hr;
}

HRESULT CVSUserControlHostCtl::FinalConstruct()
{
	CComPtr<IUnknown> pAppDomainPunk;
	HRESULT hr = CorBindToRuntimeEx(NULL, NULL, STARTUP_LOADER_OPTIMIZATION_SINGLE_DOMAIN | STARTUP_CONCURRENT_GC, __uuidof(CorRuntimeHost), __uuidof(ICorRuntimeHost), (LPVOID*)&m_pHost);
	if(FAILED(hr))
		return hr;
	hr = m_pHost->Start();
	if(FAILED(hr))
		return hr;
	hr = m_pHost->GetDefaultDomain(&pAppDomainPunk);
	if(FAILED(hr) || !pAppDomainPunk)
		return hr;
	hr = pAppDomainPunk->QueryInterface(__uuidof(mscorlib::_AppDomain), (LPVOID*)&m_pDefaultDomain);
	if(FAILED(hr) || !m_pDefaultDomain)
		return hr;

	return hr;
}