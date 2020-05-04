namespace Autobackuper
{
    partial class WatchedFolderDialog
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelWatched = new System.Windows.Forms.Label();
            this.textBoxWatched = new System.Windows.Forms.TextBox();
            this.buttonWatched = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelSlots = new System.Windows.Forms.Label();
            this.labelInterval = new System.Windows.Forms.Label();
            this.numericUpDownSlots = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.labelBackup = new System.Windows.Forms.Label();
            this.textBoxBackup = new System.Windows.Forms.TextBox();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSlots)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelWatched, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxWatched, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonWatched, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonOK, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelSlots, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelInterval, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownSlots, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelBackup, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBoxBackup, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonBackup, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBoxType, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(982, 315);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelWatched
            // 
            this.labelWatched.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelWatched.AutoSize = true;
            this.labelWatched.Location = new System.Drawing.Point(3, 12);
            this.labelWatched.Name = "labelWatched";
            this.labelWatched.Size = new System.Drawing.Size(205, 25);
            this.labelWatched.TabIndex = 0;
            this.labelWatched.Text = "Watched folder path";
            this.labelWatched.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxWatched
            // 
            this.textBoxWatched.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWatched.Location = new System.Drawing.Point(263, 9);
            this.textBoxWatched.Name = "textBoxWatched";
            this.textBoxWatched.Size = new System.Drawing.Size(616, 31);
            this.textBoxWatched.TabIndex = 3;
            this.textBoxWatched.Tag = "0";
            // 
            // buttonWatched
            // 
            this.buttonWatched.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonWatched.AutoSize = true;
            this.buttonWatched.Location = new System.Drawing.Point(885, 7);
            this.buttonWatched.Name = "buttonWatched";
            this.buttonWatched.Size = new System.Drawing.Size(93, 35);
            this.buttonWatched.TabIndex = 6;
            this.buttonWatched.Text = "Browse";
            this.buttonWatched.UseVisualStyleBackColor = true;
            this.buttonWatched.Click += new System.EventHandler(this.ButtonWatched_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.AutoSize = true;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(785, 277);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 35);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.AutoSize = true;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(885, 277);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(94, 35);
            this.buttonOK.TabIndex = 11;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // labelSlots
            // 
            this.labelSlots.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelSlots.AutoSize = true;
            this.labelSlots.Location = new System.Drawing.Point(3, 212);
            this.labelSlots.Name = "labelSlots";
            this.labelSlots.Size = new System.Drawing.Size(254, 25);
            this.labelSlots.TabIndex = 9;
            this.labelSlots.Text = "Backup slots per element";
            this.labelSlots.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelInterval
            // 
            this.labelInterval.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelInterval.AutoSize = true;
            this.labelInterval.Location = new System.Drawing.Point(3, 162);
            this.labelInterval.Name = "labelInterval";
            this.labelInterval.Size = new System.Drawing.Size(238, 25);
            this.labelInterval.TabIndex = 2;
            this.labelInterval.Text = "Minimal backup interval";
            this.labelInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDownSlots
            // 
            this.numericUpDownSlots.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDownSlots.Location = new System.Drawing.Point(263, 209);
            this.numericUpDownSlots.Name = "numericUpDownSlots";
            this.numericUpDownSlots.Size = new System.Drawing.Size(120, 31);
            this.numericUpDownSlots.TabIndex = 10;
            // 
            // numericUpDownInterval
            // 
            this.numericUpDownInterval.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDownInterval.Location = new System.Drawing.Point(3, 3);
            this.numericUpDownInterval.Name = "numericUpDownInterval";
            this.numericUpDownInterval.Size = new System.Drawing.Size(120, 31);
            this.numericUpDownInterval.TabIndex = 8;
            // 
            // labelBackup
            // 
            this.labelBackup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelBackup.AutoSize = true;
            this.labelBackup.Location = new System.Drawing.Point(3, 112);
            this.labelBackup.Name = "labelBackup";
            this.labelBackup.Size = new System.Drawing.Size(192, 25);
            this.labelBackup.TabIndex = 1;
            this.labelBackup.Text = "Backup folder path";
            this.labelBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxBackup
            // 
            this.textBoxBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBackup.Location = new System.Drawing.Point(263, 109);
            this.textBoxBackup.Name = "textBoxBackup";
            this.textBoxBackup.Size = new System.Drawing.Size(616, 31);
            this.textBoxBackup.TabIndex = 4;
            this.textBoxBackup.Tag = "1";
            // 
            // buttonBackup
            // 
            this.buttonBackup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonBackup.AutoSize = true;
            this.buttonBackup.Location = new System.Drawing.Point(885, 107);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(93, 35);
            this.buttonBackup.TabIndex = 7;
            this.buttonBackup.Text = "Browse";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.ButtonBackup_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 25);
            this.label1.TabIndex = 13;
            this.label1.Text = "Place backups in";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.Items.AddRange(new object[] {
            "watched folder",
            "program folder",
            "specified folder"});
            this.comboBoxType.Location = new System.Drawing.Point(263, 53);
            this.comboBoxType.MaxDropDownItems = 3;
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(220, 33);
            this.comboBoxType.TabIndex = 14;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.numericUpDownInterval);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(262, 152);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(618, 46);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "seconds";
            // 
            // WatchedFolderDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(982, 315);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "WatchedFolderDialog";
            this.Text = "Watched folder setup";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSlots)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.Label labelWatched;
        private System.Windows.Forms.Label labelBackup;
        private System.Windows.Forms.Label labelInterval;
        private System.Windows.Forms.TextBox textBoxWatched;
        private System.Windows.Forms.TextBox textBoxBackup;
        private System.Windows.Forms.Button buttonWatched;
        private System.Windows.Forms.NumericUpDown numericUpDownInterval;
        private System.Windows.Forms.Label labelSlots;
        private System.Windows.Forms.NumericUpDown numericUpDownSlots;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label2;
    }
}