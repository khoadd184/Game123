using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JewelGame._Scripts;

namespace JewelGame
{
    public partial class FormXemThongTinJewel : Form
    {
        JewelTile _jewelTile;
        public FormXemThongTinJewel()
        {
            InitializeComponent();
        }
        public void _SetJewelTile( JewelTile JewelTile)
        {
            this._jewelTile = JewelTile;
        }
        private void btnClosed_Click(object sender, FormClosedEventArgs e)
        {
        }

        private void FormXemThongTinJewel_Load(object sender, EventArgs e)
        {
            this.Text = _jewelTile._NameJewel;
            pictureBox_jewel.Image = this._jewelTile.Image;
            label_toaDoX.Text = "X: " + (this._jewelTile.X + 1);
            label_toaDoY.Text = "Y: " + (this._jewelTile.Y + 1);
            label_moTa.Text = "Mô tả: " + this._jewelTile._Description;
        }
    }
}
