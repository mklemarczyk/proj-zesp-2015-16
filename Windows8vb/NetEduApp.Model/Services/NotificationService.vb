Imports Windows.Data.Xml.Dom
Imports Windows.UI.Notifications

Namespace Services
    Public Class NotificationService
        Public Shared Sub ShowToast(title As String, body As String)
            ' Get a toast XML template
            Dim toastXml As XmlDocument = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02)

            ' Fill in the text elements
            Dim stringElements As XmlNodeList = toastXml.GetElementsByTagName("text")
            stringElements(0).AppendChild(toastXml.CreateTextNode(title))
            stringElements(1).AppendChild(toastXml.CreateTextNode(body))

            Dim toast As ToastNotification = New ToastNotification(toastXml)

            ToastNotificationManager.CreateToastNotifier().Show(toast)
        End Sub

        Public Shared Sub ShowToast(body As String)
            ' Get a toast XML template
            Dim toastXml As XmlDocument = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02)

            ' Fill in the text elements
            Dim stringElements As XmlNodeList = toastXml.GetElementsByTagName("text")
            stringElements(0).AppendChild(toastXml.CreateTextNode(Package.Current.DisplayName))
            stringElements(1).AppendChild(toastXml.CreateTextNode(body))

            Dim toast As ToastNotification = New ToastNotification(toastXml)

            ToastNotificationManager.CreateToastNotifier().Show(toast)
        End Sub
    End Class
End Namespace
