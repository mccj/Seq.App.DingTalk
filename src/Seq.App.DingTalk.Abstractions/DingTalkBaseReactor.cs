using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Threading.Tasks;

namespace Seq.App.DingTalk
{
    public abstract class DingTalkBaseReactor : BaseReactor
    {
        [SeqAppSetting(DisplayName = "Webhook URL", IsOptional = true, InputType = SettingInputType.Text, HelpText = "Add the Incoming WebHooks app to your DingTalk to get this URL.")]
        public string WebhookUrl { get; set; }
        [SeqAppSetting(DisplayName = "Secret", IsOptional = true, InputType = SettingInputType.Password, HelpText = "On the robot security settings page, sign the string at the beginning of SEC displayed under the column")]
        public string Secret { get; set; }
        public abstract override Task OnAsync(Event<LogEventData> evt);
        public DingtalkChatbot.WebhookClient GetWebhookClient()
        {
            if (string.IsNullOrWhiteSpace(this.WebhookUrl))
            {
                Log.Error(new ArgumentNullException(nameof(WebhookUrl)), "WebhookUrl 未填,无法发生钉钉消息");
                return null;
            }
            var client = new DingtalkChatbot.WebhookClient(this.WebhookUrl, this.Secret);
            return client;
        }
    }
}
