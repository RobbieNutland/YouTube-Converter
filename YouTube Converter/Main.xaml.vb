Class MainWindow

    Dim MyProcess As New Process

    Delegate Sub BeginConversionCallback(passedVal)

    Private Sub BeginConversion(passedVal)
        If Application.Current.Dispatcher.CheckAccess() = False Then
            'MessageBox.Show("Thread Access: " + Application.Current.Dispatcher.CheckAccess().ToString)
            Dim MyDelegate As BeginConversionCallback = New BeginConversionCallback(AddressOf BeginConversion)
            Application.Current.Dispatcher.Invoke(MyDelegate, New Object() {passedVal})
        Else
            BtnConvert.Content = passedVal
            BtnConvert.IsEnabled = False

            MyProcess.StartInfo.EnvironmentVariables.Item("url") = TxtUrl.Text
            MyProcess.StartInfo.EnvironmentVariables.Item("name") = TxtName.Text
            MyProcess.StartInfo.EnvironmentVariables.Item("outpath") = TxtOutPath.Text

            If CBoxCropOutput.IsChecked Then
                MyProcess.StartInfo.EnvironmentVariables.Item("cropped") = 1
                MyProcess.StartInfo.EnvironmentVariables.Item("starttime") = TxtStart.Text
                MyProcess.StartInfo.EnvironmentVariables.Item("finishtime") = TxtFinish.Text
            End If

        End If

    End Sub

    Delegate Sub FinishConversionCallback(passedVal)

    Private Sub FinishConversion(passedVal)
        If Application.Current.Dispatcher.CheckAccess() = False Then
            Dim MyDelegate As FinishConversionCallback = New FinishConversionCallback(AddressOf FinishConversion)
            Application.Current.Dispatcher.Invoke(MyDelegate, New Object() {passedVal})
        Else
            BtnConvert.Content = passedVal
            BtnConvert.IsEnabled = True
            MessageBox.Show("Conversion Complete!" + vbNewLine + "Downloaded to: " + TxtOutPath.Text.ToString(), "YouTube Converter", MessageBoxButton.OK, MessageBoxImage.Information)

        End If

    End Sub

    Sub Convert(script As String)

        MyProcess.StartInfo.Arguments = "/c Resources\" + script

        BeginConversion("Converting...")

        MyProcess.Start()
        MyProcess.WaitForExit()
        MyProcess.Close()
        FinishConversion("Convert")
        Threading.Thread.CurrentThread.Abort()

    End Sub

    Sub Button_Click(sender As Object, e As RoutedEventArgs) Handles BtnConvert.Click

        e.Handled = True

        Dim thread As New Threading.Thread(AddressOf Convert)

        If TxtName.Text = "" Or TxtName.Text = "Output Name Here" Then
            'MessageBox.Show("TEST from " + e.Source.GetType().ToString)
            MessageBox.Show("Invlid Output Name", "YouTube Converter", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        ElseIf TxtUrl.Text = "" Or TxtUrl.Text = "Enter URL Here" Then
            MessageBox.Show("Invlid URL", "YouTube Converter", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        ElseIf RadioVideo.IsChecked Then
            thread.Start("Video.bat")
            Exit Sub
        ElseIf RadioAudio.IsChecked Then
            thread.Start("Audio.bat")
            Exit Sub
        Else MessageBox.Show("Invalid Output Type", "YouTube Converter", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        End If
    End Sub

    Private Sub TxtName_GotFocus(sender As Object, e As RoutedEventArgs) Handles TxtName.GotFocus
        If TxtName.Text = "Output Name Here" Then
            TxtName.Text = ""
            TxtName.Foreground = New SolidColorBrush(Color.FromRgb(0, 0, 0))
        End If
    End Sub

    Private Sub TxtName_LostFocus(sender As Object, e As RoutedEventArgs) Handles TxtName.LostFocus
        If TxtName.Text = "" Then
            TxtName.Text = "Output Name Here"
            TxtName.Foreground = New SolidColorBrush(Color.FromRgb(190, 190, 190))
        End If
    End Sub

    Private Sub TxtUrl_GotFocus(sender As Object, e As RoutedEventArgs) Handles TxtUrl.GotFocus
        If TxtUrl.Text = "Enter URL Here" Then
            TxtUrl.Text = ""
            TxtUrl.Foreground = New SolidColorBrush(Color.FromRgb(0, 0, 0))
        End If
    End Sub

    Private Sub TxtUrl_LostFocus(sender As Object, e As RoutedEventArgs) Handles TxtUrl.LostFocus
        If TxtUrl.Text = "" Then
            TxtUrl.Text = "Enter URL Here"
            TxtUrl.Foreground = New SolidColorBrush(Color.FromRgb(190, 190, 190))
        End If
    End Sub

    Private Sub CBoxCropOutput_Checked(sender As Object, e As RoutedEventArgs) Handles CBoxCropOutput.Checked
        LblStart.Opacity = 1
        LblFinish.Opacity = 1
        TxtStart.Opacity = 1
        TxtFinish.Opacity = 1
        TxtStart.Focusable = True
        TxtFinish.Focusable = True
    End Sub

    Private Sub CBoxCropOutput_Unchecked(sender As Object, e As RoutedEventArgs) Handles CBoxCropOutput.Unchecked
        LblStart.Opacity = 0.2
        LblFinish.Opacity = 0.2
        TxtStart.Opacity = 0.2
        TxtFinish.Opacity = 0.2
        TxtStart.Focusable = False
        TxtFinish.Focusable = False
    End Sub

    Private Sub MainWindow_Initialized(sender As Object, e As EventArgs) Handles Me.Initialized
        TxtOutPath.Text = Environment.GetEnvironmentVariable("userprofile") + "\YouTube Conversions"
    End Sub

    Private Sub BtnLog_Click(sender As Object, e As RoutedEventArgs) Handles BtnLog.Click
        Process.Start("Notepad.exe", ".\Resources\YTC-Log.txt")
    End Sub

    Private Sub Window_Initialized(sender As Object, e As EventArgs)

        MyProcess.StartInfo.FileName = "cmd.exe"
        MyProcess.StartInfo.CreateNoWindow = True
        MyProcess.StartInfo.UseShellExecute = False
        MyProcess.EnableRaisingEvents = True

        MyProcess.StartInfo.EnvironmentVariables.Add("url", String.Empty)
        MyProcess.StartInfo.EnvironmentVariables.Add("name", String.Empty)
        MyProcess.StartInfo.EnvironmentVariables.Add("outpath", String.Empty)
        MyProcess.StartInfo.EnvironmentVariables.Add("cropped", String.Empty)
        MyProcess.StartInfo.EnvironmentVariables.Add("starttime", String.Empty)
        MyProcess.StartInfo.EnvironmentVariables.Add("finishtime", String.Empty)
        MyProcess.StartInfo.EnvironmentVariables.Item("Path") += ";" + My.Application.Info.DirectoryPath + "\Resources\FFmpeg;" + My.Application.Info.DirectoryPath + "\Resources\YouTube-DL"
        'MessageBox.Show("Env Path: " + MyProcess.StartInfo.EnvironmentVariables.Item("Path"))

    End Sub
End Class