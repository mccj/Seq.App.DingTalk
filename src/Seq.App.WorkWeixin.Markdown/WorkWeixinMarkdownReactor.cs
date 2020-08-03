using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.WorkWeixin.Markdown
{
    [SeqApp("WorkWeixin Markdown Notifier", Description = "Uses a Handlebars template to send events as WorkWeixin For Markdown.")]
    public class WorkWeixinMarkdownReactor : WorkWeixinBaseReactor
    {
        readonly Lazy<Func<object, string>> _textTemplate;
        const string DefaultTextTemplate = @"[{{$Level}}] {{{$Message}}}";
        public WorkWeixinMarkdownReactor()
        {
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

        [SeqAppSetting(DisplayName = "Text template", IsOptional = false, InputType = SettingInputType.LongText, HelpText = "The template to use when generating the WorkWeixin Text, using Handlebars.NET syntax.")]
        public string Text { get; set; } = DefaultTextTemplate;

        [SeqAppSetting(DisplayName = "Debug Log", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? DebugLog { get; set; }

        public override async Task OnAsync(Event<LogEventData> evt)
        {
            var text = FormatTemplate(_textTemplate.Value, evt, base.App, base.Host);

            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};Event:{@LogEventData};Text:{Text}", App.Title, evt, text);
            using (var client = GetWebhookClient())
            {
                if (client != null)
                    await client.SendMarkdown(text);
            }
        }
    }
}
