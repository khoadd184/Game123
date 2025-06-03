using JewelGame._Scripts;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JewelGame
{
    public partial class Form_cheDo2Nguoi : Form
    {
        TimeSpan timeSpan;
        DateTime startTime;
        DataRow thongTinTranDau;
        JewelGrid jewelGrid;
        Player player1 = GameManager._player1;
        Player player2 = GameManager._player2;
        GameManager gameManager;
        private bool gameOff = false;
        private bool loadGame = true;
        int a, b, c;
        int i = 0;
        public Form_cheDo2Nguoi()
        {
            InitializeComponent();

            label7.Location = new System.Drawing.Point(panel1.Location.X, panel1.Location.Y - this.ClientSize.Height / 30);
            label10.Location = new System.Drawing.Point(this.ClientSize.Width / 5, this.ClientSize.Height / 2);
            a = this.ClientSize.Height;
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            if (thongTinTranDau == null) return;


            label7.Location = new System.Drawing.Point(panel1.Location.X, panel1.Location.Y - this.ClientSize.Height / 30);
            label8.Location = new System.Drawing.Point(this.ClientSize.Width / 21 + this.ClientSize.Width / 2, label7.Location.Y);
            if (this.ClientSize.Height <= a + a / 2)
            {
                Lbresize(9);
            }
            else if (this.ClientSize.Height > a + a / 2)
            {
                Lbresize(15);
            }
            else if (this.ClientSize.Height > a * 2)
            {
                Lbresize(26);
            }

        }
        private void Form1_Load(object sender, System.EventArgs e)
        {

            TimeCount();
            gameManager = new GameManager(jewelGrid);
            
            
        }
        void loadData()
        {
            gameManager._isPlayer1Turn = Convert.ToBoolean(thongTinTranDau["luotNguoiChoi"]);

            player1.Name = thongTinTranDau["tenNguoiChoi1"].ToString().TrimEnd();
            player1.HP = Convert.ToInt32(thongTinTranDau["hpNguoiChoi1"]);
            player1.Shield = Convert.ToInt32(thongTinTranDau["giapNguoiChoi1"]);
            player1.controlMana = Convert.ToInt32(thongTinTranDau["nangLuongNguoiChoi1"]);
            player2.Name = thongTinTranDau["tenNguoiChoi2"].ToString().TrimEnd();
            player2.HP = Convert.ToInt32(thongTinTranDau["hpNguoiChoi2"]);
            player2.Shield = Convert.ToInt32(thongTinTranDau["giapNguoiChoi2"]);
            player2.controlMana = Convert.ToInt32(thongTinTranDau["nangLuongNguoiChoi2"]);
            ///
            gameManager.Update(player1SMana, player1.Shield);
            gameManager.Update(player2SMana, player2.Shield);
            gameManager.Update(Cp1, player1.controlMana);
            gameManager.Update(Cp2, player2.controlMana);

            Player1Health.Value = player1.HP;
            Player2Health.Value = player2.HP;
            player1Name.Text = player1.Name;
            player2Name.Text = player2.Name;
            

            BeginInvoke((Action)(() =>
            {
                if (player1.IsDefeated() | player2.IsDefeated())
                {
                    DialogResult result = MessageBox.Show(
                                            "Trận đấu đã kết thúc, trận đấu này sẽ bị xóa!!!",
                                            "Thông báo",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Question);
                    DatabaseGame.DeleteData(Convert.ToInt32(thongTinTranDau["maTranDau"]));
                }
            }));


        }
        public void _SetData(DataRow data)
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
            timeSpan = (TimeSpan)thongTinTranDau["thoiGian"];
            startTime = DateTime.Now;
        }
        private void Form_cheDo2Nguoi_FormClosed(object sender, FormClosedEventArgs e)
        {
            thongTinTranDau["thoiGian"] = timeSpan + (TimeSpan)(DateTime.Now - startTime);
            thongTinTranDau["luotNguoiChoi"] = gameManager.playerTurn();
            thongTinTranDau["hpNguoiChoi1"] = player1.HP;
            thongTinTranDau["giapNguoiChoi1"] = player1.Shield;
            thongTinTranDau["nangLuongNguoiChoi1"] = player1.controlMana;
            thongTinTranDau["hpNguoiChoi2"] = player2.HP;
            thongTinTranDau["giapNguoiChoi2"] = player2.Shield;
            thongTinTranDau["nangLuongNguoiChoi2"] = player2.controlMana;
            if (Convert.ToInt32(thongTinTranDau["maTranDau"]) != -1)
            {
                DatabaseGame.UpdateData_tranDau2Nguoi(thongTinTranDau, jewelGrid._GetDataTable_Jewels());
            }
            else
            {
                DatabaseGame.InsertData_tranDau2Nguoi(thongTinTranDau, jewelGrid._GetDataTable_Jewels());
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TurnManager();
            label1.Text = "HP: " + player1.HP.ToString() + "/100";
            label2.Text = "HP: " + player2.HP.ToString() + "/100";
            label3.Text = "Shield: " + player1.Shield.ToString();
            label4.Text = "Shield: " + player2.Shield.ToString();
            label5.Text = "Control: " + player1.controlMana.ToString();
            label6.Text = "Control: " + player2.controlMana.ToString();
            textControl();
            textDame();
            if (loadGame == true)
            {
                loadGame = false;
                loadData();
            }
            Player1Health.Value = player1.HP;
            Player2Health.Value = player2.HP;
            gameManager.Update(player1SMana, player1.Shield);
            gameManager.Update(player2SMana, player2.Shield);
            gameManager.Update(Cp1, player1.controlMana);
            gameManager.Update(Cp2, player2.controlMana);
            if (player1.IsDefeated() && gameOff == false)
            {
                gameOff = true;
                MessageBox.Show(player2.Name + " chiến thắng!");

                return;

            }
            else if (player2.IsDefeated() && gameOff == false)
            {
                gameOff = true;
                MessageBox.Show(player1.Name + " chiến thắng!");

                return;
            }
        }

        private async Task textDame()
        {
            if (player1.isDamage == true)
            {

                label7.Visible = true;
                label7.Text = " - " + player1._damage.ToString();
                for (int i = 0; i < 3; i++)
                {

                    label7.Top -= 1;
                    await Task.Delay(500);
                }

                label7.Location = new System.Drawing.Point(panel1.Location.X, panel1.Location.Y - this.ClientSize.Height / 30);
                player1.isDamage = false;
                label7.Visible = false;


            }
            else if (player2.isDamage == true)
            {
                {
                    label8.Visible = true;
                    label8.Text = " - " + player2._damage.ToString();
                    for (int i = 0; i < 3; i++)
                    {

                        label8.Top -= 1;
                        await Task.Delay(500);
                    }

                    label8.Location = new System.Drawing.Point(this.ClientSize.Width / 3 + this.ClientSize.Width / 10, label7.Location.Y);
                    player2.isDamage = false;
                    label8.Visible = false;
                }
            }

        }
        private async Task textControl()
        {
            if (player2.isControl == true )
            { 
                label10.Text = player1.Name + " bị khống chế";
                label10.Visible = true;       
                for (int i = 0; i < 3; i++)
                {
                    label10.Top -= 1;
                    await Task.Delay(500);
                }
                player2.isControl = false;
            }
            else if (player1.isControl == true)
            { 
                label10.Text = player2.Name + " bị khống chế";
                label10.Visible = true;
                for (int i = 0; i < 3; i++)
                {
                    label10.Top -= 1;
                    await Task.Delay(500);
                }
                await Task.Delay(1000);
                player1.isControl = false;
            }
                label10.Visible = false;
            label10.Location = new System.Drawing.Point(this.ClientSize.Width / 5, this.ClientSize.Height / 2);
        }
        private void TurnManager()
        {
            if (gameManager._isPlayer1Turn == true && GameManager.canClick == true)
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
            }
            else if (gameManager._isPlayer1Turn == false && GameManager.canClick == true)
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
            }
        }
        private async Task TimeCount()
        {
            while (true)
            {
                label9.Text = i.ToString();
                await Task.Delay(1000);
                i++;
            }
        }
        private void Lbresize(int a)
        {
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", a, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", a * 2, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

    }
}
