Namespace Models
    Public Class FakeDeviceViewModel
        Inherits DeviceViewModel

        Public Shared ReadOnly Fake As FakeDeviceViewModel = New FakeDeviceViewModel

        Public Overrides ReadOnly Property ImagePath As String = Nothing

        Private Sub New()
            MyBase.New(Nothing)
        End Sub

        Protected Overrides Function GetNamePattern() As String
            Return Nothing
        End Function

        Protected Overrides Function GetNewName() As String
            Return Nothing
        End Function
    End Class
End Namespace
