Imports Newtonsoft.Json

Public Class WebViewResponse

    Public Property Status As Integer

    Public Property Body As String

    Public Shared Function BadRequestResponse(Optional message As String = "") As WebViewResponse
        Return New WebViewResponse() With {
            .Status = 400,
            .Body = message
        }
    End Function

    Public Shared Function NotFoundResponse() As WebViewResponse
        Return New WebViewResponse() With {
            .Status = 404,
            .Body = String.Empty
        }
    End Function

    Public Shared Function InternalServerErrorResponse() As WebViewResponse
        Return New WebViewResponse() With {
            .Status = 500,
            .Body = String.Empty
        }
    End Function

    Public Function ToJson() As String
        Return JsonConvert.SerializeObject(Me)
    End Function

End Class
