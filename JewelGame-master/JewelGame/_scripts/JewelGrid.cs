using JewelGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JewelGame._Scripts
{
    public partial class JewelGrid: TableLayoutPanel
    {
        //-----------------------------------------------------------------------------
        public int _GridCount => this._gridCount;
        public event Action _OnStartTurn;
        public event Action<int[]> _OnEndTurn;
        public event Action<int[]> _OnCollectJewels;
        //-----------------------------------------------------------------------------
        //-
        //Dữ liệu bảng jewel
        private int _gridCount;//kích thước bàn cờ
        private JewelTile[,] _grid;//lưu các jewel ( vị trí, loại)
        //-
        //Dữ liệu tạm thời bảng jewel
        private JewelTile _firstJewel = null;//ô Jewel được chọn số 1
        private bool _canInteract = true;//trạng thái có thể swap các ô Jewel
        private bool _canClickJewel = true;
        //-----------------------------------------------------------------------------

        public JewelGrid( int GridCount)
        {
            _setTablePanel(GridCount);
            _setGrid();
            _resolveJewelGrid(). //Xử lý bảng
                ContinueWith(task =>{});
        }
        public JewelGrid(int GridCount, DataTable Jewels)
        {
            _setTablePanel(GridCount);
            _setGrid();
            try
            {
                var rows = Jewels.Rows;
                for ( int i = 0; i < rows.Count; i++)
                {
                    int toaDoX = Convert.ToInt32(rows[i]["toaDoX"]);
                    int toaDoY = Convert.ToInt32(rows[i]["toaDoY"]);
                    int loaiJewel = Convert.ToInt32(rows[i]["loaiJewel"]);

                    _grid[toaDoX, toaDoY]._SetType(loaiJewel);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc dữ liệu từ DataTable: " + ex.Message);
            }
            _resolveJewelGrid(). //Xử lý bảng
                ContinueWith(task => { });
            while (_updateJewelGrid() != 0)
            {

            }
        }
        //-----------------------------------------------------------------------------
        public DataTable _GetDataTable_Jewels()
        {
            var result = new DataTable();
            result.Columns.Add("toaDoX", typeof(int));
            result.Columns.Add("toaDoY", typeof(int));
            result.Columns.Add("loaiJewel", typeof(int));
            foreach (var item in _grid)
            {
                var newRow = result.NewRow();
                newRow["toaDoX"] = item.X;
                newRow["toaDoY"] = item.Y;
                newRow["loaiJewel"] = item.Type;
                result.Rows.Add(newRow);
            }
            return result;
        }
        public bool _Get_IsCanClickJewel() => this._canClickJewel;
        public void _Set_CanClickJewel( bool CanClickJewel) => this._canClickJewel = CanClickJewel;
        //-----------------------------------------------------------------------------
        private void _setAutoSize()
        {
            this.ParentChanged += (sender, e) =>
            {
                //Căn chỉnh size hình vuông khi form thay đổi kích cỡ
                if (this.Parent != null)
                {
                    Size _gridSize_ = this.Parent.Height > this.Parent.Width
                        ? new Size(this.Parent.Width, this.Parent.Width)
                        : new Size(this.Parent.Height, this.Parent.Height);
                    if (_gridSize_.Height % _gridCount != 0)
                    {
                        int excessLength = _gridSize_.Height % _gridCount;
                        _gridSize_ = new Size(_gridSize_.Height - excessLength, _gridSize_.Height - excessLength);
                    }
                    this.Size = _gridSize_;

                    this.Location = new Point(
                        (this.Parent.Width - this.Width) / 2,
                        (this.Parent.Size.Height - this.Height) / 2
                        );

                    this.Parent.SizeChanged += (sender_, e_) =>
                    {
                        if( this.Parent == null) return;

                        Size _gridSize = this.Parent.Height > this.Parent.Width
                            ? new Size(this.Parent.Width, this.Parent.Width)
                            : new Size(this.Parent.Height, this.Parent.Height);
                        if (_gridSize.Height % _gridCount != 0)
                        {
                            int excessLength = _gridSize.Height % _gridCount;
                            _gridSize = new Size(_gridSize.Height - excessLength, _gridSize.Height - excessLength);
                        }
                        this.Size = _gridSize;

                        this.Location = new Point(
                            (this.Parent.Width - this.Width) / 2,
                            (this.Parent.Size.Height - this.Height) / 2
                            );
                    };
                }
            };
        }
        private void _setTablePanel(int GridCount)
        {
            _gridCount = GridCount;
            _grid = new JewelTile[_gridCount, _gridCount];

            this.RowCount = _gridCount;
            this.ColumnCount = _gridCount;
            for (int i = 0; i < _gridCount; i++)
            {
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                this.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            }
            _setAutoSize();
        }
        private void _setGrid()
        {
            for (int rowX = 0; rowX < _gridCount; rowX++)
            {
                for (int columnY = 0; columnY < _gridCount; columnY++)
                {
                    JewelTile tile = new JewelTile
                    {
                        Point = new Point(rowX, columnY),
                        Type = JewelTile._GetRandomType(),
                    };
                    tile._Render();
                    tile.Click += _clickJewel;

                    _grid[tile.X, tile.Y] = tile;
                    this.Controls.Add(tile, tile.X, tile.Y);
                }
            }
            _addContextMenuForJewel();
        }
        private void _addContextMenuForJewel()
        {
            ContextMenuStrip contextMenuStrip_xemThongTinJewel = new ContextMenuStrip();
            ToolStripMenuItem xemThôngTinToolStripMenuItem = new ToolStripMenuItem();

            contextMenuStrip_xemThongTinJewel.ImageScalingSize = new System.Drawing.Size(20, 20);
            contextMenuStrip_xemThongTinJewel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    xemThôngTinToolStripMenuItem});
            contextMenuStrip_xemThongTinJewel.Size = new System.Drawing.Size(211, 56);

            xemThôngTinToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            xemThôngTinToolStripMenuItem.Text = "Xem thông tin";
            xemThôngTinToolStripMenuItem.Click += _toolMenuStripClick_xemThongTin;

            foreach (var item in _grid)
            {
                item.ContextMenuStrip = contextMenuStrip_xemThongTinJewel;
            }
        }
        //_____________________________________________________________________________
        private void _clickJewel(object sender, EventArgs e)
        {
            JewelTile clickTile = sender as JewelTile;
            if (clickTile == null | !_canInteract | GameManager.canClick == false) return;
            //Chọn ô thứ nhất
            if (_firstJewel == null)
            {
                _firstJewel = clickTile;
                _firstJewel._SelectTile();
                if (_firstJewel.X + 1 != _gridCount) _grid[_firstJewel.X + 1, _firstJewel.Y]?._AdjacentTile();
                if (_firstJewel.X - 1 != -1) _grid[_firstJewel.X - 1, _firstJewel.Y]?._AdjacentTile();
                if (_firstJewel.Y + 1 != _gridCount) _grid[_firstJewel.X, _firstJewel.Y + 1]?._AdjacentTile();
                if (_firstJewel.Y - 1 != -1) _grid[_firstJewel.X, _firstJewel.Y - 1]?._AdjacentTile();
            }
            //Chọn ô thứ 2
            else
            {
                JewelTile secondTile = clickTile;
                if (_firstJewel._IsAdjacent(secondTile))//Kiểm tra liền kề
                {
                    _firstJewel._SwapType(secondTile);//Đổi vị trí

                    _OnStartTurn?.Invoke();//Bắt đầu lượt
                    _canInteract = false;// không thế tương tác với bảng
                    _resolveJewelGrid(). //Xử lý bảng
                        ContinueWith(task =>
                        {
                            //Kết thúc lượt
                            _canInteract = true; // Có thể tương tác lại với bảng
                            _OnEndTurn?.Invoke(task.Result);//Tổng kết Jewel thu thập được
                        });
                }
                _firstJewel._DeselectTile();
                if (_firstJewel.X + 1 != _gridCount) _grid[_firstJewel.X + 1, _firstJewel.Y]?._NonAdjacentTile();
                if (_firstJewel.X - 1 != -1) _grid[_firstJewel.X - 1, _firstJewel.Y]?._NonAdjacentTile();
                if (_firstJewel.Y + 1 != _gridCount) _grid[_firstJewel.X, _firstJewel.Y + 1]?._NonAdjacentTile();
                if (_firstJewel.Y - 1 != -1) _grid[_firstJewel.X, _firstJewel.Y - 1]?._NonAdjacentTile();
                this._firstJewel = null;
            }
        }
        private void _toolMenuStripClick_xemThongTin(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
            ContextMenuStrip contextMenuStrip = toolStripMenuItem?.Owner as ContextMenuStrip;
            JewelTile sourceControl = contextMenuStrip?.SourceControl as JewelTile;
            if (sourceControl == null | !_canInteract) return;

            //Mở 1 dialog để xem thông tin
            FormXemThongTinJewel formXemThongTinJewel = new FormXemThongTinJewel();
            formXemThongTinJewel._SetJewelTile(sourceControl);
            formXemThongTinJewel.ShowDialog();
        }
        //-----------------------------------------------------------------------------
        private async Task<int[]> _resolveJewelGrid()//Xử lý bảng
        {
            //Bắt đầu lượt
            _canInteract = false;// không thế tương tác với bảng

            int[] result = new int[ JewelTile._NumberOftype];
            List<JewelTile> listPoints = _findMatches();
            while (listPoints.Count > 0)
            {
                int[] numberOfJewelIsCollectes = new int[JewelTile._NumberOftype];
                await Task.Delay(100);
                foreach (var item in listPoints)
                {
                    result[item.Type] += 1;
                    numberOfJewelIsCollectes[item.Type] += 1;
                    item._SetEmpty();
                }
                _OnCollectJewels?.Invoke(numberOfJewelIsCollectes);

                do
                {
                    await Task.Delay(100);
                }
                while (_updateJewelGrid() != 0);

                listPoints = _findMatches();
            }

            //Kết thúc lượt
            _canInteract = true;
            return result;
        }

        private List<JewelTile> _findMatches()
        {
            HashSet<JewelTile> points = new HashSet<JewelTile>();
            for (int rowX = 0; rowX < _gridCount; rowX++)
            {
                for (int columnY = 0; columnY < _gridCount; columnY++)
                {
                    int pointValue = _grid[rowX, columnY].Type;

                    //Kiểm tra hàng ngang
                    if (rowX < _gridCount - 2 && _grid[rowX + 1, columnY]._IsType(pointValue) && _grid[rowX + 2, columnY]._IsType(pointValue))
                    {
                        points.Add(_grid[rowX, columnY]);
                        points.Add(_grid[rowX + 1, columnY]);
                        points.Add(_grid[rowX + 2, columnY]);
                    }

                    //Kiểm tra hàng dọc
                    if (columnY < _gridCount - 2 && _grid[rowX, columnY + 1]._IsType(pointValue) && _grid[rowX, columnY + 2]._IsType(pointValue))
                    {
                        points.Add(_grid[rowX, columnY]);
                        points.Add(_grid[rowX, columnY + 1]);
                        points.Add(_grid[rowX, columnY + 2]);
                    }
                }
            }
            return points.ToList<JewelTile>();
        }

        private int _updateJewelGrid()//Cập nhật lại bảng ô để lấp các vị trí trống, nhưng chỉ lấp xuống 1 ô
        {
            int numberOfBlackTiles = 0;
            for (int rowX = _gridCount - 1; rowX >= 0; rowX--)
            {
                for (int columnY = _gridCount - 1; columnY >= 0; columnY--)
                {
                    if (_grid[rowX, columnY]._IsEmpty())
                    {
                        for (int pY_1 = columnY; pY_1 > 0; pY_1--)
                        {
                            _grid[rowX, pY_1]._SetType(_grid[rowX, pY_1 - 1].Type);
                        }
                        _grid[rowX, 0]._SetType(JewelTile._GetRandomType());
                        numberOfBlackTiles++;
                    }
                }
            }
            return numberOfBlackTiles;
        }
        //-----------------------------------------------------------------------------
    }
}