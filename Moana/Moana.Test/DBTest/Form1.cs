using DBTest.DatabaseEngine;
using DBTest.Models;
using System;
using System.Windows.Forms;

namespace DBTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (Muse db = new Muse())
            {
                Pigs pig = new Pigs()
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = 10,
                    Name = "Tom"
                };
                db.Add(pig);
            }
        }
    }
}
