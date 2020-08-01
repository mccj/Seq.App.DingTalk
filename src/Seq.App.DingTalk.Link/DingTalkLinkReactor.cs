using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.DingTalk.Link
{
    [SeqApp("DingTalk Link Notifier", Description = "Uses a Handlebars template to send events as DingTalk For Link.")]
    public class DingTalkLinkReactor : DingTalkBaseReactor
    {
        readonly Lazy<Func<object, string>> _titleTemplate, _contentTemplate, _picUrlTemplate, _messageUrlTemplate;
        const string DefaultContentTemplate = @"[{{$Level}}] {{{$Message}}}";
        public DingTalkLinkReactor()
        {
            _titleTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = Title;
                if (string.IsNullOrEmpty(template))
                    return o => Title;
                return Handlebars.Compile(template);
            });
            _contentTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = Content;
                if (string.IsNullOrEmpty(template))
                    template = DefaultContentTemplate;
                //return o => Content;
                return Handlebars.Compile(template);
            });
            _picUrlTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = PicUrl;
                if (string.IsNullOrEmpty(template))
                    return o => PicUrl;
                return Handlebars.Compile(template);
            });
            _messageUrlTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = MessageUrl;
                if (string.IsNullOrEmpty(template))
                    return o => MessageUrl;
                return Handlebars.Compile(template);
            });
        }
        [SeqAppSetting(DisplayName = "Title", IsOptional = false)]
        public string Title { get; set; }

        [SeqAppSetting(DisplayName = "Content template", IsOptional = false, InputType = SettingInputType.LongText, HelpText = "The template to use when generating the DingTalk Content, using Handlebars.NET syntax.")]
        public string Content { get; set; } = DefaultContentTemplate;
        [SeqAppSetting(DisplayName = "Pic Url", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public string PicUrl { get; set; }
        [SeqAppSetting(DisplayName = "Message Url", IsOptional = false, InputType = SettingInputType.LongText)]
        public string MessageUrl { get; set; }

        [SeqAppSetting(DisplayName = "Debug Log", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? DebugLog { get; set; }

        public override async Task OnAsync(Event<LogEventData> evt)
        {
            var title = FormatTemplate(_titleTemplate.Value, evt, base.App, base.Host);
            var content = FormatTemplate(_contentTemplate.Value, evt, base.App, base.Host);
            var picUrl = FormatTemplate(_picUrlTemplate.Value, evt, base.App, base.Host);
            var messageUrl = FormatTemplate(_messageUrlTemplate.Value, evt, base.App, base.Host);

            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};Event:{@LogEventData};Title:{Title};Content:{Content};PicUrl:{PicUrl};MessageUrl:{MessageUrl}", App.Title, evt, title, content, picUrl, messageUrl);
            using (var client = GetWebhookClient())
            {
                if (client != null)
                    await client.SendLink(title, content, picUrl, messageUrl);
            }
        }
    }
}
