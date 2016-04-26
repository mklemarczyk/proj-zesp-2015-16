Namespace Common
    Public Class OffsetConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            If TypeOf value Is Double Then
                Dim x = CDbl(value)
                If TypeOf parameter Is Double Then
                    Dim o = CDbl(parameter)
                    Return x + o
                Else
                    Return x + 35
                End If
            End If
            Throw New NotImplementedException("Type not supported")
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            If TypeOf value Is Double Then
                Dim x = CDbl(value)
                If TypeOf parameter Is Double Then
                    Dim o = CDbl(parameter)
                    Return x - o
                Else
                    Return x - 35
                End If
            End If
            Throw New NotImplementedException("Type not supported")
        End Function
    End Class
End Namespace
