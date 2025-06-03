using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace JewelGame._Scripts
{
    public class DatabaseGame
    {
        private const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=GameKimCuong;Integrated Security=True;";
        public static DataRow NewRow_TranDau1Nguoi()
        {
            DataTable resultTable = new DataTable();

            // Thêm cột của TranDau
            resultTable.Columns.Add("maTranDau", typeof(int));
            resultTable.Columns.Add("kichCo", typeof(int));
            resultTable.Columns.Add("thoiGian", typeof(TimeSpan));
            resultTable.Columns.Add("cheDoChoi", typeof(string));

            // Thêm cột của TranDau_1Nguoi
            resultTable.Columns.Add("maTranDau1Nguoi", typeof(int));
            resultTable.Columns.Add("diemSo", typeof(int));
            resultTable.Columns.Add("tenNguoiChoi", typeof(string));

            DataRow row = resultTable.NewRow();
            //Giá trị mặc định
            row["maTranDau"] = -1;
            row["kichCo"] = 10;
            row["thoiGian"] = new TimeSpan(0);
            row["cheDoChoi"] = "1 Player";

            row["maTranDau1Nguoi"] = -1;
            row["diemSo"] = 0;
            row["tenNguoiChoi"] = "Player không có tên";

            return row;
        }
        public static DataRow NewRow_TranDau2Nguoi()
        {
            DataTable resultTable = new DataTable();

            // Thêm cột của TranDau
            resultTable.Columns.Add("maTranDau", typeof(int));
            resultTable.Columns.Add("kichCo", typeof(int));
            resultTable.Columns.Add("thoiGian", typeof(TimeSpan));
            resultTable.Columns.Add("cheDoChoi", typeof(string));

            // Thêm cột của TranDau_1Nguoi
            resultTable.Columns.Add("maTranDau2Nguoi", typeof(int));
            resultTable.Columns.Add("luotNguoiChoi", typeof(bool));

            resultTable.Columns.Add("tenNguoiChoi1", typeof(string));
            resultTable.Columns.Add("hpNguoiChoi1", typeof(int));
            resultTable.Columns.Add("giapNguoiChoi1", typeof(int));
            resultTable.Columns.Add("nangLuongNguoiChoi1", typeof(int));

            resultTable.Columns.Add("tenNguoiChoi2", typeof(string));
            resultTable.Columns.Add("hpNguoiChoi2", typeof(int));
            resultTable.Columns.Add("giapNguoiChoi2", typeof(int));
            resultTable.Columns.Add("nangLuongNguoiChoi2", typeof(int));

            DataRow row = resultTable.NewRow();
            //Giá trị mặc định
            row["maTranDau"] = -1;
            row["kichCo"] = 10;
            row["thoiGian"] = new TimeSpan(0);
            row["cheDoChoi"] = "2 Player";

            row["maTranDau2Nguoi"] = -1;
            row["luotNguoiChoi"] = true;

            row["tenNguoiChoi1"] = "Player 1 không có tên";
            row["hpNguoiChoi1"] = 100;
            row["giapNguoiChoi1"] = 0;
            row["nangLuongNguoiChoi1"] = 0;

            row["tenNguoiChoi2"] = "Player 2 không có tên";
            row["hpNguoiChoi2"] = 100;
            row["giapNguoiChoi2"] = 0;
            row["nangLuongNguoiChoi2"] = 0;

            return row;
        }
        public static DataTable GetDataTable_TranDau()
        {
            var result = new DataTable();
            string query = "SELECT * FROM dbo.TranDau";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.Fill(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu TranDau từ SQL: " + ex.Message);
            }

            return result;
        }
        public static DataRow GetDataRow_TranDau1Nguoi(int maTranDau)
        {
            var resultTable = new DataTable();
            string query = @"
                    SELECT td.*, td_1P.*
                    FROM dbo.TranDau td
                    JOIN dbo.TranDau_1Nguoi td_1P ON td.maTranDau = td_1P.maTranDau
                    WHERE td.maTranDau = @maTranDau";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDau);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(resultTable);
                    }
                }

                if (resultTable.Rows.Count > 0) return resultTable.Rows[0];
                else return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu TranDau_1Nguoi từ SQL: " + ex.Message);
                return null;
            }
        }
        public static DataRow GetDataRow_TranDau2Nguoi(int maTranDau)
        {
            var resultTable = new DataTable();
            string query = @"
                    SELECT td.*, td_2P.*
                    FROM dbo.TranDau td
                    JOIN dbo.TranDau_2Nguoi td_2P ON td.maTranDau = td_2P.maTranDau
                    WHERE td.maTranDau = @maTranDau";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDau);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(resultTable);
                    }
                }

                // Trả về dòng đầu tiên nếu có
                if (resultTable.Rows.Count > 0)
                {
                    return resultTable.Rows[0];
                }
                else
                {
                    return null; // hoặc throw nếu muốn xử lý lỗi dữ liệu không tồn tại
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu TranDau_2Nguoi từ SQL: " + ex.Message);
                return null;
            }
        }
        public static DataTable GetDataTable_Jewels(int MaTranDau)
        {
            var result = new DataTable();

            string query = @"SELECT * FROM dbo.Jewel
                             WHERE maTranDau = @maTranDau";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", MaTranDau);

                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(result);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải Jewel dữ liệu từ SQL: " + ex.Message);
            }
            return result;
        }
        public static void InsertData_tranDau1Nguoi(DataRow dataRow_tranDau1Nguoi, DataTable dataTable_jewel)
        {
            if (dataRow_tranDau1Nguoi == null)
            {
                MessageBox.Show("Không có dữ liệu TranDau để thêm.");
                return;
            }

            string query_tranDau = @"INSERT INTO dbo.TranDau (thoiGian, kichCo, cheDoChoi)
                     OUTPUT INSERTED.maTranDau
                     VALUES (@thoiGian, @kichCo, @cheDoChoi)";

            string query_tranDau1Nguoi = @"INSERT INTO dbo.TranDau_1Nguoi (maTranDau, diemSo, tenNguoiChoi)
                           VALUES (@maTranDau, @diemSo, @tenNguoiChoi)";

            string query_jewel = @"INSERT INTO dbo.Jewel (maTranDau, toaDoX, toaDoY, loaiJewel)
                   VALUES (@maTranDau, @toaDoX, @toaDoY, @loaiJewel)";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    int maTranDauMoi;

                    // Insert vào bảng TranDau
                    using (var cmd = new SqlCommand(query_tranDau, conn))
                    {
                        cmd.Parameters.AddWithValue("@thoiGian", dataRow_tranDau1Nguoi["thoiGian"]);
                        cmd.Parameters.AddWithValue("@kichCo", dataRow_tranDau1Nguoi["kichCo"]);
                        cmd.Parameters.AddWithValue("@cheDoChoi", dataRow_tranDau1Nguoi["cheDoChoi"]);

                        maTranDauMoi = (int)cmd.ExecuteScalar();
                    }

                    // Insert vào bảng TranDau_1Nguoi
                    using (var cmd = new SqlCommand(query_tranDau1Nguoi, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDauMoi);
                        cmd.Parameters.AddWithValue("@diemSo", dataRow_tranDau1Nguoi["diemSo"]);
                        cmd.Parameters.AddWithValue("@tenNguoiChoi", dataRow_tranDau1Nguoi["tenNguoiChoi"]);

                        cmd.ExecuteNonQuery();
                    }

                    // Insert các Jewel
                    foreach (DataRow row in dataTable_jewel.Rows)
                    {
                        using (var cmd = new SqlCommand(query_jewel, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTranDau", maTranDauMoi);
                            cmd.Parameters.AddWithValue("@toaDoX", row["toaDoX"]);
                            cmd.Parameters.AddWithValue("@toaDoY", row["toaDoY"]);
                            cmd.Parameters.AddWithValue("@loaiJewel", row["loaiJewel"]);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu TranDau vào SQL: " + ex.Message);
            }
        }
        public static void InsertData_tranDau2Nguoi(DataRow dataRow_tranDau2Nguoi, DataTable dataTable_jewel)
        {
            if (dataRow_tranDau2Nguoi == null)
            {
                MessageBox.Show("Không có dữ liệu TranDau để thêm.");
                return;
            }

            string query_tranDau = @"INSERT INTO dbo.TranDau (thoiGian, kichCo, cheDoChoi)
                                     OUTPUT INSERTED.maTranDau
                                     VALUES (@thoiGian, @kichCo, @cheDoChoi)";

            string query_tranDau2Nguoi = @"INSERT INTO dbo.TranDau_2Nguoi (
                                                maTranDau, luotNguoiChoi,
                                                tenNguoiChoi1, hpNguoiChoi1, giapNguoiChoi1, nangLuongNguoiChoi1, 
                                                tenNguoiChoi2, hpNguoiChoi2, giapNguoiChoi2, nangLuongNguoiChoi2)
                                            VALUES (
                                                @maTranDau, @luotNguoiChoi, @tenNguoiChoi1, @hpNguoiChoi1, @giapNguoiChoi1, @nangLuongNguoiChoi1,
                                                @tenNguoiChoi2, @hpNguoiChoi2, @giapNguoiChoi2, @nangLuongNguoiChoi2)";

            string query_jewel = @"INSERT INTO dbo.Jewel (maTranDau, toaDoX, toaDoY, loaiJewel)
                   VALUES (@maTranDau, @toaDoX, @toaDoY, @loaiJewel)";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    int maTranDauMoi;

                    // Insert vào bảng TranDau
                    using (var cmd = new SqlCommand(query_tranDau, conn))
                    {
                        cmd.Parameters.AddWithValue("@thoiGian", dataRow_tranDau2Nguoi["thoiGian"]);
                        cmd.Parameters.AddWithValue("@kichCo", dataRow_tranDau2Nguoi["kichCo"]);
                        cmd.Parameters.AddWithValue("@cheDoChoi", dataRow_tranDau2Nguoi["cheDoChoi"]);

                        maTranDauMoi = (int)cmd.ExecuteScalar();
                    }

                    // Insert vào bảng TranDau_2Nguoi
                    using (var cmd = new SqlCommand(query_tranDau2Nguoi, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDauMoi);
                        cmd.Parameters.AddWithValue("@luotNguoiChoi", dataRow_tranDau2Nguoi["luotNguoiChoi"]);
                        cmd.Parameters.AddWithValue("@tenNguoiChoi1", dataRow_tranDau2Nguoi["tenNguoiChoi1"]);
                        cmd.Parameters.AddWithValue("@hpNguoiChoi1", dataRow_tranDau2Nguoi["hpNguoiChoi1"]);
                        cmd.Parameters.AddWithValue("@giapNguoiChoi1", dataRow_tranDau2Nguoi["giapNguoiChoi1"]);
                        cmd.Parameters.AddWithValue("@nangLuongNguoiChoi1", dataRow_tranDau2Nguoi["nangLuongNguoiChoi1"]);
                        cmd.Parameters.AddWithValue("@tenNguoiChoi2", dataRow_tranDau2Nguoi["tenNguoiChoi2"]);
                        cmd.Parameters.AddWithValue("@hpNguoiChoi2", dataRow_tranDau2Nguoi["hpNguoiChoi2"]);
                        cmd.Parameters.AddWithValue("@giapNguoiChoi2", dataRow_tranDau2Nguoi["giapNguoiChoi2"]);
                        cmd.Parameters.AddWithValue("@nangLuongNguoiChoi2", dataRow_tranDau2Nguoi["nangLuongNguoiChoi2"]);

                        cmd.ExecuteNonQuery();
                    }

                    // Insert các Jewel
                    foreach (DataRow row in dataTable_jewel.Rows)
                    {
                        using (var cmd = new SqlCommand(query_jewel, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTranDau", maTranDauMoi);
                            cmd.Parameters.AddWithValue("@toaDoX", row["toaDoX"]);
                            cmd.Parameters.AddWithValue("@toaDoY", row["toaDoY"]);
                            cmd.Parameters.AddWithValue("@loaiJewel", row["loaiJewel"]);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu TranDau 2 người vào SQL: " + ex.Message);
            }
        }


        public static void UpdateData_tranDau1Nguoi(DataRow dataRow_tranDau1Nguoi, DataTable dataTable_jewel)
        {
            if (dataRow_tranDau1Nguoi == null)
            {
                MessageBox.Show("Không có dữ liệu TranDau để cập nhật.");
                return;
            }

            string query_tranDau = @"UPDATE dbo.TranDau
                            SET 
                                thoiGian = @thoiGian,
                                kichCo = @kichCo,
                                cheDoChoi = @cheDoChoi
                            WHERE 
                                maTranDau = @maTranDau";

            string query_tranDau1Nguoi = @"UPDATE dbo.TranDau_1Nguoi
                                  SET diemSo = @diemSo,
                                      tenNguoiChoi = @tenNguoiChoi
                                  WHERE maTranDau = @maTranDau";

            string query_jewel = @"UPDATE dbo.Jewel 
                            SET loaiJewel = @loaiJewel
                            WHERE maTranDau = @maTranDau AND toaDoX = @toaDoX AND toaDoY = @toaDoY";

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Cập nhật bảng TranDau
                    using (var cmd = new SqlCommand(query_tranDau, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", dataRow_tranDau1Nguoi["maTranDau"]);
                        cmd.Parameters.AddWithValue("@thoiGian", dataRow_tranDau1Nguoi["thoiGian"]);
                        cmd.Parameters.AddWithValue("@kichCo", dataRow_tranDau1Nguoi["kichCo"]);
                        cmd.Parameters.AddWithValue("@cheDoChoi", dataRow_tranDau1Nguoi["cheDoChoi"]);

                        if (cmd.ExecuteNonQuery() == 0)
                            MessageBox.Show("Lỗi khi cập nhật dữ liệu TranDau.");
                    }

                    // Cập nhật bảng TranDau_1Nguoi
                    using (var cmd = new SqlCommand(query_tranDau1Nguoi, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", dataRow_tranDau1Nguoi["maTranDau"]);
                        cmd.Parameters.AddWithValue("@diemSo", dataRow_tranDau1Nguoi["diemSo"]);
                        cmd.Parameters.AddWithValue("@tenNguoiChoi", dataRow_tranDau1Nguoi["tenNguoiChoi"]);

                        if (cmd.ExecuteNonQuery() == 0)
                            MessageBox.Show("Lỗi khi cập nhật dữ liệu TranDau_1Nguoi.");
                    }

                    // Cập nhật các Jewel
                    foreach (DataRow jewelRow in dataTable_jewel.Rows)
                    {
                        using (var cmd = new SqlCommand(query_jewel, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTranDau", dataRow_tranDau1Nguoi["maTranDau"]);
                            cmd.Parameters.AddWithValue("@toaDoX", jewelRow["toaDoX"]);
                            cmd.Parameters.AddWithValue("@toaDoY", jewelRow["toaDoY"]);
                            cmd.Parameters.AddWithValue("@loaiJewel", jewelRow["loaiJewel"]);

                            if (cmd.ExecuteNonQuery() == 0)
                                MessageBox.Show("Lỗi khi cập nhật dữ liệu Jewel.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu TranDau vào SQL: " + ex.Message);
            }
        }
        public static void UpdateData_tranDau2Nguoi(DataRow dataRow_tranDau2Nguoi, DataTable dataTable_jewel)
        {
            if (dataRow_tranDau2Nguoi == null)
            {
                MessageBox.Show("Không có dữ liệu TranDau để cập nhật.");
                return;
            }
            string query_tranDau = @"UPDATE dbo.TranDau
                            SET 
                                thoiGian = @thoiGian,
                                kichCo = @kichCo,
                                cheDoChoi = @cheDoChoi
                            WHERE 
                                maTranDau = @maTranDau";
            string query_tranDau_2Nguoi = @"UPDATE dbo.TranDau_2Nguoi
                                   SET 
                                       luotNguoiChoi = @luotNguoiChoi,
                                       tenNguoiChoi1 = @tenNguoiChoi1,
                                       hpNguoiChoi1 = @hpNguoiChoi1,
                                       giapNguoiChoi1 = @giapNguoiChoi1,
                                       nangLuongNguoiChoi1 = @nangLuongNguoiChoi1,
                                       tenNguoiChoi2 = @tenNguoiChoi2,
                                       hpNguoiChoi2 = @hpNguoiChoi2,
                                       giapNguoiChoi2 = @giapNguoiChoi2,
                                       nangLuongNguoiChoi2 = @nangLuongNguoiChoi2
                                   WHERE 
                                       maTranDau = @maTranDau";
            string query_jewel = @"UPDATE dbo.Jewel 
                          SET loaiJewel = @loaiJewel
                          WHERE maTranDau = @maTranDau AND toaDoX = @toaDoX AND toaDoY = @toaDoY";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Cập nhật bảng TranDau
                    using (var cmd = new SqlCommand(query_tranDau, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", dataRow_tranDau2Nguoi["maTranDau"]);
                        cmd.Parameters.AddWithValue("@thoiGian", dataRow_tranDau2Nguoi["thoiGian"]);
                        cmd.Parameters.AddWithValue("@kichCo", dataRow_tranDau2Nguoi["kichCo"]);
                        cmd.Parameters.AddWithValue("@cheDoChoi", dataRow_tranDau2Nguoi["cheDoChoi"]);

                        if (cmd.ExecuteNonQuery() == 0)
                            MessageBox.Show("Lỗi khi cập nhật dữ liệu TranDau.");
                    }
                    // Cập nhật bảng TranDau_2Nguoi
                    using (var cmd = new SqlCommand(query_tranDau_2Nguoi, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", dataRow_tranDau2Nguoi["maTranDau"]);
                        cmd.Parameters.AddWithValue("@luotNguoiChoi", dataRow_tranDau2Nguoi["luotNguoiChoi"]);

                        cmd.Parameters.AddWithValue("@tenNguoiChoi1", dataRow_tranDau2Nguoi["tenNguoiChoi1"]);
                        cmd.Parameters.AddWithValue("@hpNguoiChoi1", dataRow_tranDau2Nguoi["hpNguoiChoi1"]);
                        cmd.Parameters.AddWithValue("@giapNguoiChoi1", dataRow_tranDau2Nguoi["giapNguoiChoi1"]);
                        cmd.Parameters.AddWithValue("@nangLuongNguoiChoi1", dataRow_tranDau2Nguoi["nangLuongNguoiChoi1"]);

                        cmd.Parameters.AddWithValue("@tenNguoiChoi2", dataRow_tranDau2Nguoi["tenNguoiChoi2"]);
                        cmd.Parameters.AddWithValue("@hpNguoiChoi2", dataRow_tranDau2Nguoi["hpNguoiChoi2"]);
                        cmd.Parameters.AddWithValue("@giapNguoiChoi2", dataRow_tranDau2Nguoi["giapNguoiChoi2"]);
                        cmd.Parameters.AddWithValue("@nangLuongNguoiChoi2", dataRow_tranDau2Nguoi["nangLuongNguoiChoi2"]);

                        if (cmd.ExecuteNonQuery() == 0)
                            MessageBox.Show("Lỗi khi cập nhật dữ liệu TranDau_2Nguoi vào SQL");
                    }
                    // Cập nhật bảng Jewel
                    foreach (DataRow dataRow_Jewel in dataTable_jewel.Rows)
                    {
                        using (var cmd = new SqlCommand(query_jewel, conn))
                        {
                            cmd.Parameters.AddWithValue("@maTranDau", dataRow_tranDau2Nguoi["maTranDau"]);
                            cmd.Parameters.AddWithValue("@toaDoX", dataRow_Jewel["toaDoX"]);
                            cmd.Parameters.AddWithValue("@toaDoY", dataRow_Jewel["toaDoY"]);
                            cmd.Parameters.AddWithValue("@loaiJewel", dataRow_Jewel["loaiJewel"]);

                            if (cmd.ExecuteNonQuery() == 0)
                                MessageBox.Show($"Lỗi khi cập nhật dữ liệu của Jewel tại tọa độ ({dataRow_Jewel["toaDoX"]}, {dataRow_Jewel["toaDoY"]})");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật dữ liệu TranDau và Jewel vào SQL: " + ex.Message);
            }
        }

        //----------------------------------------------------------

        public static void DeleteData(int maTranDau)
        {
            string query_tranDau = @"DELETE FROM dbo.TranDau
                                    WHERE maTranDau = @maTranDau";

            string query_tranDau_1Nguoi = @"DELETE FROM dbo.TranDau_1Nguoi
                                    WHERE maTranDau = @maTranDau";

            string query_tranDau_2Nguoi = @"DELETE FROM dbo.TranDau_2Nguoi
                                    WHERE maTranDau = @maTranDau";

            string query_jewel = @"DELETE FROM dbo.Jewel
                                    WHERE maTranDau = @maTranDau";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query_jewel, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDau);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected <= 0) MessageBox.Show("Không tìm thấy dữ liệu để xóa.");
                    }
                    using (var cmd = new SqlCommand(query_tranDau_1Nguoi, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDau);

                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SqlCommand(query_tranDau_2Nguoi, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDau);

                        cmd.ExecuteNonQuery();
                    }
                    using (var cmd = new SqlCommand(query_tranDau, conn))
                    {
                        cmd.Parameters.AddWithValue("@maTranDau", maTranDau);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected <= 0) MessageBox.Show("Không tìm thấy dữ liệu để xóa.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa dữ liệu TranDau từ SQL: " + ex.Message);
            }
        }
    }
}
