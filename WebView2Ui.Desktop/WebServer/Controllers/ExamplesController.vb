Imports Newtonsoft.Json
Imports WebView2Ui.Domain.WebView2Ui.Domain.Database.Models
Imports WebView2Ui.Domain.WebView2Ui.Domain.Services
Imports WebView2Ui.Domain.WebView2Ui.Domain.Services.Loggers

Public Class ExamplesController
    Implements IController

    Public Sub New(logger As ILogger, service As ExampleService)
        _Logger = logger
        _Routes = BuildRoutes()
        _exampleService = service
    End Sub

    Public ReadOnly Property Routes As Dictionary(Of String, IController.Route) Implements IController.Routes
    Public ReadOnly Property Logger As ILogger Implements IController.Logger
    Private ReadOnly _exampleService As ExampleService

    Private Function GetById(data As WebViewRequest) As WebViewResponse
        Dim id As Guid? = Nothing
        Try
            id = data.GetModelFromJson(Of Guid)()
        Catch ex As Exception
            _Logger.LogError($"Invalid id passed to the route. Exception:{Environment.NewLine}{ex}")
            Return WebViewResponse.BadRequestResponse("The data is not a valid id.")
        End Try

        Dim matchedExample = _exampleService.GetRecordById(id.Value)
        Dim body As String
        Dim status As Integer
        If (matchedExample IsNot Nothing) Then
            status = 200
            body = JsonConvert.SerializeObject(matchedExample)
        Else
            status = 404
            body = $"The example does not exist with id: {id}"
        End If

        ' simulate a long running task
        Threading.Thread.Sleep(1000)

        Return New WebViewResponse() With {
            .Status = status,
            .Body = body
        }
    End Function

    Private Function GetAll(data As WebViewRequest) As WebViewResponse
        Dim roles = _exampleService.GetAll()

        Dim body As String
        Dim status As Integer
        If (roles Is Nothing) Then
            status = 500
            body = "Something went wrong getting the examples."
        Else
            status = 200
            body = JsonConvert.SerializeObject(roles)
        End If

        ' simulate a long running task
        Threading.Thread.Sleep(1000)

        Return New WebViewResponse() With {
            .Body = body,
            .Status = status
        }
    End Function

    Private Function Create(data As WebViewRequest) As WebViewResponse
        Dim model As ExampleModel = Nothing
        Try
            model = data.GetModelFromJson(Of ExampleModel)()
        Catch ex As Exception
            _Logger.LogError($"Invalid model passed to the route. Exception:{Environment.NewLine}{ex}")
            Return WebViewResponse.BadRequestResponse("The data is not a valid example.")
        End Try

        Dim matchedExample = _exampleService.Create(model)
        Dim body As String
        Dim status As Integer
        If (matchedExample IsNot Nothing) Then
            status = 200
            body = JsonConvert.SerializeObject(matchedExample)
        Else
            status = 500
            body = $"Something went wrong creating the example."
        End If

        ' simulate a long running task
        Threading.Thread.Sleep(1000)

        Return New WebViewResponse() With {
            .Status = status,
            .Body = body
        }
    End Function

    Private Function Update(data As WebViewRequest) As WebViewResponse
        Dim model As ExampleModel = Nothing
        Try
            model = data.GetModelFromJson(Of ExampleModel)()
        Catch ex As Exception
            _Logger.LogError($"Invalid model passed to the route. Exception:{Environment.NewLine}{ex}")
            Return WebViewResponse.BadRequestResponse("The data is not a valid example.")
        End Try

        Dim matchedExample = _exampleService.Update(model)
        Dim body As String
        Dim status As Integer
        If (matchedExample IsNot Nothing) Then
            status = 200
            body = JsonConvert.SerializeObject(matchedExample)
        Else
            status = 500
            body = $"Something went wrong creating the example."
        End If

        ' simulate a long running task
        Threading.Thread.Sleep(1000)

        Return New WebViewResponse() With {
            .Status = status,
            .Body = body
        }
    End Function

    Private Function BuildRoutes() As Dictionary(Of String, IController.Route)
        Dim routes = New Dictionary(Of String, IController.Route) From {
            {"GetAll", New IController.Route(AddressOf GetAll)},
            {"Get", New IController.Route(AddressOf GetById)},
            {"Create", New IController.Route(AddressOf Create)},
            {"Update", New IController.Route(AddressOf Update)}
        }
        Return routes
    End Function

End Class
