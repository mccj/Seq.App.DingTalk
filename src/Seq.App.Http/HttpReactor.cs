using HandlebarsDotNet;
using Seq.Apps;
using Seq.Apps.LogEvents;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Seq.App.DingTalk.Markdown
{
    [SeqApp("Http Markdown Notifier", Description = "Uses a Handlebars template to send events as Http For Markdown.")]
    public class HttpReactor : SeqApp, ISubscribeToJsonAsync
    {
        public HttpReactor()
        {
        }
        [SeqAppSetting(DisplayName = "Url", IsOptional = true)]
        public string Url { get; set; }

        [SeqAppSetting(DisplayName = "Debug Log", IsOptional = true, InputType = SettingInputType.Checkbox)]
        public bool? DebugLog { get; set; }

        public async Task OnAsync(string json)
        {
            if (DebugLog == true) this.Log.Debug("[DebugLog] {AppTitle};InstanceName:{InstanceName};BaseUri:{BaseUri};json:{json}", App.Title, Host.InstanceName, Host.BaseUri, json);
            if (!string.IsNullOrWhiteSpace(this.Url))
            {
                using (var http = new System.Net.Http.HttpClient())
                {
                    var sss = new System.Net.Http.StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    sss.Headers.Add("Seq_Title", App.Title);
                    sss.Headers.Add("Seq_InstanceName", Host.InstanceName);
                    sss.Headers.Add("Seq_BaseUri", Host.BaseUri);
                    var r = await http.SendAsync(new System.Net.Http.HttpRequestMessage()
                    {
                        RequestUri = new Uri(this.Url),
                        Method = System.Net.Http.HttpMethod.Post,
                        Content = sss
                    });
                    var d = await r.Content.ReadAsStringAsync();
                    if (!r.IsSuccessStatusCode)
                    {
                        this.Log.Error("http请求异常，Error:" + d);
                    }
                }
            }
        }
    }
}
