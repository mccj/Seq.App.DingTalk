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
            //   var s1 = await client.SendText(@"广州今日天气：29度，大部分多云，降雨概率：60%", mentionedMobileList: new[] { "18868602880" });
            //   var s2 = await client.SendMarkdown(@"实时新增用户反馈<font color=""warning"">132例</font>，请相关同事注意。
            //>类型:<font color=""comment"">用户反馈</font>
            //>普通用户反馈:<font color=""comment"">117例</font>
            //>VIP用户反馈:<font color=""comment"">15例</font>");
            //   var s3 = await client.SendImage(System.Drawing.Image.FromFile(@"C:\Users\mccj\source\repos\Seq.App.DingTalk\图标\故障.fw.png"));
            //var s4 = await client.SendNews(new[] {
            //    new Article("中秋节礼品领取", "今年中秋节公司有豪礼相送", "http://www.qq.com", "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png"),
            //    new Article("中秋节礼品领取2", "今年中秋节公司有豪礼相送", "http://www.qq.com", "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png"),
            //    new Article("中秋节礼品领取3", "今年中秋节公司有豪礼相送", "http://www.qq.com", "http://res.mail.qq.com/node/ww/wwopenmng/images/independent/doc/test_pic_msg1.png")
            //});

            var s5 = await client.UploadMedia(System.IO.File.ReadAllBytes(@"C:\Users\mccj\source\repos\Seq.App.DingTalk\图标\故障.fw.png"), "eeeee.jpg");
            var s6 = await client.SendFile(s5.MediaId);
        }
    }
}
