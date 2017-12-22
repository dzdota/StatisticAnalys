using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testgistogr
{
    class SecThread
    {
        public void DataGridViewWriteData()
        {
            dataGridView.Rows.Add(mainlist.Count);
            for (int i = 0; i < mainlist.Count; i++)
                dataGridView[0, i].Value = mainlist[i];
        }
    }
}
