namespace ADO_CodeGenerator
{
    partial class main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            cmsPeopertiesList = new ContextMenuStrip(components);
            editToolStripMenuItem = new ToolStripMenuItem();
            removeToolStripMenuItem = new ToolStripMenuItem();
            errorProvider1 = new ErrorProvider(components);
            btnGenerateClass = new Button();
            gbMainInfo = new GroupBox();
            label3 = new Label();
            rbDotNetCore = new RadioButton();
            rbDotNetFramework = new RadioButton();
            btnAddPresentationPath = new Button();
            txtPresentationPath = new TextBox();
            cmsProjectPathTextBox = new ContextMenuStrip(components);
            deleteToolStripMenuItem = new ToolStripMenuItem();
            label5 = new Label();
            label1 = new Label();
            txtDataAccessFolder = new TextBox();
            txtBusinessFolder = new TextBox();
            btnAddDataAccessPath = new Button();
            btnAddBusinessPath = new Button();
            label10 = new Label();
            label6 = new Label();
            txtDataAcessPath = new TextBox();
            label8 = new Label();
            txtBusinessPath = new TextBox();
            label7 = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            toolTip1 = new ToolTip(components);
            cmsPeopertiesList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            gbMainInfo.SuspendLayout();
            cmsProjectPathTextBox.SuspendLayout();
            SuspendLayout();
            // 
            // cmsPeopertiesList
            // 
            cmsPeopertiesList.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmsPeopertiesList.ImageScalingSize = new Size(20, 20);
            cmsPeopertiesList.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, removeToolStripMenuItem });
            cmsPeopertiesList.Name = "cmsPeopertiesList";
            cmsPeopertiesList.Size = new Size(130, 60);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(129, 28);
            editToolStripMenuItem.Text = "Edit";
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(129, 28);
            removeToolStripMenuItem.Text = "Delete";
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // btnGenerateClass
            // 
            btnGenerateClass.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGenerateClass.Location = new Point(441, 663);
            btnGenerateClass.Name = "btnGenerateClass";
            btnGenerateClass.Size = new Size(199, 75);
            btnGenerateClass.TabIndex = 16;
            btnGenerateClass.Text = "Generate Class";
            btnGenerateClass.UseVisualStyleBackColor = true;
            btnGenerateClass.Click += btnGenerateClass_Click;
            // 
            // gbMainInfo
            // 
            gbMainInfo.Controls.Add(label3);
            gbMainInfo.Controls.Add(rbDotNetCore);
            gbMainInfo.Controls.Add(rbDotNetFramework);
            gbMainInfo.Controls.Add(btnAddPresentationPath);
            gbMainInfo.Controls.Add(txtPresentationPath);
            gbMainInfo.Controls.Add(label5);
            gbMainInfo.Controls.Add(label1);
            gbMainInfo.Controls.Add(txtDataAccessFolder);
            gbMainInfo.Controls.Add(txtBusinessFolder);
            gbMainInfo.Controls.Add(btnAddDataAccessPath);
            gbMainInfo.Controls.Add(btnAddBusinessPath);
            gbMainInfo.Controls.Add(label10);
            gbMainInfo.Controls.Add(label6);
            gbMainInfo.Controls.Add(txtDataAcessPath);
            gbMainInfo.Controls.Add(label8);
            gbMainInfo.Controls.Add(txtBusinessPath);
            gbMainInfo.Controls.Add(label7);
            gbMainInfo.Location = new Point(47, 66);
            gbMainInfo.Name = "gbMainInfo";
            gbMainInfo.Size = new Size(1067, 411);
            gbMainInfo.TabIndex = 17;
            gbMainInfo.TabStop = false;
            gbMainInfo.Text = "Main Info";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(226, 321);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(90, 24);
            label3.TabIndex = 37;
            label3.Text = "Platform:";
            // 
            // rbDotNetCore
            // 
            rbDotNetCore.AutoSize = true;
            rbDotNetCore.Location = new Point(333, 318);
            rbDotNetCore.Name = "rbDotNetCore";
            rbDotNetCore.Size = new Size(108, 27);
            rbDotNetCore.TabIndex = 36;
            rbDotNetCore.TabStop = true;
            rbDotNetCore.Text = ".Net Core";
            rbDotNetCore.UseVisualStyleBackColor = true;
            // 
            // rbDotNetFramework
            // 
            rbDotNetFramework.AutoSize = true;
            rbDotNetFramework.Location = new Point(333, 351);
            rbDotNetFramework.Name = "rbDotNetFramework";
            rbDotNetFramework.Size = new Size(159, 27);
            rbDotNetFramework.TabIndex = 35;
            rbDotNetFramework.TabStop = true;
            rbDotNetFramework.Text = ".Net Framework";
            rbDotNetFramework.UseVisualStyleBackColor = true;
            // 
            // btnAddPresentationPath
            // 
            btnAddPresentationPath.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddPresentationPath.Location = new Point(513, 218);
            btnAddPresentationPath.Name = "btnAddPresentationPath";
            btnAddPresentationPath.Size = new Size(103, 44);
            btnAddPresentationPath.TabIndex = 34;
            btnAddPresentationPath.Text = "Add Path";
            btnAddPresentationPath.UseVisualStyleBackColor = true;
            btnAddPresentationPath.Click += btnAddPath_Click;
            // 
            // txtPresentationPath
            // 
            txtPresentationPath.ContextMenuStrip = cmsProjectPathTextBox;
            txtPresentationPath.Location = new Point(239, 214);
            txtPresentationPath.Multiline = true;
            txtPresentationPath.Name = "txtPresentationPath";
            txtPresentationPath.ReadOnly = true;
            txtPresentationPath.ScrollBars = ScrollBars.Vertical;
            txtPresentationPath.Size = new Size(268, 48);
            txtPresentationPath.TabIndex = 33;
            txtPresentationPath.Text = "E:\\C #\\KOKO.CORE";
            // 
            // cmsProjectPathTextBox
            // 
            cmsProjectPathTextBox.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            cmsProjectPathTextBox.ImageScalingSize = new Size(20, 20);
            cmsProjectPathTextBox.Items.AddRange(new ToolStripItem[] { deleteToolStripMenuItem });
            cmsProjectPathTextBox.Name = "cmsProjectPathTextBox";
            cmsProjectPathTextBox.Size = new Size(123, 28);
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(122, 24);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(7, 217);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(237, 24);
            label5.TabIndex = 32;
            label5.Text = "Presentation Project Path :";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(45, 241);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(0, 24);
            label1.TabIndex = 31;
            // 
            // txtDataAccessFolder
            // 
            txtDataAccessFolder.Location = new Point(809, 161);
            txtDataAccessFolder.Name = "txtDataAccessFolder";
            txtDataAccessFolder.Size = new Size(226, 29);
            txtDataAccessFolder.TabIndex = 30;
            toolTip1.SetToolTip(txtDataAccessFolder, "Folder\\Subfolder...");
            // 
            // txtBusinessFolder
            // 
            txtBusinessFolder.Location = new Point(809, 95);
            txtBusinessFolder.Name = "txtBusinessFolder";
            txtBusinessFolder.Size = new Size(226, 29);
            txtBusinessFolder.TabIndex = 29;
            toolTip1.SetToolTip(txtBusinessFolder, "Folder\\Subfolder...");
            // 
            // btnAddDataAccessPath
            // 
            btnAddDataAccessPath.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddDataAccessPath.Location = new Point(513, 153);
            btnAddDataAccessPath.Name = "btnAddDataAccessPath";
            btnAddDataAccessPath.Size = new Size(103, 44);
            btnAddDataAccessPath.TabIndex = 26;
            btnAddDataAccessPath.Text = "Add Path";
            btnAddDataAccessPath.UseVisualStyleBackColor = true;
            btnAddDataAccessPath.Click += btnAddPath_Click;
            // 
            // btnAddBusinessPath
            // 
            btnAddBusinessPath.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddBusinessPath.Location = new Point(513, 88);
            btnAddBusinessPath.Name = "btnAddBusinessPath";
            btnAddBusinessPath.Size = new Size(103, 44);
            btnAddBusinessPath.TabIndex = 25;
            btnAddBusinessPath.Text = "Add Path";
            btnAddBusinessPath.UseVisualStyleBackColor = true;
            btnAddBusinessPath.Click += btnAddPath_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(690, 162);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(112, 24);
            label10.TabIndex = 24;
            label10.Text = "Folder Path:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(690, 98);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(112, 24);
            label6.TabIndex = 22;
            label6.Text = "Folder Path:";
            // 
            // txtDataAcessPath
            // 
            txtDataAcessPath.ContextMenuStrip = cmsProjectPathTextBox;
            txtDataAcessPath.Location = new Point(239, 147);
            txtDataAcessPath.Multiline = true;
            txtDataAcessPath.Name = "txtDataAcessPath";
            txtDataAcessPath.ReadOnly = true;
            txtDataAcessPath.ScrollBars = ScrollBars.Vertical;
            txtDataAcessPath.Size = new Size(268, 48);
            txtDataAcessPath.TabIndex = 21;
            txtDataAcessPath.Text = "E:\\C #\\KOKODATA";
            txtDataAcessPath.Validating += txtPath_Validating;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(16, 150);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(231, 24);
            label8.TabIndex = 20;
            label8.Text = "Data Access project Path :";
            // 
            // txtBusinessPath
            // 
            txtBusinessPath.ContextMenuStrip = cmsProjectPathTextBox;
            txtBusinessPath.Location = new Point(239, 88);
            txtBusinessPath.Multiline = true;
            txtBusinessPath.Name = "txtBusinessPath";
            txtBusinessPath.ReadOnly = true;
            txtBusinessPath.ScrollBars = ScrollBars.Vertical;
            txtBusinessPath.Size = new Size(268, 48);
            txtBusinessPath.TabIndex = 19;
            txtBusinessPath.Text = "E:\\C #\\KOKOBUSINESS";
            txtBusinessPath.Validating += txtPath_Validating;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(45, 88);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(202, 24);
            label7.TabIndex = 18;
            label7.Text = "Business project Path :";
            // 
            // main
            // 
            AutoScaleDimensions = new SizeF(10F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1163, 834);
            Controls.Add(gbMainInfo);
            Controls.Add(btnGenerateClass);
            Font = new Font("Microsoft YaHei UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4);
            Name = "main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Code Generator";
            Load += Form1_Load;
            cmsPeopertiesList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            gbMainInfo.ResumeLayout(false);
            gbMainInfo.PerformLayout();
            cmsProjectPathTextBox.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnGenerateClass;
        private System.Windows.Forms.ContextMenuStrip cmsPeopertiesList;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbMainInfo;
        private System.Windows.Forms.TextBox txtDataAcessPath;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBusinessPath;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnAddBusinessPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnAddDataAccessPath;
        private System.Windows.Forms.TextBox txtDataAccessFolder;
        private System.Windows.Forms.TextBox txtBusinessFolder;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip cmsProjectPathTextBox;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddPresentationPath;
        private System.Windows.Forms.TextBox txtPresentationPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbDotNetCore;
        private System.Windows.Forms.RadioButton rbDotNetFramework;
    }
}
