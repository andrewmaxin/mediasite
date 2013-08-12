Imports Microsoft.VisualBasic

Public Class clsCrypto


#Region "Var"

    Private m_InitVecKey As String = "initialvector" 'chose your own default Key and Passwords
    Private m_PasswordKey As String = ConfigurationManager.AppSettings("fileencryptionkey")

    Private m_InputFile As String 'Use full drive/path/file names
    Private m_OutputFile As String

    Private Enum CryptoAction
        actionEncrypt = 1
        actionDecrypt = 2
    End Enum
#End Region


#Region "Property"

    Public Property FileInput() As String
        Get
        End Get
        Set(ByVal Value As String)
            m_InputFile = Value
        End Set
    End Property

    Public Property FileOutput() As String
        Get
        End Get
        Set(ByVal Value As String)
            m_OutputFile = Value
        End Set
    End Property

    Public Property Key() 'if you change from your defaults remember them!
        Get
            Key = GetKeyByteArray(m_PasswordKey)
        End Get
        Set(ByVal Value)
            m_PasswordKey = Value
        End Set
    End Property

    Public Property IntiKey() 'if you change from your defaults remember them!
        Get
            IntiKey = GetKeyByteArray(m_InitVecKey)
        End Get
        Set(ByVal Value)
            m_InitVecKey = Value
        End Set
    End Property

#End Region


#Region "Events"

#End Region

#Region "Methods"


    'encrypt file
    Public Sub EncryptFile()
        EncryptOrDecryptFile(CryptoAction.actionEncrypt)
    End Sub

    Public Sub DecryptFile()
        EncryptOrDecryptFile(CryptoAction.actionDecrypt)
    End Sub

    Private Function GetKeyByteArray(ByVal sPassword As String) As Byte()
        Dim byteTemp(7) As Byte
        sPassword = sPassword.PadRight(8) ' make sure we have 8 chars

        Dim iCharIndex As Integer
        For iCharIndex = 0 To 7
            byteTemp(iCharIndex) = Asc(Mid$(sPassword, iCharIndex + 1, 1))
        Next
        Return byteTemp
    End Function

    Private Sub EncryptOrDecryptFile(ByVal Direction As CryptoAction)

        'Create the file streams to handle the input and output files.
        Dim fsInput As New System.IO.FileStream(m_InputFile, System.IO.FileMode.Open, System.IO.FileAccess.Read)
        Dim fsOutput As New System.IO.FileStream(m_OutputFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write)

        fsOutput.SetLength(0)

        'Variables needed during encrypt/decrypt process
        Dim byteBuffer(4096) As Byte 'holds a block of bytes for processing
        Dim nBytesProcessed As Long = 0 'running count of bytes encrypted
        Dim nFileLength As Long = fsInput.Length
        Dim iBytesInCurrentBlock As Integer
        Dim desProvider As New System.Security.Cryptography.DESCryptoServiceProvider()
        Dim csMyCryptoStream As System.Security.Cryptography.CryptoStream

        ' Set up for encryption or decryption
        Select Case Direction
            Case CryptoAction.actionEncrypt
                csMyCryptoStream = New System.Security.Cryptography.CryptoStream(fsOutput, desProvider.CreateEncryptor(Key, IntiKey), System.Security.Cryptography.CryptoStreamMode.Write)

            Case CryptoAction.actionDecrypt
                csMyCryptoStream = New System.Security.Cryptography.CryptoStream(fsOutput, desProvider.CreateDecryptor(Key, IntiKey), System.Security.Cryptography.CryptoStreamMode.Write)

        End Select

        'Read from the input file, then encrypt or decrypt and write to the output file.
        While nBytesProcessed < nFileLength
            iBytesInCurrentBlock = fsInput.Read(byteBuffer, 0, 4096)
            csMyCryptoStream.Write(byteBuffer, 0, iBytesInCurrentBlock)
            nBytesProcessed = nBytesProcessed + CLng(iBytesInCurrentBlock)
        End While

        csMyCryptoStream.Close()
        fsInput.Close()
        fsOutput.Close()

    End Sub

#End Region



End Class
