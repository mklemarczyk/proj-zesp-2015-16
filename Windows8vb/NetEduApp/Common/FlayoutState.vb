Namespace Common

	Public Class FlyoutState
		Inherits DependencyObject

		Public Shared ReadOnly Property IsOpenProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsOpen", GetType(Boolean), GetType(FlyoutState), New PropertyMetadata(False, AddressOf OnIsOpenPropertyChanged))

		Public Shared ReadOnly Property ParentProperty As DependencyProperty = DependencyProperty.RegisterAttached("Parent", GetType(UIElement), GetType(FlyoutState), New PropertyMetadata(Nothing, AddressOf OnParentPropertyChanged))

		Public Shared Sub SetIsOpen(sender As DependencyObject, value As Boolean)
			If sender IsNot Nothing And IsOpenProperty IsNot Nothing Then
				sender.SetValue(IsOpenProperty, value)
			End If
		End Sub

		Public Shared Function GetIsOpen(sender As DependencyObject) As Boolean
			If sender IsNot Nothing And IsOpenProperty IsNot Nothing Then
				Return sender.GetValue(IsOpenProperty)
			Else
				Return Nothing
			End If
		End Function

		Public Shared Sub SetParent(sender As DependencyObject, value As UIElement)
			If sender IsNot Nothing And IsOpenProperty IsNot Nothing Then
				sender.SetValue(ParentProperty, value)
			End If
		End Sub

		Public Shared Function GetParent(sender As DependencyObject) As UIElement
			If sender IsNot Nothing And IsOpenProperty IsNot Nothing Then
				Return sender.GetValue(ParentProperty)
			Else
				Return Nothing
			End If
		End Function

		Private Shared Sub OnParentPropertyChanged(sender As DependencyObject, args As DependencyPropertyChangedEventArgs)
			If sender IsNot Nothing Then
				Dim Flyout = CType(sender, Flyout)
				If (Flyout Is Nothing) Then
					AddHandler Flyout.Opening,
					Sub(_sender, _args)
						Flyout.SetValue(IsOpenProperty, True)
					End Sub
					AddHandler Flyout.Closed,
					Sub(_sender, _args)
						Flyout.SetValue(IsOpenProperty, False)
					End Sub
				End If
			End If
		End Sub

		Private Shared Sub OnIsOpenPropertyChanged(sender As DependencyObject, args As DependencyPropertyChangedEventArgs)
			If sender IsNot Nothing Then
				Dim Flyout = CType(sender, Flyout)
				Dim parent = GetParent(sender)

				If (Flyout Is Nothing OrElse parent IsNot Nothing) Then
					Dim newValue = CType(args.NewValue, Boolean)
					If (newValue) Then
						Flyout.ShowAt(parent)
					Else
						Flyout.Hide()
					End If
				End If
			End If
		End Sub
	End Class
End Namespace
