using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.WorkWeixin.Text
{
    [SeqApp("WorkWeixin Text Notifier", Description = "Uses a Handlebars template to send events as WorkWeixin For Text.")]
    public class WorkWeixinTextReactor : WorkWeixinBaseReactor
    {
        readonly Lazy<Func<object, string>> _contentTemplate;
        const string DefaultContentTemplate = @"[{{$Level}}] {{{$Message}}}";
        public WorkWeixinTextReactor()
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
        [SeqAppSetting(DisplayName = "Content template", IsOptional = true, InputType = SettingInputType.LongText, HelpText = "The template to use when generating the WorkWeixin Content, using Handlebars.NET syntax.")]
        public string Content { get; set; } = DefaultContentTemplate;
        [SeqAppSetting(DisplayName = "MentionedMobileList", IsOptional = true, InputType = SettingInputType.LongText)]
        public string MentionedMobileList { get; set; }
        [SeqAppSetting(DisplayName = "MentionedList", IsOptional = true, InputType = SettingInputType.LongText)]
        public string MentionedList { get; set; }

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

            var mentionedMobileList = this.MentionedMobileList?.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)?.Where(f => !string.IsNullOrEmpty(f)).Select(f => f.Trim()).ToArray();
            var mentionedList = this.MentionedList?.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)?.Where(f => !string.IsNullOrEmpty(f)).Select(f => f.Trim()).ToArray();
            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};Event:{@LogEventData};Content:{Content};MentionedList:{MentionedList};MentionedMobileList:{MentionedMobileList}", App.Title, evt, content, mentionedList, mentionedMobileList);
            using (var client = GetWebhookClient())
            {
                if (client != null)
                    await client.SendText(content, mentionedList, mentionedMobileList);
            }
        }
    }
}
