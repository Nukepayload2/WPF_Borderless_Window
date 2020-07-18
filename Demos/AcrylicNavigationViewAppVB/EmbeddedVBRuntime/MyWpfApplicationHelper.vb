Namespace My
    <HideModuleName>
    Module MyWpfApplicationHelper
        Public ReadOnly Property Application As Application
            Get
                Return TryCast(Windows.Application.Current, Application)
            End Get
        End Property
    End Module
End Namespace