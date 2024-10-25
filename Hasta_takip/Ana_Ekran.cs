using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hasta_takip
{
    public partial class Ana_Ekran : Form

    {
        private string connectionString = "Data Source = (DESCRIPTION = " + "(ADDRESS = (PROTOCOL = TCP)(HOST = DESKTOP-3GS3IHC)(PORT = 1521))" + "  (CONNECT_DATA = " + "  (SERVER = DEDICATED)" + "   (SERVICE_NAME = XE)" + ")" + " );User Id = HASTA_TAKIP; password = 1;";
        private OracleDataAdapter dataAdapter;
        private DataTable dataTable;
        private string kisim;

        public Ana_Ekran()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {

            Temizle();

            /*String connectionString = "Data Source = (DESCRIPTION = " + "(ADDRESS = (PROTOCOL = TCP)(HOST = DESKTOP-3GS3IHC)(PORT = 1521))" + "  (CONNECT_DATA = " + "  (SERVER = DEDICATED)" + "   (SERVICE_NAME = XE)" + ")" + " );User Id = HASTA_TAKIP; password = 1;";

            OracleConnection con = new OracleConnection();
            con.ConnectionString = connectionString;

            con.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.CommandText = "SELECT * FROM HASTALAR";
            cmd.Connection = con;

            cmd.CommandType = CommandType.Text;

            OracleDataReader dr = cmd.ExecuteReader();

            dr.Read();

            dataGridView1.Text = dr.GetString(0);*/
        }
        private void LoadData()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // Veri çekme sorgusu
                string sqlQuery = "SELECT * FROM HASTALAR";

                // OracleDataAdapter ve DataTable kullanarak verileri çekme
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView'e verileri yükleme
                dataGridView1.DataSource = dataTable;
            }
        }
        private void Temizle()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            pictureBox1.ImageLocation = "";

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO HASTALAR (TC,AD,SOYAD,ADRES,TELEFON,TESHIS,UCRET,G_TARIHI,C_TARIHI,RESIM) VALUES (:param1, :param2, :param3, :param4, :param5, :param6, :param7, :param8, :param9, :param10)";

                    using (OracleCommand command = new OracleCommand(insertQuery, connection))
                    {
                        command.Parameters.Add(":param1", OracleDbType.Int64).Value = textBox6.Text;
                        command.Parameters.Add(":param2", OracleDbType.Varchar2).Value = textBox1.Text;
                        command.Parameters.Add(":param3", OracleDbType.Varchar2).Value = textBox8.Text;
                        command.Parameters.Add(":param4", OracleDbType.Varchar2).Value = textBox5.Text;
                        command.Parameters.Add(":param5", OracleDbType.Int64).Value = textBox4.Text;
                        command.Parameters.Add(":param6", OracleDbType.Varchar2).Value = textBox3.Text;
                        command.Parameters.Add(":param7", OracleDbType.Int64).Value = textBox2.Text;
                        command.Parameters.Add(":param8", OracleDbType.Date).Value = dateTimePicker1.Value;
                        command.Parameters.Add(":param9", OracleDbType.Date).Value = dateTimePicker2.Value;
                        command.Parameters.Add(":param10", OracleDbType.Varchar2).Value = textBox9.Text;

                        if (textBox6.Text==""||textBox1.Text==""||textBox8.Text=="")
                        {
                            MessageBox.Show("Lütfen Doldurulması Zorunlu Alanları Doldurunuz");
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Yeni Kayıt Başarıyla Eklendi.");
                        }
                    }
            
                    LoadData();
                    Temizle();

                }
            }

            catch
            {
                MessageBox.Show("Hatalı İşlem!!!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog dosya = new OpenFileDialog();
            dosya.Filter = "C://Users//srtk//source//repos//Hasta_takip_Otomasyonu//Resimler |*.jpg; *.png";
            dosya.ShowDialog();
            string dosyayolu = dosya.FileName;
            textBox9.Text = dosyayolu;
            pictureBox1.ImageLocation = dosyayolu;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                //textBox7.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox6.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox8.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();

                pictureBox1.ImageLocation = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {

            try {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "DELETE FROM HASTALAR WHERE TC = :param1";

                    using (OracleCommand command = new OracleCommand(insertQuery, connection))
                    {
                        command.Parameters.Add(":param1", OracleDbType.Int64).Value = textBox6.Text;

                        if (textBox6.Text=="")
                        {
                            MessageBox.Show("Lütfen TC Kısmını Doldurunuz.");
                        }

                        else
                        {
                            int sonuc = command.ExecuteNonQuery();
                            if (sonuc==1)
                            {
                                MessageBox.Show("Mevcut Kayıt Başarıyla Silindi");
                            }
                            else
                            {
                                MessageBox.Show("Lütfen Silmek İstediğiniz Kişinin TC Kimlik Numarasını Doğru Giriniz.");
                            }
                            LoadData();
                            Temizle();
                        }
                    }
                }
            }
            catch {
                MessageBox.Show("Hatalı İşlem Lütfen Tekrar Deneyiniz.");
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "UPDATE HASTALAR SET AD=:param2,SOYAD=:param3,ADRES=:param4,TELEFON=:param5,TESHIS=:param6,UCRET=:param7,G_TARIHI=:param8,C_TARIHI=:param9,RESIM=:param10 WHERE TC=:param1";

                    using (OracleCommand command = new OracleCommand(insertQuery, connection))
                    {

                        command.Parameters.Add(":param2", OracleDbType.Varchar2).Value = textBox1.Text;
                        command.Parameters.Add(":param3", OracleDbType.Varchar2).Value = textBox8.Text;
                        command.Parameters.Add(":param4", OracleDbType.Varchar2).Value = textBox5.Text;
                        command.Parameters.Add(":param5", OracleDbType.Int64).Value = textBox4.Text;
                        command.Parameters.Add(":param6", OracleDbType.Varchar2).Value = textBox3.Text;
                        command.Parameters.Add(":param7", OracleDbType.Int64).Value = textBox2.Text;
                        command.Parameters.Add(":param8", OracleDbType.Date).Value = dateTimePicker1.Value;
                        command.Parameters.Add(":param9", OracleDbType.Date).Value = dateTimePicker2.Value;
                        command.Parameters.Add(":param10", OracleDbType.Varchar2).Value = textBox9.Text;
                        command.Parameters.Add(":param1", OracleDbType.Int64).Value = textBox6.Text;

                        if (textBox6.Text == "" || textBox1.Text == "" || textBox8.Text == "")
                        {
                            MessageBox.Show("Lütfen Doldurulması Zorunlu Alanları Doldurunuz");
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Mevcut Kayıt Başarıyla Güncellendi.");
                            LoadData();
                            Temizle();
                        }
                    }
                }
            }
            catch {
                MessageBox.Show("Hatalı İşlem Lütfen Tekrar Deneyiniz.");
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // Veri çekme sorgusu
                string sqlQuery = "SELECT * FROM HASTALAR WHERE TC LIKE :param1TC";

                // OracleDataAdapter ve DataTable kullanarak verileri çekme
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.Add(":param1TC", OracleDbType.Varchar2).Value = textBox10.Text + "%"; // TC'nin başındaki kısma göre filtreleme

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView'e verileri yükleme
                dataGridView1.DataSource = dataTable;
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            Diger_Islemler_Ekrani lgn2 = new Diger_Islemler_Ekrani();

            lgn2.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

