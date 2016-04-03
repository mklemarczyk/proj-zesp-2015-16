Namespace Mvvm
	Public Class DelegateCommand
		Implements ICommand

		Private _executeAction As ExecuteAction
		Private _canExecuteAction As CanExecuteAction

		Public Sub New(executeAction As ExecuteAction, canExecuteAction As CanExecuteAction)
			Me._executeAction = executeAction
			Me._canExecuteAction = canExecuteAction
		End Sub

		Public Delegate Sub ExecuteAction(parameter As Object)
		Public Delegate Function CanExecuteAction(parameter As Object) As Boolean

		Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

		Public Sub Execute(parameter As Object) Implements ICommand.Execute
			If _executeAction IsNot Nothing Then
				_executeAction(parameter)
			End If
		End Sub

		Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
			If _canExecuteAction IsNot Nothing Then
				Return _canExecuteAction(parameter)
			Else
				Return True
			End If
		End Function

		Public Sub RaiseCanExecuteChanged()
			RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
		End Sub
	End Class
End Namespace
