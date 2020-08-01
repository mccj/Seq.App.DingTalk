using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.DingTalk.Markdown
{
    [SeqApp("DingTalk Markdown Notifier", Description = "Uses a Handlebars template to send events as DingTalk For Markdown.")]
    public class DingTalkMarkdownReactor : DingTalkBaseReactor
    {
        readonly Lazy<Func<object, string>> _titleTemplate, _textTemplate;
        const string DefaultTextTemplate = @"[{{$Level}}] {{{$Message}}}";
        public DingTalkMarkdownReactor()
        {
            _titleTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = Title;
                if (string.IsNullOrEmpty(template))
                    return o => Title;
                return Handlebars.Compile(template);
            });
            _textTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = Text;
                if (string.IsNullOrEmpty(template))
                    template = Properties.Resources.DefaultBodyTemplate;
                //template = DefaultTextTemplate;
                //return o => Content;
                return Handlebars.Compile(template);
            });
        }
        [SeqAppSetting(DisplayName = "Title", IsOptional = false)]
        public string Title { get; set; }

        [SeqAppSetting(DisplayName = "Text template", IsOptional = false, InputType = SettingInputType.LongText, HelpText = "The template to use when generating the DingTalk Text, using Handlebars.NET syntax.")]
        public string Text { get; set; } = DefaultTextTemplate;

        [SeqAppSetting(DisplayName = "AtAll", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? IsAtAll { get; set; }
        [SeqAppSetting(DisplayName = "AtMobiles", IsOptional = true, InputType = SettingInputType.LongText)]
        public string AtMobiles { get; set; }

        [SeqAppSetting(DisplayName = "Debug Log", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? DebugLog { get; set; }

        public override async Task OnAsync(Event<LogEventData> evt)
        {
            var title = FormatTemplate(_titleTemplate.Value, evt, base.App, base.Host);
            var text = FormatTemplate(_textTemplate.Value, evt, base.App, base.Host);

            var atMobiles = this.AtMobiles?.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)?.Where(f => !string.IsNullOrEmpty(f)).Select(f => f.Trim()).ToArray();
            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};Event:{@LogEventData};Title:{Title};Text:{Text};IsAtAll:{IsAtAll};AtMobiles:{AtMobiles}", App.Title, evt, title, text, this.IsAtAll, atMobiles);
            using (var client = GetWebhookClient())
            {
                if (client != null)
                    await client.SendMarkdown(title, text, this.IsAtAll, atMobiles);
            }
        }
    }
}
