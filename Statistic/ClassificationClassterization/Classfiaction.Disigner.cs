using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace testgistogr
{
    partial class Classification
    {
        public TabPage ClassificationTabPage;
        private TabControl DataTabControl;
        private TabPage DataTabPage;
        private TabPage ResultTabPage;
        private DataGridView ResultDataGrid;

        private DataGridView DataGridView;
        private DataGridView NewDataDataGridView;
        private DataGridView pDataGridView;
        private DataGridView ResultAddDataGridView;
        private ComboBox TypeDComboBox;
        private GroupBox Rightgroupbox;
        private GroupBox NewDataAndPgroupbox;

        private Chart chart;
        public Series ClassterCenterSer = null;

        ContextMenuStrip ContextMenuStrip;
        private ToolStripMenuItem AddNewData;
        private ToolStripMenuItem ChangeIndex;



        private void InitializeComponent()
        {
            InitializeDataTabPage();
            InitializeResultTabPage();


            TypeDComboBox = new ComboBox()
            {
                Dock = DockStyle.Top,
                Size = new Size(100, 50),
            };

            for (int i = 0; i < Classter.dstruct.Length; i++)
                TypeDComboBox.Items.Add(Classter.dstruct[i].Name);
            TypeDComboBox.SelectedIndex = 0;

            Rightgroupbox = new GroupBox()
            {
                Dock = DockStyle.Right,
                Width = 100,
                Text = ""
            };
            Rightgroupbox.Controls.Add(TypeDComboBox);

            pDataGridView = new DataGridView()
            {
                AllowUserToAddRows = false,
                RowHeadersWidth = 4,
                ColumnHeadersHeight = 4,
                Dock = DockStyle.Right
            };

            NewDataDataGridView = new DataGridView()
            {
                AllowUserToAddRows = false,
                RowHeadersWidth = 4,
                ColumnHeadersHeight = 4,
                Dock = DockStyle.Left
            };

            NewDataAndPgroupbox = new GroupBox()
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Text = "Нове спостереження" +
                "                                    " +
                "                                    " +
                "                                    " +
                "                                    " +
                "                                    " +
                "                                    " +
                "Апріорні імовірності появи об’єктів з класу"
            };
            NewDataAndPgroupbox.Controls.Add(NewDataDataGridView);
            NewDataAndPgroupbox.Controls.Add(pDataGridView);


            DataTabControl = new TabControl() { Dock = DockStyle.Fill };
            DataTabControl.Controls.Add(DataTabPage);
            DataTabControl.Controls.Add(ResultTabPage);

            ClassificationTabPage = new TabPage()
            {
                UseVisualStyleBackColor = true,
                TabIndex = 0,
                Text = "Класифікація"
            };
            ClassificationTabPage.Controls.Add(Rightgroupbox);
            ClassificationTabPage.Controls.Add(DataTabControl);
            ClassificationTabPage.Controls.Add(NewDataAndPgroupbox);


            ContextMenuStrip = new ContextMenuStrip();
            NewDataAndPgroupbox.ContextMenuStrip = ContextMenuStrip;
            DataTabControl.ContextMenuStrip = ContextMenuStrip;
            NewDataAndPgroupbox.ContextMenuStrip = ContextMenuStrip;

            this.AddNewData = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "AddNewData",
                Size = new System.Drawing.Size(223, 22),
                Text = "Додати дані"
            };
            this.AddNewData.Click += new System.EventHandler(this.AddNewData_Click);

            this.ChangeIndex = new System.Windows.Forms.ToolStripMenuItem()
            {
                Name = "ChangeIndex",
                Size = new System.Drawing.Size(223, 22),
                Text = "Змінити індекс кластеризації"
            };
            this.ChangeIndex.Click += new System.EventHandler(this.ChangeIndex_Click);

            this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                AddNewData,
                ChangeIndex,
            });


        }


        private void InitializeResultTabPage()
        {

            ResultDataGrid = new DataGridView()
            {
                Dock = DockStyle.Top,
                AllowUserToAddRows = false,
            };

            ResultAddDataGridView = new DataGridView()
            {
                Dock = DockStyle.Bottom,
                AllowUserToAddRows = false,
            };

            ResultTabPage = new TabPage()
            {
                Text = "Результат"
            };
            ResultTabPage.Controls.Add(ResultAddDataGridView);
            ResultTabPage.Controls.Add(ResultDataGrid);
        }

        private void InitializeDataTabPage()
        {
            DataGridView = new DataGridView()
            {
                Location = new Point(0, 0),
                AllowUserToAddRows = false,
                RowHeadersWidth = 60,
                Width = 350,
            };
            if (n == 2)
                DataGridView.Dock = DockStyle.Left;
            else
                DataGridView.Dock = DockStyle.Fill;

            System.Windows.Forms.TabPage tabPagenew = new TabPage();

            if (n == 2)
            {
                chart = new Chart()
                {
                    Dock = DockStyle.Left,
                    Width = 550,
                    Name = "chart",
                    TabIndex = 0,
                };

                tabPagenew.Controls.Add(chart);
                this.chart.ChartAreas.Add(new ChartArea() { });
                this.chart.Legends.Add(new Legend("legend"){});

                ClassterCenterSer = new Series("Classter")
                {
                    ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point,
                    BorderWidth = 1,
                    Color = Color.Red,
                    BorderColor = Color.Black,
                    ["PointWidth"] = "1",
                };
            }

            tabPagenew.Location = new System.Drawing.Point(4, 22);
            tabPagenew.Name = "DataTabPage";
            tabPagenew.Padding = new System.Windows.Forms.Padding(3);
            tabPagenew.Size = new System.Drawing.Size(1005, 273);
            tabPagenew.TabIndex = 0;
            tabPagenew.Text = "Дані";
            tabPagenew.UseVisualStyleBackColor = true;
            tabPagenew.ResumeLayout(false);
            tabPagenew.Controls.Add(DataGridView);

            DataTabPage = tabPagenew;
        }
    }
}
