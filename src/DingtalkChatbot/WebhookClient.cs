using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DingtalkChatbot
{
    /// <summary>
    /// 钉钉群机器人
    /// https://ding-doc.dingtalk.com/doc#/serverapi2/qf2nxq
    /// </summary>
    public class WebhookClient : IDisposable
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private string _webhookUrl;

        public WebhookClient(string webhookUrl, string secret = null) : this(new HttpClient(), webhookUrl, secret)
        {
        }
        public WebhookClient(HttpClient httpClient, string webhookUrl, string secret = null)
        {
            this._httpClient = httpClient;

            if (string.IsNullOrWhiteSpace(secret))
            {
                this._webhookUrl = webhookUrl;
            }
            else
            {
                //https://oapi.dingtalk.com/robot/send?access_token=XXXXXX&timestamp=XXX&sign=XXX
                var timestamp = GetTimeStamp();
                var sign = Uri.EscapeDataString(signSecret(secret, timestamp));

                var url = new Uri(webhookUrl);
                var s = url.ParseQueryString();
                s.Add("timestamp", timestamp);
                s.Add("sign", sign);
                this._webhookUrl = new Uri(url, "?" + s.ToString()).AbsoluteUri;
            }
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        private string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private string signSecret(string secret, string timestamp)
        {
            var stringToSign = timestamp + "\n" + secret;
            var encoding = new System.Text.UTF8Encoding();
            var keyByte = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(stringToSign);
            using (var hmacsha256 = new System.Security.Cryptography.HMACSHA256(keyByte))
            {
                var signData = hmacsha256.ComputeHash(messageBytes);
                var sign = Convert.ToBase64String(signData);
                return sign;
            }
        }
        /// <summary>
        /// 发送text类型消息
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="atMobiles">被@人的手机号(在content里添加@人的手机号)</param>
        /// <param name="isAtAll">@所有人时：true，否则为：false</param>
        /// <returns>返回消息发送结果</returns>
        public async Task<ResponseInfo> SendText(string content, bool? isAtAll = null, string[] atMobiles = null)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                    new
                    {
                        msgtype = "text",
                        text = new { content = content },
                        @at = new
                        {
                            atMobiles = atMobiles,
                            isAtAll = isAtAll
                        }
                    });
                var response = await responseMessage.Content.ReadAsAsync<ResponseInfo>();
                //var json = await responseMessage.Content.ReadAsStringAsync();
                //var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseInfo>(json);

                return response;
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 发送图片类型消息
        /// </summary>
        /// <param name="picUrl">图片链接</param>
        /// <returns>返回消息发送结果</returns>
        public async Task<ResponseInfo> SendImage(string picUrl)
        {
            if (string.IsNullOrWhiteSpace(picUrl))
                throw new ArgumentNullException(nameof(picUrl));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                    new
                    {
                        msgtype = "image",
                        image = new { picURL = picUrl }
                    });
                var response = await responseMessage.Content.ReadAsAsync<ResponseInfo>();
                //var json = await responseMessage.Content.ReadAsStringAsync();
                //var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseInfo>(json);
                return response;
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 发送link类型消息
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <param name="text">消息内容。如果太长只会部分展示</param>
        /// <param name="picUrl">图片URL</param>
        /// <param name="messageUrl">点击消息跳转的URL</param>
        /// <returns></returns>
        public async Task<ResponseInfo> SendLink(string title, string text, string picUrl = null, string messageUrl = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(messageUrl))
                messageUrl = string.IsNullOrWhiteSpace(picUrl) ? "about:blank" : picUrl;
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "link",
                           link = new
                           {
                               text = text,
                               title = title,
                               picUrl = picUrl,
                               messageUrl = messageUrl
                           }
                       });
                var response = await responseMessage.Content.ReadAsAsync<ResponseInfo>();
                //var json = await responseMessage.Content.ReadAsStringAsync();
                //var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseInfo>(json);
                return response;
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 发送Markdown类型消息
        /// </summary>
        /// <param name="title">首屏会话透出的展示内容</param>
        /// <param name="text">markdown格式的消息</param>
        /// <param name="atMobiles">被@人的手机号(在content里添加@人的手机号)</param>
        /// <param name="isAtAll">@所有人时：true，否则为：false</param>
        /// <returns></returns>
        public async Task<ResponseInfo> SendMarkdown(string title, string text, bool? isAtAll = null, string[] atMobiles = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "markdown",
                           markdown = new
                           {
                               title = title,
                               text = text
                           },
                           @at = new
                           {
                               atMobiles = atMobiles,
                               isAtAll = isAtAll
                           }
                       });
                var response = await responseMessage.Content.ReadAsAsync<ResponseInfo>();
                return response;
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// 整体跳转ActionCard类型
        /// </summary>
        /// <param name="title">首屏会话透出的展示内容</param>
        /// <param name="text">markdown格式的消息</param>
        /// <param name="btns"></param>
        /// <param name="btnOrientation">0-按钮竖直排列，1-按钮横向排列</param>
        /// <param name="hideAvatar">0-正常发消息者头像，1-隐藏发消息者头像</param>
        /// <returns></returns>
        public async Task<ResponseInfo> SendActionCard(string title, string text, ActionCardBtn[] btns, bool? btnOrientation = null, bool? hideAvatar = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentNullException(nameof(text));
            if (btns == null || btns.Length <= 0)
                throw new ArgumentNullException(nameof(btns));
            try
            {
                var responseMessage = btns.Length == 1 ?
                    await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "actionCard",
                           actionCard = new
                           {
                               title = title,
                               text = text,
                               singleTitle = btns[0].Title,
                               singleURL = btns[0].ActionURL,
                               hideAvatar = hideAvatar == true ? "1" : "0",
                               btnOrientation = btnOrientation == true ? "1" : "0"
                           },
                       })
                    :
                       await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "actionCard",
                           actionCard = new
                           {
                               title = title,
                               text = text,
                               hideAvatar = hideAvatar == true ? "1" : "0",
                               btnOrientation = btnOrientation == true ? "1" : "0",
                               btns = btns.Select(f => new
                               {
                                   title = f.Title,
                                   actionURL = f.ActionURL
                               }).ToArray()
                           }
                       })
                       ;
                var response = await responseMessage.Content.ReadAsAsync<ResponseInfo>();

                return response;
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// FeedCard类型
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<ResponseInfo> SendFeedCard(params FeedCardLinkMsg[] msg)
        {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "feedCard",
                           feedCard = new
                           {
                               links = msg.Select(f => new
                               {
                                   title = f.Title,
                                   messageURL = f.MessageURL,
                                   picURL = f.PicURL
                               }).ToArray()
                           },
                       });
                var response = await responseMessage.Content.ReadAsAsync<ResponseInfo>();

                return response;
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
