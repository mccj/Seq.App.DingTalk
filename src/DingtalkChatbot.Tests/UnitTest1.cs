using System;
using Xunit;

namespace Seq.App.DingTalk.Tests
{
    public class UnitTest1
    {

        [Fact]
        public async void Test1()
        {
            //var webhookUrl = "https://oapi.dingtalk.com/robot/send?access_token=18af14d937f5e97885ecbb96239edd825d421b8d62429dab0477584c86b34ab1";
            var webhookUrl = "https://oapi.dingtalk.com/robot/send?access_token=4ae804ea926fde280482f119069e4e47cdeedb7788fa08ab1e8daa4ea8e26ec1";
            var secret = "SECaed0a0b2bec78747bbac5d679c31ff98f226c3d1eba30c4d5a86dab7b3e2174e";
            var client = new DingtalkChatbot.WebhookClient(webhookUrl,secret);
            // Text消息@所有人
            //var r1 = await client.SendText("我就是小丁，小丁就是我！", isAtAll: true);
            // image表情消息
            //var r2 = await client.SendImage("https://www.baidu.com/img/PCfb_5bf082d29588c07f842ccde3f97243ea.png");
            // Link消息
            //var r3 = await client.SendLink(title: "万万没想到，李小璐竟然...", text: "故事是这样子的...", picUrl: "https://www.baidu.com/img/flexible/logo/pc/result.png", messageUrl: "https://www.baidu.com/");
            // Markdown消息@所有人
            //var r4 = await client.SendMarkdown(title: "氧气文字", text: "#### 广州天气\n" +
            //    "> 9度，西北风1级，空气良89，相对温度73%\n\n" +
            //    "> ![美景](http://www.sinaimg.cn/dy/slidenews/5_img/2013_28/453_28488_469248.jpg)\n" +
            //    "> ###### 10点20分发布 [天气](https://www.seniverse.com/) \n",
            //    isAtAll: false);
            // Markdown消息@指定用户
            //var r5 = await client.SendMarkdown(title: "氧气文字", text: "#### 广州天气 @18868602880\n" +
            //    "> 9度，西北风1级，空气良89，相对温度73%\n\n" +
            //    "> ![美景](http://www.sinaimg.cn/dy/slidenews/5_img/2013_28/453_28488_469248.jpg)\n" +
            //    "> ###### 10点20分发布 [天气](https://www.seniverse.com/) \n",
            //    atMobiles: new[] { "18868602880" });
            // FeedCard消息类型
            //var r6 = await client.SendFeedCard(new[] {
            //    new  DingtalkChatbot.FeedCardLinkMsg(title : "氧气美女", messageURL : "https://www.dingtalk.com/", picURL: "https://www.baidu.com/img/flexible/logo/pc/result.png"),
            //    new  DingtalkChatbot.FeedCardLinkMsg(title : "氧眼美女", messageURL : "https://www.dingtalk.com/", picURL : "https://www.baidu.com/img/flexible/logo/pc/result.png"),
            //    new  DingtalkChatbot.FeedCardLinkMsg(title : "氧神美女", messageURL : "https://www.dingtalk.com/", picURL : "https://www.baidu.com/img/flexible/logo/pc/result.png")
            //});
            // ActionCard整体跳转消息类型
            var r7 = await client.SendActionCard("万万没想到，竟然...",
                "![选择](http://www.songshan.es/wp-content/uploads/2016/01/Yin-Yang.png) \n### 故事是这样子的...",
                new[] { new DingtalkChatbot.ActionCardBtn("查看详情", "https://www.dingtalk.com/") },
                true, true);
            // ActionCard独立跳转消息类型（双选项）
            var r8 = await client.SendActionCard("万万没想到，竟然...",
               "![选择](http://www.songshan.es/wp-content/uploads/2016/01/Yin-Yang.png) \n### 故事是这样子的...",
               new[] {
                   new DingtalkChatbot.ActionCardBtn("支持", "https://www.dingtalk.com/"),
                   new DingtalkChatbot.ActionCardBtn("反对", "https://www.dingtalk.com/")
               },
               true, true);
            // ActionCard独立跳转消息类型（列表选项）        
            var r9 = await client.SendActionCard("万万没想到，竟然...",
               "![选择](http://www.songshan.es/wp-content/uploads/2016/01/Yin-Yang.png) \n### 故事是这样子的...",
               new[] {
                   new DingtalkChatbot.ActionCardBtn("支持", "https://www.dingtalk.com/"),
                   new DingtalkChatbot.ActionCardBtn("中立", "https://www.dingtalk.com/"),
                   new DingtalkChatbot.ActionCardBtn("反对", "https://www.dingtalk.com/")
               },
               true, true);
        }
    }
}
