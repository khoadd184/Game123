namespace JewelGame
{
    partial class FormXemThongTinJewel
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_moTa = new System.Windows.Forms.Label();
            this.label_toaDoY = new System.Windows.Forms.Label();
            this.label_toaDoX = new System.Windows.Forms.Label();
            this.pictureBox_jewel = new System.Windows.Forms.PictureBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_jewel)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tọa độ:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_moTa);
            this.panel2.Controls.Add(this.label_toaDoY);
            this.panel2.Controls.Add(this.label_toaDoX);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(241, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(298, 200);
            this.panel2.TabIndex = 2;
            // 
            // label_moTa
            // 
            this.label_moTa.Location = new System.Drawing.Point(16, 61);
            this.label_moTa.Name = "label_moTa";
            this.label_moTa.Size = new System.Drawing.Size(266, 126);
            this.label_moTa.TabIndex = 4;
            this.label_moTa.Text = "Mô tả: ";
            // 
            // label_toaDoY
            // 
            this.label_toaDoY.AutoSize = true;
            this.label_toaDoY.Location = new System.Drawing.Point(134, 37);
            this.label_toaDoY.Name = "label_toaDoY";
            this.label_toaDoY.Size = new System.Drawing.Size(29, 16);
            this.label_toaDoY.TabIndex = 3;
            this.label_toaDoY.Text = "Y: 0";
            // 
            // label_toaDoX
            // 
            this.label_toaDoX.AutoSize = true;
            this.label_toaDoX.Location = new System.Drawing.Point(39, 37);
            this.label_toaDoX.Name = "label_toaDoX";
            this.label_toaDoX.Size = new System.Drawing.Size(28, 16);
            this.label_toaDoX.TabIndex = 2;
            this.label_toaDoX.Text = "X: 0";
            // 
            // pictureBox_jewel
            // 
            this.pictureBox_jewel.Location = new System.Drawing.Point(12, 30);
            this.pictureBox_jewel.Name = "pictureBox_jewel";
            this.pictureBox_jewel.Size = new System.Drawing.Size(200, 200);
            this.pictureBox_jewel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_jewel.TabIndex = 3;
            this.pictureBox_jewel.TabStop = false;
            // 
            // FormXemThongTinJewel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 245);
            this.Controls.Add(this.pictureBox_jewel);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormXemThongTinJewel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormXemThongTinJewel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.btnClosed_Click);
            this.Load += new System.EventHandler(this.FormXemThongTinJewel_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_jewel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label_moTa;
        private System.Windows.Forms.Label label_toaDoY;
        private System.Windows.Forms.Label label_toaDoX;
        private System.Windows.Forms.PictureBox pictureBox_jewel;
    }
}