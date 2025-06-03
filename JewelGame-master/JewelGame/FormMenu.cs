using JewelGame._Scripts;
using System;
using System.Data;
using System.Windows.Forms;

namespace JewelGame
{
    public partial class FormMenu : Form
    {
        int maTranDauDuocChon = -1;
        bool cheDoChoiDuocChon => rd1nguoi.Checked;
        int kichCoDuocChon;

        DataRow tranDau1NguoiDuocChon;
        DataRow tranDau2NguoiDuocChon;

        public FormMenu()
        {
            InitializeComponent();
            cbKichCo.SelectedIndex = 0;
            updateDataSoure();
        }

        private void rd1nguoi_CheckedChanged(object sender, EventArgs e)
        {
            panel_form1Nguoi.Show();
            panel_form2Nguoi.Hide();
        }

        private void rd2nguoi_CheckedChanged(object sender, EventArgs e)
        {
            panel_form1Nguoi.Hide();
            panel_form2Nguoi.Show();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (dgv1.Visible) dgv1.Hide();
            else dgv1.Show();
        }

        private void cbKichCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbKichCo.Text)
            {
                case ("8x8"): kichCoDuocChon = 8; break;
                case ("10x10"): kichCoDuocChon = 10; break;
                case ("12x12"): kichCoDuocChon = 12; break;
            }
        }

        private void dgv1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellCollection clickCell = dgv1.Rows[dgv1.CurrentCell.RowIndex].Cells;
            maTranDauDuocChon = Convert.ToInt32(clickCell["maTranDau"].Value);
            btnTiepTuc.Text = "Tiếp tục trận số " + maTranDauDuocChon;
            btnXoa.Text = "Xóa trận đấu số " + maTranDauDuocChon;

            if (clickCell["cheDoChoi"].Value.ToString() == "1 người")
            {
                rd1nguoi.Checked = true;
                tranDau1NguoiDuocChon = DatabaseGame.GetDataRow_TranDau1Nguoi(maTranDauDuocChon);
                txtTenNgChoi.Text = tranDau1NguoiDuocChon["tenNguoiChoi"].ToString().TrimEnd();
            }
            else
            {
                rd2nguoi.Checked = true;
                tranDau2NguoiDuocChon = DatabaseGame.GetDataRow_TranDau2Nguoi(maTranDauDuocChon);
                txtNgChoi1.Text = tranDau2NguoiDuocChon["tenNguoiChoi1"].ToString().TrimEnd();
                txtNgChoi2.Text = tranDau2NguoiDuocChon["tenNguoiChoi2"].ToString().TrimEnd();
            }
        }
        private void updateDataSoure()
        {
            DataTable data = DatabaseGame.GetDataTable_TranDau();
            dgv1.DataSource = data;
            for (int i = 0; i < dgv1.Rows.Count; i++)
            {
                var rowItem = dgv1.Rows[i];
                bool cheDoChoi = rowItem.Cells["cheDoChoi"].Value.ToString().TrimEnd() == "1 Player";
                if(cheDoChoi)
                {
                    rowItem.Cells["cheDoChoi"].Value = "1 người";
                }
                else
                {
                    rowItem.Cells["cheDoChoi"].Value = "2 người";
                }
            }
        }

        private void btnChoiMoi_Click(object sender, EventArgs e)
        {
            if(cheDoChoiDuocChon)
            {
                if (txtTenNgChoi.TextLength == 0)
                {
                    MessageBox.Show("Hãy điền đầy đủ thông tin!");
                    return;
                }

                DataRow newData = DatabaseGame.NewRow_TranDau1Nguoi();
                newData["kichCo"] = kichCoDuocChon;
                newData["tenNguoiChoi"] = txtTenNgChoi.Text;

                Form_cheDo1Nguoi a = new Form_cheDo1Nguoi();
                a._SetData(newData);
                a.ShowDialog();
            }
            else
            {
                if (txtNgChoi1.TextLength == 0 || txtNgChoi2.TextLength == 0)
                {
                    MessageBox.Show("Hãy điền đầy đủ thông tin!");
                    return;
                }
            

                    DataRow newData = DatabaseGame.NewRow_TranDau2Nguoi();
                newData["kichCo"] = kichCoDuocChon;
                newData["tenNguoiChoi1"] = txtNgChoi1.Text;
                newData["tenNguoiChoi2"] = txtNgChoi2.Text;
                Form_cheDo2Nguoi a = new Form_cheDo2Nguoi();
                a._SetData(newData);
                a.ShowDialog();
            }

            updateDataSoure();
        }
        private void btnTiepTuc_Click(object sender, EventArgs e)
        {
            if( maTranDauDuocChon == -1) return;
            if (cheDoChoiDuocChon)
            {
                if (txtTenNgChoi.Text != tranDau1NguoiDuocChon["tenNguoiChoi"].ToString().TrimEnd())
                {
                    DialogResult result = MessageBox.Show(
                                            "Bạn vừa thay đổi tên, bạn có muốn dùng tên mới không?",
                                            "Xác nhận hành động",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        tranDau1NguoiDuocChon["tenNguoiChoi"] = txtTenNgChoi.Text;
                    }
                }

                Form_cheDo1Nguoi a = new Form_cheDo1Nguoi();
                a._SetData(tranDau1NguoiDuocChon);
                a.ShowDialog();
            }
            else
            {
                if (txtNgChoi1.Text != tranDau2NguoiDuocChon["tenNguoiChoi1"].ToString().TrimEnd()
                    | txtNgChoi2.Text != tranDau2NguoiDuocChon["tenNguoiChoi2"].ToString().TrimEnd())
                {
                    DialogResult result = MessageBox.Show(
                                            "Bạn vừa thay đổi tên, bạn có muốn dùng tên mới không?",
                                            "Xác nhận hành động",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        tranDau2NguoiDuocChon["tenNguoiChoi1"] = txtNgChoi1.Text;
                        tranDau2NguoiDuocChon["tenNguoiChoi2"] = txtNgChoi2.Text;
                    }
                }

                Form_cheDo2Nguoi a = new Form_cheDo2Nguoi();
                a._SetData(tranDau2NguoiDuocChon);
                a.ShowDialog();
            }
            updateDataSoure();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (maTranDauDuocChon == -1)
            {
                MessageBox.Show("Bạn chưa chọn trận đấu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn xóa trận đấu số " + maTranDauDuocChon + " không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );
            if (result == DialogResult.Yes)
            {
                DatabaseGame.DeleteData(maTranDauDuocChon);
                updateDataSoure();
            }
        }

        private void lbTenNgChoi_Click(object sender, EventArgs e)
        {

        }

        private void txtNgChoi1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
