
Namespace Mvvm

    Public Class PointerRoutedArgsConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.Convert
            If TypeOf value Is PointerRoutedEventArgs And TypeOf parameter Is UIElement Then
                Dim args As PointerRoutedEventArgs = value
                Dim element As UIElement = parameter
                Return args.GetCurrentPoint(element).Position
            End If
            Return Nothing
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, language As String) As Object Implements IValueConverter.ConvertBack
            Return Nothing
        End Function

    End Class

End Namespace
