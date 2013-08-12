Imports Microsoft.VisualBasic
Imports System.Net.Mail

Public Class clsEmail
    Dim mSmtpClient As SmtpClient

    Public Sub SendPwdEmail(ByRef p_objSecurity As clsSecurity)
        Dim strTemp As String = ""
        Dim strRecipient As String = p_objSecurity.EmailAddress
        Dim strFrom As String = ConfigurationManager.AppSettings("g_MailUserName").ToString

        'strTemp = "Your password is: " & p_objSecurity.Password

        SendMailMessage(strFrom, strRecipient, "", "", "Password Reminder", strTemp)

    End Sub

    Public Sub SendMailMessage(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String)
        ' Instantiate a new instance of MailMessage
        Dim mMailMessage As New MailMessage()

        ' Set the sender address of the mail message
        mMailMessage.From = New MailAddress(from)
        ' Set the recepient address of the mail message
        mMailMessage.To.Add(New MailAddress(recepient))

        ' Check if the bcc value is nothing or an empty string
        If Not bcc Is Nothing And bcc <> String.Empty Then
            ' Set the Bcc address of the mail message
            mMailMessage.Bcc.Add(New MailAddress(bcc))
        End If

        ' Check if the cc value is nothing or an empty value
        If Not cc Is Nothing And cc <> String.Empty Then
            ' Set the CC address of the mail message
            mMailMessage.CC.Add(New MailAddress(cc))
        End If

        ' Set the subject of the mail message
        mMailMessage.Subject = subject
        ' Set the body of the mail message
        mMailMessage.Body = body

        ' Set the format of the mail message body as HTML
        mMailMessage.IsBodyHtml = True
        ' Set the priority of the mail message to normal
        mMailMessage.Priority = MailPriority.Normal

        ' Instantiate a new instance of SmtpClient

        ' Send the mail message
        mSmtpClient.Send(mMailMessage)

    End Sub


    Public Sub New()

        mSmtpClient = New SmtpClient(ConfigurationManager.AppSettings("gsMailHost").ToString, 465)

        mSmtpClient.EnableSsl = True
        mSmtpClient.Credentials = New Net.NetworkCredential(ConfigurationManager.AppSettings("g_MailUserName").ToString, ConfigurationManager.AppSettings("g_MailPassword").ToString)


    End Sub

    Protected Overrides Sub Finalize()

        mSmtpClient = Nothing

        MyBase.Finalize()

    End Sub
End Class
