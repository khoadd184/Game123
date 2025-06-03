using JewelGame._Scripts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace JewelGame
{
    public partial class Form_cheDo1Nguoi : Form
    {
        TimeSpan timeSpan;
        DateTime startTime;
        DataRow thongTinTranDau;
        JewelGrid jewelGrid;
        List<Label> _listLabel_jewelTileView;
        public Form_cheDo1Nguoi()
        {
            InitializeComponent();
            _listLabel_jewelTileView = new List<Label>();
            _listLabel_jewelTileView.Add(label_jewelTileView0);
            _listLabel_jewelTileView.Add(label_jewelTileView1);
            _listLabel_jewelTileView.Add(label_jewelTileView2);
            _listLabel_jewelTileView.Add(label_jewelTileView3);
            _listLabel_jewelTileView.Add(label_jewelTileView4);
            for (int i = 0; i < tableLayoutPanel_jewelTile.RowCount; i++)
            {
                JewelTile tile = new JewelTile();
                tile._SetType(i);
                tableLayoutPanel_jewelTile.Controls.Add( tile, 0, i);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void _SetData( DataRow data)
        {
            thongTinTranDau = data;
            if (Convert.ToInt32(thongTinTranDau["maTranDau"]) != -1)
            {
                jewelGrid = new JewelGrid(Convert.ToInt32(thongTinTranDau["kichCo"]), DatabaseGame.GetDataTable_Jewels(Convert.ToInt32(thongTinTranDau["maTranDau"])));
                panel_JewelGrid.Controls.Add(jewelGrid);
            }
            else
            {
                jewelGrid = new JewelGrid(Convert.ToInt32(thongTinTranDau["kichCo"]));
                panel_JewelGrid.Controls.Add(jewelGrid);
            }
            jewelGrid._OnCollectJewels += (jewels) =>
            {
                for (int i = 0; i < jewels.Length; i++)
                {
                    _listLabel_jewelTileView[i].Text = (Convert.ToInt32(_listLabel_jewelTileView[i].Text) + jewels[i]).ToString();
                }
            };
            jewelGrid._OnStartTurn += () =>
            {
                this.Invoke(new Action(() =>
                {
                    for (int i = 0; i < _listLabel_jewelTileView.Count; i++)
                    {
                        _listLabel_jewelTileView[i].Text = "0";
                    }
                }));
            };
            jewelGrid._OnEndTurn += (jewels) =>
            {
                this.Invoke(new Action(() =>
                {
                    int diemSo = (int)thongTinTranDau["diemSo"];
                    for (int i = 0; i < jewels.Length; i++)
                    {
                        diemSo += jewels[i];
                    }
                    thongTinTranDau["diemSo"] = diemSo;
                    label_diem.Text = thongTinTranDau["diemSo"].ToString();
                }));
            };
            panel_JewelGrid.Controls.Add(jewelGrid);
            label_tenNguoiChoi.Text = thongTinTranDau["tenNguoiChoi"].ToString().TrimEnd();
            label_diem.Text = thongTinTranDau["diemSo"].ToString();
            timeSpan = (TimeSpan)thongTinTranDau["thoiGian"];
            startTime = DateTime.Now;
        }
        private void Form_cheDo1Nguoi_FormClosed(object sender, FormClosedEventArgs e)
        {
            thongTinTranDau["thoiGian"] = timeSpan + (TimeSpan)(DateTime.Now - startTime);
            if (Convert.ToInt32(thongTinTranDau["maTranDau"]) != -1)
            {
                DatabaseGame.UpdateData_tranDau1Nguoi(thongTinTranDau, jewelGrid._GetDataTable_Jewels());
            }
            else
            {
                DatabaseGame.InsertData_tranDau1Nguoi(thongTinTranDau, jewelGrid._GetDataTable_Jewels());
            }
        }
    }
}
