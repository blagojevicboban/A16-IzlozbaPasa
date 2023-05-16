using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BLOKA16
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A16.mdf;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopuniPas();
            PopuniKategoriju();
            PopuniIzlozba();
            PopuniIzlozba1();
        }
        private void PopuniPas()
        {
            SqlCommand cmd = new SqlCommand("SELECT PasID, CONCAT(PasID, '-',Ime) as PasIme from Pas", conn);
            SqlDataAdapter da=new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                comboBoxPas.DataSource = dt;
                comboBoxPas.DisplayMember = "PasIme";
                comboBoxPas.ValueMember = "PasID";
                comboBoxPas.SelectedIndex = 0;
            }
            catch (Exception)
            {

                MessageBox.Show("Doslo je do greske");
            }
            finally{
                cmd.Dispose();
                da.Dispose();
            }
        }
        private void PopuniKategoriju()
        {
            SqlCommand cmd = new SqlCommand("SELECT KategorijaID, CONCAT(KategorijaID, '-',Naziv) as KategorijaIme from Kategorija", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                comboBoxKategorija.DataSource = dt;
                comboBoxKategorija.DisplayMember = "KategorijaIme";
                comboBoxKategorija.ValueMember = "KategorijaID";
                comboBoxKategorija.SelectedIndex = 0;
            }
            catch (Exception)
            {

                MessageBox.Show("Doslo je do greske");
            }
            finally
            {
                cmd.Dispose();
                da.Dispose();
            }
        }
        private void PopuniIzlozba()
        {
           
            SqlCommand cmd = new SqlCommand("SELECT IzlozbaID, CONCAT(IzlozbaID, '-',Mesto,'-',FORMAT(Datum, 'dd.MM.yyyy')) as IzlozbaIme from Izlozba where DATEDIFF(hour, Datum,SYSDATETIME())>48", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                comboBoxIzlozba.DataSource = dt;
                comboBoxIzlozba.DisplayMember = "IzlozbaIme";
                comboBoxIzlozba.ValueMember = "IzlozbaID";
                comboBoxIzlozba.SelectedIndex = 0;
                
            }
            catch (Exception)
            {

                MessageBox.Show("Doslo je do greske");
            }
            finally
            {
                cmd.Dispose();
                da.Dispose();
            }
        }
        private void PopuniIzlozba1()
        {

            SqlCommand cmd = new SqlCommand("SELECT IzlozbaID, CONCAT(IzlozbaID, '-',Mesto,'-',FORMAT(Datum, 'dd.MM.yyyy')) as IzlozbaIme from Izlozba where  Datum<SYSDATETIME()", conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                
                comboBoxizlozba1.DataSource = dt;
                comboBoxizlozba1.DisplayMember = "IzlozbaIme";
                comboBoxizlozba1.ValueMember = "IzlozbaID";
                comboBoxizlozba1.SelectedIndex = 0;
            }
            catch (Exception)
            {

                MessageBox.Show("Doslo je do greske");
            }
            finally
            {
                cmd.Dispose();
                da.Dispose();
            }
        }

        private void buttonZATVORI_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonizadji1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonPrijava_Click(object sender, EventArgs e)
        {
            if (comboBoxPas.Text == "" || comboBoxIzlozba.Text=="" || comboBoxKategorija.Text == "")
            {
                MessageBox.Show("Popunite sve podatke");
                return;
            }
            SqlCommand cmd = new SqlCommand("select * from Rezultat where IzlozbaID=@Izlozba and KategorijaID=@Kategorija and PasID=@Pas", conn);
            cmd.Parameters.AddWithValue("@Izlozba", comboBoxIzlozba.SelectedValue);
            cmd.Parameters.AddWithValue("@Kategorija", comboBoxKategorija.SelectedValue);
            cmd.Parameters.AddWithValue("@Pas", comboBoxPas.SelectedValue);
            SqlDataAdapter da=new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);


            }
            catch (Exception)
            {

                MessageBox.Show("Doslo je do greske");
            }
            if(dt.Rows.Count > 0)
            {
                MessageBox.Show("Pas je vec prijavljen");
                return;
            }
            try
            {
                SqlCommand cmd1 = new SqlCommand("insert into Rezultat(IzlozbaID, KategorijaID, PasID) values (@Izlozba, @Kategorija, @Pas) ",conn);
                cmd1.Parameters.AddWithValue("@Izlozba", comboBoxIzlozba.SelectedValue);
                cmd1.Parameters.AddWithValue("@Kategorija", comboBoxKategorija.SelectedValue);
                cmd1.Parameters.AddWithValue("@Pas", comboBoxPas.SelectedValue);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                MessageBox.Show("Uspesno prijavljen");
            }
            catch (Exception)
            {

                MessageBox.Show("Doslo je do greske");
            }
        }

        private void buttonPrikazi_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("select k.KategorijaID as 'Sifra', k.Naziv as 'Naziv Kategorije',COUNT(*) as 'Broj pasa' from Rezultat as r, Kategorija as k where r.KategorijaID=r.KategorijaID and IzlozbaID=@Izlozba group by k.KategorijaID, k.Naziv",conn);
            cmd.Parameters.AddWithValue("@Izlozba", comboBoxizlozba1.SelectedValue);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                chart1.DataSource = dt;
                chart1.Series[0].XValueMember = "Naziv Kategorije";
                chart1.Series[0].YValueMembers = "Broj pasa";
                chart1.Series[0].IsValueShownAsLabel = true;

            }
            catch (Exception)
            {

                MessageBox.Show("doslo je do greske");
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
            }
            SqlCommand cmd1 = new SqlCommand("select count(*) from Rezultat where IzlozbaID=@Izlozba", conn);
            SqlCommand cmd2 = new SqlCommand("select count(*) from Rezultat where IzlozbaID=@Izlozba and LEN(Napomena)>0", conn);
            cmd1.Parameters.AddWithValue("@Izlozba", comboBoxizlozba1.SelectedValue);
            cmd2.Parameters.AddWithValue("@Izlozba", comboBoxizlozba1.SelectedValue);
            try
            {
                conn.Open();
                int prijavljeno = (Int32)cmd1.ExecuteScalar();
                int takmicilo = (Int32)cmd2.ExecuteScalar();
                labelUkupnoPrijavljeno.Text = "Ukupan broj pasa koji je prijavljen " + prijavljeno.ToString();
                labelUkupnoTakmicilo.Text = "Ukupan broj pasa koji se takmicio " + takmicilo.ToString();
            }
            catch (Exception)
            {


                MessageBox.Show("doslo je do greske");
            }
            finally
            {
                conn.Close();
            }


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 3)
                this.Close();
        }
    }
}
