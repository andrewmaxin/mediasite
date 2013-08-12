
Partial Class usercontrols_navicontrol
    Inherits System.Web.UI.UserControl

    Public Event Refresh(ByVal p_intType As Integer, p_intFilter As Integer, p_strKeywords As String)


    Public ReadOnly Property GroupID As Integer
        Get
            Return DropDownList1.SelectedValue
        End Get
    End Property

    Public ReadOnly Property SearchText As String
        Get
            Return txtSearchText.Text
        End Get
    End Property

    Public ReadOnly Property SearchType As Integer
        Get
            Return DropDownList2.SelectedValue
        End Get
    End Property

    Protected Sub cmdGo_Click(sender As Object, e As System.EventArgs) Handles cmdGo.Click
        RaiseEvent Refresh(DropDownList2.SelectedValue, DropDownList1.SelectedValue, txtSearchText.Text)
    End Sub

    Protected Sub cmdReset_Click(sender As Object, e As System.EventArgs) Handles cmdReset.Click
        DropDownList2.SelectedValue = 0
        Try
            DropDownList1.SelectedValue = 11

        Catch ex As Exception

        End Try

        txtSearchText.Text = ""

    End Sub
End Class
