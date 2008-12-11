

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 6.00.0361 */
/* at Fri Jun 10 13:10:40 2005
 */
/* Compiler settings for .\VSUserControlHost.idl:
    Oicf, W1, Zp8, env=Win32 (32b run)
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
//@@MIDL_FILE_HEADING(  )

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __VSUserControlHost_h__
#define __VSUserControlHost_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IVSUserControlHostCtl_FWD_DEFINED__
#define __IVSUserControlHostCtl_FWD_DEFINED__
typedef interface IVSUserControlHostCtl IVSUserControlHostCtl;
#endif 	/* __IVSUserControlHostCtl_FWD_DEFINED__ */


#ifndef __IWin32Window_FWD_DEFINED__
#define __IWin32Window_FWD_DEFINED__
typedef interface IWin32Window IWin32Window;
#endif 	/* __IWin32Window_FWD_DEFINED__ */


#ifndef __VSUserControlHostCtl_FWD_DEFINED__
#define __VSUserControlHostCtl_FWD_DEFINED__

#ifdef __cplusplus
typedef class VSUserControlHostCtl VSUserControlHostCtl;
#else
typedef struct VSUserControlHostCtl VSUserControlHostCtl;
#endif /* __cplusplus */

#endif 	/* __VSUserControlHostCtl_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 

void * __RPC_USER MIDL_user_allocate(size_t);
void __RPC_USER MIDL_user_free( void * ); 

#ifndef __IVSUserControlHostCtl_INTERFACE_DEFINED__
#define __IVSUserControlHostCtl_INTERFACE_DEFINED__

/* interface IVSUserControlHostCtl */
/* [unique][helpstring][nonextensible][dual][uuid][object] */ 


EXTERN_C const IID IID_IVSUserControlHostCtl;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("5BC4D5A3-F1C8-44F5-8C62-E0BF5CBE92EB")
    IVSUserControlHostCtl : public IDispatch
    {
    public:
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE HostUserControl( 
            BSTR Assembly,
            BSTR Class,
            /* [retval][out] */ IDispatch **ppControlObject) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE HostUserControl2( 
            IUnknown *pToolWindow,
            BSTR Assembly,
            BSTR Class,
            BSTR SatelliteDLL,
            int ResourceID,
            /* [retval][out] */ IUnknown **ppControlObject) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IVSUserControlHostCtlVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IVSUserControlHostCtl * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IVSUserControlHostCtl * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IVSUserControlHostCtl * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IVSUserControlHostCtl * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IVSUserControlHostCtl * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IVSUserControlHostCtl * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IVSUserControlHostCtl * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *HostUserControl )( 
            IVSUserControlHostCtl * This,
            BSTR Assembly,
            BSTR Class,
            /* [retval][out] */ IDispatch **ppControlObject);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *HostUserControl2 )( 
            IVSUserControlHostCtl * This,
            IUnknown *pToolWindow,
            BSTR Assembly,
            BSTR Class,
            BSTR SatelliteDLL,
            int ResourceID,
            /* [retval][out] */ IUnknown **ppControlObject);
        
        END_INTERFACE
    } IVSUserControlHostCtlVtbl;

    interface IVSUserControlHostCtl
    {
        CONST_VTBL struct IVSUserControlHostCtlVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IVSUserControlHostCtl_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IVSUserControlHostCtl_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IVSUserControlHostCtl_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IVSUserControlHostCtl_GetTypeInfoCount(This,pctinfo)	\
    (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo)

#define IVSUserControlHostCtl_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo)

#define IVSUserControlHostCtl_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)

#define IVSUserControlHostCtl_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)


#define IVSUserControlHostCtl_HostUserControl(This,Assembly,Class,ppControlObject)	\
    (This)->lpVtbl -> HostUserControl(This,Assembly,Class,ppControlObject)

#define IVSUserControlHostCtl_HostUserControl2(This,pToolWindow,Assembly,Class,SatelliteDLL,ResourceID,ppControlObject)	\
    (This)->lpVtbl -> HostUserControl2(This,pToolWindow,Assembly,Class,SatelliteDLL,ResourceID,ppControlObject)

#endif /* COBJMACROS */


#endif 	/* C style interface */



/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE IVSUserControlHostCtl_HostUserControl_Proxy( 
    IVSUserControlHostCtl * This,
    BSTR Assembly,
    BSTR Class,
    /* [retval][out] */ IDispatch **ppControlObject);


void __RPC_STUB IVSUserControlHostCtl_HostUserControl_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);


/* [helpstring][id] */ HRESULT STDMETHODCALLTYPE IVSUserControlHostCtl_HostUserControl2_Proxy( 
    IVSUserControlHostCtl * This,
    IUnknown *pToolWindow,
    BSTR Assembly,
    BSTR Class,
    BSTR SatelliteDLL,
    int ResourceID,
    /* [retval][out] */ IUnknown **ppControlObject);


void __RPC_STUB IVSUserControlHostCtl_HostUserControl2_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IVSUserControlHostCtl_INTERFACE_DEFINED__ */



#ifndef __VSUserControlHostLib_LIBRARY_DEFINED__
#define __VSUserControlHostLib_LIBRARY_DEFINED__

/* library VSUserControlHostLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_VSUserControlHostLib;

#ifndef __IWin32Window_INTERFACE_DEFINED__
#define __IWin32Window_INTERFACE_DEFINED__

/* interface IWin32Window */
/* [object][uuid] */ 


EXTERN_C const IID IID_IWin32Window;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("458AB8A2-A1EA-4D7B-8EBE-DEE5D3D9442C")
    IWin32Window : public IUnknown
    {
    public:
        virtual HRESULT STDMETHODCALLTYPE get_Handle( 
            /* [retval][out] */ long *pHWnd) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IWin32WindowVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IWin32Window * This,
            /* [in] */ REFIID riid,
            /* [iid_is][out] */ void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IWin32Window * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IWin32Window * This);
        
        HRESULT ( STDMETHODCALLTYPE *get_Handle )( 
            IWin32Window * This,
            /* [retval][out] */ long *pHWnd);
        
        END_INTERFACE
    } IWin32WindowVtbl;

    interface IWin32Window
    {
        CONST_VTBL struct IWin32WindowVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IWin32Window_QueryInterface(This,riid,ppvObject)	\
    (This)->lpVtbl -> QueryInterface(This,riid,ppvObject)

#define IWin32Window_AddRef(This)	\
    (This)->lpVtbl -> AddRef(This)

#define IWin32Window_Release(This)	\
    (This)->lpVtbl -> Release(This)


#define IWin32Window_get_Handle(This,pHWnd)	\
    (This)->lpVtbl -> get_Handle(This,pHWnd)

#endif /* COBJMACROS */


#endif 	/* C style interface */



HRESULT STDMETHODCALLTYPE IWin32Window_get_Handle_Proxy( 
    IWin32Window * This,
    /* [retval][out] */ long *pHWnd);


void __RPC_STUB IWin32Window_get_Handle_Stub(
    IRpcStubBuffer *This,
    IRpcChannelBuffer *_pRpcChannelBuffer,
    PRPC_MESSAGE _pRpcMessage,
    DWORD *_pdwStubPhase);



#endif 	/* __IWin32Window_INTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_VSUserControlHostCtl;

#ifdef __cplusplus

class DECLSPEC_UUID("FE224107-D60D-4775-913F-4DED40FD84F5")
VSUserControlHostCtl;
#endif
#endif /* __VSUserControlHostLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

unsigned long             __RPC_USER  BSTR_UserSize(     unsigned long *, unsigned long            , BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserMarshal(  unsigned long *, unsigned char *, BSTR * ); 
unsigned char * __RPC_USER  BSTR_UserUnmarshal(unsigned long *, unsigned char *, BSTR * ); 
void                      __RPC_USER  BSTR_UserFree(     unsigned long *, BSTR * ); 

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


