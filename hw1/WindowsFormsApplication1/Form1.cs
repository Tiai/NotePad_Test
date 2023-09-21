using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool Status = true;

        /* 開啟記事本時執行 */
        private void Openf(object sender, EventArgs e)
        {
            Clipboard.Clear(); //清楚剪貼板
            richTextBox1.ContextMenuStrip = contextMenuStrip1; //新建ContextMenuStrip
        }

        private RichTextBox GetRichTextBox1()
        {
            return richTextBox1;
        }

        /* 若有記事本內容有更改即把儲存狀態設為未儲存 */
        private void UnSave(object sender, EventArgs e)
        {
            Status = false;
        }

        /* timer每變更一次即執行一次，表動態顯示現在時間，顯示在狀態列 */
        private void Timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "目前時間 : " + DateTime.Now.ToString();
        }

        /* 若原為空則把文件檔名改為新文字文件，若不為空則讀取使用者選擇決定是否存檔在清空文件內容 */
        private void 新增NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                richTextBox1.Clear();
            }

            else
            {
                DialogResult dr = MessageBox.Show("是否儲存此文字文件?", "新檔提醒", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text, Encoding.Default);
                        richTextBox1.Clear();
                    }
                }
                else if(dr == DialogResult.No)
                {
                    richTextBox1.Clear();
                }
            }

            Text = "新文字文件";
        }

        /* 未儲存狀態時，呼叫開啟新檔事件儲存並清空，在讀取所要文件 */
        private void 開啟OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Status == false)
            {
                新增NToolStripMenuItem_Click(sender, e);
                Status = true;
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Clear();
                richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName, Encoding.Default);
                Text = Path.GetFileName(openFileDialog1.FileName);
                Status = true;
            }
        }

        /* 若要儲存文件為之前所修改的原文件，則直接儲存不詢問，否則同另存 */
        private void 儲存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName == "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text, Encoding.Default);
                    Text = Path.GetFileName(saveFileDialog1.FileName);
                }
            }
            else
            {
                File.WriteAllText(openFileDialog1.FileName, richTextBox1.Text, Encoding.Default);
            }

            Status = true;
        }

        /* 跳出儲存視窗詢問是否儲存 */
        private void 另存新檔AToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text, Encoding.Default);
                Text = Path.GetFileName(saveFileDialog1.FileName);
                Status = true;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics gr = e.Graphics; //建立grapgics物件
            SolidBrush sb = new SolidBrush(Color.Black); //建立筆刷物件
            Font f = new Font("新細明體", 12); //建立字形物件
            gr.DrawString(richTextBox1.Text, f, sb, 10, 10); //呼叫drawstring方法
        }

        private void 列印PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void 預覽列印VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        private void 結束XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /* 若目前為未儲存狀態則若要儲存則儲存完再關閉 */
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Status == false)
            {
                DialogResult dr = MessageBox.Show("是否儲存此文字文件?", "關閉儲存提醒", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    if (openFileDialog1.FileName == "")
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllText(saveFileDialog1.FileName, richTextBox1.Text, Encoding.Default);
                            Text = Path.GetFileName(saveFileDialog1.FileName);
                        }
                    }
                    else
                    {
                        File.WriteAllText(openFileDialog1.FileName, richTextBox1.Text, Encoding.Default);
                    }
                }
            }
        }

        /* new出版面設定相關物件 */
        private void 版面設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog pgs = new PageSetupDialog();
            pgs.PageSettings = new System.Drawing.Printing.PageSettings();//紙張設定
            pgs.PrinterSettings = new System.Drawing.Printing.PrinterSettings();//列印相關設定
            pgs.ShowNetwork = false;

            pgs.ShowDialog(); //最後在show出
        }

        /* 若有復原，重做，剪下，複製，貼上，刪除造成文件有所修改的指令則把狀態改為未儲存 */
        private void 復原UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
            Status = false;
        }

        private void 取消復原RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
            Status = false;
        }

        private void 剪下TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
            Status = false;
        }

        private void 複製CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
            Status = false;
        }

        private void 貼上PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
            Status = false;
        }

        /* 刪除所選取的字串從起始到整個字串長 */
        private void 刪除LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart, richTextBox1.SelectionLength);
            }
            Status = false;
        }

        /* 每次要使用編輯時確認目前編輯裡的事件是否可以執行 */
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.ToString() == "編輯(&E)") //若按編輯主功能項目
            {
                if (richTextBox1.SelectionLength > 0) //有選取字串才能剪下，複製，刪除
                {
                    剪下TToolStripMenuItem.Enabled = true;
                    複製CToolStripMenuItem.Enabled = true;
                    刪除LToolStripMenuItem.Enabled = true;
                }
                else
                {
                    剪下TToolStripMenuItem.Enabled = false;
                    複製CToolStripMenuItem.Enabled = false;
                    刪除LToolStripMenuItem.Enabled = false;

                }
                if (Clipboard.GetText() == "") //剪貼簿為空則不能貼上
                {
                    貼上PToolStripMenuItem.Enabled = false;
                }
                else
                {
                    貼上PToolStripMenuItem.Enabled = true;
                }

                if(richTextBox1.Text == "")  //文件沒內容則不能全選，搜尋
                {
                    全選AToolStripMenuItem.Enabled = false;
                    搜尋FToolStripMenuItem.Enabled = false;
                }
                else
                {
                    全選AToolStripMenuItem.Enabled = true;
                    搜尋FToolStripMenuItem.Enabled = true;
                }

                if (richTextBox1.CanUndo == true)
                {
                    復原UToolStripMenuItem.Enabled = true;
                }
                else
                {
                    復原UToolStripMenuItem.Enabled = false;
                }

                if (richTextBox1.CanRedo == true)
                {
                    取消復原RToolStripMenuItem.Enabled = true;
                }
                else
                {
                    取消復原RToolStripMenuItem.Enabled = false;
                }
            }
        }

        /* 每次右鍵確認目前右鍵事件是否可以執行 */
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                剪下ToolStripMenuItem.Enabled = true;
                複製ToolStripMenuItem.Enabled = true;
                刪除LToolStripMenuItem1.Enabled = true;
            }
            else
            {
                剪下ToolStripMenuItem.Enabled = false;
                複製ToolStripMenuItem.Enabled = false;
                刪除LToolStripMenuItem1.Enabled = false;

            }
            if (Clipboard.GetText() == "")
            {
                貼上ToolStripMenuItem.Enabled = false;
            }
            else
            {
                貼上ToolStripMenuItem.Enabled = true;
            }

            if(richTextBox1.Text == "")
            {
                全選AToolStripMenuItem1.Enabled = false;
            }
            else
            {
                全選AToolStripMenuItem1.Enabled = true;
            }

            if (richTextBox1.CanUndo == true)
            {
                復原ToolStripMenuItem.Enabled = true;
            }
            else
            {
                復原ToolStripMenuItem.Enabled = false;
            }

            if (richTextBox1.CanRedo == true)
            {
                取消復原ToolStripMenuItem.Enabled = true;
            }
            else
            {
                取消復原ToolStripMenuItem.Enabled = false;
            }
        }

        private void 全選AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        /* 若要使用搜尋則把panel(搜尋視窗)的Visible設為true */
        private void 搜尋FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            int p; //表搜尋起始位置
            if (richTextBox1.SelectionLength > 0) //檢查目前文件是否已存在搜尋的字串
            {
                p = richTextBox1.Text.IndexOf(textBox1.Text, richTextBox1.SelectionStart + 1); //若有則找下一個，但必須+1否則會同原字串
            }
            else
            {
                p = richTextBox1.Text.IndexOf(textBox1.Text, richTextBox1.SelectionStart);
            }
            if(p < 0) //找不到，回傳-1
            {
                MessageBox.Show("未搜尋到該字串!");
                DialogResult dr = MessageBox.Show("是否移動到文章起始重新搜尋?", "搜尋提醒", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    richTextBox1.SelectionStart = richTextBox1.GetFirstCharIndexFromLine(0); //回到第一行
                    richTextBox1.SelectionLength = 0;
                    richTextBox1.Focus();
                }
            }
            else
            {
                richTextBox1.SelectionStart = p; //起始位置為找到的位置
                richTextBox1.SelectionLength = textBox1.TextLength; //選取長度為所搜尋字串
                richTextBox1.Select(); //選取文件中所找到字串
            }
        }

        /* 把所搜尋字串取代成取代textbox所輸入的text */
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = textBox2.Text;
        }

        /* 取消則把Visible設為false */
        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        /* 控制移至只能輸入數字 */
        private void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (自動換行WToolStripMenuItem.Checked == false && textBox3.Text!="" 
                && Convert.ToInt32(textBox3.Text) <= richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart)+1 
                && Convert.ToInt32(textBox3.Text) > 0) //若沒有自動換行才能使用移至功能，且若沒有超過文件行數且不為0行才能移至
            {
                int p = Convert.ToInt32(textBox3.Text) - 1;
                richTextBox1.SelectionStart = richTextBox1.GetFirstCharIndexFromLine(p); //將游標所指起始改為所要移至的位置
                richTextBox1.SelectionLength = 0;
                richTextBox1.Focus();
            }
            else if(自動換行WToolStripMenuItem.Checked == true)
            {
                MessageBox.Show("請取消自動換行!");
            }
            else if(Convert.ToInt32(textBox3.Text) > richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) + 1)
            {
                MessageBox.Show("輸入行數超過文件行數!");
            }
            else if (Convert.ToInt32(textBox3.Text) <= 0)
            {
                MessageBox.Show("行數為1開始輸入!");
            }
        }

        int mx, my;

        /* 先紀錄移動panel目前的x,y座標 */
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mx = e.X;
            my = e.Y;
        }

        /* 有移動則紀錄移動距離並修改panel座標 */
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                panel1.Left += e.X - mx;
                panel1.Top += e.Y - my;
            }
        }

        private void 時間日期ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text.Insert(richTextBox1.Text.Length, System.DateTime.Now.ToString());//插入到目前文件最尾端
            richTextBox1.SelectionStart = richTextBox1.Text.Length;//移動指標
            richTextBox1.Focus();
            richTextBox1.ScrollToCaret();
            Status = false;
        }

        /* 若有選取則修改選取部分，否則修改整份文件 */
        private void 整份文件AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.Font = fontDialog1.Font;
            Status = false;
        }

        private void 部份文件SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.SelectionFont = fontDialog1.Font;
            Status = false;
        }

        private void 整份文件AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            richTextBox1.ForeColor = colorDialog1.Color;
            Status = false;
        }

        private void 部份文件SToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            richTextBox1.SelectionColor = colorDialog1.Color;
            Status = false;
        }

        private void 自動換行WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (自動換行WToolStripMenuItem.Checked != true)
            {
                自動換行WToolStripMenuItem.Checked = false;
                richTextBox1.WordWrap = false;
            }
            else
            {
                自動換行WToolStripMenuItem.Checked = true;
                richTextBox1.WordWrap = true;
                ShowrRowCol(sender, e);
            }
        }

        private void 編輯EToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 檢視ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /* 每次點選，文件內容改變便執行事件 */
        private void ShowrRowCol(object sender, EventArgs e)
        {
            if (狀態列ToolStripMenuItem.Checked == true) //有選取才執行事件
            {
                statusStrip1.Visible = true;
                toolStripStatusLabel2.Alignment = ToolStripItemAlignment.Right; //將顯示行列靠右，區分時間，方便閱讀
                int row = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart); //起始值為目前位置
                int col;
                int start = 0; //每列的行起始
                int cursor = richTextBox1.SelectionStart; //目前指標位置
                while (start < cursor) //當start跟cursor不同行
                {
                    if (richTextBox1.GetLineFromCharIndex(start) == row)
                    {
                        break;
                    }
                    else
                    {
                        start++;
                    }
                }
                col = cursor - start;
                row++;//因為從0開始計算，習慣為從1開始
                col++;
                toolStripStatusLabel2.Text = "目前位置 : " + "第 " + row.ToString() + " 列" + "，" + "第 " + col.ToString() + " 行";
            }

            else
            {
                statusStrip1.Visible = false;
            }
        }
    }
}
