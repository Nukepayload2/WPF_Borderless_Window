﻿Imports System.Runtime.InteropServices
Imports System.Windows.Interop

Public Interface IWindowComposition
    Function TrySetBlur(window As Window, enable As Boolean) As Boolean
    Function IsBlurred(window As Window) As Boolean
End Interface

Public Class WindowCompositionFactory
    Public Function TryCreateForCurrentView() As IWindowComposition
        Return If(Win32ApiInformation.IsWindowAcrylicApiPresent,
                  New WindowComposition10, If(Win32ApiInformation.IsAeroGlassApiPresent,
                                              New WindowComposition7, Nothing))
    End Function
End Class

Public Class WindowComposition7
    Implements IWindowComposition

    Public Sub New()

    End Sub

    Public Sub New(extendFrameIntoClientArea As Boolean, Optional frameMargin As Thickness? = Nothing)
        Me.ExtendFrameIntoClientArea = extendFrameIntoClientArea
        Me.FrameMargin = frameMargin
    End Sub

    Public ReadOnly Property IsCompositionEnabled As Boolean
        Get
            Dim canEnableComposition As Boolean
            DwmIsCompositionEnabled(canEnableComposition)
            Return canEnableComposition
        End Get
    End Property

    Friend Property IsBlurEnabled As Boolean

    Public Property ExtendFrameIntoClientArea As Boolean

    Public Property FrameMargin As Thickness?

    Public Function TrySetBlur(window As Window, enable As Boolean) As Boolean Implements IWindowComposition.TrySetBlur
        If Not IsCompositionEnabled Then
            Return False
        End If
        If Not enable Then
            Return False
        End If

        Dim mainWindowPtr As IntPtr = New WindowInteropHelper(window).Handle

        Dim margins As DwmMargins
        If FrameMargin Is Nothing Then
            margins = New DwmMargins With {
                .LeftWidth = -1,
                .RightWidth = -1,
                .TopHeight = -1,
                .BottomHeight = -1
            }
        Else
            Dim marginValue = FrameMargin.Value
            margins = New DwmMargins With {
                .LeftWidth = marginValue.Left,
                .RightWidth = marginValue.Right,
                .TopHeight = marginValue.Top,
                .BottomHeight = marginValue.Bottom
            }
        End If

        If ExtendFrameIntoClientArea Then
            Return 0 = DwmExtendFrameIntoClientArea(mainWindowPtr, margins)
        Else
            Return 0 = DwmEnableBlurBehindWindow(mainWindowPtr, DwmBlurBehind.Create)
        End If
        Return True
    End Function

    Public Function IsBlurred(window As Window) As Boolean Implements IWindowComposition.IsBlurred
        If Not IsCompositionEnabled Then
            Return False
        End If
        Return IsBlurEnabled
    End Function
End Class

Public Class WindowComposition10
    Implements IWindowComposition

    Public Function TrySetBlur(window As Window, enable As Boolean) As Boolean Implements IWindowComposition.TrySetBlur
        Dim windowHelper As New WindowInteropHelper(window)

        Dim accent As New AccentPolicy With {
            .AccentState = If(enable, AccentState.EnableBlurBehind,
                                      If(window.AllowsTransparency, AccentState.EnableTransparentGradient,
                                                                    AccentState.EnableGradient))
        }

        Dim hGc = GCHandle.Alloc(accent, GCHandleType.Pinned)
        Try
            Dim data As New WindowCompositionAttributeData With {
                .Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                .SizeOfData = Marshal.SizeOf(accent),
                .Data = hGc.AddrOfPinnedObject
            }
            Return SetWindowCompositionAttribute(windowHelper.Handle, data)
        Finally
            hGc.Free()
        End Try
    End Function

    Public Function IsBlurred(window As Window) As Boolean Implements IWindowComposition.IsBlurred
        Dim windowHelper As New WindowInteropHelper(window)

        Dim accent As New AccentPolicy

        Dim hGc = GCHandle.Alloc(accent, GCHandleType.Pinned)
        Dim data As New WindowCompositionAttributeData With {
            .Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
            .SizeOfData = Marshal.SizeOf(accent),
            .Data = hGc.AddrOfPinnedObject
        }
        GetWindowCompositionAttribute(windowHelper.Handle, data)
        hGc.Free()

        Return accent.AccentState = AccentState.EnableBlurBehind
    End Function
End Class
