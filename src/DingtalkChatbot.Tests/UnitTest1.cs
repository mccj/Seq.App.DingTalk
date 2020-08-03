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
            var webhookUrl = "https://oapi.dingtalk.com/robot/send?access_token=2ca5bc8fda42bb8f3bd809ea59e5c4526c38aab5559f4020925b20c2f17dd636";
            var secret = "SECe2e84a572a9882b2786319b17bdc55989314ac4533eeee8f7e3338d0a29ed06a";
            var client = new DingtalkChatbot.WebhookClient(webhookUrl,secret);
            // Text��Ϣ@������
            //var r1 = await client.SendText("�Ҿ���С����С�������ң�", isAtAll: true);
            // image������Ϣ
            //var r2 = await client.SendImage("https://www.baidu.com/img/PCfb_5bf082d29588c07f842ccde3f97243ea.png");
            // Link��Ϣ
            //var r3 = await client.SendLink(title: "����û�뵽����С贾�Ȼ...", text: "�����������ӵ�...", picUrl: "https://www.baidu.com/img/flexible/logo/pc/result.png", messageUrl: "https://www.baidu.com/");
            // Markdown��Ϣ@������
            //var r4 = await client.SendMarkdown(title: "��������", text: "#### ��������\n" +
            //    "> 9�ȣ�������1����������89������¶�73%\n\n" +
            //    "> ![����](http://www.sinaimg.cn/dy/slidenews/5_img/2013_28/453_28488_469248.jpg)\n" +
            //    "> ###### 10��20�ַ��� [����](https://www.seniverse.com/) \n",
            //    isAtAll: false);
            // Markdown��Ϣ@ָ���û�
            //var r5 = await client.SendMarkdown(title: "��������", text: "#### �������� @18868602880\n" +
            //    "> 9�ȣ�������1����������89������¶�73%\n\n" +
            //    "> ![����](http://www.sinaimg.cn/dy/slidenews/5_img/2013_28/453_28488_469248.jpg)\n" +
            //    "> ###### 10��20�ַ��� [����](https://www.seniverse.com/) \n",
            //    atMobiles: new[] { "18868602880" });
            // FeedCard��Ϣ����
            //var r6 = await client.SendFeedCard(new[] {
            //    new  DingtalkChatbot.FeedCardLinkMsg(title : "������Ů", messageURL : "https://www.dingtalk.com/", picURL: "https://www.baidu.com/img/flexible/logo/pc/result.png"),
            //    new  DingtalkChatbot.FeedCardLinkMsg(title : "������Ů", messageURL : "https://www.dingtalk.com/", picURL : "https://www.baidu.com/img/flexible/logo/pc/result.png"),
            //    new  DingtalkChatbot.FeedCardLinkMsg(title : "������Ů", messageURL : "https://www.dingtalk.com/", picURL : "https://www.baidu.com/img/flexible/logo/pc/result.png")
            //});
            // ActionCard������ת��Ϣ����
            var r7 = await client.SendActionCard("����û�뵽����Ȼ...",
                "![ѡ��](http://www.songshan.es/wp-content/uploads/2016/01/Yin-Yang.png) \n### �����������ӵ�...",
                new[] { new DingtalkChatbot.ActionCardBtn("�鿴����", "https://www.dingtalk.com/") },
                true, true);
            // ActionCard������ת��Ϣ���ͣ�˫ѡ�
            var r8 = await client.SendActionCard("����û�뵽����Ȼ...",
               "![ѡ��](http://www.songshan.es/wp-content/uploads/2016/01/Yin-Yang.png) \n### �����������ӵ�...",
               new[] {
                   new DingtalkChatbot.ActionCardBtn("֧��", "https://www.dingtalk.com/"),
                   new DingtalkChatbot.ActionCardBtn("����", "https://www.dingtalk.com/")
               },
               true, true);
            // ActionCard������ת��Ϣ���ͣ��б�ѡ�        
            var r9 = await client.SendActionCard("����û�뵽����Ȼ...",
               "![ѡ��](http://www.songshan.es/wp-content/uploads/2016/01/Yin-Yang.png) \n### �����������ӵ�...",
               new[] {
                   new DingtalkChatbot.ActionCardBtn("֧��", "https://www.dingtalk.com/"),
                   new DingtalkChatbot.ActionCardBtn("����", "https://www.dingtalk.com/"),
                   new DingtalkChatbot.ActionCardBtn("����", "https://www.dingtalk.com/")
               },
               true, true);
        }
    }
}
