using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Hasta_takip
{
    public partial class Diger_Islemler_Ekrani : Form
    {
        private string connectionString = "Data Source = (DESCRIPTION = " + "(ADDRESS = (PROTOCOL = TCP)(HOST = DESKTOP-3GS3IHC)(PORT = 1521))" + "  (CONNECT_DATA = " + "  (SERVER = DEDICATED)" + "   (SERVICE_NAME = XE)" + ")" + " );User Id = HASTA_TAKIP; password = 1;";
        private OracleDataAdapter dataAdapter;
        private DataTable dataTable;
        private string kisim;


        private void button7_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            printPreviewDialog.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            printPreviewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(dataGridView1.Width, dataGridView1.Height);
            dataGridView1.DrawToBitmap(bmp, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
            e.Graphics.DrawImage(bmp, 50, 50);
        }

        public Diger_Islemler_Ekrani()
        {
            InitializeComponent();
        }


        private void Diger_Islemler_Ekrani_Load(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // Veri çekme sorgusu
                string sqlQuery = "SELECT * FROM YAPILAN_ISLEMLER";

                // OracleDataAdapter ve DataTable kullanarak verileri çekme
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView'e verileri yükleme
                dataGridView1.DataSource = dataTable;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Temizle()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            numericUpDown1.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            label15.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM YAPILAN_ISLEMLER WHERE TC LIKE :param1TC";

                // OracleDataAdapter ve DataTable kullanarak verileri çekme
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.Add(":param1TC", OracleDbType.Varchar2).Value = textBox1.Text + "%"; // TC'nin başındaki kısma göre filtreleme

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // DataGridView'e verileri yükleme
                dataGridView1.DataSource = dataTable;
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM HASTALAR WHERE AD LIKE :param1Ad";
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.Add(":param1Ad", OracleDbType.Varchar2).Value = textBox3.Text + "%"; // Hasta adının başındaki kısma göre filtreleme

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT * FROM HASTALAR WHERE SOYAD LIKE :param1Soyad";
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.Add(":param1Soyad", OracleDbType.Varchar2).Value = textBox4.Text + "%"; // Hasta soyadının başındaki kısma göre filtreleme

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM HASTALAR WHERE TESHIS LIKE :param1Teşhis";
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.Add(":param1Teşhis", OracleDbType.Varchar2).Value = textBox5.Text + "%"; // Teşhis adının başındaki kısma göre filtreleme

                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Ana_Ekran lgn3 = new Ana_Ekran();
            lgn3.Show();
            this.Hide();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {

                connection.Open();

                // Poliklinik verilerini çekme sorgusu
                string sqlQuery = "SELECT  POLIKLINIK_ID FROM POLIKLINIKLER";

                // OracleDataAdapter ve DataTable kullanarak verileri çekme
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                // ComboBox'ı doldur
                comboBox1.DataSource = dataTable;
                comboBox1.DisplayMember = "POLIKLINIK_ID";

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // Rastgele bir ISLEM_ID oluştur
                int randomIslemId = GenerateRandomIslemId();

                string insertQuery = "INSERT INTO YAPILAN_ISLEMLER (TC, POLIKLINIK_ID, YAPILAN_ISLEM_ID, DOKTOR_ID, MIKTAR, BIRIM_FIYAT, TOPLAM) " +
                                    "VALUES (:param1, :param2, :param3, :param4, :param5, :param6, :param7)";

                using (OracleCommand command = new OracleCommand(insertQuery, connection))
                {
                    command.Parameters.Add(":param1", OracleDbType.Int64).Value = Convert.ToInt64(textBox6.Text);
                    command.Parameters.Add(":param2", OracleDbType.Int64).Value = Convert.ToInt64(((DataRowView)comboBox1.SelectedItem)["POLIKLINIK_ID"]);
                    command.Parameters.Add(":param3", OracleDbType.Int64).Value = randomIslemId; // Rastgele ISLEM_ID
                    DataRowView selectedRow = (DataRowView)comboBox3.SelectedItem;
                    int doktorID = Convert.ToInt32(selectedRow["DOKTOR_ID"]);
                    command.Parameters.Add(":param4", OracleDbType.Int64).Value = doktorID;
                    command.Parameters.Add(":param5", OracleDbType.Int64).Value = Convert.ToInt64(numericUpDown1.Value);
                    command.Parameters.Add(":param6", OracleDbType.Int64).Value = Convert.ToInt64(textBox7.Text);
                    command.Parameters.Add(":param7", OracleDbType.Int64).Value = Convert.ToInt64(numericUpDown1.Value) * Convert.ToInt64(textBox7.Text);
                    command.ExecuteNonQuery();

                    int toplamTutar = Convert.ToInt32(numericUpDown1.Value) * Convert.ToInt32(textBox7.Text);
                    label15.Text = toplamTutar.ToString();

                    MessageBox.Show("Yeni İşlem Başarıyla Eklendi.");
                }
            }
            HesaplaVeToplamYaz();
        }

        // Rastgele bir ISLEM_ID oluşturan metod
        private int GenerateRandomIslemId()
        {
            Random random = new Random();
            return random.Next(10000, 19999); // 10000 ile 19999 arasında rastgele bir sayı döndürür.
        }

        private void comboBox2_DropDown_1(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = "SELECT  ISLEM_ID FROM ISLEM_DETAY";
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                comboBox2.DataSource = dataTable;
                comboBox2.DisplayMember = "ISLEM_ID";

            }
        }

        private void comboBox3_DropDown_1(object sender, EventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT  DOKTOR_ID FROM DOKTORLAR";
                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                comboBox3.DataSource = dataTable;
                comboBox3.DisplayMember = "DOKTOR_ID";

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Formu yenileme işlemleri burada yapılır.
            dataGridView1.DataSource = null;
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM YAPILAN_ISLEMLER";

                dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                Temizle();
            }
        }

        private void Sil(int islemID)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM YAPILAN_ISLEMLER WHERE TC = :param11";

                    using (OracleCommand command = new OracleCommand(deleteQuery, connection))
                    {
                        command.Parameters.Add(":param11", OracleDbType.Int64).Value = islemID;
                        int affectedRows = command.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Kayıt Başarıyla Silindi.");
                        }
                        else
                        {
                            MessageBox.Show("Belirtilen ISLEM_ID'ye sahip kayıt bulunamadı.");
                        }
                    }
                }

                // Verileri güncelle
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM YAPILAN_ISLEMLER";
                    dataAdapter = new OracleDataAdapter(sqlQuery, connection);
                    dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            // Kullanıcıdan TC'yi al
            if (int.TryParse(textBox6.Text, out int islemID))
            {
                // Geçerli bir ISLEM_ID değeri varsa silme işlemini gerçekleştir
                Sil(islemID);
            }

            else
            {
                MessageBox.Show("Geçerli bir ISLEM_ID giriniz.");
            }
        }
        private void TaburcuEt()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // Taburcu işlemi ekleme sorgusu
                string insertTaburcuQuery = "INSERT INTO TABURCU (TC, TABURCU_ID, TARIH) VALUES (:param1, :param2, :param3)";

                using (OracleCommand insertTaburcuCommand = new OracleCommand(insertTaburcuQuery, connection))
                {
                    // Taburcu işlemi için gerekli parametreleri ekleyin
                    insertTaburcuCommand.Parameters.Add(":param1", OracleDbType.Int64).Value = Convert.ToInt64(textBox6.Text);
                    insertTaburcuCommand.Parameters.Add(":param2", OracleDbType.Int64).Value = GenerateRandomTaburcuID(); // Rastgele taburcu ID oluşturun
                    insertTaburcuCommand.Parameters.Add(":param3", OracleDbType.Date).Value = DateTime.Now; // Şu anki tarihi kullanabilirsiniz

                    insertTaburcuCommand.ExecuteNonQuery();
                }

                // Yapılan işlem tablosundan ilgili kaydı silme sorgusu
                string deleteYapilanIslemQuery = "DELETE FROM YAPILAN_ISLEMLER WHERE TC = :param1";

                using (OracleCommand deleteYapilanIslemCommand = new OracleCommand(deleteYapilanIslemQuery, connection))
                {
                    // Yapılan işlem silme işlemi için gerekli parametreyi ekleyin
                    deleteYapilanIslemCommand.Parameters.Add(":param1", OracleDbType.Int64).Value = Convert.ToInt64(textBox6.Text);

                    deleteYapilanIslemCommand.ExecuteNonQuery();
                }
            }
        }

        private int GenerateRandomTaburcuID()
        {
            // Rastgele bir taburcu ID oluşturun (örneğin)
            Random random = new Random();
            return random.Next(1000, 9999);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TaburcuEt();
            MessageBox.Show("Hasta Taburcu Edildi Lütfen Değişikleri Görmek İçin Formu Yenileyin.");
        }


        private void HesaplaVeToplamYaz()
        {
            try
            {
                long miktar;
                if (long.TryParse(numericUpDown1.Text, out miktar))
                {
                    // miktar değeri geçerli bir long türüne dönüşebilirse devam et
                }
                else
                {
                    MessageBox.Show("Miktar için geçerli bir sayı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                long birimFiyat;
                if (long.TryParse(textBox7.Text, out birimFiyat))
                {
                    // birimFiyat değeri geçerli bir long türüne dönüşebilirse devam et
                }
                else
                {
                    MessageBox.Show("Birim fiyat için geçerli bir sayı girin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Toplam tutarı hesaplayın
                long toplamTutar = miktar * birimFiyat;

                // Toplam tutarı TextBox'a yazdırın
                label15.Text = toplamTutar.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hesaplama sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

    }
}



