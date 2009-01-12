// TachyAddInImages.h : main header file for the TachyAddInImages DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols


// CTachyAddInImagesApp
// See TachyAddInImages.cpp for the implementation of this class
//

class CTachyAddInImagesApp : public CWinApp
{
public:
	CTachyAddInImagesApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
