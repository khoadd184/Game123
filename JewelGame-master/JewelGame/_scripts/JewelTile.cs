using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JewelGame._Scripts
{
    public class Animation_JewelTile
    {
        static private Timer _animationTimer;
        static private int timer;

        static public Image[,] _Image_Jewels;
        static public Image[,] _Image_Outline;

        public JewelTile _JewelTile { get; set; }
        static Animation_JewelTile()
        {
            timer = 0;
            _animationTimer = new Timer();
            _animationTimer.Interval = 100;
            _animationTimer.Tick += (object sender, EventArgs e) =>
            {
                timer += 1;
            };
            _animationTimer.Start();

            string basePath_ImageOutline1 = Path.Combine(Application.StartupPath, "..\\..\\Resources\\animation_Outline1");
            _Image_Outline = new Image[2,8];
            for (int i = 0; i < 8; i++)
            {
                string path = Path.Combine(basePath_ImageOutline1, $"SpriteOutline_1_{i + 1}.png");
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Không tìm thấy file ảnh: {path}");
                _Image_Outline[0, i] = Image.FromFile(path);
            }

            string basePath_ImageOutline2 = Path.Combine(Application.StartupPath, "..\\..\\Resources\\animation_Outline2");
            for (int i = 0; i < 8; i++)
            {
                string path = Path.Combine(basePath_ImageOutline2, $"SpriteOutline_2_{i + 1}.png");
                if (!File.Exists(path))
                    throw new FileNotFoundException($"Không tìm thấy file ảnh: {path}");
                _Image_Outline[1, i] = Image.FromFile(path);
            }

            string basePath_ImageJewel = Path.Combine(Application.StartupPath, "..\\..\\Resources");
            _Image_Jewels = new Image[JewelTile._NumberOftype, 2];
            for (int i = 0; i < JewelTile._NumberOftype; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    string path = Path.Combine(basePath_ImageJewel, $"SpriteJewel_{i + 1}_{ j + 1}.png");
                    if (!File.Exists(path))
                        throw new FileNotFoundException($"Không tìm thấy file ảnh: {path}");
                    _Image_Jewels[i, j] = Image.FromFile(path);
                }
            }
        }
        public Animation_JewelTile( JewelTile JewelTile)
        {
            _JewelTile = JewelTile;
        }
        //-----------------------------------------------------------------------------
        public void _Set_TypeJewel( int type)
        {
            _JewelTile.Image = type == JewelTile._EmptyType ? null : _Image_Jewels[type, 0];
        }
        //-----------------------------------------------------------------------------
        public void _Set_SelectTile()
        {
            _JewelTile.BackgroundImage = _Image_Outline[0, timer % 8];
            _animationTimer.Tick += _animation_SelectTile;
        }
        public void _Unset_SelectTile()
        {
            _animationTimer.Tick -= _animation_SelectTile;
            _JewelTile.Image = _Image_Jewels[_JewelTile.Type, 0];
            _JewelTile.BackgroundImage = null;
        }
        private void _animation_SelectTile(object sender, EventArgs e)
        {
            _JewelTile.BackgroundImage = _Image_Outline[0, timer % 8];
        }
        //-----------------------------------------------------------------------------
        public void _Set_IsMatched()
        {
            _JewelTile.Image = _Image_Jewels[_JewelTile.Type, 1];
        }
        public void _Unset_IsMatched()
        {
            _JewelTile.Image = _Image_Jewels[_JewelTile.Type, 0];
        }
        //-----------------------------------------------------------------------------
        public void _Set_AdjacentTile()
        {
            _JewelTile.BackgroundImage = _Image_Outline[1, timer % 8];
            _animationTimer.Tick += _animation_AdjacentTile;
        }
        public void _Unset_AdjacentTile()
        {
            _animationTimer.Tick -= _animation_AdjacentTile;
            _JewelTile.BackgroundImage = null;
        }
        private void _animation_AdjacentTile(object sender, EventArgs e)
        {
            _JewelTile.BackgroundImage = _Image_Outline[1, timer % 8];
        }
        //-----------------------------------------------------------------------------
    }
    public partial class JewelTile : PictureBox
    {
        static private Random _random = new Random();
        static private string[] _name;
        static private string[] _description;
        //-----------------------------------------------------------------------------
        static public int _NumberOftype = 5;
        static public int _EmptyType = _NumberOftype;
        static public int _GetRandomType() => _random.Next(_NumberOftype);
        //-----------------------------------------------------------------------------
        //-----------------------------------------------------------------------------
        public Point Point;
        public int X => Point.X;
        public int Y => Point.Y;
        public int Type;
        public string _NameJewel => _name[this.Type];
        public string _Description => _description[this.Type];
        //-----------------------------------------------------------------------------
        private Animation_JewelTile _animation;

        static JewelTile()
        {
            _name = new string[]
            {
                "Ngọc Sát Thương",
                "Ngọc Sinh Mệnh",
                "Ngọc Phòng Thủ",
                "Ngọc Hỗn Hợp",
                "Ngọc Khống Chế",
                "Ngọc Vô Năng"
            };
            _description = new string[]
            {
                "Mỗi viên ngọc thu thập được sẽ khiến đối thủ mất 7 điểm máu.",
                "Mỗi viên ngọc thu thập được sẽ hồi 7 điểm HP cho bản thân. Nếu đã đạt tối đa HP, sẽ không hồi điểm HP.",
                "Mỗi viên ngọc thu thập được sẽ cộng thêm 1 điểm giáp cho bản thân. Khi tích lũy đủ 10 điểm giáp trở lên, toàn bộ điểm giáp sẽ bị tiêu hao để chặn một đòn tấn công của đối phương ( hiệu ứng này không thể cộng dồn nhiều lần).",
                "Mỗi viên ngọc thu thập được sẽ khiến đối thủ mất 3 điểm máu và hồi cho bản thân 3 điểm HP, 0.5 điểm giáp và 0.5 điểm năng lượng.",
                "Mỗi viên ngọc thu thập được sẽ cộng thêm 1 điểm năng lượng cho bản thân. Khi đạt 10 điểm năng lượng trở lên, toàn bộ năng lượng sẽ bị tiêu hao để khống chế đối thủ: đối phương sẽ mất lượt hành động kế tiếp ( hiệu ứng này không thể cộng dồn nhiều lần).",
                "Viên ngọc này chưa có hiệu quả gì!!!"
            };
        }
        public JewelTile()
        {
            this.DoubleBuffered = true;
            this.Name = "PictureBox_JewelTile_" + X + "/" + Y;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Dock = DockStyle.Fill;
            this.Margin = new Padding(0);
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            _animation = new Animation_JewelTile(this);
        }
        //-----------------------------------------------------------------------------
        public void _AdjacentTile() => _animation._Set_AdjacentTile();
        public void _NonAdjacentTile() => _animation._Unset_AdjacentTile();
        //-----------------------------------------------------------------------------
        public void _SelectTile() => _animation._Set_SelectTile();
        public void _DeselectTile() => _animation._Unset_SelectTile();
        //-----------------------------------------------------------------------------
        public void _IsMatched()
        {
            _animation._Set_IsMatched();
        }
        public void _IsNotMatched()
        {
            _animation._Unset_IsMatched();
        }
        //-----------------------------------------------------------------------------
        public void _SetEmpty() => this._SetType(_EmptyType);
        public void _SetType(int newType)
        {
            this.Type = newType;
            _animation._Set_TypeJewel(this.Type);
        }
        //-----------------------------------------------------------------------------
        public bool _IsAdjacent(JewelTile jewelTile)
        {
            int dx = Math.Abs(this.X - jewelTile.X);
            int dy = Math.Abs(this.Y - jewelTile.Y);
            return (dx + dy == 1);
        }
        public bool _IsType(int type) => this.Type == type;
        public bool _IsEmpty() => this.Type == _EmptyType;
        //-----------------------------------------------------------------------------
        public void _SwapType(JewelTile jewelTile)
        {
            int oldType = this.Type;
            this._SetType(jewelTile.Type);
            jewelTile._SetType(oldType);
        }
        public void _Render() => _animation._Set_TypeJewel(this.Type);
        //-----------------------------------------------------------------------------
    }
}
