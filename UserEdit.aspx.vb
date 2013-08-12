Imports System.Data

Partial Class UserEdit
    Inherits System.Web.UI.Page
    Public m_objEditingUser As clsSecurity
    Public m_objUserToEdit As clsSecurity
    Private m_objPageConn As OleDb.OleDbConnection
    Private m_blnUserCreateMode As Boolean = False
    Private m_blnAdminMode As Boolean = False
    Private m_blnEditingSelf As Boolean = False

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim lngID As Integer

        If Not Page.IsPostBack = True Then

            If HttpContext.Current.Session("Security") Is Nothing Then Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

            m_objEditingUser = HttpContext.Current.Session("Security")

            lngID = CInt(Val(Request.QueryString("ID") & ""))

            m_objUserToEdit = New clsSecurity()


            If lngID = 0 Then
                'No User ID spec'd --- Bounce!
                Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))

            ElseIf lngID = -1 Then
                'Create User Mode

                If m_objEditingUser.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserAdd) = False Then
                    Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))
                End If

                m_blnUserCreateMode = True
                m_blnAdminMode = True
                permPanel.Visible = True

            ElseIf lngID <> m_objEditingUser.UserID Then

                If m_objEditingUser.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserEdit) = False Then
                    Response.Redirect(ConfigurationManager.AppSettings("gsSiteURL"))
                End If

                m_blnUserCreateMode = False
                m_blnAdminMode = True
                permPanel.Visible = True

                Dim intRes As Integer
                intRes = m_objUserToEdit.LoadUser(lngID)

                Select intRes
                    Case 0 'not found
                        Response.Write("Not Found")
                    Case 69
                        Response.Write(m_objUserToEdit.LastError)
                    Case 1
                        LoadForm()
                End Select

            ElseIf lngID = m_objEditingUser.UserID Then
                'Self-Edit, were cool

                If m_objEditingUser.CheckPermission(clsSecurity.Permission_ActionCodes.act_UserEdit) = True Then
                    permPanel.Visible = True
                Else
                    permPanel.Visible = False
                End If

                m_blnEditingSelf = True
                m_blnUserCreateMode = False
                m_blnAdminMode = False

                m_objUserToEdit = m_objEditingUser
                LoadForm()

            End If

        End If



    End Sub

    Private Sub LoadForm()

        If Not IsPostBack Then
            With m_objUserToEdit

                txtFirstName.Text = .FirstName
                txtLastName.Text = .LastName
                txtEmail.Text = .EmailAddress
                txtPassword.Text = ""
                txtUserName.Text = .UserName
                UserID.Value = .UserID

                chkAllowReporting.Checked = .Permission(clsSecurity.Permissions.reporting)

                chkDocBinDownload.Checked = .Permission(clsSecurity.Permissions.docbinDL)
                chkDocBinUpload.Checked = .Permission(clsSecurity.Permissions.docbinUL)

                chkmp3Download.Checked = .Permission(clsSecurity.Permissions.mp3dwload)
                chkMp3Upload.Checked = .Permission(clsSecurity.Permissions.mp3upload)

                chkpptDownload.Checked = .Permission(clsSecurity.Permissions.pptdwn)
                chkpptUpload.Checked = .Permission(clsSecurity.Permissions.pptupl)

                chkSetAdmin.Checked = .Permission(clsSecurity.Permissions.setadmin)
                chkSiteAdmin.Checked = .Permission(clsSecurity.Permissions.siteadmin)

                chkSongDel.Checked = .Permission(clsSecurity.Permissions.songdel)
                chkSongEdit.Checked = .Permission(clsSecurity.Permissions.songedit)
                chkSongView.Checked = .Permission(clsSecurity.Permissions.songview)

                chkUserEnabled.Checked = Not .UserLocked


            End With
        End If


    End Sub

    Protected Sub cmdCancel_Click(sender As Object, e As System.EventArgs) Handles cmdCancel.Click
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close()", True)
    End Sub

    Protected Sub cmdSave_Click(sender As Object, e As System.EventArgs) Handles cmdSave.Click

        SaveForm()

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close()", True)

    End Sub

    Private Sub SaveForm()


        With m_objEditingUser
            '.EmailAddress = txtEmail.Text
            '.FirstName = txtFirstName.Text
            '.LastName = txtLastName.Text
            '.UserName = txtUserName.Text
            '.UserLocked = Not (chkUserEnabled.Checked)

            'If txtPassword.Text <> "" Then
            '    .Password = txtPassword.Text
            'End If

            '.Permission(clsSecurity.Permissions.reporting) = chkAllowReporting.Checked

            'chkDocBinDownload.Checked = .Permission(clsSecurity.Permissions.docbinDL)
            'chkDocBinUpload.Checked = .Permission(clsSecurity.Permissions.docbinUL)

            'chkmp3Download.Checked = .Permission(clsSecurity.Permissions.mp3dwload)
            'chkMp3Upload.Checked = .Permission(clsSecurity.Permissions.mp3upload)

            'chkpptDownload.Checked = .Permission(clsSecurity.Permissions.pptdwn)
            'chkpptUpload.Checked = .Permission(clsSecurity.Permissions.pptupl)

            'chkSetAdmin.Checked = .Permission(clsSecurity.Permissions.setadmin)
            'chkSiteAdmin.Checked = .Permission(clsSecurity.Permissions.siteadmin)

            'chkSongDel.Checked = .Permission(clsSecurity.Permissions.songdel)
            'chkSongEdit.Checked = .Permission(clsSecurity.Permissions.songedit)
            'chkSongView.Checked = .Permission(clsSecurity.Permissions.songview)


        End With

        'm_objEditingUser.SaveUser


        If m_blnEditingSelf = True Then
            HttpContext.Current.Session("Security") = m_objEditingUser
        End If

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myCloseScript", "window.close()", True)

    End Sub

    Protected Sub lstInstruments_PreRender(sender As Object, e As System.EventArgs) Handles lstInstruments.PreRender
        Try


            'Response.Write(m_objUserToEdit.AvailInst)
            Dim objItem As ListItem
            Dim strArrInsts() As String = Split(m_objUserToEdit.AvailInst, "|")
            Dim strArrItem As String


            lstInstruments.SelectionMode = ListSelectionMode.Multiple
            lstInstruments.ClearSelection()

            If m_objUserToEdit.AvailInst <> "" Then
                For Each strArrItem In strArrInsts

                    If strArrItem <> "" Then
                        'Response.Write(strArrItem)
                        For Each objItem In lstInstruments.Items

                            If objItem.Value = Trim(strArrItem) Then
                                'Response.Write(objItem.Text)
                                objItem.Selected = True
                                Exit For
                            End If

                        Next

                    End If

                Next

            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
