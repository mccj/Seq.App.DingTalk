using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.DingTalk.Image
{
    [SeqApp("DingTalk Image Notifier", Description = "Uses a Handlebars template to send events as DingTalk For Image.")]
    public class DingTalkImageReactor : DingTalkBaseReactor
    {
        readonly Lazy<Func<object, string>> _picUrlTemplate;
        public DingTalkImageReactor()
        {
            _picUrlTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = PicUrl;
                if (string.IsNullOrEmpty(template))
                    //template = DefaultContentTemplate;
                    return o => PicUrl;
                return Handlebars.Compile(template);
            });
        }
        [SeqAppSetting(DisplayName = "Pic Url template", IsOptional = false, InputType = SettingInputType.Text, HelpText = "The template to use when generating the DingTalk Pic Url, using Handlebars.NET syntax.")]
        public string PicUrl { get; set; }

        [SeqAppSetting(DisplayName = "Debug Log", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? DebugLog { get; set; }

        public override async Task OnAsync(Event<LogEventData> evt)
        {
            var picUrl = FormatTemplate(_picUrlTemplate.Value, evt, base.App, base.Host);

            if (string.IsNullOrEmpty(picUrl))
                throw new ArgumentNullException(nameof(PicUrl));


            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};Event:{@LogEventData};PicUrl:{PicUrl}", App.Title, evt, picUrl);
            using (var client = GetWebhookClient())
            {
                if (client != null)
                    await client.SendImage(picUrl);
            }
        }
    }
}
