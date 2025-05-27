namespace QuanLyNhaHangDaChiNhanh
{
    partial class frmHeThong
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
            this.lbThoiGian = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnTroLai = new System.Windows.Forms.Button();
            this.btnQuanLyHeThong = new System.Windows.Forms.Button();
            this.btnBaoCaoHeThong = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbThoiGian
            // 
            this.lbThoiGian.AutoSize = true;
            this.lbThoiGian.Location = new System.Drawing.Point(18, 57);
            this.lbThoiGian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbThoiGian.Name = "lbThoiGian";
            this.lbThoiGian.Size = new System.Drawing.Size(282, 18);
            this.lbThoiGian.TabIndex = 14;
            this.lbThoiGian.Text = "thứ ngày tháng năm giờ phút giây ở đây";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(399, 18);
            this.label1.TabIndex = 13;
            this.label1.Text = "Hi, (Tên người đăng nhập hoặc vai trò người đăng nhập)";
            // 
            // btnTroLai
            // 
            this.btnTroLai.Location = new System.Drawing.Point(772, 464);
            this.btnTroLai.Name = "btnTroLai";
            this.btnTroLai.Size = new System.Drawing.Size(175, 54);
            this.btnTroLai.TabIndex = 15;
            this.btnTroLai.Text = "Trở lại";
            this.btnTroLai.UseVisualStyleBackColor = true;
            // 
            // btnQuanLyHeThong
            // 
            this.btnQuanLyHeThong.Location = new System.Drawing.Point(183, 145);
            this.btnQuanLyHeThong.Name = "btnQuanLyHeThong";
            this.btnQuanLyHeThong.Size = new System.Drawing.Size(210, 183);
            this.btnQuanLyHeThong.TabIndex = 16;
            this.btnQuanLyHeThong.Text = "Quản Lý";
            this.btnQuanLyHeThong.UseVisualStyleBackColor = true;
            this.btnQuanLyHeThong.Click += new System.EventHandler(this.btnQuanLyHeThong_Click);
            // 
            // btnBaoCaoHeThong
            // 
            this.btnBaoCaoHeThong.Location = new System.Drawing.Point(442, 145);
            this.btnBaoCaoHeThong.Name = "btnBaoCaoHeThong";
            this.btnBaoCaoHeThong.Size = new System.Drawing.Size(210, 183);
            this.btnBaoCaoHeThong.TabIndex = 17;
            this.btnBaoCaoHeThong.Text = "Báo cáo";
            this.btnBaoCaoHeThong.UseVisualStyleBackColor = true;
            this.btnBaoCaoHeThong.Click += new System.EventHandler(this.btnBaoCaoHeThong_Click);
            // 
            // frmHeThong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 530);
            this.Controls.Add(this.btnBaoCaoHeThong);
            this.Controls.Add(this.btnQuanLyHeThong);
            this.Controls.Add(this.btnTroLai);
            this.Controls.Add(this.lbThoiGian);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmHeThong";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống";
            this.Load += new System.EventHandler(this.frmHeThong_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbThoiGian;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnTroLai;
        private System.Windows.Forms.Button btnQuanLyHeThong;
        private System.Windows.Forms.Button btnBaoCaoHeThong;
    }
}