using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace testgistogr
{
    public class AdditionalInformationForm : Form
    {
        TextBox textBox;
        ComboBox combobox;
        Label label;

        public AdditionalInformationForm(string S)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Size = new Size(300, 150);
            this.Text = "Input box";

            /* Создаем текстовое поле. -> */

            textBox = new TextBox();
            textBox.Size = new Size(250, 25);
            textBox.Font = new Font(TextBox.DefaultFont, FontStyle.Regular);
            textBox.Location = new Point(20, 50);
            textBox.Text = "";

            this.Controls.Add(textBox);

            textBox.Show();

            textBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);

            /* Создаем текстовое поле. <- */

            /* Создаем метку. -> */

            label = new Label();
            label.AutoSize = false;
            label.Size = new Size(250, 25);
            label.Font = new Font(label.Font, FontStyle.Regular);
            label.Location = new Point(20, 25);
            label.Text = S;

            this.Controls.Add(label);

            label.Show();

            /* Создаем метку. <- */

            combobox = new ComboBox();
            combobox.AutoSize = false;
            combobox.Size = new Size(250, 25);
            combobox.Font = new Font(label.Font, FontStyle.Regular);
            combobox.Location = new Point(20, 50);

            this.Controls.Add(combobox);


            /* Создаем кнопку "OK". -> */

            Button buttonOK = new Button();
            buttonOK.Size = new Size(80, 25);
            buttonOK.Location = new Point(105, 75);
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Text = "OK";

            this.Controls.Add(buttonOK);

            buttonOK.Show();
            buttonOK.Focus();
            /* Создаем кнопку "OK". <- */

            /* Создаем кнопку "Cancel". -> */

            Button buttonCancel = new Button();
            buttonCancel.Size = new Size(80, 25);
            buttonCancel.Location = new Point(190, 75);
            buttonCancel.Text = "Cancel";

            this.Controls.Add(buttonCancel);

            buttonCancel.Show();
            buttonCancel.Click += new EventHandler(buttonCancel_Click);
            /* Создаем кнопку "OK". <- */
        }

        public void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
        public void buttonCancel_Click(object sander, EventArgs e)
        {
            this.Close();
        }
        public string getString()
        {
            combobox.Visible = false;
            textBox.Visible = true;
            if (this.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return null;
            return textBox.Text;
        }
        public string SelectType(string type)
        {
            combobox.Visible = true;
            combobox.Items.Clear();
            combobox.Items.AddRange(Regex.Split(type,"\n"));
            if (combobox.Items.Count > 0)
                combobox.SelectedIndex = 0;
            textBox.Visible = false;
            if (this.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return null;
            return combobox.Text;
        }
    }
}
