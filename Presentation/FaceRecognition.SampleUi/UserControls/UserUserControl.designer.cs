
namespace FaceRecognition.SampleUi.UserControls
{
    partial class UserUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.UserImagePictureBox = new System.Windows.Forms.PictureBox();
            this.UserImagesDataGridView = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddImageButton = new System.Windows.Forms.Button();
            this.StartCaptureButton = new System.Windows.Forms.Button();
            this.StopCaptureButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UserImagePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserImagesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // NameTextBox
            // 
            this.NameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NameTextBox.Location = new System.Drawing.Point(372, 37);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(356, 22);
            this.NameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(372, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // UserImagePictureBox
            // 
            this.UserImagePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UserImagePictureBox.Location = new System.Drawing.Point(16, 14);
            this.UserImagePictureBox.Name = "UserImagePictureBox";
            this.UserImagePictureBox.Size = new System.Drawing.Size(350, 350);
            this.UserImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.UserImagePictureBox.TabIndex = 4;
            this.UserImagePictureBox.TabStop = false;
            // 
            // UserImagesDataGridView
            // 
            this.UserImagesDataGridView.AllowUserToAddRows = false;
            this.UserImagesDataGridView.BackgroundColor = System.Drawing.SystemColors.Control;
            this.UserImagesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UserImagesDataGridView.Location = new System.Drawing.Point(16, 398);
            this.UserImagesDataGridView.MultiSelect = false;
            this.UserImagesDataGridView.Name = "UserImagesDataGridView";
            this.UserImagesDataGridView.ReadOnly = true;
            this.UserImagesDataGridView.RowHeadersWidth = 51;
            this.UserImagesDataGridView.RowTemplate.Height = 24;
            this.UserImagesDataGridView.Size = new System.Drawing.Size(712, 242);
            this.UserImagesDataGridView.TabIndex = 5;
            this.UserImagesDataGridView.SelectionChanged += new System.EventHandler(this.UserImagesDataGridView_SelectionChanged);
            this.UserImagesDataGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.UserImagesDataGridView_UserDeletingRow);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(643, 667);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(85, 36);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Location = new System.Drawing.Point(552, 667);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(85, 36);
            this.DeleteButton.TabIndex = 7;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddImageButton
            // 
            this.AddImageButton.Enabled = false;
            this.AddImageButton.Location = new System.Drawing.Point(622, 80);
            this.AddImageButton.Name = "AddImageButton";
            this.AddImageButton.Size = new System.Drawing.Size(109, 36);
            this.AddImageButton.TabIndex = 8;
            this.AddImageButton.Text = "Add Image";
            this.AddImageButton.UseVisualStyleBackColor = true;
            this.AddImageButton.Click += new System.EventHandler(this.AddImageButton_Click);
            // 
            // StartCaptureButton
            // 
            this.StartCaptureButton.Location = new System.Drawing.Point(375, 80);
            this.StartCaptureButton.Name = "StartCaptureButton";
            this.StartCaptureButton.Size = new System.Drawing.Size(105, 36);
            this.StartCaptureButton.TabIndex = 9;
            this.StartCaptureButton.Text = "Start Capture";
            this.StartCaptureButton.UseVisualStyleBackColor = true;
            this.StartCaptureButton.Click += new System.EventHandler(this.StartCaptureButton_Click);
            // 
            // StopCaptureButton
            // 
            this.StopCaptureButton.Enabled = false;
            this.StopCaptureButton.Location = new System.Drawing.Point(486, 80);
            this.StopCaptureButton.Name = "StopCaptureButton";
            this.StopCaptureButton.Size = new System.Drawing.Size(105, 36);
            this.StopCaptureButton.TabIndex = 10;
            this.StopCaptureButton.Text = "Stop Capture";
            this.StopCaptureButton.UseVisualStyleBackColor = true;
            this.StopCaptureButton.Click += new System.EventHandler(this.StopCaptureButton_Click);
            // 
            // UserUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StopCaptureButton);
            this.Controls.Add(this.StartCaptureButton);
            this.Controls.Add(this.AddImageButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.UserImagesDataGridView);
            this.Controls.Add(this.UserImagePictureBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameTextBox);
            this.Name = "UserUserControl";
            this.Size = new System.Drawing.Size(748, 725);
            ((System.ComponentModel.ISupportInitialize)(this.UserImagePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserImagesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox UserImagePictureBox;
        private System.Windows.Forms.DataGridView UserImagesDataGridView;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddImageButton;
        private System.Windows.Forms.Button StartCaptureButton;
        private System.Windows.Forms.Button StopCaptureButton;
    }
}
