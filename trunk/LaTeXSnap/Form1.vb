Public Class Form1

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim myTempPath As String = Environment.GetEnvironmentVariable("TEMP")

        ToolStripStatusLabel1.Text = "Rendering image..."
        ToolStripProgressBar1.Increment(10)

        ' Create a LaTeX file with the desired equation inside
        My.Computer.FileSystem.WriteAllText(myTempPath & "\myeqn.tex", _
            "\documentclass[fleqn]{article}" & vbCrLf & "\usepackage{amssymb,amsmath}" & vbCrLf & "\usepackage[latin1]{inputenc}" & _
            vbCrLf & "\begin{document}" & vbCrLf & "\thispagestyle{empty}" & vbCrLf & "\mathindent0cm" & vbCrLf & "\parindent0cm" & vbCrLf & _
            TextBox1.Text & vbCrLf & "\end{document}", False, System.Text.Encoding.Default)

        ToolStripProgressBar1.Increment(20)

        ' Run LaTeX now
        Dim myProcess As New System.Diagnostics.Process()
        myProcess.StartInfo.FileName = "latex.exe"
        myProcess.StartInfo.Arguments = "-interaction=nonstopmode myeqn.tex"
        myProcess.StartInfo.WorkingDirectory = myTempPath
        myProcess.StartInfo.WindowStyle = _
            System.Diagnostics.ProcessWindowStyle.Hidden

        ToolStripProgressBar1.Increment(30)

        myProcess.Start()
        Do Until myProcess.HasExited
            Application.DoEvents()
            System.Threading.Thread.Sleep(250)
        Loop

        ToolStripProgressBar1.Increment(40)

        ' Run dvipng now
        myProcess.StartInfo.FileName = "dvipng.exe"
        myProcess.StartInfo.Arguments = "-q -T tight -bg Transparent -D " & TextBox2.Text & " -o myeqn.png myeqn.dvi"
        myProcess.StartInfo.WorkingDirectory = myTempPath
        myProcess.StartInfo.WindowStyle = _
            System.Diagnostics.ProcessWindowStyle.Hidden

        myProcess.Start()
        Do Until myProcess.HasExited
            Application.DoEvents()
            System.Threading.Thread.Sleep(250)
        Loop

        ToolStripProgressBar1.Increment(50)

        ' You have to run it twice to ensure all fonts are there
        myProcess.Start()
        Do Until myProcess.HasExited
            Application.DoEvents()
            System.Threading.Thread.Sleep(250)
        Loop

        ToolStripProgressBar1.Increment(60)

        ' Copy the image to the clipboard
        Clipboard.Clear()
        ' Create and initializes a new StringCollection.
        Dim myCol As New System.Collections.Specialized.StringCollection()
        ' Add a range of elements from an array to the end of the StringCollection.
        Dim myArr() As String = {myTempPath & "\myeqn.png"}
        myCol.AddRange(myArr)
        Clipboard.SetFileDropList(myCol)

        ToolStripProgressBar1.Increment(80)

        ' Display the image file
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.ImageLocation = myTempPath & "\myeqn.png"

        ToolStripProgressBar1.Increment(100)
        ToolStripStatusLabel1.Text = "The image is on the clipboard."

    End Sub

    ' NOT WORKING YET! Enable drag and drop support for copying the equation out to another program
    'Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown
    '    PictureBox1.DoDragDrop(PictureBox1.Image, DragDropEffects.Copy Or DragDropEffects.Move)
    'End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        System.Diagnostics.Process.Start("http://code.google.com/p/latexsnap/")
    End Sub

    Private Sub AboutToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem2.Click
        AboutBox1.Show()
    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class
