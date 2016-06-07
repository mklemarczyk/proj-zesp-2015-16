Namespace Common

	Public Class FlyoutState
		Inherits DependencyObject

		Public Shared ReadOnly Property IsOpenProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsOpen", GetType(Boolean), GetType(FlyoutState), New PropertyMetadata(False, AddressOf OnIsOpenPropertyChanged))

		Public Shared ReadOnly Property ParentProperty As DependencyProperty = DependencyProperty.RegisterAttached("Parent", GetType(UIElement), GetType(FlyoutState), New PropertyMetadata(Nothing, Nothing))

		Public Shared Sub SetIsOpen(sender As DependencyObject, value As Boolean)
			sender.SetValue(IsOpenProperty, value)
		End Sub

		Public Shared Function GetIsOpen(sender As DependencyObject) As Boolean
			Return sender.GetValue(IsOpenProperty)
		End Function

		Public Shared Sub SetParent(sender As DependencyObject, value As UIElement)
			sender.SetValue(ParentProperty, value)
		End Sub

		Public Shared Function GetParent(sender As DependencyObject) As UIElement
			Return sender.GetValue(ParentProperty)
		End Function

		Private Shared Sub OnIsOpenPropertyChanged(sender As DependencyObject, args As DependencyPropertyChangedEventArgs)
			If sender IsNot Nothing Then
				Dim Flyout = CType(sender, Flyout)
				Dim parent = GetParent(sender)
				Dim newValue As Boolean = args.NewValue
				Dim oldValue As Boolean = args.OldValue
				If (Flyout Is Nothing OrElse parent IsNot Nothing) Then
					If newValue <> oldValue Then
						If newValue Then
							RemoveHandler Flyout.Opening, AddressOf Flyout_Opened
							AddHandler Flyout.Closed, AddressOf Flyout_Closed
							Flyout.ShowAt(parent)
						Else
							RemoveHandler Flyout.Closed, AddressOf Flyout_Closed
							AddHandler Flyout.Opening, AddressOf Flyout_Opened
							Flyout.Hide()
						End If
					End If
				End If
			End If
		End Sub

		Private Shared Sub Flyout_Opened(_sender As Object, _args As Object)
			_sender.SetValue(IsOpenProperty, True)
		End Sub
		Private Shared Sub Flyout_Closed(_sender As Object, _args As Object)
			_sender.SetValue(IsOpenProperty, False)
		End Sub
	End Class
End Namespace
