namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Top = 10;
            button2.Top = 50;
            button3.Top = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Text = "uno";
            button2.Text = "due";
            button3.Text = "tre";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Text = "o";
            button1.Text = button1.Text + " => 1";
            button2.Text = button2.Text + " => 2";
            button3.Text = button3.Text + " => 3";

        }
    }
}