Imports System.Runtime.InteropServices
Imports System.Windows.Interop

Friend Enum AccentState
    Disabled
    EnableGradient
    EnableTransparentGradient
    EnableBlurBehind
    Invalid
End Enum

Friend Structure AccentPolicy
    Dim AccentState As AccentState
    Dim AccentFlags As Integer
    Dim GradientColor As Integer
    Dim AnimationId As Integer
End Structure

Friend Structure WindowCompositionAttributeData
    Dim Attribute As WindowCompositionAttribute
    Dim Data As IntPtr
    Dim SizeOfData As Integer
End Structure

Friend Enum WindowCompositionAttribute
    ' ...
    WCA_ACCENT_POLICY = 19
    ' ...
End Enum

Friend Class WindowComposition

    Friend Declare Function SetWindowCompositionAttribute Lib "user32.dll" (hwnd As IntPtr, ByRef data As WindowCompositionAttributeData) As Boolean
    Friend Declare Function GetWindowCompositionAttribute Lib "user32.dll" (hwnd As IntPtr, ByRef data As WindowCompositionAttributeData) As Boolean

    Public Sub SetBlur(window As Window, enable As Boolean)
        Dim windowHelper As New WindowInteropHelper(window)

        Dim accent As New AccentPolicy With {
            .AccentState = If(enable, AccentState.EnableBlurBehind,
                                      If(window.AllowsTransparency, AccentState.EnableTransparentGradient,
                                                                    AccentState.EnableGradient))
        }

        Dim hGc = GCHandle.Alloc(accent, GCHandleType.Pinned)
        Dim data As New WindowCompositionAttributeData With {
            .Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
            .SizeOfData = Marshal.SizeOf(accent),
            .Data = hGc.AddrOfPinnedObject
        }
        SetWindowCompositionAttribute(windowHelper.Handle, data)
        hGc.Free()
    End Sub

    Public Function IsBlurred(window As Window) As Boolean
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
