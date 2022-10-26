using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeerPark_StatusCheck
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Call Api
            string login_status = Database_Connect();

            //if login_status != True then send mail
            if (login_status != "normal")
            {
                Send_Mail();
            }

            Application.Exit();
        }

        /// <summary>
        /// 未用
        /// </summary>
        /// <returns></returns>
        private static string call_api()
        {
            string json = api_call.CallPostController("UserLogin", "{\"userID\": \"4046601\",\"userPassword\": \"4046601\"}");

            JObject JObjectData = JObject.Parse(json);

            //DataTable dt_temp = (DataTable)JsonConvert.DeserializeObject(JObjectData["result"]["result"].ToString(), (typeof(DataTable)));
            string login_status = JObjectData["result"]["success"].ToString();

            return login_status;
        }

        private static string Database_Connect()
        {
            string databaseInfo = ConfigurationManager.ConnectionStrings["db_conn"].ConnectionString;

            string str_sql = "SELECT TOP(1) * FROM tb_new";


            using (SqlConnection connection1 = new SqlConnection(databaseInfo))
            {
                try
                {
                    connection1.Open();
                    using (SqlCommand command1 = new SqlCommand(str_sql, connection1))
                    {
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                connection1.Close();
                                return "normal";
                            }
                            else
                            {
                                connection1.Close();
                                return "error";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    connection1.Close();
                    return "error";
                }
            }
        }

        private static void Send_Mail()
        {
            try
            {
                System.Net.Mail.SmtpClient MySmtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                //設定你的帳號密碼
                MySmtp.Credentials = new System.Net.NetworkCredential("itmc.srv@gmail.com", "ogrevjrrzxxozzio");

                MySmtp.EnableSsl = true;

                MailMessage mms = new MailMessage();

                // 指定寄信人 
                mms.From = new MailAddress("itmc.srv@gmail.com", "台灣鹿園網");

                // 新增收件人
                mms.To.Add("itmc.srv@gmail.com");
                mms.To.Add("Kira.Lin@itmc.com.tw");
                mms.To.Add("Andy.yu@itmc.com.tw");
                mms.To.Add("trigger@applet.ifttt.com");

                // 設定郵件屬性，在此設定為「高」
                mms.Priority = MailPriority.High;

                // 設定主旨
                mms.Subject = "台灣鹿園網伺服器異常";

                mms.Body = "<p style='font-size:18px;'>台灣鹿園網伺服器異常請確認!!</p>";

                // 設定信件內容是否為 Html 格式
                mms.IsBodyHtml = true;

                // 設定 DeliveryMethod 的傳送信件方法
                MySmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                // 送出郵件 
                MySmtp.Send(mms);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
