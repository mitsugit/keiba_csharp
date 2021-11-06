﻿using MySql.Data.MySqlClient;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // JVSetUIPropertiesの呼出し
            if (this.AxJVLink1.JVSetUIProperties() == -100)
            {
                // レジストリへの登録に失敗すると-100が返る
                MessageBox.Show("エラーのためＪＶ－Ｌｉｎｋの設定に失敗しました。");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int ReturnCode; // JVLinkからの戻り値
            string Data_Spec; // JVOpen データ種別
            string From_Time; // JVOpen From時刻
            int Option_Flag; // JVOpen オプション
            int ReadCount = 0; // データファイルの総読み込み数
            int DownloadCount = 0; // データファイルのダウンロード数
            string LastTime; // データファイルの最終時刻
            var DownloadDialog = new Form2(); // ダウンロードダイアログ

            // ************
            // JVInit処理
            // ************
            ReturnCode = this.AxJVLink1.JVInit("UNKNOWN");
            if (ReturnCode != 0L)
            {
                MessageBox.Show("JVInitエラー。RC=" + ReturnCode.ToString());
            }
            // ********************
            // JVOpen処理
            // ********************
            Data_Spec = "RACE"; // データ種別に「レース情報」を設定
            From_Time = "20030101000000"; // Fromタイムに2003年1月1日を設定
            Option_Flag = 2; // オプションに「今週データ」を設定
            ReturnCode = this.AxJVLink1.JVOpen(Data_Spec, From_Time, Option_Flag, ref ReadCount, ref DownloadCount, out LastTime);
            // JVOpenエラー処理
            if (ReturnCode < 0)
            {
                MessageBox.Show("JVOpenエラー。RC=" + ReturnCode.ToString());
                ReturnCode = this.AxJVLink1.JVClose();
                return;
            }
            // 該当データ無し
            if (ReadCount == 0)
            {
                MessageBox.Show("該当するデータがありません。");
                ReturnCode = this.AxJVLink1.JVClose();
                return;
            }

            if (DownloadCount > 0)
            {
                // *******************************
                // ダウンロードプログレスバー表示
                // *******************************
                //DownloadDialog = new Form2();
                // Form2からJVStatusを呼ぶためのJVLinkの参照をセット
                DownloadDialog.JVLink = this.AxJVLink1;
                DownloadDialog.DownloadCount = DownloadCount;
                DownloadDialog.ShowDialog();
                // ダウンロード処理がキャンセルかエラーで終了した場合
                if (DownloadDialog.DialogResult != DialogResult.OK)
                {
                    ReturnCode = this.AxJVLink1.JVClose();
                    return;
                }
            }
            // ********************
            // JVRead処理
            // ********************
            string RecordSpec; // JV-Dataレコード種別ID
            var Race = new JVData_Struct.JV_SE_RACE_UMA();// = default(JVData_Struct.JV_SE_RACE_UMA); // JV-Dataレース詳細レコード構造体
            int buffSize = 1500;
            string buff = new string('\0',buffSize);
            string fName;
            // バッファ領域確保
            //buff = new string(Conversions.ToChar(Constants.vbNullChar), buffSize);
            // 該当のデータ（レース情報）が1ファイル以上の場合に読み込み
            if (ReadCount > 0)
            {
                var flag = false;
                do
                {
                    ReturnCode = this.AxJVLink1.JVRead(out buff, out buffSize, out fName);
                    switch (ReturnCode)
                    {
                        case int n when n > 0: // 正常に1レコード読み込み
                            
                                // レコード共通ヘッダ構造体にデータをセット
                                RecordSpec = buff.Substring(0,2);
                                Console.WriteLine(buff);
                                if (RecordSpec == "SE")
                                {
                                    // レース詳細レコード構造体にデータをセット
                                    Race.SetDataB(ref buff);
                                    this.TextBox1.AppendText(Race.id.Year + Race.id.MonthDay + " " + Race.id.JyoCD + " " + Race.id.Kaiji + " " + Race.id.Nichiji + " " + Race.id.RaceNum + "\r\n");
                                
                                    // save(Race); //save関数に飛ばしてsql実行
                            }
                                else
                                {
                                    // 「レース詳細」以外は読み飛ばし
                                }

                                break;
                            

                        case -1: // ファイルの切れ目
                            
                                
                                break;
                            
                        // ファイルの切り替わり時にデータは返されない
                        case 0: // 全レコード読み込み終了(EOF)
                   
                                flag = true;
                                break; // 読み込みエラー
                            
                        case -3: // ダウンロード中
                            Console.WriteLine("ダウンロード中\n");
                            break;
                        case -201: // JVInit されてない
                            Console.WriteLine("JVInit が行われていません。");
                            flag = true;
                            break;
                        case -203: // JVOpen されてない
                            Console.WriteLine("JVOpen が行われていません。");
                            flag = true;
                            break;
                        case -503: // ファイルがない
                            Console.WriteLine(fName + "が存在しません。");
                            flag = true;
                            break;

                        default:
                            // 読み飛ばし
                            Console.WriteLine("読み飛ばし\n");
                            this.AxJVLink1.JVSkip();
                            break;
                    }
                }
                while (!flag);
            }



            // ***************
            // JVClose処理
            // ***************
            if (DownloadCount > 0)
            {
                DownloadDialog.Dispose();
                DownloadDialog = null;
            }

            ReturnCode = this.AxJVLink1.JVClose();
            MessageBox.Show("終了");
        }

        private void button3_Click(object sender, EventArgs e)
        {


            // MySQLへの接続情報
            string server = "localhost";
            string user = "root";
            string pass = "root";
            string database = "jra_csharp";
            string connectionString = string.Format("Server={0};Database={1};Uid={2};Pwd={3}", server, database, user, pass);

            // MySQLへの接続
            MySqlConnection cn = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
            MySqlCommand com = new MySqlCommand();
            com.Connection = cn;

            cn.Open();
            //com.Transaction = cn.BeginTransaction();

            try
            {
               
                
                    com.CommandText = @"INSERT INTO `jra_csharp`.`n_uma_race`
(`RecordSpec`,`DataKubun`,`MakeDate`,`Year`,`MonthDay`,`JyoCD`,`Kaiji`,`Nichiji`,`RaceNum`,`Wakuban`,`Umaban`,
`KettoNum`,`Bamei`,`Barei`,`KisyuCode`,`KisyuRyakusyo`,`Odds`,`Ninki`,`NyusenJyuni`,`KakuteiJyuni`)

VALUES
('RA', '6', '20210606', '2021', '0606', @JyoCD, '01', '01', '01', '1', '01',
'201815555', 'test', '04', '04', 'テスト', '0240', '01', '05', '05')";

                com.Parameters.Add(new MySqlParameter("JyoCD","07"));

                //結果を返さない
                com.ExecuteNonQuery();
                

                //com.Transaction.Commit();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //com.Transaction.Rollback();
            }
            finally
            {
                cn.Close();
            }
        }
    }
}
