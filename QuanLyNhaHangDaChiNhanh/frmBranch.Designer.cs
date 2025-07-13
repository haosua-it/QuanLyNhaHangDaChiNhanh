namespace QuanLyNhaHangDaChiNhanh
{
    partial class frmBranch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBranch));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbTrangThai = new System.Windows.Forms.ComboBox();
            this.txtSoDienThoai = new System.Windows.Forms.TextBox();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.txtTenChiNhanh = new System.Windows.Forms.TextBox();
            this.txtMaChiNhanh = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnRedo = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblPage = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.luoichinhanh = new System.Windows.Forms.DataGridView();
            this.cbMaChucVuQL = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luoichinhanh)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbMaChucVuQL);
            this.panel1.Controls.Add(this.cbTrangThai);
            this.panel1.Controls.Add(this.txtSoDienThoai);
            this.panel1.Controls.Add(this.txtDiaChi);
            this.panel1.Controls.Add(this.txtTenChiNhanh);
            this.panel1.Controls.Add(this.txtMaChiNhanh);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1321, 179);
            this.panel1.TabIndex = 0;
            // 
            // cbTrangThai
            // 
            this.cbTrangThai.FormattingEnabled = true;
            this.cbTrangThai.Location = new System.Drawing.Point(937, 121);
            this.cbTrangThai.Name = "cbTrangThai";
            this.cbTrangThai.Size = new System.Drawing.Size(254, 30);
            this.cbTrangThai.TabIndex = 22;
            // 
            // txtSoDienThoai
            // 
            this.txtSoDienThoai.Location = new System.Drawing.Point(136, 124);
            this.txtSoDienThoai.Name = "txtSoDienThoai";
            this.txtSoDienThoai.Size = new System.Drawing.Size(254, 29);
            this.txtSoDienThoai.TabIndex = 20;
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(937, 63);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(254, 29);
            this.txtDiaChi.TabIndex = 19;
            // 
            // txtTenChiNhanh
            // 
            this.txtTenChiNhanh.Location = new System.Drawing.Point(557, 63);
            this.txtTenChiNhanh.Name = "txtTenChiNhanh";
            this.txtTenChiNhanh.Size = new System.Drawing.Size(254, 29);
            this.txtTenChiNhanh.TabIndex = 18;
            // 
            // txtMaChiNhanh
            // 
            this.txtMaChiNhanh.Location = new System.Drawing.Point(136, 63);
            this.txtMaChiNhanh.Name = "txtMaChiNhanh";
            this.txtMaChiNhanh.Size = new System.Drawing.Size(254, 29);
            this.txtMaChiNhanh.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(834, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 22);
            this.label7.TabIndex = 16;
            this.label7.Text = "Địa Chỉ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(425, 124);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 22);
            this.label1.TabIndex = 15;
            this.label1.Text = "Mã Chức Vụ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(425, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 22);
            this.label4.TabIndex = 14;
            this.label4.Text = "Tên Chi Nhánh";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(832, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 22);
            this.label2.TabIndex = 13;
            this.label2.Text = "Trạng Thái";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 22);
            this.label3.TabIndex = 12;
            this.label3.Text = "Số Điện Thoại";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(126, 22);
            this.label6.TabIndex = 11;
            this.label6.Text = "Mã Chi Nhánh";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(406, 44);
            this.label5.TabIndex = 10;
            this.label5.Text = "QUẢN LÝ CHI NHÁNH";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnBack);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnUndo);
            this.panel2.Controls.Add(this.btnRedo);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnEdit);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.lblPage);
            this.panel2.Controls.Add(this.btnNextPage);
            this.panel2.Controls.Add(this.btnPrevPage);
            this.panel2.Controls.Add(this.luoichinhanh);
            this.panel2.Location = new System.Drawing.Point(12, 197);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1318, 556);
            this.panel2.TabIndex = 1;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.White;
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBack.Location = new System.Drawing.Point(743, 477);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 70);
            this.btnBack.TabIndex = 115;
            this.btnBack.Text = "Back";
            this.btnBack.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSearch.Location = new System.Drawing.Point(662, 478);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 70);
            this.btnSearch.TabIndex = 114;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnCancel.Location = new System.Drawing.Point(581, 478);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 70);
            this.btnCancel.TabIndex = 113;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.BackColor = System.Drawing.Color.White;
            this.btnUndo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUndo.Image = ((System.Drawing.Image)(resources.GetObject("btnUndo.Image")));
            this.btnUndo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnUndo.Location = new System.Drawing.Point(499, 478);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(75, 70);
            this.btnUndo.TabIndex = 112;
            this.btnUndo.Text = "Undo";
            this.btnUndo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnUndo.UseVisualStyleBackColor = false;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.BackColor = System.Drawing.Color.White;
            this.btnRedo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRedo.Image = ((System.Drawing.Image)(resources.GetObject("btnRedo.Image")));
            this.btnRedo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRedo.Location = new System.Drawing.Point(418, 478);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Size = new System.Drawing.Size(75, 70);
            this.btnRedo.TabIndex = 109;
            this.btnRedo.Text = "Redo";
            this.btnRedo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRedo.UseVisualStyleBackColor = false;
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrint.Location = new System.Drawing.Point(337, 477);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 70);
            this.btnPrint.TabIndex = 108;
            this.btnPrint.Text = "Print";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDelete.Location = new System.Drawing.Point(256, 477);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 70);
            this.btnDelete.TabIndex = 107;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(175, 478);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 70);
            this.btnSave.TabIndex = 106;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.White;
            this.btnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEdit.Location = new System.Drawing.Point(94, 478);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 70);
            this.btnEdit.TabIndex = 105;
            this.btnEdit.Text = "Edit";
            this.btnEdit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.Location = new System.Drawing.Point(13, 478);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 70);
            this.btnAdd.TabIndex = 104;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Location = new System.Drawing.Point(577, 403);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(74, 22);
            this.lblPage.TabIndex = 103;
            this.lblPage.Text = "Trang 1";
            // 
            // btnNextPage
            // 
            this.btnNextPage.BackColor = System.Drawing.Color.White;
            this.btnNextPage.Image = ((System.Drawing.Image)(resources.GetObject("btnNextPage.Image")));
            this.btnNextPage.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnNextPage.Location = new System.Drawing.Point(661, 381);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(75, 56);
            this.btnNextPage.TabIndex = 102;
            this.btnNextPage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNextPage.UseVisualStyleBackColor = false;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.BackColor = System.Drawing.Color.White;
            this.btnPrevPage.Image = ((System.Drawing.Image)(resources.GetObject("btnPrevPage.Image")));
            this.btnPrevPage.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrevPage.Location = new System.Drawing.Point(481, 381);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(75, 55);
            this.btnPrevPage.TabIndex = 100;
            this.btnPrevPage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrevPage.UseVisualStyleBackColor = false;
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // luoichinhanh
            // 
            this.luoichinhanh.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.luoichinhanh.Location = new System.Drawing.Point(3, 3);
            this.luoichinhanh.Name = "luoichinhanh";
            this.luoichinhanh.RowTemplate.Height = 24;
            this.luoichinhanh.Size = new System.Drawing.Size(1307, 232);
            this.luoichinhanh.TabIndex = 1;
            this.luoichinhanh.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.luoichinhanh_CellContentClick);
            // 
            // cbMaChucVuQL
            // 
            this.cbMaChucVuQL.FormattingEnabled = true;
            this.cbMaChucVuQL.Location = new System.Drawing.Point(557, 125);
            this.cbMaChucVuQL.Name = "cbMaChucVuQL";
            this.cbMaChucVuQL.Size = new System.Drawing.Size(254, 30);
            this.cbMaChucVuQL.TabIndex = 23;
            // 
            // frmBranch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1342, 756);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Arial", 11.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmBranch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBrand";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmBrand_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.luoichinhanh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSoDienThoai;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.TextBox txtTenChiNhanh;
        private System.Windows.Forms.TextBox txtMaChiNhanh;
        private System.Windows.Forms.ComboBox cbTrangThai;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView luoichinhanh;
        private System.Windows.Forms.Button btnPrevPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnRedo;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.ComboBox cbMaChucVuQL;
    }
}