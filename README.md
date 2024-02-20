# WebView2Ui
In case you missed it, Google won the browser wars although FireFox still nips at their heels. Most every browser now uses Chromium as the base web browser. Before this more universal approach to browsers, there were many shim libraries like jQuery that handled cross-browser capability, but now we really don't have that problem anymore; this makes front-end web development much easier for developers.

So how does this relate to desktop development and .NET specifically? Well as a result of Internet Explorer being dropped and Edge being Microsoft's browser, they also released a new desktop control to replace the old WebBrowser called WebView2 ([documentation]("https://aka.ms/webview")). What is great with the WebView2 is that not only can you send information to the browser from your application like you could in the old WebBrowser **but** you can send information from the browser back to your application. This means that we can display all of our UI in a WebView2 using all the niceties that front-end web development bring us, but have all of the business logic and simplicity of C# or VB.NET.

## Prerequisites
The first thing you will need to do is install the WebView2 NuGet package, do this from your WinForm application project:

1. From the solution explorer, right-click on your project.
2. Click on *Manage NuGet Packages*.
3. From the NuGet Package Manager, click on the *Browse* tab.
4. Search for: Microsoft.Web.WebView2
5. Install the most recent version.
6. Build your project (Ctrl + Shift + B or Build > Build Solution).

After that the WebView2 control should show up in your toolbox and you can drag and drop the control onto a form.

I am also using Newtonsoft.Json to handle the data as JSON, follow those same steps above for the Newtonsoft.Json package.

The second thing you will need to do is essentially setup a mini web server in your desktop project. This is a major step, so I going to do my best to explain how this work.

I would suggest setting up a file structure in your project to look like this:

```
/
└── my-app
    ├── WebAssets
    ├── WebPages
    └── WebServer
        ├── Controllers
        └── Models
            ├── WebViewRequest.vb
            └── WebViewResponse.vb
```

- WebAssets would hold things like your third party libraries (Bootstrap) or custom shared code.
- WebPages will hold only folders and the sub-folders will hold the actual HTML, CSS, and JavaScript pages.
- The WebServer > Controllers will have files that represent the controllers which handle things like routing.
- The WebServer > Models hold the request and response models, essentially replicating an HTTP request/response.

Because I will be showing you how to setup this mini web server using a MVC pattern, let's assume that you have: `www.something.com/Users/Index.html` and `www.something.com/Users/Update.html` then this would live in WebPages > Users > Index.html and WebPages > Users > Update.html respectively.

Here are the two code files from the file structure:
- [WebViewRequest.vb]("./WebView2Ui.Desktop/WebServer/Models/WebViewRequest.vb")
- [WebViewResponse.vb]("./WebView2Ui.Desktop/WebServer/Models/WebViewResponse.vb")

## Controllers
Now that you have the file structure and web request/response classes, it's time to start working on the controllers. Because the controllers will essentially be doing the same thing just for different pages, I would suggest creating a "base" controller. This can either be an abstract class (mustinherit in VB.NET) or an interface.

Regardless the code should define a few things:

1. A delegate called "Route" that represents a reference of a function that will take in a request and return a response.
2. A key/value collection of strings and "Route" delegates.

Here is an example (using an interface) [IController.vb]("./WebView2Ui.Desktop/WebServer/Controllers/IController.vb") and here is a class that implements the interface [ExamplesController]("./WebView2Ui.Desktop/WebServer/Controllers/ExamplesController.vb")

By setting it up like this, you can then create controller classes that implement/inherit from the base controller then build the collection of Routes as well as the corresponding business logic.

Now in the Form's code that uses the WebView2, you can build a controller mapping by defining a key/value collection of strings and BaseControllers so that when a user hits the API endpoint */Controller/Route* it will find the controller's key and the route's key to do a lookup on the controllers/routes mapping.

## WebPages
Now that you have the controllers setup, it's time to start working on the actual web pages. I mentioned earlier that the WebPages folder will only hold folders and that these sub-folders will hold the actual HTML, CSS, and JavaScript pages.

Assuming I want a search page, create page, and update page for an entity called "Example" then I would probably setup the following file structure:

```
/
└── my-app
    └── WebPages
        └── Examples
            ├── Index.html
            └── Upsert.html
```

**IMPORTANT** If you miss this step then your HTML pages cannot be loaded into the WebView2. After you add your HTML (or CSS, JavaScript, etc.) file, you need to:

1. Open the file properties.
2. Change the Build Action to *Content*.
3. Change the Copy to Output Directory to *Copy if newer*.


### WebPage to .NET Communication
There are a couple of key points for the communicate between your webpages and .NET code. The first is that if you want to send data from your webpage to your .NET code, you will need to invoke the JavaScript window.chrome.webview.postMessage method ([documentation]("https://learn.microsoft.com/en-us/microsoft-edge/webview2/reference/win32/icorewebview2#add_webmessagereceived")):

```
window.chrome.webview.postMessage('...');
```

Look at the buildWebReqest method ([definition]("./WebView2Ui.Desktop/WebAssets/Shared.js#L10")) to see how to build a request to be sent to the .NET code.

Using a combination of the postMessage and buildWebRequest methods, we can now send a mock HttpRequest to the .NET code:
```
' const id = 'some random GUID';
const webMessage = buildWebRequest('Examples', 'Get', JSON.stringify({ Id: id }));
window.chrome.webview.postMessage(webMessage);
```

### .NET to WebPage Communication
If you want to send data from your .NET code to your webpage, you will need to invoke the CoreWebView2.ExecuteScriptAsync method ([documentation]("https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2.executescriptasync")) to execute JavaScript code:

```
' vb.net
Await MyWebView2.CoreWebView2.ExecuteScriptAsync("console.log('...');")

// C#
await MyWebView2.CoreWebView2.ExecuteScriptAsync("console.log('...');");
```

This is important to know because we need to essentially send requests asynchronously from our JavaScript code and then wait on a response from our VB.NET code. This means that you cannot use typical AJAX requests like fetch. Instead, what I would suggest doing is creating a shared JavaScript function that essentially dispatches an event that indicates we have received data from our .NET code.

Look at the dispatchMessageReceived event ([definition]("./WebView2Ui.Desktop/WebAssets/Shared.js#L1")) to see how that is being done.

Using a combination of the ExecuteScriptAsync method and handling the dispatchMessageReceived event for the window, we can now push a mock HttpResponse back to the JavaScript code:

```
' vb.net
Await WebView2Container.CoreWebView2.ExecuteScriptAsync($"dispatchMessageReceivedEvent({request.ToJson()}, {response.ToJson()});")

// c#
await WebView2Container.CoreWebView2.ExecuteScriptAsync($"dispatchMessageReceivedEvent({request.ToJson()}, {response.ToJson()});");
```

## Virtual Hosting

From the WebView2, it is possible to setup virtual hosting. This allows you to point a URL (e.g `https://app-assets.local`) to a physical directory on the filesystem. This is useful because by setting up virtual hosting, you no longer have to rely on relative paths.

To do this, create a method to "configure" the WebView2 that gets called in the Form's Load event that does the following:

1. Call the EnsureCoreWebView2Async method ([documentation]("https://learn.microsoft.com/en-us/windows/winui/api/microsoft.ui.xaml.controls.webview2.ensurecorewebview2async")).
2. Assert that the directory exists.
3. Set the virtual mapping to the directory using the CoreWebView2.SetVirtualHostNameToFolderMapping method ([documentation]("https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2.setvirtualhostnametofoldermapping")).
4. Set the CoreWebView2.Settings.IsWebMessageEnabled property ([documentation]("https://learn.microsoft.com/en-us/dotnet/api/microsoft.web.webview2.core.corewebview2settings.iswebmessageenabled#microsoft-web-webview2-core-corewebview2settings-iswebmessageenabled")) to true.

VB.NET Example:
```
Private Async Function ConfigureWebView2() As Task
    Await MyWebView2.EnsureCoreWebView2Async(Nothing)
    Dim webAssets = AssertApplicationDirectoryPath("WebAssets")
    MyWebView2.CoreWebView2.SetVirtualHostNameToFolderMapping("app-assets.local", webAssets, CoreWebView2HostResourceAccessKind.Allow)
    MyWebView2.CoreWebView2.Settings.IsWebMessageEnabled = True
End Function

Private Shared Function AssertApplicationDirectoryPath(ParamArray filePathParts() As String) As String
    Dim paths = {Application.StartupPath}.Concat(filePathParts).ToArray()
    Dim contentPath = Path.Combine(paths)
    If (Not Directory.Exists(contentPath)) Then
        Throw New ArgumentOutOfRangeException(NameOf(filePathParts), $"The following file does not exit: {contentPath}")
    End If
    Return contentPath
End Function
```

C# Example:

```
private async Task ConfigureWebView2()
{
    await MyWebView2.EnsureCoreWebView2Async(null);
    var webAssets = AssertApplicationDirectoryPath("WebAssets");
    MyWebView2.CoreWebView2.SetVirtualHostNameToFolderMapping("app-assets.local", webAssets, CoreWebView2HostResourceAccessKind.Allow);
    MyWebView2.CoreWebView2.Settings.IsWebMessageEnabled = true;
}

private static string AssertApplicationDirectoryPath(params string[] filePathParts)
{
    var paths =
    {
        Application.StartupPath
    }.Concat(filePathParts).ToArray();

    var contentPath = Path.Combine(paths);
    if ((!Directory.Exists(contentPath)))
    {
        throw new ArgumentOutOfRangeException(nameof(filePathParts), $"The following file does not exit: {contentPath}");
    }
    return contentPath;
}
```

With this example, anytime the page is loaded in the WebView2, any instance of `https://app-assets.local/` knows to point to the application's WebAssets directory. This is useful so that we do not have to rely on relative paths, which in my experience are a pain in the rear to manage.

## Loading WebPages
Finally, we need to actually load the webpage into our WebView2. This is done by:

1. Assert that the HTML file we want to load actually exists.
2. Construct a new URL using the file we want to load's path.
3. Call the CoreWebView2.Navigate method, passing the URL.

VB.NET Code
```
Private Sub OpenWebPage(container As WebView2, ParamArray filePathParts() As String)
    Dim fileContentPath = AssertApplicationFilePath(filePathParts)
    Dim url = New Uri(fileContentPath).ToString()
    container.CoreWebView2.Navigate(url)
End Sub

Private Shared Function AssertApplicationFilePath(ParamArray filePathParts() As String) As String
    Dim filePaths = {Application.StartupPath}.Concat(filePathParts).ToArray()
    Dim contentPath = Path.Combine(filePaths)
    If (Not File.Exists(contentPath)) Then
        Throw New ArgumentOutOfRangeException(NameOf(filePathParts), $"The following file does not exit: {contentPath}")
    End If

    Return contentPath
End Function
```

C# Code
```
private void OpenWebPage(WebView2 container, params string[] filePathParts)
{
	var fileContentPath = AssertApplicationFilePath(filePathParts);
	var url = new Uri(fileContentPath).ToString();
	container.CoreWebView2.Navigate(url);
}

private static string AssertApplicationFilePath(params string[] filePathParts)
{
	var filePaths =
	{
		Application.StartupPath
	}.Concat(filePathParts).ToArray();
	var contentPath = Path.Combine(filePaths);
	if ((!File.Exists(contentPath)))
		throw new ArgumentOutOfRangeException(nameof(filePathParts), $"The following file does not exit: {contentPath}");

	return contentPath;
}
```

##  Security Concerns
Using a WebView2 to essentially build the application's UI does come with some security risks. Keep in mind that just about anyone can open open the WebView2's developer tools to modify content in the DOM (e.g. change `<input type="password" />` to `<input type="text" />` and even post messages back to the .NET application.

It is important when building applications where the WebView2 is the UI to take those security concerns into consideration.

## Conclusion
The solution provided in this repository is meant to be a template in how to build rich UI desktop applications using front-end web development and  traditional .NET code.

Some of the stuff like what is going on in the Domain project is not what I'd use for enterprise level software, it is mearly designed to have the minimal code needed to demonstrate how the code works.
