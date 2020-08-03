using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.WorkWeixin
{
    public abstract class WorkWeixinBaseReactor : BaseReactor
    {
        [SeqAppSetting(DisplayName = "Webhook URL", IsOptional = true, InputType = SettingInputType.Text, HelpText = "Add the Incoming WebHooks app to your WorkWeixin to get this URL.")]
        public string WebhookUrl { get; set; }
        public abstract override Task OnAsync(Event<LogEventData> evt);
        public WorkWeixinWebHook.WebhookClient GetWebhookClient()
        {
            if (string.IsNullOrWhiteSpace(this.WebhookUrl))
            {
                Log.Error(new ArgumentNullException(nameof(WebhookUrl)), "WebhookUrl 未填,无法发生钉钉消息");
                return null;
            }
            var client = new WorkWeixinWebHook.WebhookClient(this.WebhookUrl);
            return client;
        }
    }
}
