<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        WebView2Container = New Microsoft.Web.WebView2.WinForms.WebView2()
        CType(WebView2Container, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' WebView2Container
        ' 
        WebView2Container.AllowExternalDrop = True
        WebView2Container.CreationProperties = Nothing
        WebView2Container.DefaultBackgroundColor = Color.White
        WebView2Container.Dock = DockStyle.Fill
        WebView2Container.Location = New Point(0, 0)
        WebView2Container.Name = "WebView2Container"
        WebView2Container.Size = New Size(800, 450)
        WebView2Container.TabIndex = 0
        WebView2Container.ZoomFactor = 1R
        ' 
        ' FormMain
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(WebView2Container)
        Name = "FormMain"
        Text = "WebView2Ui"
        WindowState = FormWindowState.Maximized
        CType(WebView2Container, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents WebView2Container As Microsoft.Web.WebView2.WinForms.WebView2

End Class
