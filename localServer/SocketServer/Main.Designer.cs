namespace SocketServer
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.launchServer = new System.Windows.Forms.Button();
            this.IP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.iPText = new System.Windows.Forms.TextBox();
            this.portText = new System.Windows.Forms.TextBox();
            this.content = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // launchServer
            // 
            this.launchServer.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.launchServer.Location = new System.Drawing.Point(584, 12);
            this.launchServer.Name = "launchServer";
            this.launchServer.Size = new System.Drawing.Size(81, 31);
            this.launchServer.TabIndex = 0;
            this.launchServer.Text = "启动服务器";
            this.launchServer.UseVisualStyleBackColor = false;
            this.launchServer.Click += new System.EventHandler(this.launchServer_Click);
            // 
            // IP
            // 
            this.IP.AutoSize = true;
            this.IP.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IP.Location = new System.Drawing.Point(28, 19);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(26, 12);
            this.IP.TabIndex = 1;
            this.IP.Text = "IP:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(275, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port:";
            // 
            // iPText
            // 
            this.iPText.BackColor = System.Drawing.SystemColors.Window;
            this.iPText.Location = new System.Drawing.Point(60, 16);
            this.iPText.Name = "iPText";
            this.iPText.Size = new System.Drawing.Size(128, 21);
            this.iPText.TabIndex = 3;
            // 
            // portText
            // 
            this.portText.BackColor = System.Drawing.SystemColors.Window;
            this.portText.Location = new System.Drawing.Point(321, 18);
            this.portText.Name = "portText";
            this.portText.Size = new System.Drawing.Size(123, 21);
            this.portText.TabIndex = 4;
            this.portText.Text = "8700";
            // 
            // content
            // 
            this.content.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.content.Location = new System.Drawing.Point(30, 62);
            this.content.Multiline = true;
            this.content.Name = "content";
            this.content.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.content.Size = new System.Drawing.Size(529, 359);
            this.content.TabIndex = 5;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 450);
            this.Controls.Add(this.content);
            this.Controls.Add(this.portText);
            this.Controls.Add(this.iPText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IP);
            this.Controls.Add(this.launchServer);
            this.Name = "Main";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button launchServer;
        private System.Windows.Forms.Label IP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox iPText;
        private System.Windows.Forms.TextBox portText;
        private System.Windows.Forms.TextBox content;
    }
}