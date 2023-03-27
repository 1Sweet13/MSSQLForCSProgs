using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace MSSQLForCSProgs
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlConnection nrthwndConnection = null;

       
        public Form1()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString); 
            
            sqlConnection.Open();

            nrthwndConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwindDB"].ConnectionString);

            nrthwndConnection.Open();

            //-----------------------------------------------------

            SqlDataAdapter dataAdapter= new SqlDataAdapter("SELECT * FROM Products",nrthwndConnection);

            DataSet db = new DataSet();

            dataAdapter.Fill(db);

            dataGridView2.DataSource= db.Tables[0];


            //if (sqlConnection.State == ConnectionState.Open)  // если статус SQLConnection равен ОТКРЫКТО  то будет выведено подключение установлено
            //{
            //    MessageBox.Show("Подключение установлено"); 
            //}
        }

        private void INSERT_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand(
            //$"INSERT INTO [Students] (Name, Surname, Birthday) VALUES (N'{textBox1.Text}',N'{textBox2.Text}','{textBox3.Text}')",
            //sqlConnection); Простой способ 


            // Сложный способ
            $"INSERT INTO [Students] (Name, Surname, Birthday, Mesto_rozhdeniya, Phone, Email) VALUES (@Name, @Surname, @Birthday, @Mesto_rozhdeniya, @Phone, @Email)",sqlConnection);
            
            DateTime date= DateTime.Parse(textBox3.Text);

            command.Parameters.AddWithValue("Name", textBox1.Text);
            command.Parameters.AddWithValue("Surname", textBox2.Text);
            command.Parameters.AddWithValue("Birthday", $"{date.Month}/{date.Day}/{date.Year}");
            command.Parameters.AddWithValue("Mesto_rozhdeniya", textBox4.Text);
            command.Parameters.AddWithValue("Phone", textBox5.Text);
            command.Parameters.AddWithValue("Email", textBox6.Text);

            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter= new SqlDataAdapter(
               textBox7.Text,nrthwndConnection);

            DataSet dataSet= new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource= dataSet.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            SqlDataReader dataReader = null;

            string[] row = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName, QuantityPerUnit,UnitPrice FROM Products",
                    nrthwndConnection);

                dataReader = sqlCommand.ExecuteReader();

                ListViewItem item = null;

                while (dataReader.Read())
                { 
                    row = new string[0];
                {
                    item = new ListViewItem(new string[] { 
                        Convert.ToString(dataReader["ProductName"]),
                        Convert.ToString(dataReader["QuantityPerUnit"]), 
                        Convert.ToString(dataReader["UnitPrice"]) });

                      listView1.Items.Add(item);    
                }
                
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            
        }
     

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox8.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock <= 10";
                    break;

                case 1:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 10 AND UnitsInStock <= 50";
                    break;

                case 2:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 50";
                    break;
                case 3:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;

            }
        }

       

      
    }
}
