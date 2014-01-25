namespace OxronNotifier
{
    partial class ConfigureForm
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdTest = new System.Windows.Forms.Button();
            this.txtServerKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbConfigs = new System.Windows.Forms.ComboBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdRemove = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(157, 197);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOk
            // 
            this.cmdOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Location = new System.Drawing.Point(238, 197);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(75, 23);
            this.cmdOk.TabIndex = 10;
            this.cmdOk.Text = "Ok";
            this.cmdOk.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdTest);
            this.groupBox1.Controls.Add(this.txtServerKey);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtApiKey);
            this.groupBox1.Controls.Add(this.txtUsername);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(308, 150);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Settings";
            // 
            // cmdTest
            // 
            this.cmdTest.Location = new System.Drawing.Point(213, 117);
            this.cmdTest.Name = "cmdTest";
            this.cmdTest.Size = new System.Drawing.Size(88, 23);
            this.cmdTest.TabIndex = 9;
            this.cmdTest.Text = "Test";
            this.cmdTest.UseVisualStyleBackColor = true;
            this.cmdTest.Click += new System.EventHandler(this.cmdTest_Click);
            // 
            // txtServerKey
            // 
            this.txtServerKey.Location = new System.Drawing.Point(82, 39);
            this.txtServerKey.Name = "txtServerKey";
            this.txtServerKey.Size = new System.Drawing.Size(220, 20);
            this.txtServerKey.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Server Key";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(82, 91);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(220, 20);
            this.txtApiKey.TabIndex = 7;
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(82, 65);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(220, 20);
            this.txtUsername.TabIndex = 6;
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(82, 13);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(220, 20);
            this.txtServer.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Url";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "API Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Username";
            // 
            // cmbConfigs
            // 
            this.cmbConfigs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbConfigs.FormattingEnabled = true;
            this.cmbConfigs.Location = new System.Drawing.Point(12, 12);
            this.cmbConfigs.Name = "cmbConfigs";
            this.cmbConfigs.Size = new System.Drawing.Size(145, 21);
            this.cmbConfigs.TabIndex = 1;
            this.cmbConfigs.SelectedIndexChanged += new System.EventHandler(this.cmbConfigs_SelectedIndexChanged);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAdd.Location = new System.Drawing.Point(163, 10);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdAdd.TabIndex = 2;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdRemove
            // 
            this.cmdRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRemove.Location = new System.Drawing.Point(244, 10);
            this.cmdRemove.Name = "cmdRemove";
            this.cmdRemove.Size = new System.Drawing.Size(75, 23);
            this.cmdRemove.TabIndex = 3;
            this.cmdRemove.Text = "Remove";
            this.cmdRemove.UseVisualStyleBackColor = true;
            this.cmdRemove.Click += new System.EventHandler(this.cmdRemove_Click);
            // 
            // ConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 232);
            this.Controls.Add(this.cmdRemove);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.cmbConfigs);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cmdCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOk;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtApiKey;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbConfigs;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdRemove;
        private System.Windows.Forms.TextBox txtServerKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdTest;
    }
}

