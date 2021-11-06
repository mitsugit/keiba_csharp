using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace keiba_c
{
    public partial class Form2 : Form
    {
        public AxJVDTLabLib.AxJVLink JVLink; 
        public int DownloadCount;
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // *****************
            // JVCancel
            // *****************
            JVLink.JVCancel();
            this.DialogResult = DialogResult.Cancel;
            this.Timer1.Stop();
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.ProgressBar1.Maximum = DownloadCount; // プログレスバー最大値
            this.ProgressBar1.Minimum = 0; // プログレスバー最小値
            this.ProgressBar1.Value = 0; // プログレスバー初期値
            this.Timer1.Interval = 10; // タイマー間隔
            this.Timer1.Start(); // タイマー始動
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //int Status; // 進捗度合い
            int Count; // JVStatusの戻り値
                       // ******************
                       // JVStatus呼出
                       // ******************
            Count = JVLink.JVStatus();
            // エラー判定
            if (Count < 0)
            {
                this.Timer1.Stop();
                MessageBox.Show("ダウンロード失敗。RC=" + Count.ToString());
                this.DialogResult = DialogResult.Abort;
                // 終了処理
                this.Close();
            }
            else
            {
                // プログレスバーを進める
                this.ProgressBar1.Value = Count;
                this.Label1.Text = Count + " / " + DownloadCount;
                // 全てのデータをダウンロードしたら終了
                if (Count == DownloadCount)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
