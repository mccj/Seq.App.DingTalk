using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.DingTalk.Text
{
    [SeqApp("DingTalk Text Notifier", Description = "Uses a Handlebars template to send events as DingTalk For Text.")]
    public class DingTalkTextReactor : DingTalkBaseReactor
    {
        readonly Lazy<Func<object, string>> _contentTemplate;
        const string DefaultContentTemplate = @"[{{$Level}}] {{{$Message}}}";
        public DingTalkTextReactor()
        {
            _contentTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = Content;
                if (string.IsNullOrEmpty(template))
                    template = DefaultContentTemplate;
                //return o => Content;
                return Handlebars.Compile(template);
            });
        }
        [SeqAppSetting(DisplayName = "Content template", IsOptional = true, InputType = SettingInputType.LongText, HelpText = "The template to use when generating the DingTalk Content, using Handlebars.NET syntax.")]
        public string Content { get; set; } = DefaultContentTemplate;
        [SeqAppSetting(DisplayName = "AtAll", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? IsAtAll { get; set; }
        [SeqAppSetting(DisplayName = "AtMobiles", IsOptional = true, InputType = SettingInputType.LongText)]
        public string AtMobiles { get; set; }

        [SeqAppSetting(DisplayName = "Debug Log", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? DebugLog { get; set; }

        public override async Task OnAsync(Event<LogEventData> evt)
        {
            //var sss1 = Newtonsoft.Json.JsonConvert.SerializeObject(this.App);
            //var sss2 = Newtonsoft.Json.JsonConvert.SerializeObject(this.Host);
            //var sss3 = Newtonsoft.Json.JsonConvert.SerializeObject(evt);
            //System.IO.File.WriteAllText("d:/aa1.txt", sss1);
            //System.IO.File.WriteAllText("d:/aa2.txt", sss2);
            //System.IO.File.WriteAllText("d:/aa3.txt", sss3);
            //await Task.Delay(0);

            var content = FormatTemplate(_contentTemplate.Value, evt, base.App, base.Host);

            var atMobiles = this.AtMobiles?.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)?.Where(f => !string.IsNullOrEmpty(f)).Select(f => f.Trim()).ToArray();
            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};Event:{@LogEventData};Content:{Content};IsAtAll:{IsAtAll};AtMobiles:{AtMobiles}", App.Title, evt, content, this.IsAtAll, atMobiles);
            using (var client = GetWebhookClient())
            {
                if (client != null)
                    await client.SendText(content, this.IsAtAll, atMobiles);
            }
        }
    }
}
