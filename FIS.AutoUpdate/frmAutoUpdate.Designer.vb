<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAutoUpdate
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblProccess = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnLogin = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.prbUpdate = New System.Windows.Forms.ProgressBar()
        Me.tmr = New System.Windows.Forms.Timer(Me.components)
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblProccess
        '
        Me.lblProccess.AutoSize = True
        Me.lblProccess.BackColor = System.Drawing.Color.Transparent
        Me.lblProccess.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProccess.Location = New System.Drawing.Point(16, 326)
        Me.lblProccess.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblProccess.Name = "lblProccess"
        Me.lblProccess.Size = New System.Drawing.Size(80, 17)
        Me.lblProccess.TabIndex = 12
        Me.lblProccess.Text = "lblProccess"
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(517, 370)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(128, 32)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Thoát"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnLogin
        '
        Me.btnLogin.Location = New System.Drawing.Point(279, 369)
        Me.btnLogin.Margin = New System.Windows.Forms.Padding(4)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(128, 32)
        Me.btnLogin.TabIndex = 10
        Me.btnLogin.Text = "Đăng nhập"
        Me.btnLogin.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(16, 369)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(128, 32)
        Me.btnUpdate.TabIndex = 9
        Me.btnUpdate.Text = "Cập nhật"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'prbUpdate
        '
        Me.prbUpdate.Location = New System.Drawing.Point(16, 345)
        Me.prbUpdate.Margin = New System.Windows.Forms.Padding(4)
        Me.prbUpdate.Name = "prbUpdate"
        Me.prbUpdate.Size = New System.Drawing.Size(629, 18)
        Me.prbUpdate.TabIndex = 8
        '
        'tmr
        '
        Me.tmr.Interval = 1000
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.BackColor = System.Drawing.Color.Transparent
        Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersion.Location = New System.Drawing.Point(16, 309)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(70, 17)
        Me.lblVersion.TabIndex = 13
        Me.lblVersion.Text = "lblVersion"
        '
        'frmAutoUpdate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.FIS.AutoUpdate.My.Resources.Resources.Background_Autoupdate1
        Me.ClientSize = New System.Drawing.Size(665, 409)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblProccess)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.prbUpdate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAutoUpdate"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COMS AutoUpdate "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblProccess As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnLogin As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents prbUpdate As System.Windows.Forms.ProgressBar
    Friend WithEvents tmr As System.Windows.Forms.Timer
    Friend WithEvents lblVersion As System.Windows.Forms.Label

End Class
