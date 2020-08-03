using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WorkWeixinWebHook
{
    /// <summary>
    /// 企业微信群机器人
    /// https://work.weixin.qq.com/api/doc/90000/90136/91770
    /// </summary>
    public class WebhookClient : IDisposable
    {
        private readonly System.Net.Http.HttpClient _httpClient;
        private string _webhookUrl;
        private string _uploadMediaUrl;

        public WebhookClient(string webhookUrl) : this(new HttpClient(), webhookUrl)
        {
        }
        public WebhookClient(HttpClient httpClient, string webhookUrl)
        {
            this._httpClient = httpClient;

            //https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=693a91f6-7xxx-4bc4-97a0-0ec2sifa5aaa
            this._webhookUrl = webhookUrl;

            var url = new Uri(webhookUrl);
            var s = url.ParseQueryString();
            s.Add("type", "file");
            //https://qyapi.weixin.qq.com/cgi-bin/webhook/upload_media?key=KEY&type=TYPE
            this._uploadMediaUrl = new Uri("https://qyapi.weixin.qq.com/cgi-bin/webhook/upload_media?" + s.ToString()).AbsoluteUri;
        }
        /// <summary>
        /// 发送text类型消息
        /// </summary>
        /// <param name="content">文本内容，最长不超过2048个字节，必须是utf8编码</param>
        /// <param name="mentionedList">userid的列表，提醒群中的指定成员(@某个成员)，@all表示提醒所有人，如果开发者获取不到userid，可以使用mentioned_mobile_list</param>
        /// <param name="mentionedMobileList">手机号列表，提醒手机号对应的群成员(@某个成员)，@all表示提醒所有人</param>
        /// <returns>返回消息发送结果</returns>
        public async Task<ResponseInfo> SendText(string content, string[] mentionedList = null, string[] mentionedMobileList = null)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                    new
                    {
                        msgtype = "text",
                        text = new
                        {
                            content = content,
                            mentioned_list = mentionedList,
                            mentioned_mobile_list = mentionedMobileList
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
        /// <param name="base64">图片内容的base64编码</param>
        /// <param name="md5">图片内容（base64编码前）的md5值</param>
        /// <returns>返回消息发送结果</returns>
        public async Task<ResponseInfo> SendImage(string base64, string md5)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentNullException(nameof(base64));
            if (string.IsNullOrWhiteSpace(md5))
                throw new ArgumentNullException(nameof(md5));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                    new
                    {
                        msgtype = "image",
                        image = new
                        {
                            base64 = base64,
                            md5 = md5
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
        public async Task<ResponseInfo> SendImage(byte[] imageBytes)
        {
            if (imageBytes == null)
                throw new ArgumentNullException(nameof(imageBytes));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                    new
                    {
                        msgtype = "image",
                        image = new
                        {
                            base64 = Convert.ToBase64String(imageBytes),
                            md5 = UserMd5(imageBytes)
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

            //// <summary>
            /// MD5　32位加密
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            string UserMd5(byte[] inArray)
            {
                var md5 = System.Security.Cryptography.MD5.Create();//实例化一个md5对像
                var source = md5.ComputeHash(inArray);
                return BitConverter.ToString(source).Replace("-", "");
                //var builder = new System.Text.StringBuilder();
                //for (int i = 0; i < source.Length; i++)
                //{
                //    builder.Append(source[i].ToString("x2"));
                //}
                //return builder.ToString();
            }
        }
        public async Task<ResponseInfo> SendImage(System.Drawing.Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));
            try
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return await SendImage(ms.ToArray());
                }
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
        /// <param name="content">markdown格式的消息</param>
        /// <returns></returns>
        public async Task<ResponseInfo> SendMarkdown(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "markdown",
                           markdown = new
                           {
                               content = content
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
        /// FeedCard类型
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<ResponseInfo> SendNews(params Article[] msg)
        {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "news",
                           news = new
                           {
                               articles = msg.Select(f => new
                               {
                                   title = f.Title,
                                   description = f.Description,
                                   url = f.Url,
                                   picurl = f.PicURL
                               }).ToArray()
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
        /// 文件类型
        /// </summary>
        /// <param name="mediaId">文件id，通过下文的文件上传接口获取</param>
        /// <returns></returns>
        public async Task<ResponseInfo> SendFile(string mediaId)
        {
            if (string.IsNullOrWhiteSpace(mediaId))
                throw new ArgumentNullException(nameof(mediaId));
            try
            {
                var responseMessage = await _httpClient.PostAsJsonAsync(_webhookUrl,
                       new
                       {
                           msgtype = "file",
                           file = new
                           {
                               media_id = mediaId
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
        public async Task<FileResponseInfo> UploadMedia(byte[] fileBytes, string fileName)
        {
            if (fileBytes == null)
                throw new ArgumentNullException(nameof(fileBytes));
            try
            {
                var ss = new System.Net.Http.ByteArrayContent(fileBytes);
                ss.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "media", FileName = fileName ?? "media" };
                ss.Headers.ContentDisposition.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("filelength", fileBytes.Length.ToString()));
                ss.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                var dataContent = new MultipartFormDataContent() { ss };
                var responseMessage = await _httpClient.PostAsync(_uploadMediaUrl, dataContent);
                var response = await responseMessage.Content.ReadAsAsync<FileResponseInfo>();
                return response;
            }
            catch (System.Exception ex)
            {
                throw (ex);
            }
        }

        public async Task<ResponseInfo> UploadMedia(System.IO.Stream fileStream, string fileName)
        {
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));
            try
            {
                var dataContent = new MultipartFormDataContent();
                dataContent.Add(new System.Net.Http.StreamContent(fileStream), "media", fileName ?? "media");
                var responseMessage = await _httpClient.PostAsync(_uploadMediaUrl, dataContent);
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
