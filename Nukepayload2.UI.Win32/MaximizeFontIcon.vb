' 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
'
' 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
' 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
' 元素中:
'
'     xmlns:MyNamespace="clr-namespace:Nukepayload2.UI.Win32"
'
'
' 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
' 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
' 元素中:
'
'     xmlns:MyNamespace="clr-namespace:Nukepayload2.UI.Win32;assembly=Nukepayload2.UI.Win32"
'
' 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
' 并重新生成以避免编译错误:
'
'     在解决方案资源管理器中右击目标项目，然后依次单击
'     “添加引用”->“项目”->[浏览查找并选择此项目]
'
'
' 步骤 2)
' 继续操作并在 XAML 文件中使用控件。请注意，XML 编辑器中的 Intellisense
' 目前对自定义控件及其子元素不起作用。
'
'     <MyNamespace:MaximizeFontIcon/>
'

Imports System.Windows.Controls.Primitives


Public Class MaximizeFontIcon
    Inherits System.Windows.Controls.Control

    Shared Sub New()
        '此 OverrideMetadata 调用通知系统该元素希望提供不同于其基类的样式。
        '此样式定义在 themes\generic.xaml 中
        DefaultStyleKeyProperty.OverrideMetadata(GetType(MaximizeFontIcon), new FrameworkPropertyMetadata(GetType(MaximizeFontIcon)))
    End Sub

    Public Property WindowState As WindowState
        Get
            Return GetValue(WindowStateProperty)
        End Get
        Set
            SetValue(WindowStateProperty, Value)
        End Set
    End Property
    Public Shared ReadOnly WindowStateProperty As DependencyProperty =
                           DependencyProperty.Register(NameOf(WindowState),
                           GetType(WindowState), GetType(MaximizeFontIcon),
                           New PropertyMetadata(WindowState.Normal))

End Class
