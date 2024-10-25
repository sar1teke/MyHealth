using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace Hasta_takip
{
    public partial class login : Form
    {
        private string connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=DESKTOP-3GS3IHC)(PORT=1521))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=XE)));User Id=HASTA_TAKIP;Password=1;";

        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string kullaniciAdi = textBox1.Text;
            string sifre = textBox2.Text;

            if (GirisKontrolu(kullaniciAdi, sifre))
            {
                MessageBox.Show("Giriş başarılı!");
                Ana_Ekran lgn = new Ana_Ekran();

                lgn.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı!");
            }
        }

        private bool GirisKontrolu(string kullaniciAdi, string sifre)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT COUNT(*) FROM KULLANICILAR WHERE KULLANICI_ADI = :paramKullaniciAdi AND SIFRE = :paramSifre";

                    using (OracleCommand command = new OracleCommand(sqlQuery, connection))
                    {
                        command.Parameters.Add(":paramKullaniciAdi", OracleDbType.Varchar2).Value = kullaniciAdi;
                        command.Parameters.Add(":paramSifre", OracleDbType.Varchar2).Value = sifre;

                        int kullaniciSayisi = Convert.ToInt32(command.ExecuteScalar());

                        return kullaniciSayisi > 0;
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Giriş kontrolü hatası: " + ex.Message);
                return false;
            }
        }
    }
}
