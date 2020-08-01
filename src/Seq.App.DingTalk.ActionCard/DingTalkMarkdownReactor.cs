using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.DingTalk.ActionCard
{
    [SeqApp("DingTalk ActionCard Notifier", Description = "Uses a Handlebars template to send events as DingTalk For ActionCard.")]
    public class DingTalkActionCardReactor : DingTalkBaseReactor
    {
        readonly Lazy<Func<object, string>> _titleTemplate, _textTemplate, _actionTitleTemplate, _actionURLTemplate;
        const string DefaultTextTemplate = @"[{{$Level}}] {{{$Message}}}";
        public DingTalkActionCardReactor()
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
                    template = DefaultTextTemplate;
                //return o => Content;
                return Handlebars.Compile(template);
            });
            _actionTitleTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = ActionTitle;
                if (string.IsNullOrEmpty(template))
                    return o => ActionTitle;
                return Handlebars.Compile(template);
            });
            _actionURLTemplate = new Lazy<Func<object, string>>(() =>
            {
                var template = ActionURL;
                if (string.IsNullOrEmpty(template))
                    return o => ActionURL;
                return Handlebars.Compile(template);
            });
        }
        [SeqAppSetting(DisplayName = "Title", IsOptional = false)]
        public string Title { get; set; }

        [SeqAppSetting(DisplayName = "Text template", IsOptional = false, InputType = SettingInputType.LongText, HelpText = "The template to use when generating the DingTalk Text, using Handlebars.NET syntax.")]
        public string Text { get; set; } = DefaultTextTemplate;


        [SeqAppSetting(DisplayName = "Action Title", IsOptional = false)]
        public string ActionTitle { get; set; }
        [SeqAppSetting(DisplayName = "Action URL", IsOptional = false)]
        public string ActionURL { get; set; }

        [SeqAppSetting(DisplayName = "Btn Orientation", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? BtnOrientation { get; set; }
        [SeqAppSetting(DisplayName = "Hide Avatar", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? HideAvatar { get; set; }

        [SeqAppSetting(DisplayName = "Debug Log", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? DebugLog { get; set; }

        public override async Task OnAsync(Event<LogEventData> evt)
        {
            var title = FormatTemplate(_titleTemplate.Value, evt, base.App, base.Host);
            var text = FormatTemplate(_textTemplate.Value, evt, base.App, base.Host);
            var actionTitle = FormatTemplate(_actionTitleTemplate.Value, evt, base.App, base.Host);
            var actionURL = FormatTemplate(_actionURLTemplate.Value, evt, base.App, base.Host);

            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};Event:{@LogEventData};Title:{Title};Text:{Text};ActionTitle:{ActionTitle};ActionURL:{ActionURL};BtnOrientation:{BtnOrientation};HideAvatar:{HideAvatar}", App.Title, evt, title, text, actionTitle, actionURL, this.BtnOrientation, HideAvatar);
            using (var client = GetWebhookClient())
            {
                if (client != null)
                    await client.SendActionCard(title, text, new[] { new DingtalkChatbot.ActionCardBtn(actionTitle, actionURL) }, this.BtnOrientation, this.HideAvatar);
            }
        }
    }
}
