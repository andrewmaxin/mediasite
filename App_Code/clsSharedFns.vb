Imports Microsoft.VisualBasic

Public Class clsSharedFns

    Public Shared Function GetFileNameString(ByVal p_intID As Integer, ByVal p_intfileType As Integer) As String

        Dim strRet As String = "00000000"

        If p_intfileType = 0 Then
            strRet = "00000000" & p_intID
            strRet = Right(strRet, 8)
        Else
            strRet = "00000000" & p_intID
            strRet = "1" & Right(strRet, 7)
        End If

        GetFileNameString = strRet
    End Function


    Public Shared Function DeleteFileFromRepos(ByVal p_intID As Integer, ByVal p_intfileType As Integer) As Boolean
        Dim strFileName As String

        Try


            strFileName = ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot").ToString & "\" & GetFileNameString(p_intID, p_intfileType) & ".dat"
            If System.IO.File.Exists(strFileName) = True Then
                System.IO.File.Delete(strFileName)
            End If

            DeleteFileFromRepos = True

        Catch ex As Exception
            DeleteFileFromRepos = False
        End Try


    End Function

    Public Shared Sub PurgeTempFiles()

        Try

            Dim strTempBin As String = ConfigurationManager.AppSettings("gsSiteFilesPhysicalRoot_temp")
            Dim objFolder As New System.IO.DirectoryInfo(strTempBin)
            Dim objFile As System.IO.FileInfo

            For Each objFile In objFolder.GetFiles
                Try
                    objFile.Delete()
                Catch ex As Exception

                End Try
            Next

        Catch ex As Exception

        End Try

    End Sub

    Public Shared Function DeleteFileFromRepos(ByVal p_strFileToDel As String) As Boolean
        Dim strFileName As String = p_strFileToDel

        Try

            If System.IO.File.Exists(strFileName) = True Then
                System.IO.File.Delete(strFileName)
            End If

            DeleteFileFromRepos = True

        Catch ex As Exception
            DeleteFileFromRepos = False
        End Try


    End Function

End Class
