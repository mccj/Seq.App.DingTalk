using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.DingTalk
{
    public abstract class DingTalkBaseReactor : SeqApp, ISubscribeToAsync<LogEventData>
    {
        static DingTalkBaseReactor()
        {
            HandlebarsHelpers.Register();
        }

        [SeqAppSetting(DisplayName = "Webhook URL", IsOptional = true, InputType = SettingInputType.Text, HelpText = "Add the Incoming WebHooks app to your DingTalk to get this URL.")]
        public string WebhookUrl { get; set; }
        [SeqAppSetting(DisplayName = "Secret", IsOptional = true, InputType = SettingInputType.Password, HelpText = "On the robot security settings page, sign the string at the beginning of SEC displayed under the column")]
        public string Secret { get; set; }
        public abstract Task OnAsync(Event<LogEventData> evt);
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
        public string FormatTemplate(Func<object, string> template, Event<LogEventData> evt, Apps.App app, Host host)
        {
            var properties = (IDictionary<string, object>)ToDynamic(evt.Data.Properties ?? new Dictionary<string, object>());

            var payload = (IDictionary<string, object>)ToDynamic(new Dictionary<string, object>
            {
                { "$Id",                  evt.Id },
                { "$UtcTimestamp",        evt.TimestampUtc },
                { "$LocalTimestamp",      evt.Data.LocalTimestamp },
                { "$Level",               evt.Data.Level },
                { "$MessageTemplate",     evt.Data.MessageTemplate },
                { "$Message",             evt.Data.RenderedMessage },
                { "$Exception",           evt.Data.Exception },
                { "$Properties",          properties },
                { "$EventType",           "$" + evt.EventType.ToString("X8") },
                { "$Instance",            host.InstanceName },
                { "$ServerUri",           host.BaseUri },
                { "$AppTitle",           app.Title }
            });

            foreach (var property in properties)
            {
                payload[property.Key] = property.Value;
            }

            return template(payload);
        }
        object ToDynamic(object o)
        {
            if (o is IEnumerable<KeyValuePair<string, object>> dictionary)
            {
                var result = new ExpandoObject();
                var asDict = (IDictionary<string, object>)result;
                foreach (var kvp in dictionary)
                    asDict.Add(kvp.Key, ToDynamic(kvp.Value));
                return result;
            }

            if (o is IEnumerable<object> enumerable)
            {
                return enumerable.Select(ToDynamic).ToArray();
            }

            return o;
        }

    }
}
