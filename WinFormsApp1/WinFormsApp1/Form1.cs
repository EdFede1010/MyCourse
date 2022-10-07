namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Button concatena = new Button();
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = textBox1.Text + textBox2.Text;
            label4.Text = "PI: " + Math.Round(Math.PI,2).ToString();
        }
    }
}