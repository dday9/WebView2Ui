Imports Newtonsoft.Json

Public Class WebViewRequest

    Public Property Controller As String

    Public Property Route As String

    Public Property Data As String

    Public Function GetModelFromJson(Of T)() As T
        If (String.IsNullOrWhiteSpace(Data)) Then
            Throw New ArgumentNullException(NameOf(Data))
        End If

        Dim model = JsonConvert.DeserializeObject(Of T)(Data)
        Return model
    End Function

    Public Function ToJson() As String
        Return JsonConvert.SerializeObject(Me)
    End Function

End Class
