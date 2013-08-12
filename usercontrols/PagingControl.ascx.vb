
Partial Class usercontrols_WebUserControl
    Inherits System.Web.UI.UserControl

    Public Event First()
    Public Event Prev()
    Public Event Go_To(ByVal p_int As Integer)
    Public Event goNext()
    Public Event goLast()

    Private m_intPgCt As Integer

    Public Property PageCount As Integer
        Get
            Return m_intPgCt
        End Get
        Set(value As Integer)
            m_intPgCt = value
            Label1.Text = "of " & m_intPgCt & " pages"
        End Set
    End Property

    Public Property PageNum As Integer
        Get
            Return CInt(txtPage.Text)
        End Get
        Set(ByVal value As Integer)
            txtPage.Text = value
        End Set
    End Property

    Protected Sub cmdFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdFirst.Click
        ' RaiseEvent First()
        RaiseEvent Go_To(1)
        txtPage.Text = "1"

    End Sub

    Protected Sub cmdLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdLast.Click
        ' RaiseEvent goLast()
        If m_intPgCt > 0 Then

            RaiseEvent Go_To(m_intPgCt)
            txtPage.Text = CStr(m_intPgCt)
        Else
            RaiseEvent Go_To(1)
            txtPage.Text = "1"
        End If

    End Sub

    Protected Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdNext.Click

        If IsNumeric(txtPage.Text) Then
            RaiseEvent Go_To(CInt(txtPage.Text) + 1)
            txtPage.Text = CStr(CInt(txtPage.Text) + 1)
        Else
            RaiseEvent Go_To(1)
            txtPage.Text = "1"
        End If
        ' RaiseEvent goNext()
    End Sub

    Protected Sub cmdPrev_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdPrev.Click

        If IsNumeric(txtPage.Text) Then
            RaiseEvent Go_To(CInt(txtPage.Text) - 1)
            txtPage.Text = CStr(CInt(txtPage.Text) - 1)
        Else
            RaiseEvent Go_To(1)
            txtPage.Text = "1"
        End If

        'RaiseEvent Prev()
    End Sub

    Protected Sub txtPage_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPage.TextChanged

        If IsNumeric(txtPage.Text) Then
            RaiseEvent Go_To(CInt(txtPage.Text))
        End If

    End Sub
End Class
