using System;
using Xunit;

namespace WorkWeixinWebHook.Test
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            var webhookUrl = "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=2d8f89cc-a941-4602-b508-4ec133d138c0";
            var client = new WebhookClient(webhookUrl);
            //   var s1 = await client.SendText(@"���ݽ���������29�ȣ��󲿷ֶ��ƣ�������ʣ�60%", mentionedMobileList: new[] { "18868602880" });
            //   var s2 = await client.SendMarkdown(@"ʵʱ�����û�����<font color=""warning"">132��</font>�������ͬ��ע�⡣
            //>����:<font color=""comment"">�û�����</font>
            //>��ͨ�û�����:<font color=""comment"">117��</font>
            //>VIP�û�����:<font color=""comment"">15��</font>");
            //   var s3 = await client.SendImage(System.Drawing.Image.FromFile(@"C:\Users\mccj\source\repos\Seq.App.DingTalk\ͼ��\����.fw.png"));
            //var s4 = await client.SendNews(new[] {
            //    new Article("�������Ʒ��ȡ", "��������ڹ�˾�к�������", "http://www.qq.com", "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png"),
            //    new Article("�������Ʒ��ȡ2", "��������ڹ�˾�к�������", "http://www.qq.com", "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png"),
            //    new Article("�������Ʒ��ȡ3", "��������ڹ�˾�к�������", "http://www.qq.com", "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png")
            //});

            var s5 = await client.UploadMedia(System.IO.File.ReadAllBytes(@"C:\Users\mccj\source\repos\Seq.App.DingTalk\ͼ��\����.fw.png"), "eeeee.jpg");
            var s6 = await client.SendFile(s5.MediaId);
        }
    }
}
