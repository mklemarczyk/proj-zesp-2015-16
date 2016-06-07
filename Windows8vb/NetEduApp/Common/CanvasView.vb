Imports Windows.UI.Xaml.Markup

Namespace Common
	Public Class CanvasView
		Inherits ItemsControl

		Protected Shared DefaultItemsPanelTemplate = XamlReader.Load("<ItemsPanelTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""><Canvas /></ItemsPanelTemplate>")
		Public Shared ReadOnly Property LeftPathProperty As DependencyProperty = DependencyProperty.Register("LeftPath", GetType(String), GetType(CanvasView), New PropertyMetadata(Nothing, Nothing))
		Public Shared ReadOnly Property TopPathProperty As DependencyProperty = DependencyProperty.Register("TopPath", GetType(String), GetType(CanvasView), New PropertyMetadata(Nothing, Nothing))
		Public Property LeftPath As String
		Public Property TopPath As String

		Public Sub New()
			Me.ItemsPanel = DefaultItemsPanelTemplate
		End Sub

		Protected Overrides Sub PrepareContainerForItemOverride(element As DependencyObject, item As Object)
			Dim leftBinding = New Binding()
			Dim topBinding = New Binding()

			leftBinding.Path = New PropertyPath(Me.LeftPath)
			topBinding.Path = New PropertyPath(Me.TopPath)

			Dim ContentControl = DirectCast(element, FrameworkElement)

			ContentControl.SetBinding(Canvas.LeftProperty, leftBinding)
			ContentControl.SetBinding(Canvas.TopProperty, topBinding)

			MyBase.PrepareContainerForItemOverride(element, item)
		End Sub

	End Class
End Namespace
